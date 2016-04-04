/* ------------------------------------------------------------------------- */
///
/// DocumentResource.cs
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
using System;
using System.Collections.Generic;
using Cube.Pdf.Editing;
using IoEx = System.IO;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentResource
    /// 
    /// <summary>
    /// テストで DocumentReader を使用する際の補助クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    class DocumentResource : FileResource, IDisposable
    {
        #region Constructors and destructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentResource
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentResource() : base() { }

        /* ----------------------------------------------------------------- */
        ///
        /// ~DocumentResource
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~DocumentResource()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// DocumentReader オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentReader Create(string filename)
        {
            return Create(filename, string.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// DocumentReader オブジェクトを生成します。
        /// </summary>
        /// 
        /// <remarks>
        /// 内部に DocumentReader のキャッシュを保持し、同じファイルに
        /// 対しては 1 回だけ読み込む事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected DocumentReader Create(string filename, string password)
        {
            if (!_cache.ContainsKey(filename))
            {
                var src = IoEx.Path.Combine(Examples, filename);
                var reader = new DocumentReader(src, password);
                _cache.Add(filename, reader);
            }
            return _cache[filename];
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Virtual methods

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
            if (_disposed) return;
            _disposed = true;
            if (disposing)
            {
                foreach (var item in _cache) item.Value.Dispose();
                _cache.Clear();
            }
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        private Dictionary<string, DocumentReader> _cache = new Dictionary<string, DocumentReader>();
        #endregion
    }
}
