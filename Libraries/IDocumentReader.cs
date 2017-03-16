/* ------------------------------------------------------------------------- */
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
    /// IDocumentReader
    /// 
    /// <summary>
    /// PDF ファイルの読み込むためのインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IDocumentReader : IDisposable
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileBase
        /// 
        /// <summary>
        /// ファイルオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        FileBase File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        /// 
        /// <summary>
        /// PDF ファイルに関するメタ情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Metadata Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        /// 
        /// <summary>
        /// PDF ファイルに関する暗号化情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Encryption Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        /// 
        /// <summary>
        /// PDF ファイルの各ページ情報へアクセスするための反復子を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerable<Page> Pages { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments
        /// 
        /// <summary>
        /// 添付ファイルの情報へアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerable<IAttachment> Attachments { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsOpen
        /// 
        /// <summary>
        /// ファイルが正常に開いているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        bool IsOpen { get; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordRequired
        /// 
        /// <summary>
        /// パスワードが要求された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        event QueryEventHandler<string, string> PasswordRequired;

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
        /// <param name="path">PDF ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        void Open(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// PDF ファイルをパスワードを入力して開きます。
        /// </summary>
        /// 
        /// <param name="path">PDF ファイルのパス</param>
        /// <param name="password">
        /// オーナパスワードまたはユーザパスワード
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        void Open(string path, string password);

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        /// 
        /// <summary>
        /// 指定されたページ番号に対応するページ情報を取得します。
        /// </summary>
        /// 
        /// <param name="pagenum">ページ番号</param>
        ///
        /* ----------------------------------------------------------------- */
        Page GetPage(int pagenum);

        #endregion
    }
}
