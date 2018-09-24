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
using System;

namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// GsApiException
    ///
    /// <summary>
    /// Ghostscript API 実行時にエラーが発生した時に送出される
    /// 例外クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class GsApiException : Exception
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// GsApiException
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public GsApiException(int status) :
            this((GsApiStatus)Enum.ToObject(typeof(GsApiStatus), status)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// GsApiException
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="status">ステータスコード</param>
        ///
        /* ----------------------------------------------------------------- */
        public GsApiException(GsApiStatus status) : this(status, status.ToString()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// GsApiException
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="status">ステータスコード</param>
        /// <param name="message">メッセージ</param>
        ///
        /* ----------------------------------------------------------------- */
        public GsApiException(GsApiStatus status, string message) : base(message)
        {
            Status = status;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Status
        ///
        /// <summary>
        /// ステータスコードを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public GsApiStatus Status { get; }

        #endregion
    }
}
