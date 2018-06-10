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
using System.Collections.Generic;

namespace Cube.Pdf.Itext
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
        public DocumentSplitter() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentSplitter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentSplitter(IO io) : base(io) { }

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
            try
            {
                if (!IO.Exists(folder)) IO.CreateDirectory(folder);
                Results.Clear();
                foreach (var page in Pages) SaveCore(page, folder);
            }
            finally { base.OnReset(); } // see remarks
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// SaveCore
        ///
        /// <summary>
        /// PDF ファイルを分割して保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveCore(Page src, string folder)
        {
            var reader = GetRawReader(src);
            if (src.File is PdfFile) reader.Rotate(src);

            var dest = Unique(folder, src.File, src.Number);
            SaveOne(reader, src.Number, dest);
            Results.Add(dest);
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
            var kv = WriterFactory.Create(dest, Metadata, UseSmartCopy, IO);

            kv.Value.Set(Encryption);
            kv.Key.Open();
            kv.Value.AddPage(kv.Value.GetImportedPage(reader, pagenum));
            kv.Key.Close();
            kv.Value.Close();
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
        private string Unique(string dir, File src, int pagenum)
        {
            var name  = src.NameWithoutExtension;
            var digit = string.Format("D{0}", src.Count.ToString("D").Length);

            for (var i = 1; i < int.MaxValue; ++i)
            {
                var filename = (i == 1) ?
                               string.Format("{0}-{1}.pdf", name, pagenum.ToString(digit)) :
                               string.Format("{0}-{1} ({2}).pdf", name, pagenum.ToString(digit), i);
                var dest = IO.Combine(dir, filename);
                if (!IO.Exists(dest)) return dest;
            }

            return IO.Combine(dir, System.IO.Path.GetRandomFileName());
        }

        #endregion
    }
}
