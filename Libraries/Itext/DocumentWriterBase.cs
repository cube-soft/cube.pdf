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
using Cube.Generics;
using Cube.Log;
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
    /// DocumentWriter の基底クラスです。
    /// </summary>
    ///
    /// <remarks>
    /// このクラスのオブジェクトを直接生成する事はできません。
    /// このクラスを継承して OnSave メソッドをオーバーライドし、必要な
    /// 処理を実装して下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class DocumentWriterBase : IDocumentWriter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentWriterBase
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentWriterBase(IO io)
        {
            _dispose = new OnceAction<bool>(Dispose);
            IO = io;
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
        /// UseSmartCopy
        ///
        /// <summary>
        /// ファイルサイズを抑えるための結合方法を使用するかどうかを取得、
        /// または設定します。
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
        /// Dpi
        ///
        /// <summary>
        /// PDF の DPI 値を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// iTextSharp では、72dpi を基準に用紙サイズ等の大きさを算出して
        /// いるものと推測されます。これが PDF の規格で決まっているのか、
        /// iTextSharp 独自の処理なのかは要調査。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected float Dpi => 72.0f;

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// PDF ファイルの各ページ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Page> Pages => _pages;

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        ///
        /// <summary>
        /// 添付ファイル一覧の情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<Attachment> Attachments => _attach;

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// PDF ファイルのメタデータを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Metadata Metadata { get; private set; } = new Metadata();

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// 暗号化に関する情報をを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Encryption Encryption { get; private set; } = new Encryption();

        #endregion

        #region Methods

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~DocumentWriterBase
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~DocumentWriterBase() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// オブジェクトを破棄する際に必要な終了処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// オブジェクトを破棄する際に必要な終了処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) Release();
        }

        #endregion

        #region IDocumentWriter

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => OnReset();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// プロパティが現在保持している、メタ情報、暗号化に関する情報、
        /// ページ情報に基づいた PDF ファイルを生成し、指定されたパスに
        /// 保存します。
        /// </summary>
        ///
        /// <param name="path">保存パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path) => OnSave(path);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ページを追加します。
        /// </summary>
        ///
        /// <param name="page">ページ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(Page page) => _pages.Add(page);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ページを追加します。
        /// </summary>
        ///
        /// <param name="pages">ページ情報一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Page> pages)
        {
            foreach (var page in pages) Add(page);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// ファイルを添付します。
        /// </summary>
        ///
        /// <param name="file">添付ファイル</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(Attachment file) => _attach.Add(file);

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// ファイルを添付します。
        /// </summary>
        ///
        /// <param name="files">添付ファイル一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(IEnumerable<Attachment> files)
        {
            foreach (var item in files) Attach(item);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// メタ情報を設定します。
        /// </summary>
        ///
        /// <param name="src">メタ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(Metadata src) => Metadata = src;

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 暗号化情報を設定します。
        /// </summary>
        ///
        /// <param name="src">暗号化情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(Encryption src) => Encryption = src;

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// DocumentReader オブジェクトを束縛します。
        /// </summary>
        ///
        /// <param name="src">DocumentReader オブジェクト</param>
        ///
        /// <remarks>
        /// 指定された DocumentReader オブジェクトは DocumentWriter
        /// オブジェクトに所有権が移り、Dispose 等の処理も自動的に
        /// 実行されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Bind(DocumentReader src)
        {
            var key = src.File.FullName;

            if (_bounds.ContainsKey(key))
            {
                if (_bounds[key] != src)
                {
                    this.LogWarn($"{key}:Other instance has already bound.");
                    src.Dispose();
                }
            }
            else _bounds.Add(key, src);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsBound
        ///
        /// <summary>
        /// 指定されたパスを指す DocumentReader オブジェクトが束縛されて
        /// いるかどうかを判別します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        ///
        /// <returns>束縛されているかどうかを示す値</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsBound(string src) => _bounds.ContainsKey(src);

        /* ----------------------------------------------------------------- */
        ///
        /// IsBound
        ///
        /// <summary>
        /// 指定された DocumentReader オブジェクトが束縛されているか
        /// どうかを判別します。
        /// </summary>
        ///
        /// <param name="src">DocumentReader オブジェクト</param>
        ///
        /// <returns>束縛されているかどうかを示す値</returns>
        ///
        /// <remarks>
        /// このメソッドは、引数に指定されたオブジェクトと完全に等しい
        /// オブジェクトが束縛されているかどうかを判別します。そのため、
        /// このメソッドが false を返した場合でも、同じパスの別の
        /// DocumentReader オブジェクトが束縛されている可能性があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsBound(DocumentReader src) =>
            _bounds.TryGetValue(src.File.FullName, out DocumentReader dest) && src == dest;

        /* ----------------------------------------------------------------- */
        ///
        /// Release
        ///
        /// <summary>
        /// 束縛されている DocumentReader オブジェクトを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Release()
        {
            foreach (var kv in _bounds) kv.Value?.Dispose();
            _bounds.Clear();

            foreach (var kv in _images) kv.Value?.Dispose();
            _images.Clear();
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnReset
        ///
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReset()
        {
            _pages.Clear();
            _attach.Clear();
            Set(new Metadata());
            Set(new Encryption());
            Release();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// 保存処理を実行します。
        /// </summary>
        ///
        /// <remarks>
        /// DocumentWriterBase を継承したクラスで実装する必要があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSave(string path) { }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetRawReader
        ///
        /// <summary>
        /// PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PdfReader GetRawReader(Page src)
        {
            Debug.Assert(src != null);
            var path = src.File.FullName;

            if (src.File is PdfFile pdf)
            {
                if (!IsBound(path)) Bind(new DocumentReader(path, pdf.Password, IO));
                return _bounds[path].RawObject.TryCast<PdfReader>();
            }
            else if (src.File is ImageFile img)
            {
                if (!_images.ContainsKey(path))
                {
                    var obj = ReaderFactory.CreateFromImage(path, IO);
                    _images.Add(path, obj);
                }
                return _images[path];
            }
            else return null;
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly List<Page> _pages = new List<Page>();
        private readonly List<Attachment> _attach = new List<Attachment>();
        private readonly IDictionary<string, DocumentReader> _bounds = new Dictionary<string, DocumentReader>();
        private readonly IDictionary<string, PdfReader> _images = new Dictionary<string, PdfReader>();
        #endregion
    }
}
