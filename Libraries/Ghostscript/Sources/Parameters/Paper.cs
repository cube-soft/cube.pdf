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
    /// Paper
    ///
    /// <summary>
    /// Specifies paper sizes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Paper
    {
        /// <summary>Auto</summary>
        Auto = 0,
        /// <summary>A0 (841x1189 mm)</summary>
        A0,
        /// <summary>A1 (594x841 mm)</summary>
        A1,
        /// <summary>A2 (420x594 mm)</summary>
        A2,
        /// <summary>A3 (297x420 mm)</summary>
        A3,
        /// <summary>A4 (210x297 mm)</summary>
        A4,
        /// <summary>A5 (148x210 mm)</summary>
        A5,
        /// <summary>A6 (105x148mm)</summary>
        A6,
        /// <summary>C0 (917x1297 mm)</summary>
        C0,
        /// <summary>C1 (648x917 mm)</summary>
        C1,
        /// <summary>C2 (458x648 mm)</summary>
        C2,
        /// <summary>C3 (324x458 mm)</summary>
        C3,
        /// <summary>C4 (229x324 mm)</summary>
        C4,
        /// <summary>C5 (162x229 mm)</summary>
        C5,
        /// <summary>C6 (114x162 mm)</summary>
        C6,
        /// <summary>ISO B0 (1000x1414 mm)</summary>
        IsoB0,
        /// <summary>ISO B1 (707x1000 mm)</summary>
        IsoB1,
        /// <summary>ISO B2 (500x707 mm)</summary>
        IsoB2,
        /// <summary>ISO B3 (353x500 mm)</summary>
        IsoB3,
        /// <summary>ISO B4 (250x353 mm)</summary>
        IsoB4,
        /// <summary>ISO B5 (176x250 mm)</summary>
        IsoB5,
        /// <summary>ISO B6 (125x176 mm)</summary>
        IsoB6,
        /// <summary>JIS B0 (1030x1456 mm)</summary>
        JisB0,
        /// <summary>JIS B1 (728x1030 mm)</summary>
        JisB1,
        /// <summary>JIS B2 (515x728 mm)</summary>
        JisB2,
        /// <summary>JIS B3 (364x515 mm)</summary>
        JisB3,
        /// <summary>JIS B4 (257x364 mm)</summary>
        JisB4,
        /// <summary>JIS B5 (182x257 mm)</summary>
        JisB5,
        /// <summary>JIS B6 (128x182 mm)</summary>
        JisB6,
        /// <summary>Ledger (432x279 mm)</summary>
        Ledger,
        /// <summary>Legal (216x356 mm)</summary>
        Legal,
        /// <summary>Letter (216x279 mm)</summary>
        Letter,
        /// <summary>Hagaki, the Japanese postcard (100x148 mm)</summary>
        Hagaki,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PaperExtension
    ///
    /// <summary>
    /// Paper の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PaperExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgument
        ///
        /// <summary>
        /// Paper を表す Argument オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Paper</param>
        ///
        /// <returns>Argument オブジェクト</returns>
        ///
        /// <remarks>
        /// Paper.Auto の場合、返り値は null になります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Argument GetArgument(this Paper src) =>
            src != Paper.Auto ?
            new Argument('s', "PAPERSIZE", src.ToString().ToLowerInvariant()) :
            null;

        #endregion
    }
}
