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
using Cube.Pdf.Editing.ITextReader;

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
            _names = _core.GetAttachments();
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
        public int Count => _names?.Count() ?? 0;

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
        public IEnumerator<Attachment> GetEnumerator()
        {
            foreach (var name in _names)
            {
                yield return new Attachment
                {
                    Name = name,
                    File = File
                };
            }
        }

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

        #region Fields
        private PdfReader _core = null;
        private IEnumerable<string> _names = null;
        #endregion
    }
}
