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
    /// Interpreter
    ///
    /// <summary>
    /// Ghostscript 変換プログラムの基底クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Interpreter
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Device
        ///
        /// <summary>
        /// 変換時に使用するデバイスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Device Device { get; set; } = Device.Pdf;

        /* ----------------------------------------------------------------- */
        ///
        /// Paper
        ///
        /// <summary>
        /// 用紙サイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Paper Paper { get; set; } = Paper.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// Orientation
        ///
        /// <summary>
        /// ページの向きを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Orientation Orientation { get; set; } = Orientation.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// 画像データの変換時に適用する解像度を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Resolution { get; set; } = 600;

        #endregion
    }
}
