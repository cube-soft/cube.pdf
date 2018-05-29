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
namespace Cube.Pdf.Clip.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipItem
    ///
    /// <summary>
    /// 添付ファイルの情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ClipItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ClipItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ClipItem(Attachment raw)
        {
            RawObject = raw;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// 添付ファイルの名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name => RawObject.Name;

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// 添付ファイルのサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length => RawObject.Length;

        /* ----------------------------------------------------------------- */
        ///
        /// Condition
        ///
        /// <summary>
        /// 添付状況を表す文字列を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Condition { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// ClipItem クラスが参照している Attachment オブジェクトを
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment RawObject { get; }

        #endregion
    }
}
