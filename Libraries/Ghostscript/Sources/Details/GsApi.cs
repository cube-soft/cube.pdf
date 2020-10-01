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
using System.Runtime.InteropServices;
using Cube.FileSystem;
using Cube.Mixin.IO;
using Cube.Mixin.String;

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
    internal static class GsApi
    {
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
                    var code = NativeMethods.GetInformation(ref _info, Marshal.SizeOf(_info));
                    if (code != 0) throw new GsApiException(GsApiStatus.UnknownError, "gsapi_revision");
                }
                return _info;
            }
        }

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
        public static void Invoke(IEnumerable<string> args, string tmp, IO io)
        {
            lock (_lock)
            {
                SetTemp(tmp, io, () =>
                {
                    NativeMethods.NewInstance(out var core, IntPtr.Zero);
                    if (core == IntPtr.Zero) throw new GsApiException(GsApiStatus.UnknownError, "gsapi_new_instance");

                    try
                    {
                        var array = args.ToArray();
                        var code = NativeMethods.InitWithArgs(core, array.Length, array);
                        if (IsError(code)) throw new GsApiException(code);
                    }
                    finally
                    {
                        NativeMethods.Exit(core);
                        NativeMethods.DeleteInstance(core);
                    }
                });
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Sets the working directory and invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetTemp(string tmp, IO io, Action callback)
        {
            var e0 = Environment.GetEnvironmentVariable("Tmp");
            var e1 = Environment.GetEnvironmentVariable("Temp");

            try
            {
                if (tmp.HasValue())
                {
                    if (!io.Exists(tmp)) io.CreateDirectory(tmp);

                    SetVariable("Tmp", tmp);
                    Logger.Debug(typeof(GsApi), $"Tmp:{e0.Quote()} -> {tmp.Quote()}");

                    SetVariable("Temp", tmp);
                    Logger.Debug(typeof(GsApi), $"Temp:{e1.Quote()} -> {tmp.Quote()}");
                }
                callback();
            }
            finally
            {
                if (tmp.HasValue())
                {
                    SetVariable("Tmp",  e0);
                    SetVariable("Temp", e1);
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetVariable
        ///
        /// <summary>
        /// Sets the environment variable with the specified key and value.
        /// </summary>
        ///
        /// <remarks>
        /// 設定された環境変数は実行プロセス中でのみ有効です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetVariable(string key, string value) =>
            Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);

        #endregion

        #region Implementations

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
        private static readonly object _lock = new object();
        private static GsInformation _info = new GsInformation();
        #endregion
    }
}
