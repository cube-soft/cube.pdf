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

/* ------------------------------------------------------------------------- */
///
/// Downsampling
///
/// <summary>
/// Specifies methods of the downsampling.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum Downsampling
{
    /// <summary>None</summary>
    None,
    /// <summary>Average</summary>
    Average,
    /// <summary>Bicubic</summary>
    Bicubic,
    /// <summary>Subsample</summary>
    Subsample,
}

/* ------------------------------------------------------------------------- */
///
/// DownsamplingExtension
///
/// <summary>
/// Provides extended methods of the Downsampling enum.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class DownsamplingExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetArgument
    ///
    /// <summary>
    /// Gets a new instance of the Argument class from the specified
    /// parameters.
    /// </summary>
    ///
    /// <param name="src">Downsampling value.</param>
    /// <param name="name">Name of the argument.</param>
    ///
    /// <returns>Argument object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Argument GetArgument(this Downsampling src, string name) =>
        src != Downsampling.None ?
        new Argument(name, src.ToString()) :
        null;

    #endregion
}
