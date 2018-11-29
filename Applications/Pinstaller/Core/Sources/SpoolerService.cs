/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Generics;
using Cube.Log;
using Cube.Pdf.App.Pinstaller.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// SpoolerService
    ///
    /// <summary>
    /// Provides functionality to start or stop the printer spooler service.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SpoolerService
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SpoolerService
        ///
        /// <summary>
        /// Initializes a new instance of the SpoolerServie class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SpoolerService()
        {
            Timeout = TimeSpan.FromMinutes(1);
            _core = new ServiceController("Spooler");
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the service name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name => _core.ServiceName;

        /* ----------------------------------------------------------------- */
        ///
        /// DisplayName
        ///
        /// <summary>
        /// Gets the display name of the service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DisplayName => _core.DisplayName;

        /* ----------------------------------------------------------------- */
        ///
        /// MachineName
        ///
        /// <summary>
        /// Gets the machine name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string MachineName => _core.MachineName;

        /* ----------------------------------------------------------------- */
        ///
        /// Timeout
        ///
        /// <summary>
        /// Gets or sets the timeout value of starting or stopping the
        /// service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimeSpan Timeout { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Status
        ///
        /// <summary>
        /// Gets the current status of the service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ServiceControllerStatus Status => _core.Status;

        /* ----------------------------------------------------------------- */
        ///
        /// CanStop
        ///
        /// <summary>
        /// Gets the value indicating whether the service can be stopping.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CanStop => _core.CanStop;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts the service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Start()
        {
            Wait();
            this.Log();
            if (Status == ServiceControllerStatus.Running) return;

            var ps = _pending.TryGetValue(Status, out var dest) &&
                     dest == ServiceControllerStatus.Running;
            if (!ps) _core.Start();

            _core.WaitForStatus(ServiceControllerStatus.Running, Timeout);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Stop
        ///
        /// <summary>
        /// Stops the service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Stop()
        {
            Wait();
            this.Log();
            if (Status == ServiceControllerStatus.Stopped) return;
            if (!_core.CanStop) throw new InvalidOperationException($"{Name} cannot stop");

            var ps = _pending.TryGetValue(Status, out var dest) &&
                     dest == ServiceControllerStatus.Stopped;
            if (!ps) _core.Stop();

            _core.WaitForStatus(ServiceControllerStatus.Stopped, Timeout);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Stops the spooler service, clears all of printing jobs, and
        /// restarts the service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            var dir = Path.Combine(Environment.SpecialFolder.System.GetName(), @"spool\printers");
            var src = Directory.GetFiles(dir);
            var n   = src.Length;

            this.LogDebug($"Job:{n}");

            Invoke(() =>
            {
                foreach (var f in src)
                {
                    try { File.Delete(f); }
                    catch (Exception err) { this.LogWarn($"{f}:{err}", err); }
                }
            });
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action)
        {
            if (Status == ServiceControllerStatus.Running) Stop();
            try { action(); }
            finally { Start(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Wait
        ///
        /// <summary>
        /// Waits until the pending status reaches the corresponding status.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Wait()
        {
            if (!_pending.TryGetValue(Status, out var dest)) return;
            this.Log();
            _core.WaitForStatus(dest, Timeout);
        }

        #endregion

        #region Fields
        private readonly ServiceController _core;
        private readonly IDictionary<ServiceControllerStatus, ServiceControllerStatus> _pending =
            new Dictionary<ServiceControllerStatus, ServiceControllerStatus>
        {
            { ServiceControllerStatus.StartPending,    ServiceControllerStatus.Running },
            { ServiceControllerStatus.ContinuePending, ServiceControllerStatus.Running },
            { ServiceControllerStatus.StopPending,     ServiceControllerStatus.Stopped },
            { ServiceControllerStatus.PausePending,    ServiceControllerStatus.Paused  }
        };
        #endregion
    }
}
