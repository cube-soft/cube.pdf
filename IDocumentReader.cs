/* ------------------------------------------------------------------------- */
///
/// IDocumentReader.cs
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
using System.Collections.Generic;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.IDocumentReader
    /// 
    /// <summary>
    /// PDF ファイルの読み込むためのインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IDocumentReader
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// リソースとなる PDF ファイルのファイル名 (パス) を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string FileName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルに関するメタデータを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Metadata Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルに関する暗号化情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Encryption Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionStatus
        /// 
        /// <summary>
        /// 暗号化されている PDF ファイルへのアクセス状態を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        EncryptionStatus EncryptionStatus { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルの各ページ情報へアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ICollection<IPage> Pages { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルを開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Open(string path, string password);

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        /// 
        /// <summary>
        /// 現在、開いている PDF ファイルを閉じます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Close();

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        /// 
        /// <summary>
        /// 指定されたページ番号に対応するページ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Page GetPage(int pagenum);

        #endregion
    }
}
