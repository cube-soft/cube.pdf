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
using System.Collections.Generic;
using iTextSharp.text.pdf;
using Cube.Pdf.Editing.IText;
using IoEx = System.IO;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentSplitter
    /// 
    /// <summary>
    /// PDF ファイルを全て 1 ページの PDF ファイルに分割するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentSplitter : DocumentWriterBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentSplitter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentSplitter() : base() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        /// 
        /// <summary>
        /// 作成した PDF ファイルのパス一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<string> Results { get; } = new List<string>();

        #endregion

        #region Override Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        /// 
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnReset()
        {
            base.OnReset();
            Results.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        /// 
        /// <summary>
        /// PDF ファイルを指定フォルダ下に保存します。
        /// </summary>
        /// 
        /// <remarks>
        /// Reset() を実行すると Results まで消去されてしまうため、
        /// base.OnReset() を代わりに実行しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave(string folder)
        {
            if (!IoEx.Directory.Exists(folder)) IoEx.Directory.CreateDirectory(folder);
            Results.Clear();

            try
            {
                foreach (var page in Pages)
                {
                    if (page.File is PdfFile) SavePage(page, folder);
                    else if (page.File is ImageFile) SaveImagePage(page, folder);
                }
            }
            finally { base.OnReset(); /* see remarks */ }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// SavePage
        /// 
        /// <summary>
        /// PDF ファイルを分割して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SavePage(Page src, string folder)
        {
            var reader = GetRawReader(src);
            reader.Rotate(src);

            var dest = Unique(folder, src.File, src.Number);
            SaveOne(reader, src.Number, dest);
            Results.Add(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveImagePage
        /// 
        /// <summary>
        /// 画像ファイルを 1 ページの PDF ファイルに変換して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveImagePage(Page src, string folder)
        {
            var reader = GetRawReader(src);
            for (var i = 0; i < reader.NumberOfPages; ++i)
            {
                var pagenum = i + 1;
                var dest = Unique(folder, src.File, pagenum);
                SaveOne(reader, pagenum, dest);
                Results.Add(dest);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOne
        /// 
        /// <summary>
        /// 1 ページの PDF ファイルを保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void SaveOne(PdfReader reader, int pagenum, string dest)
        {
            var document = new iTextSharp.text.Document();
            var writer = GetRawWriter(document, dest);

            writer.Set(Encryption);
            document.Open();
            writer.AddPage(writer.GetImportedPage(reader, pagenum));

            document.Close();
            writer.Close();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unique
        /// 
        /// <summary>
        /// 一意のパス名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string Unique(string folder, MediaFile src, int pagenum)
        {
            var basename = IoEx.Path.GetFileNameWithoutExtension(src.FullName);
            var digit = string.Format("D{0}", src.PageCount.ToString("D").Length);
            for (var i = 1; i < 1000; ++i)
            {
                var filename = (i == 1) ?
                               string.Format("{0}-{1}.pdf", basename, pagenum.ToString(digit)) :
                               string.Format("{0}-{1} ({2}).pdf", basename, pagenum.ToString(digit), i);
                var dest = IoEx.Path.Combine(folder, filename);
                if (!IoEx.File.Exists(dest)) return dest;
            }

            return IoEx.Path.Combine(folder, IoEx.Path.GetRandomFileName());
        }

        #endregion
    }
}
