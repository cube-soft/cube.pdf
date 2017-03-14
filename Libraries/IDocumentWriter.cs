/* ------------------------------------------------------------------------- */
///
/// IDocumentWriter.cs
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

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// IDocumentWriter
    /// 
    /// <summary>
    /// PDF ファイルを作成、保存するためのインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IDocumentWriter : IDisposable
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        /// 
        /// <summary>
        /// 初期状態にリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 指定されたパスに PDF ファイルを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Save(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルのメタデータを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Metadata Metadata { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        /// 
        /// <summary>
        /// 暗号化に関する情報をを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Encryption Encryption { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルの各ページ情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ICollection<Page> Pages { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        /// 
        /// <summary>
        /// 添付ファイル一覧の情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ICollection<Attachment> Attachments { get; }
    }
}
