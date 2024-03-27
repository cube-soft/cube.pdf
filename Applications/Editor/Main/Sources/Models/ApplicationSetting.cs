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
using System.Threading;
using Cube.Globalization;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ApplicationSetting
    ///
    /// <summary>
    /// Represents the global settings of the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ApplicationSetting : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ApplicationSetting
        ///
        /// <summary>
        /// Initializes a new instance of the ApplicationSetting class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ApplicationSetting() => _dispose = Locale.Subscribe(e =>
        {
            var ci = e.ToCultureInfo();
            Thread.CurrentThread.CurrentCulture   = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Properties.Resources.Culture          = ci;
        });

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// Configures global settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure() => _core.Invoke();

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the
        /// ImageCollection and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { if (disposing) _dispose.Dispose(); }

        #endregion

        #region Fields
        private static readonly OnceAction _core = new(() => new ApplicationSetting());
        private readonly IDisposable _dispose;
        #endregion
    }
}
