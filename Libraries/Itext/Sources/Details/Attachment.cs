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
using Cube.FileSystem;
using iTextSharp.text.pdf;
using System.Security.Cryptography;

namespace Cube.Pdf.Itext
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
    internal class EmbeddedAttachment : Attachment
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
        /// <param name="io">I/O オブジェクト</param>
        /// <param name="name">添付ファイル名</param>
        /// <param name="src">添付元 PDF ファイルのパス</param>
        /// <param name="core">添付ストリーム</param>
        ///
        /* ----------------------------------------------------------------- */
        public EmbeddedAttachment(string name, string src, IO io, PRStream core) :
            base(name, src, io)
        {
            _core = core;
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
        protected override long GetLength() =>
            _core?.GetAsDict(PdfName.PARAMS)?.GetAsNumber(PdfName.SIZE)?.LongValue ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// GetBytes
        ///
        /// <summary>
        /// 添付ファイルの内容をバイト配列で取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override byte[] GetData() => PdfReader.GetStreamBytes(_core);

        /* ----------------------------------------------------------------- */
        ///
        /// GetChecksum
        ///
        /// <summary>
        /// 添付ファイルのチェックサムを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override byte[] GetChecksum() =>
            new SHA256CryptoServiceProvider().ComputeHash(Data);

        #endregion

        #region Fields
        private readonly PRStream _core;
        #endregion
    }
}
