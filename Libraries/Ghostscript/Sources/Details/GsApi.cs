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
    internal sealed class GsApi : IDisposable
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
            _dispose    = new OnceAction<bool>(Dispose);
        }

        #endregion

        #region Properties

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

                try
                {
                    var status = NativeMethods.InitWithArgs(_core.Handle, args.Length, args);
                    var error = status < 0 && status != (int)GsApiStatus.Quit && status != (int)GsApiStatus.Info;
                    if (error) throw new GsApiException(status);
                }
                finally { NativeMethods.Exit(_core.Handle); }
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

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~GsApi
        ///
        /// <summary>
        /// Finalizes the GsApi.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~GsApi() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the GsApi.
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
        private void Dispose(bool disposing) => NativeMethods.DeleteInstance(_handle);

        #endregion

        #endregion

        private static readonly GsApi _core = new GsApi();
        private IntPtr _handle;
        private OnceAction _initialize;
        private OnceAction<bool> _dispose;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// NativeMethods
    ///
    /// <summary>
    /// Represents the Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// NewInstance
        ///
        /// <summary>
        /// Creates a new instance of the Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_new_instance")]
        public static extern int NewInstance(out IntPtr instance, IntPtr handle);

        /* ----------------------------------------------------------------- */
        ///
        /// InitWithArgs
        ///
        /// <summary>
        /// Executes the Ghostscript with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_init_with_args")]
        public static extern int InitWithArgs(IntPtr instance, int argc, string[] argv);

        /* ----------------------------------------------------------------- */
        ///
        /// Exit
        ///
        /// <summary>
        /// Exits the Ghostscript operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_exit")]
        public static extern int Exit(IntPtr instance);

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteInstance
        ///
        /// <summary>
        /// Deletes the instance of the Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_delete_instance")]
        public static extern void DeleteInstance(IntPtr instance);

        #endregion

        #region Fields
        private const string LibName = "gsdll32.dll";
        #endregion
    }
}
