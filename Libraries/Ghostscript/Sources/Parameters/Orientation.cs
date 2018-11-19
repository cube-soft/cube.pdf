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
namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Orientation
    ///
    /// <summary>
    /// Specifies page orientation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Orientation
    {
        /// <summary>Auto</summary>
        Auto = 10,
        /// <summary>Portrait</summary>
        Portrait = 0,
        /// <summary>Upside down (Rotates to 180 degrees from the portrait orientation)</summary>
        UpsideDown = 2,
        /// <summary>Landscape</summary>
        Landscape = 3,
        /// <summary>Seaspace (Rotates to 180 degrees from the landscape orientation)</summary>
        Seascape = 1,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OrientationExtension
    ///
    /// <summary>
    /// Provides extended methods of the Orientation enum.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class OrientationExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgument
        ///
        /// <summary>
        /// Gets a new instance of the Argument class from the specified
        /// orientation.
        /// </summary>
        ///
        /// <param name="src">Orientation value.</param>
        ///
        /// <returns>Argument object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument GetArgument(this Orientation src) =>
            src == Orientation.Auto ?
            new Argument("AutoRotatePages", "PageByPage") :
            new Argument("AutoRotatePages", "None");

        /* ----------------------------------------------------------------- */
        ///
        /// GetCode
        ///
        /// <summary>
        /// Gets a new instance of the Code class with the specified
        /// orientation.
        /// </summary>
        ///
        /// <param name="src">Orientation value.</param>
        ///
        /// <returns>Code object.</returns>
        ///
        /// <remarks>
        /// Orientation に対応する内容を Ghostscript に指定する際、通常の
        /// 引数に加えていくつかの PostScript コードが必要になる場合が
        /// あります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Code GetCode(this Orientation src) =>
            src != Orientation.Auto ?
            new Code($"<</Orientation {src.ToString("d")}>> setpagedevice") :
            null;

        #endregion
    }
}
