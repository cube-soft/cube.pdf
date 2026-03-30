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
public sealed class LockFile : IDisposable
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// LockFile
    ///
    /// <summary>
    /// Initializes a new instance with the specified lock file path.
    /// </summary>
    ///
    /// <param name="path">Path of the lock file to manage.</param>
    ///
    /* --------------------------------------------------------------------- */
    public LockFile(string path) => _path = path;

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
    /// The action to execute under the lock.
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
        if (IsLocked(_state)) Logger.Try(() => File.Delete(_path));
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
        if (IsLocked(state)) return state;

        var tmp = $"{_path}.{Guid.NewGuid()}";
        File.WriteAllText(tmp, "lock");
        await WaitAsync(600);
        File.Move(tmp, _path, overwrite: true);
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
        var dir = Path.GetDirectoryName(_path);
        if (dir is null) return;

        var released = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var watcher = new FileSystemWatcher(dir, Path.GetFileName(_path))
        {
            NotifyFilter = NotifyFilters.FileName,
            EnableRaisingEvents = true,
        };
        watcher.Deleted += (_, _) => released.TrySetResult(true);

        if (!File.Exists(_path)) return;

        using var cts = new CancellationTokenSource(GetTimeout(timeout));
        try { await released.Task.WaitAsync(cts.Token); }
        catch (OperationCanceledException)
        {
            Logger.Try(() => File.Delete(_path));
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetTimeout
    ///
    /// <summary>
    /// Calculates the remaining timeout for the lock file based on its
    /// last write time. Returns the full timeout if the time cannot be
    /// determined.
    /// </summary>
    ///
    /// <param name="timeout">Maximum timeout in seconds.</param>
    ///
    /// <returns>
    /// The remaining wait duration, clamped to [100ms, timeout seconds].
    /// The 100ms minimum ensures stale files are deleted via the shared
    /// OperationCanceledException path rather than requiring a separate
    /// early-exit branch.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    private TimeSpan GetTimeout(int timeout)
    {
        var lower = TimeSpan.FromMilliseconds(100);
        var upper = TimeSpan.FromSeconds(timeout);

        try
        {
            var elapsed = DateTime.UtcNow - File.GetLastWriteTimeUtc(_path);
            var result = upper - elapsed;

            return result < lower ? lower :
                   result > upper ? upper : result;
        }
        catch { return upper; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsLocked
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
    private static bool IsLocked(LockState state) => state == LockState.Locked || state == LockState.Ready;

    #endregion

    #region Fields
    // Tracks the lifecycle of the lock file within a single job.
    private enum LockState { Idle, Locked, Ready, Released }
    private readonly string _path;
    private LockState _state;
    private bool _disposed;
    #endregion
}
