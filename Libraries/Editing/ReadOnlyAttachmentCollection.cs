/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Editing
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
        /* ----------------------------------------------------------------- */
        public ReadOnlyAttachmentCollection() : this(null, null) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyAttachmentCollection
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="core">内部処理用のオブジェクト</param>
        /// <param name="file">PDF のファイル情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyAttachmentCollection(PdfReader core, FileBase file)
        {
            File = file;
            _core = core;
            ParseAttachments();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileBase
        /// 
        /// <summary>
        /// ファイル情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileBase File { get; }

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
        private void ParseAttachments()
        {
            if (_core == null) return;

            var c = PdfReader.GetPdfObject(_core.Catalog.Get(PdfName.NAMES)) as PdfDictionary;
            if (c == null) return;
            var e = PdfReader.GetPdfObject(c.Get(PdfName.EMBEDDEDFILES)) as PdfDictionary;
            if (e == null) return;

            var files = e.GetAsArray(PdfName.NAMES);
            if (files == null) return;

            for (var i = 1; i < files.Size; i += 2) // see remarks
            {
                var item = files.GetAsDict(i);
                var ef   = item.GetAsDict(PdfName.EF);

                // NOTE: Are /F and /UF contents same ?
                foreach (var key in ef.Keys)
                {
                    var name = item.GetAsString(key)?.ToString();
                    if (string.IsNullOrEmpty(name)) continue;

                    var stream = PdfReader.GetPdfObject(ef.GetAsIndirectObject(key)) as PRStream;
                    if (stream == null) continue;
                    
                    _values.Add(new EmbeddedAttachment(name, File, stream));
                    break;
                }
            }
        }

        #region Fields
        private PdfReader _core = null;
        private IList<Attachment> _values = new List<Attachment>();
        #endregion

        #endregion
    }
}
