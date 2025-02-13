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
namespace Cube.Pdf.Ghostscript;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cube.FileSystem;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// GsApi
///
/// <summary>
/// Provides functionality to execute Ghostscript APIs.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class GsApi
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Information
    ///
    /// <summary>
    /// Gets information of the currently loaded Ghostscript library.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
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

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the Ghostscript API with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static void Invoke(string[] raw, string tmp, string log) => SetTemp(tmp, () =>
    {
        _ = NativeMethods.NewInstance(out var core, IntPtr.Zero);
        if (core == IntPtr.Zero) throw new GsApiException(GsApiStatus.UnknownError, "gsapi_new_instance");
        var args = new List<IntPtr>();

        try
        {
            foreach (var e in raw) args.Add(ToUtf8(e));
            NativeMethods.SetArgEncoding(core, 1 /*GS_ARG_ENCODING_UTF8*/ );
            var code = NativeMethods.InitWithArgs(core, args.Count, args.ToArray());
            if (code < 0 && code != (int)GsApiStatus.Quit && code != (int)GsApiStatus.Info) throw new GsApiException(code, log);
        }
        finally
        {
            foreach (var e in args) Marshal.FreeHGlobal(e);
            _ = NativeMethods.Exit(core);
            NativeMethods.DeleteInstance(core);
        }
    });

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// ToUtf8
    ///
    /// <summary>
    /// Convert string to Native UTF-8 pointer.
    /// </summary>
    ///
    /// <remarks>
    /// Remember to clean up with a call to Marshal.FreeHGlobal.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static IntPtr ToUtf8(string src)
    {
        var bytes  = System.Text.Encoding.UTF8.GetByteCount(src);
        var buffer = new byte[bytes + 1]; // Null-terminator allocated
        System.Text.Encoding.UTF8.GetBytes(src, 0, src.Length, buffer, 0);

        var dest = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, dest, buffer.Length);
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetTemp
    ///
    /// <summary>
    /// Sets the working directory and invokes the specified action.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void SetTemp(string tmp, Action callback)
    {
        var e0 = Environment.GetEnvironmentVariable("Tmp");
        var e1 = Environment.GetEnvironmentVariable("Temp");

        try
        {
            if (tmp.HasValue())
            {
                Io.CreateDirectory(tmp);

                SetVariable("Tmp", tmp);
                Logger.Debug($"[Tmp] {e0.Quote()} -> {tmp.Quote()}");

                SetVariable("Temp", tmp);
                Logger.Debug($"[Temp] {e1.Quote()} -> {tmp.Quote()}");
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

    /* --------------------------------------------------------------------- */
    ///
    /// SetVariable
    ///
    /// <summary>
    /// Sets the environment variable with the specified key and value.
    /// </summary>
    ///
    /// <remarks>
    /// The set environment variables are only valid during the
    /// execution process.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static void SetVariable(string key, string value) =>
        Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);

    #endregion

    #region Fields
    private static GsInformation _info = new();
    #endregion
}
