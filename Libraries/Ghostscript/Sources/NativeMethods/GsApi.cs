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

namespace Cube.Pdf.Ghostscript.GsApi
{
    /* --------------------------------------------------------------------- */
    ///
    /// GsApi.NativeMethods
    ///
    /// <summary>
    /// Ghostscript API を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Ghostscript API を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(string[] args)
        {
            lock (_lock)
            {
                _once.Invoke();
                if (_core == null || _core.Handle == IntPtr.Zero) throw new GsApiException(GsApiStatus.UnknownError, nameof(NewInstance));

                try
                {
                    var status = InitWithArgs(_core.Handle, args.Length, args);
                    var error  = status < 0 && status != (int)GsApiStatus.Quit && status != (int)GsApiStatus.Info;
                    if (error) throw new GsApiException(status);
                }
                finally { Exit(_core.Handle); }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        ///
        /// <summary>
        /// Initializes the PDFium library.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Initialize() => _core = new GsApiCore();

        /* ----------------------------------------------------------------- */
        ///
        /// PdfiumCore
        ///
        /// <summary>
        /// Initializes and destroys the PDFium library.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private sealed class GsApiCore : IDisposable
        {
            public IntPtr Handle => _handle;
            public GsApiCore() { NewInstance(out _handle, IntPtr.Zero); }
            ~GsApiCore() { Dispose(false); }
            public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
            private void Dispose(bool _)
            {
                if (_disposed) return;
                _disposed = true;
                DeleteInstance(_handle);
            }

            private bool _disposed = false;
            private readonly IntPtr _handle;
        }

        #region APIs

        /* ----------------------------------------------------------------- */
        ///
        /// NewInstance
        ///
        /// <summary>
        /// Ghostscript API 用インスタンスを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_new_instance")]
        private static extern int NewInstance(out IntPtr instance, IntPtr handle);

        /* ----------------------------------------------------------------- */
        ///
        /// InitWithArgs
        ///
        /// <summary>
        /// Ghostscript を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_init_with_args")]
        private static extern int InitWithArgs(IntPtr instance, int argc, string[] argv);

        /* ----------------------------------------------------------------- */
        ///
        /// Exit
        ///
        /// <summary>
        /// 処理を終了します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_exit")]
        private static extern int Exit(IntPtr instance);

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteInstance
        ///
        /// <summary>
        /// Ghostscript API 用インスタンスを削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, EntryPoint = "gsapi_delete_instance")]
        private static extern void DeleteInstance(IntPtr instance);

        #endregion

        #endregion

        #region Fields
        private const string LibName = "gsdll32.dll";
        private static readonly object _lock = new object();
        private static readonly OnceAction _once = new OnceAction(Initialize);
        private static GsApiCore _core;
        #endregion
    }
}
