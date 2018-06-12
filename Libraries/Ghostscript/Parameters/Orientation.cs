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
using System.Collections.Generic;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Orientation
    ///
    /// <summary>
    /// ページの向きを定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Orientation
    {
        /// <summary>自動</summary>
        Auto = 10,
        /// <summary>縦向き</summary>
        Portrait = 0,
        /// <summary>縦向き（180 度回転）</summary>
        PortraitReverse = 2,
        /// <summary>横向き</summary>
        Landscape = 3,
        /// <summary>横向き（180 度回転）</summary>
        LandscapeReverse = 1,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OrientationExtension
    ///
    /// <summary>
    /// Orientation の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class OrientationExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArguments
        ///
        /// <summary>
        /// Orientation を表す Argument オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">Orientation</param>
        ///
        /// <returns>Argument オブジェクト一覧</returns>
        ///
        /// <remarks>
        /// Ghostscript API における Orientation の指定方法が特殊な形に
        /// なっているため、指定順序によってはエラーが発生する場合が
        /// あります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Argument> GetArguments(this Orientation src) =>
            src == Orientation.Auto ?
            new[] { new Argument("AutoRotatePages", "PageByPage") } :
            new[]
            {
                new Argument("AutoRotatePages", "None"),
                new Argument('c'),
                new Argument($"<</Orientation {src.ToString("d")}>> setpagedevice"),
            };

        #endregion
    }
}
