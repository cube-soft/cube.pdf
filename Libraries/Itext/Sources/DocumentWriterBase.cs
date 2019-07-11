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
using Cube.Mixin.Generics;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriterBase
    ///
    /// <summary>
    /// Provides an implementation of the "IDocumentWriter" interface by
    /// using the iTextSharp library.
    /// </summary>
    ///
    /// <remarks>
    /// このクラスのオブジェクトを直接生成する事はできません。
    /// このクラスを継承して OnSave メソッドをオーバーライドし、必要な
    /// 処理を実装して下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class DocumentWriterBase : DisposableBase, IDocumentWriter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriterBase
        ///
        /// <summary>
        /// Initializes a new instance of the DocumentWriterBase class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentWriterBase(IO io) { IO = io; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// UseSmartCopy
        ///
        /// <summary>
        /// Gets or sets the value indicating whether the smart copy
        /// algorithm is enabled.
        /// </summary>
        ///
        /// <remarks>
        /// DocumentWriter は通常 iTextSharp の PdfCopy クラスを用いて
        /// 結合を行っていますが、このクラスは複数の PDF ファイルが同じ
        /// フォントを使用していたとしても別々のものとして扱うため、
        /// フォント情報が重複してファイルサイズが増大する場合があります。
        ///
        /// この問題を解決したものとして PdfSmartCopy クラスが存在すします。
        /// ただし、複雑な注釈が保存されている PDF を結合する際に使用した
        /// 場合、（別々として扱わなければならないはずの）情報が共有されて
        /// しまい、注釈の構造が壊れてしまう問題が確認されているので、
        /// 注意が必要です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseSmartCopy { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// Gets the collection of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Page> Pages => _pages;

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// Gets the collection of attached files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Attachment> Attachments => _attachments;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Metadata Metadata { get; private set; } = new Metadata();

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; } = new Encryption();

        #endregion

        #region Methods

        #region IDocumentWriter

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => OnReset();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the document to the specified path.
        /// </summary>
        ///
        /// <param name="path">Path to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path) => OnSave(path);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds pages to the document.
        /// </summary>
        ///
        /// <param name="pages">Collection of pages.</param>
        ///
        /// <remarks>
        /// DocumentReader.Pages オブジェクトを指定する場合
        /// Add(IEnumerable{Page}, IDocumentReader) メソッドを利用
        /// 下さい。
        /// </remarks>
        ///
        /// <see cref="Add(IEnumerable{Page}, IDocumentReader)"/>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> pages) => _pages.AddRange(pages);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds pages to the document.
        /// </summary>
        ///
        /// <param name="pages">Collection of pages.</param>
        /// <param name="hint">
        /// Document reader object to get more detailed information about
        /// the specified pages.
        /// </param>
        ///
        /// <remarks>
        /// IDocumentReader オブジェクトの所有権がこのクラスに移譲に
        /// され、自動的に Dispose が実行されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> pages, IDocumentReader hint)
        {
            Add(pages);
            Bind(hint);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds attached objects to the document.
        /// </summary>
        ///
        /// <param name="files">Collection of attached files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Attachment> files) => _attachments.AddRange(files);

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the PDF metadata.
        /// </summary>
        ///
        /// <param name="src">PDF metadata.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(Metadata src) => Metadata = src;

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the encryption settings.
        /// </summary>
        ///
        /// <param name="src">Encryption settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(Encryption src) => Encryption = src;

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Executes the save operation.
        /// </summary>
        ///
        /// <remarks>
        /// DocumentWriterBase を継承したクラスで実装する必要があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract void OnSave(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// Executes the reset operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReset()
        {
            _pages.Clear();
            _attachments.Clear();
            Set(new Metadata());
            Set(new Encryption());
            Release();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Binds the specified document reader to the class.
        /// </summary>
        ///
        /// <param name="src">Document reader.</param>
        ///
        /// <remarks>
        /// 指定された DocumentReader オブジェクトは DocumentWriter
        /// オブジェクトに所有権が移り、Dispose 等の処理も自動的に
        /// 実行されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected void Bind(IDocumentReader src)
        {
            if (_resources.Contains(src)) return;
            _resources.Add(src);

            if (src is DocumentReader itext)
            {
                var k = itext.File.FullName;
                var v = itext.Core.TryCast<PdfReader>();

                if (v != null && !_hints.ContainsKey(k)) _hints.Add(k, v);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Release
        ///
        /// <summary>
        /// Releases all bound objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Release()
        {
            _hints.Clear();
            foreach (var obj in _resources) obj.Dispose();
            _resources.Clear();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// Gets the PdfReader corresponding to the specified Page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfReader GetRawReader(Page src) =>
            src.File is PdfFile   pdf ? GetRawReader(pdf) :
            src.File is ImageFile img ? GetRawReader(img) : null;

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing) Release();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// Gets the PdfReader corresponding to the specified PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PdfReader GetRawReader(PdfFile file)
        {
            var key = file.FullName;
            if (_hints.TryGetValue(key, out var hit)) return hit;

            var reader = new DocumentReader(key, file.Password, false, IO);
            _resources.Add(reader);

            var dest = reader.Core.TryCast<PdfReader>();
            Debug.Assert(dest != null);
            _hints.Add(key, dest);

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// Gets the PdfReader corresponding to the specified information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PdfReader GetRawReader(ImageFile file)
        {
            var key = file.FullName;
            if (_hints.TryGetValue(key, out var hit)) return hit;

            var dest = ReaderFactory.CreateFromImage(key, IO);
            _resources.Add(dest);
            _hints.Add(key, dest);

            return dest;
        }

        #endregion

        #region Fields
        private readonly List<Page> _pages = new List<Page>();
        private readonly List<Attachment> _attachments = new List<Attachment>();
        private readonly List<IDisposable> _resources = new List<IDisposable>();
        private readonly Dictionary<string, PdfReader> _hints = new Dictionary<string, PdfReader>();
        #endregion
    }
}
