/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Converter.Psa;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cube.FileSystem;

/* ------------------------------------------------------------------------- */
///
/// LockFile
///
/// <summary>
/// Manages exclusive access between the virtual printer and the launcher
/// via a lock file. The lock is acquired atomically by writing the file
/// and released by deleting it.
/// </summary>
///
/// <remarks>
/// Typical call sequence per job:
///
/// 1. LockAsync  — acquire the lock and write the print data.
///    Returns true on success, false on failure.
/// 2. ReleaseAsync — launch the full-trust process and transfer ownership
///    of the lock file to the launcher.
///
/// Dispose() deletes the lock file when the job did not complete or
/// was not handed off to the launcher.
/// </remarks>
///
/* ------------------------------------------------------------------------- */
public sealed class LockFile(string path) : IDisposable
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// LockAsync
    ///
    /// <summary>
    /// Acquires the lock, executes action, and if action succeeds,
    /// immediately calls ReleaseAsync with user release action.
    /// </summary>
    ///
    /// <param name="action">
    /// The action to execute under the lock.
    /// </param>
    ///
    /// <param name="release">
    /// The action to execute after a successful lock.
    /// </param>
    ///
    /// <returns>true on success; false on failure.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public async Task<bool> LockAsync(Func<Task<bool>> action, Func<Task> release)
    {
        var done = await LockAsync(action);
        if (done) await ReleaseAsync(release);
        return done;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// LockAsync
    ///
    /// <summary>
    /// Acquires the lock if not already held, then executes action.
    /// </summary>
    ///
    /// <param name="action">
    /// The action to execute under the lock, e.g. writing the print data.
    /// </param>
    ///
    /// <returns>true on success; false on failure.</returns>
    ///
    /// <remarks>
    /// Skips acquisition when the lock is already held (Locked or Ready
    /// state). Re-acquires after a completed job (Released state).
    ///
    /// TODO: Consider whether re-calling after Released should be treated
    /// the same as Idle. To prevent unintended reuse, it may be better to
    /// throw ObjectDisposedException after Released, as after Dispose().
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public async Task<bool> LockAsync(Func<Task<bool>> action)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _state = await CreateAsync(_state);
        var done = await action();
        if (done) _state = LockState.Ready;
        return done;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ReleaseAsync
    ///
    /// <summary>
    /// Executes action (typically launching the full-trust process) and
    /// transfers ownership of the lock file to the launcher.
    /// </summary>
    /// 
    /// <param name="action">
    /// The action to execute before transferring lock ownership, typically
    /// launching the full-trust process.
    /// </param>
    ///
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has already been disposed.
    /// </exception>
    ///
    /* --------------------------------------------------------------------- */
    public async Task ReleaseAsync(Func<Task> action)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        await action();
        _state = LockState.Released;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the lock if still held.
    /// </summary>
    ///
    /// <remarks>
    /// Deletes the lock file when the job did not complete or was not
    /// handed off to the launcher (Locked or Ready state).
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Finalizer
    ///
    /// <summary>
    /// Ensures the lock file is deleted even if Dispose is not called.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    ~LockFile() => Dispose(false);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Deletes the lock file if the job did not complete or was not
    /// handed off. When called from the finalizer, disposing is false and
    /// only unmanaged resources are released; managed resources are
    /// released when true.
    /// </summary>
    ///
    /// <param name="disposing">
    /// true if called from Dispose method; false if called from the finalizer.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;
        if (IsHeld(_state)) Logger.Try(() => Io.Delete(path));
        _state = LockState.Idle;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateAsync
    ///
    /// <summary>
    /// Waits for any existing lock file to be deleted, then atomically
    /// creates the lock file by writing a temporary file and renaming it.
    /// Skips acquisition when the lock is already held.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private async Task<LockState> CreateAsync(LockState state)
    {
        if (IsHeld(state)) return state;

        var tmp = $"{path}.{Guid.NewGuid()}";
        File.WriteAllText(tmp, "lock");
        await WaitAsync(600);
        File.Move(tmp, path, overwrite: true);
        return LockState.Locked;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// WaitAsync
    ///
    /// <summary>
    /// Waits for the lock file to be deleted by another process.
    /// If the file is not present, returns immediately.
    /// If the wait exceeds timeout seconds, forcibly deletes the stale
    /// lock file before returning.
    /// </summary>
    /// 
    /// <param name="timeout">
    /// Timeout in seconds. If exceeded, the stale lock file is forcibly
    /// deleted before returning.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    private async Task WaitAsync(int timeout)
    {
        var dir = Path.GetDirectoryName(path);
        if (dir is null) return;

        var released = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var watcher = new FileSystemWatcher(dir, Path.GetFileName(path))
        {
            NotifyFilter = NotifyFilters.FileName,
            EnableRaisingEvents = true,
        };
        watcher.Deleted += (_, _) => released.TrySetResult(true);

        if (!Io.Exists(path)) return;

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
        try { await released.Task.WaitAsync(cts.Token); }
        catch (OperationCanceledException)
        {
            Logger.Try(() => Io.Delete(path));
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsHeld
    ///
    /// <summary>
    /// Determines whether the lock file is currently held by this
    /// instance and must be deleted on Dispose.
    /// </summary>
    ///
    /// <remarks>
    /// Returns true for both Locked (action not yet completed) and Ready
    /// (action succeeded, awaiting ReleaseAsync). In either case the lock
    /// file is on disk and this instance is responsible for cleaning it up.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static bool IsHeld(LockState state) => state == LockState.Locked || state == LockState.Ready;

    #endregion

    #region Fields
    // Tracks the lifecycle of the lock file within a single job.
    private enum LockState { Idle, Locked, Ready, Released }
    private LockState _state;
    private bool _disposed;
    #endregion
}
