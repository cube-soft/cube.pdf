/* ------------------------------------------------------------------------- */
///
/// MuPdf.cs
///
/// Copyright (c) 2010 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Drawing;

namespace Cube.Pdf.Drawing.MuPdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Operations
    /// 
    /// <summary>
    /// MuPDF の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Operations
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CreatePage
        /// 
        /// <summary>
        /// Page オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static Page CreatePage(this IntPtr core, FileBase file, int pagenum)
        {
            if (NativeMethods.SetPage(core, pagenum) < 0) return null;

            var width  = NativeMethods.GetWidth(core);
            var height = NativeMethods.GetHeight(core);

            var dest = new Page();
            dest.File = file;
            dest.Number = pagenum;
            dest.Size = new Size(width, height);
            dest.Rotation = NativeMethods.GetRotation(core);
            dest.Resolution = new Point(72, 72);
            return dest;
        }
    }
}
