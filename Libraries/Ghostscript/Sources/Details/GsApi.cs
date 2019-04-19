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
using System.Runtime.InteropServices;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// GsApi
    ///
    /// <summary>
    /// Provides functionality to execute Ghostscript APIs.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class GsApi : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// GsApi
        ///
        /// <summary>
        /// Initializes a new instance of the GsApi class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private GsApi()
        {
            _initialize = new OnceAction(() => NativeMethods.NewInstance(out _handle, IntPtr.Zero));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Information
        ///
        /// <summary>
        /// Gets information of the currently loaded Ghostscript library.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static GsInformation Information
        {
            get
            {
                if (_info.Product == IntPtr.Zero)
                {
                    var status = NativeMethods.GetInformation(ref _info, Marshal.SizeOf(_info));
                    if (status != 0) throw new GsApiException(GsApiStatus.UnknownError, "gsapi_revision");
                }
                return _info;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Handle
        ///
        /// <summary>
        /// Gets the core object for Ghostscript APIs.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IntPtr Handle => _handle;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the Ghostscript API with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(string[] args)
        {
            lock (_core)
            {
                _core.Initialize();
                if (_core.Handle == IntPtr.Zero) throw new GsApiException(GsApiStatus.UnknownError, "gsapi_new_instance");

                var code = NativeMethods.InitWithArgs(_core.Handle, args.Length, args);
                NativeMethods.Exit(_core.Handle);
                if (IsError(code)) throw new GsApiException(code);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        ///
        /// <summary>
        /// Initializes the Ghostscript APIs.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Initialize()
        {
            if (!_initialize.Invoked) _initialize.Invoke();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the GsApi
        /// and optionally releases the managed resources.
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
            if (_handle != IntPtr.Zero)
            {
                NativeMethods.DeleteInstance(_handle);
                _handle = IntPtr.Zero;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsError
        ///
        /// <summary>
        /// Determines whether the specified value represents an error code.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool IsError(int code) =>
            code < 0 &&
            code != (int)GsApiStatus.Quit &&
            code != (int)GsApiStatus.Info;

        #endregion

        #region Fields
        private static readonly GsApi _core = new GsApi();
        private static GsInformation _info = new GsInformation();
        private readonly OnceAction _initialize;
        private IntPtr _handle;
        #endregion
    }
}
