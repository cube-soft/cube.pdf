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
using iTextSharp.text.pdf;
using System.Security.Cryptography;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// EmbeddedAttachment
    ///
    /// <summary>
    /// PDF ファイルに添付済のファイルを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EmbeddedAttachment : Attachment
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="stream">添付ファイルのストリーム</param>
        ///
        /* ----------------------------------------------------------------- */
        public EmbeddedAttachment(PRStream stream) : this("", null, stream) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">添付ファイル名</param>
        /// <param name="file">PDF ファイル情報</param>
        /// <param name="stream">添付ファイルのストリーム</param>
        ///
        /* ----------------------------------------------------------------- */
        public EmbeddedAttachment(string name, MediaFile file, PRStream stream)
            : base()
        {
            Name = name;
            File = file;
            _stream = stream;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetLength
        ///
        /// <summary>
        /// 添付ファイルのサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override long GetLength()
            => _stream?.GetAsDict(PdfName.PARAMS)?
                       .GetAsNumber(PdfName.SIZE)?
                       .LongValue ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// GetBytes
        ///
        /// <summary>
        /// 添付ファイルの内容をバイト配列で取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override byte[] GetBytes() => PdfReader.GetStreamBytes(_stream);

        /* ----------------------------------------------------------------- */
        ///
        /// GetChecksum
        ///
        /// <summary>
        /// 添付ファイルのチェックサムを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override byte[] GetChecksum()
        {
            if (_cache == null)
            {
                var md5 = new MD5CryptoServiceProvider();
                _cache = md5.ComputeHash(GetBytes());
            }
            return _cache;
        }

        #endregion

        #region Fields
        private PRStream _stream;
        private byte[] _cache;
        #endregion
    }
}
