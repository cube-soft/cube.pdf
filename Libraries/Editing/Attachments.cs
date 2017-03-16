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
    /// Attachment
    /// 
    /// <summary>
    /// PDF ファイルに添付済のファイルを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Attachment : IAttachment
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
        public Attachment(PRStream stream) : this("", null, stream) { }

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
        public Attachment(string name, FileBase file, PRStream stream) : base()
        {
            Name = name;
            File = file;
            _stream = stream;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        /// 
        /// <summary>
        /// 添付ファイルの名前を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// 添付先 PDF ファイル情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileBase File { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        /// 
        /// <summary>
        /// 添付ファイルのサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length
        {
            get { return _stream?.Length ?? 0; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Checksum
        /// 
        /// <summary>
        /// 添付ファイルのチェックサムを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] Checksum
        {
            get
            {
                if (_cache == null)
                {
                    var md5 = new MD5CryptoServiceProvider();
                    _cache = md5.ComputeHash(GetBytes());
                }
                return _cache;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetBytes
        /// 
        /// <summary>
        /// 添付ファイルの内容をバイト配列で取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] GetBytes() => PdfReader.GetStreamBytes(_stream);

        #endregion

        #region Fields
        private PRStream _stream;
        private byte[] _cache;
        #endregion
    }
}
