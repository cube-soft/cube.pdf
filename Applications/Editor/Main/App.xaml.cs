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
using System.Windows;
using Cube.Logging;
using Cube.Mixin.Collections;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// App
    ///
    /// <summary>
    /// Represents the main program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class App : Application, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// App
        ///
        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public App() { _dispose = new(Dispose); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// Gets the arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<string> Arguments { get; private set; } = Enumerable.Empty<string>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ~App
        ///
        /// <summary>
        /// Finalizes the App.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~App() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the App.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnStartup
        ///
        /// <summary>
        /// Occurs when the Startup event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnStartup(StartupEventArgs e)
        {
            Arguments = e.Args ?? Enumerable.Empty<string>();

            BindingLogger.Setup();
            Logger.Info(typeof(App).Assembly);
            Logger.Info($"[ {Arguments.Join(" ")} ]");
            Logger.ObserveTaskException();
            this.ObserveUiException();

            ApplicationSetting.Configure();
            base.OnStartup(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the App
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _disposable.Dispose();
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly DisposableContainer _disposable = new();
        #endregion
    }
}
