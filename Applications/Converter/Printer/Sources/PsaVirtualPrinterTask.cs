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
namespace Cube.Pdf.Converter.Printer;

using System;
using System.IO;
using System.Threading.Tasks;
using Cube.Pdf.Converter.Psa;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Graphics.Printing.Workflow;
using Windows.Storage;
using Windows.Storage.Streams;

/* ------------------------------------------------------------------------- */
///
/// PsaVirtualPrinterTask
///
/// <summary>
/// Minimal implementation of the PrintSupportVirtualPrinterWorkflow feature.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class PsaVirtualPrinterTask : IBackgroundTask
{
    /* --------------------------------------------------------------------- */
    ///
    /// Run
    ///
    /// <summary>
    /// Performs the work of a background task.
    /// </summary>
    ///
    /// <param name="task">
    /// An interface to an instance of the background task.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    public void Run(IBackgroundTaskInstance task)
    {
        var deferral = task?.GetDeferral();
        if (task is null || deferral is null) return;
        task.Canceled += (_, _) => deferral.Complete();

        var details = task.TriggerDetails as PrintWorkflowVirtualPrinterTriggerDetails;
        var session = details?.VirtualPrinterSession;
        if (session is null) return;

        session.VirtualPrinterDataAvailable += async (_, e) =>
        {
            var done = false;
            try { done = await InvokeAsync(e); }
            finally
            {
                e.CompleteJob(done ? PrintWorkflowSubmittedStatus.Succeeded : PrintWorkflowSubmittedStatus.Failed);
                deferral.Complete();
            }
        };

        session.Start();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InvokeAsync
    ///
    /// <summary>
    /// Writes the incoming print data to the shared publisher cache and
    /// launches the full-trust launcher process to handle conversion.
    /// </summary>
    ///
    /// <param name="e">
    /// Event arguments providing access to the incoming XPS content.
    /// </param>
    ///
    /// <returns>
    /// true on success; false otherwise.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    private static async Task<bool> InvokeAsync(PrintWorkflowVirtualPrinterDataAvailableEventArgs e)
    {
        var dir = CacheFolder.Get();
        if (dir is null) return false;

        var metadata = CreateMetadata(e.Configuration);

        using var file = new LockFile(Path.Combine(dir.Path, Metadata.LockFileName));
        var done = await file.LockAsync(async () =>
        {
            var dest = await dir.CreateFileAsync(Metadata.SourceFileName, CreationCollisionOption.ReplaceExisting);
            if (dest is null) return false;

            using var s = await dest.OpenAsync(FileAccessMode.ReadWrite);
            await RandomAccessStream.CopyAndCloseAsync(e.SourceContent.GetInputStream(), s.GetOutputStreamAt(s.Size));
            await metadata.SaveAsync(Path.Combine(dir.Path, Metadata.FileName));
            return true;
        });

        if (done) await file.ReleaseAsync(LaunchAsync);
        return done;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// LaunchAsync
    ///
    /// <summary>
    /// Launches the full-trust launcher process to handle conversion.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static async Task LaunchAsync() => await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("Launcher");

    /* --------------------------------------------------------------------- */
    ///
    /// CreateMetadata
    ///
    /// <summary>
    /// Creates a new instance of the Metadata class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Metadata CreateMetadata(PrintWorkflowConfiguration src) => new()
    {
        JobTitle  = src.JobTitle,
        SessionId = src.SessionId,
        AppName   = src.SourceAppDisplayName,
    };
}
