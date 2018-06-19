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
namespace Cube.Pdf.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// IDocumentWriterExtension
    ///
    /// <summary>
    /// IDocumentWriter の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class IDocumentWriterExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ページを追加します。
        /// </summary>
        ///
        /// <param name="src">IDocumentWriter オブジェクト</param>
        /// <param name="page">ページ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Add(this IDocumentWriter src, Page page) =>
            src.Add(new[] { page });

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// IDocumentReader オブジェクト中の全てのページを追加します。
        /// </summary>
        ///
        /// <param name="src">IDocumentWriter オブジェクト</param>
        /// <param name="reader">IDocumentReader オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Add(this IDocumentWriter src, IDocumentReader reader) =>
            src.Add(reader.Pages, reader);

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// ファイルを添付します。
        /// </summary>
        ///
        /// <param name="src">IDocumentWriter オブジェクト</param>
        /// <param name="file">添付ファイル</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Attach(this IDocumentWriter src, Attachment file) =>
            src.Attach(new[] { file });

        #endregion
    }
}
