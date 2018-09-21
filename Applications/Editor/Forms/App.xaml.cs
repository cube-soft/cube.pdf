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
using Cube.Log;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Cube.Pdf.App.Editor
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
        public App()
        {
            _dispose = new OnceAction<bool>(Dispose);
        }

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
        public static IEnumerable<string> Arguments { get; private set; } = new string[0];

        #endregion

        #region Methods

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
            Logger.Configure();
            Logger.Info(GetType(), Assembly.GetExecutingAssembly());

            _resources.Add(Logger.ObserveTaskException());
            _resources.Add(this.ObserveUiException());

            Arguments = e.Args ?? new string[0];
            Logger.Info(GetType(), $"Arguments:{string.Join(" ", Arguments)}");

            base.OnStartup(e);
        }

        #region IDisposable

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
            if (disposing)
            {
                foreach (var obj in _resources) obj.Dispose();
            }
        }

        #endregion

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly IList<IDisposable> _resources = new List<IDisposable>();
        #endregion
    }
}
