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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// Facade
    ///
    /// <summary>
    /// Represents the facade of operations in the CubePDF.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class Facade : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Facade
        ///
        /// <summary>
        /// Initializes a new instance Facade class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Facade() : this(Assembly.GetCallingAssembly()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Facade
        ///
        /// <summary>
        /// Initializes a new instance of the Facade class with the
        /// specified assembly.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Facade(Assembly assembly) : this(new SettingFolder(assembly)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Facade
        ///
        /// <summary>
        /// Initializes a new instance of the Facade class with the
        /// specified settings.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Facade(SettingFolder settings) { Settings = settings; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        ///
        /// <summary>
        /// Gets the collection of created files.
        /// </summary>
        ///
        /// <remarks>
        /// Results may be different from Settings.Value.Destination
        /// when PNG format is specified, etc.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Results { get; private set; } = Enumerable.Empty<string>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes operations with the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke()
        {
            lock (_lock)
            {
                try
                {
                    Settings.Value.Busy = true;
                    var dest = new List<string>();
                    using (var fs = new FileTransfer(Settings, GetTemp()))
                    {
                        Run(() => new DigestChecker(Settings).Invoke());
                        RunGhostscript(fs.Value);
                        Run(() => new FileDecorator(Settings).Invoke(fs.Value));
                        Run(() => fs.Invoke(dest));
                        Run(() => new ProcessLauncher(Settings).Invoke(dest));
                    }
                    Results = dest;
                }
                finally { Settings.Value.Busy = false; }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            lock (_lock)
            {
                _ = Settings.IO.TryDelete(GetTemp());
                if (!Settings.Value.DeleteSource) return;
                _ = Settings.IO.TryDelete(Settings.Value.Source);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        ///
        /// <summary>
        /// Invokes the specified action unless the object is not Disposed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Run(Action action) { if (!Disposed) action(); }

        /* ----------------------------------------------------------------- */
        ///
        /// RunGhostscript
        ///
        /// <summary>
        /// Invokes the Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RunGhostscript(string dest) => Run(() =>
        {
            var gs = GhostscriptFactory.Create(Settings);
            try { gs.Invoke(Settings.Value.Source, dest); }
            finally { gs.LogDebug(); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetTemp
        ///
        /// <summary>
        /// Gets the temp directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTemp() => Settings.IO.Combine(Settings.Value.Temp, Settings.Uid.ToString("D"));

        #endregion

        #region Fields
        private readonly object _lock = new();
        #endregion
    }
}
