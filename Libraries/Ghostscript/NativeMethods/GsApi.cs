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
                NewInstance(out IntPtr core, IntPtr.Zero);
                if (core == IntPtr.Zero) throw new GsApiException();

                try
                {
                    var code = InitWithArgs(core, args.Length, args);
                    if (code < 0 && code != -101) throw new GsApiException(code);
                }
                finally
                {
                    Exit(core);
                    DeleteInstance(core);
                }
            }
        }

        #endregion

        #region Implementations

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

        #region Fields
        private const string LibName = "gsdll32.dll";
        private static readonly object _lock = new object();
        #endregion
    }
}
