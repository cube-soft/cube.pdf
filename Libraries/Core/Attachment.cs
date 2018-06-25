/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem;
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
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="path">添付ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string path) : this(path, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">表示名</param>
        /// <param name="path">添付ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string name, string path) : this(name, path, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="path">添付ファイルのパス</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string path, IO io) : this(io.Get(path).Name, path, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachment
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">表示名</param>
        /// <param name="path">添付ファイルのパス</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment(string name, string path, IO io)
        {
            Name   = name;
            Source = path;
            IO     = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// 添付ファイルの表示名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 添付ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

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
        public byte[] Data => _data ?? (_data = GetData());

        /* ----------------------------------------------------------------- */
        ///
        /// Checksum
        ///
        /// <summary>
        /// 添付ファイルのチェックサムを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public byte[] Checksum => _checksum ?? (_checksum = GetChecksum());

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
        protected virtual long GetLength() => IO.Get(Source)?.Length ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// GetBytes
        ///
        /// <summary>
        /// 添付ファイルの内容をバイト配列で取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual byte[] GetData()
        {
            if (!IO.Exists(Source)) return null;

            using (var src  = IO.OpenRead(Source))
            using (var dest = new System.IO.MemoryStream())
            {
                src.CopyTo(dest);
                return dest.ToArray();
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
            if (!IO.Exists(Source)) return null;
            using (var ss = IO.OpenRead(Source))
            {
                return new SHA256CryptoServiceProvider().ComputeHash(ss);
            }
        }

        #endregion

        #region Fields
        private byte[] _data;
        private byte[] _checksum;
        #endregion
    }
}
