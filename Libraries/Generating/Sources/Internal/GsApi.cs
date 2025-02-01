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
    public static void Invoke(string[] args, string tmp) => SetTemp(tmp, () =>
    {
        _ = NativeMethods.NewInstance(out var core, IntPtr.Zero);
        if (core == IntPtr.Zero) throw new GsApiException(GsApiStatus.UnknownError, "gsapi_new_instance");

        IntPtr[] utf8argv = new IntPtr[args.Length];
        for (int i = 0; i < utf8argv.Length; i++)
        {
            utf8argv[i] = NativeUtf8FromString(args[i]);
        }

        try
        {
            NativeMethods.SetArgEncoding(core, 1 /*GS_ARG_ENCODING_UTF8*/ );
            var code = NativeMethods.InitWithArgs(core, utf8argv.Length, utf8argv);
            if (code < 0 && code != (int)GsApiStatus.Quit && code != (int)GsApiStatus.Info) throw new GsApiException(code);
        }
        finally
        {
            for (int i = 0; i < utf8argv.Length; i++)
            {
                Marshal.FreeHGlobal(utf8argv[i]);
            }

            _ = NativeMethods.Exit(core);
            NativeMethods.DeleteInstance(core);
        }
    });

    #endregion

    #region Implementations
    /* --------------------------------------------------------------------- */
    ///
    /// NativeUtf8FromString
    ///
    /// <summary>
    /// Convert string to nativeUTF8 ptr. *remember to clean up with a call to Marshal.FreeHGlobal.*
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static IntPtr NativeUtf8FromString(string managedString)
    {
        int len = System.Text.Encoding.UTF8.GetByteCount(managedString);
        byte[] buffer = new byte[len + 1]; // null-terminator allocated
        System.Text.Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
        IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
        return nativeUtf8;
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
