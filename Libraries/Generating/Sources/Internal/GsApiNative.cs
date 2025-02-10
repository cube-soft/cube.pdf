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

/* ------------------------------------------------------------------------- */
///
/// NativeMethods
///
/// <summary>
/// Represents the Ghostscript API.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class NativeMethods
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetRevision
    ///
    /// <summary>
    /// Gets information of the currently loaded Ghostscript library.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DllImport(LibName, CharSet = CharSet.Ansi, EntryPoint = "gsapi_revision")]
    public static extern int GetInformation(ref GsInformation dest, int length);

    /* --------------------------------------------------------------------- */
    ///
    /// NewInstance
    ///
    /// <summary>
    /// Creates a new instance of the Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DllImport(LibName, EntryPoint = "gsapi_new_instance")]
    public static extern int NewInstance(out IntPtr instance, IntPtr handle);

    /* --------------------------------------------------------------------- */
    ///
    /// SetArgEncoding
    ///
    /// <summary>
    /// Sets encoding of arguments supplied via the gsapi interface
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DllImport(LibName, EntryPoint = "gsapi_set_arg_encoding")]
    public static extern int SetArgEncoding(IntPtr instance, int encoding);

    /* --------------------------------------------------------------------- */
    ///
    /// InitWithArgs
    ///
    /// <summary>
    /// Executes the Ghostscript with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DllImport(LibName, EntryPoint = "gsapi_init_with_args")]
    public static extern int InitWithArgs(IntPtr instance, int argc, IntPtr[] argv);

    /* --------------------------------------------------------------------- */
    ///
    /// Exit
    ///
    /// <summary>
    /// Exits the Ghostscript operation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DllImport(LibName, EntryPoint = "gsapi_exit")]
    public static extern int Exit(IntPtr instance);

    /* --------------------------------------------------------------------- */
    ///
    /// DeleteInstance
    ///
    /// <summary>
    /// Deletes the instance of the Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DllImport(LibName, EntryPoint = "gsapi_delete_instance")]
    public static extern void DeleteInstance(IntPtr instance);

    #endregion

    #region Fields
    private const string LibName = "gsdll32";
    #endregion
}
