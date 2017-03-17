/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.Security.Cryptography;
using iTextSharp.text.pdf;

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
        public EmbeddedAttachment(string name, MediaFile file, PRStream stream) : base()
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
        protected override long GetLength() => _stream?.Length ?? 0;

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
