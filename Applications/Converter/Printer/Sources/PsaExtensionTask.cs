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
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Graphics.Printing.PrintSupport;

/* ------------------------------------------------------------------------- */
///
/// PsaExtensionTask
///
/// <summary>
/// Minimal implementation of the Windows.PrintSupportExtension feature.
/// </summary>
///
/// <remarks>
/// This implementation is provided only to satisfy platform requirements.
/// The application does not rely on this feature, but a definition is
/// required to avoid runtime errors. Therefore, this task simply reports
/// a resolved state.
/// </remarks>
///
/* ------------------------------------------------------------------------- */
public sealed class PsaExtensionTask : IBackgroundTask
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
    public void Run(IBackgroundTaskInstance task) => Hack(() =>
    {
        var deferral = task?.GetDeferral();
        if (task is null || deferral is null) return;
        task.Canceled += (_, _) => Hack(() => deferral.Complete());

        var details = task.TriggerDetails as PrintSupportExtensionTriggerDetails;
        if (details?.Session is null)
        {
            deferral.Complete();
            return;
        }

        details.Session.PrintTicketValidationRequested += (_, e) =>
        {
            Hack(() => { using (e.GetDeferral()) e.SetPrintTicketValidationStatus(WorkflowPrintTicketValidationStatus.Resolved); });
        };

        details.Session.Start();
    });

    /* --------------------------------------------------------------------- */
    ///
    /// Hack
    ///
    /// <summary>
    /// Invokes the specified action, writing any exception to the trace log
    /// instead of letting it propagate.
    /// </summary>
    ///
    /// <param name="action">Action to invoke.</param>
    ///
    /// <remarks>
    /// Used to guard against a race condition specific to in-process
    /// background tasks: if the task Canceled event fires concurrently,
    /// the session may be torn down before the call completes, causing it
    /// to throw 0x3E3 (ERROR_OPERATION_ABORTED). Without a catch, this
    /// exception propagates into the host process and forces a termination
    /// — which is why a second printer selection in Edge reliably causes
    /// a crash.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static void Hack(Action action)
    {
        try { action(); }
        catch (Exception err) { Trace.WriteLine(err.ToString()); }
    }
}
