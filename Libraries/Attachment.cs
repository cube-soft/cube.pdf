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

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Attachment
    /// 
    /// <summary>
    /// 添付ファイルを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Attachment
    {
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
        /// 添付オブジェクトが属するファイルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File File
        {
            get { return _file; }
            set
            {
                if (_file == value) return;
                _file  = value;
                _cache = null;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        /// 
        /// <summary>
        /// 添付ファイルのサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length => GetLength();

        /* ----------------------------------------------------------------- */
        ///
        /// Data
        /// 
        /// <summary>
        /// 添付ファイルの内容を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] Data => GetBytes();

        /* ----------------------------------------------------------------- */
        ///
        /// Checksum
        /// 
        /// <summary>
        /// 添付ファイルのチェックサムを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] Checksum => GetChecksum();

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
        protected virtual long GetLength()
            => File?.Length ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// GetBytes
        /// 
        /// <summary>
        /// 添付ファイルの内容をバイト配列で取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual byte[] GetBytes()
        {
            var buffer = new byte[1024 * 1024];

            using (var input = System.IO.File.OpenRead(File.FullName))
            using (var ms = new System.IO.MemoryStream())
            {
                var n = 0;
                while ((n = input.Read(buffer, 0, buffer.Length)) > 0) ms.Write(buffer, 0, n);
                return ms.ToArray();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetChecksum
        /// 
        /// <summary>
        /// 添付ファイルのチェックサムを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual byte[] GetChecksum()
        {
            if (_cache == null) _cache = GetChecksum(File.FullName);
            return _cache;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetChecksum
        /// 
        /// <summary>
        /// チェックサムを計算します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private byte[] GetChecksum(string path)
        {
            if (!System.IO.File.Exists(path)) return null;

            using (var s = new System.IO.BufferedStream(
                System.IO.File.OpenRead(path),
                1024 * 1024
            )) return new MD5CryptoServiceProvider().ComputeHash(s);
        }

        #region Fields
        private File _file;
        private byte[] _cache;
        #endregion

        #endregion
    }
}
