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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReadOnlyAttachmentCollection
    ///
    /// <summary>
    /// 読み取り専用で添付ファイル一覧へアクセスするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ReadOnlyAttachmentCollection : IReadOnlyCollection<Attachment>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyAttachmentCollection
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="core">内部処理用のオブジェクト</param>
        /// <param name="file">PDF ファイル情報</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyAttachmentCollection(PdfReader core, PdfFile file, IO io)
        {
            File  = file;
            IO    = io;
            _core = core;

            Parse();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile File { get; }

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
        /// Count
        ///
        /// <summary>
        /// 添付ファイルの数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => _values.Count();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 各ページオブジェクトへアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<Attachment> GetEnumerator() => _values.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 各ページオブジェクトへアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ParseAttachments
        ///
        /// <summary>
        /// 添付ファイルを解析します。
        /// </summary>
        ///
        /// <remarks>
        /// /EmbededFiles, /Names で取得できる配列は、以下のような構造に
        /// なっています。
        ///
        /// [String, Object, String, Object, ...]
        ///
        /// この内、各 Object が、添付ファイルに関する実際の情報を保持
        /// しています。そのため、間の String 情報をスキップする必要が
        /// あります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Parse()
        {
            if (_core == null) return;

            if (!(PdfReader.GetPdfObject(_core.Catalog.Get(PdfName.NAMES)) is PdfDictionary c)) return;
            if (!(PdfReader.GetPdfObject(c.Get(PdfName.EMBEDDEDFILES)) is PdfDictionary e)) return;

            var files = e.GetAsArray(PdfName.NAMES);
            if (files == null) return;

            for (var i = 1; i < files.Size; i += 2) // see remarks
            {
                var item = files.GetAsDict(i);
                var name = item.GetAsString(PdfName.UF)?.ToUnicodeString();
                if (string.IsNullOrEmpty(name)) name = item.GetAsString(PdfName.F)?.ToString();
                if (string.IsNullOrEmpty(name)) continue;

                var ef = item.GetAsDict(PdfName.EF);
                if (ef == null) continue;

                foreach (var key in ef.Keys)
                {
                    if (!(PdfReader.GetPdfObject(ef.GetAsIndirectObject(key)) is PRStream stream)) continue;
                    _values.Add(new EmbeddedAttachment(name, File.FullName, IO, stream));
                    break;
                }
            }
        }

        #endregion

        #region Fields
        private readonly PdfReader _core;
        private readonly IList<Attachment> _values = new List<Attachment>();
        #endregion
    }
}
