/* ------------------------------------------------------------------------- */
///
/// Metadata.cs
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

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Metadata
    /// 
    /// <summary>
    /// PDF ファイルのメタ情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Metadata
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        /// 
        /// <summary>
        /// PDF ファイルのバージョンを取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// 現時点で有効な PDF バージョンは 1.0, 1.1, 1.2, 1.3, 1.4, 1.5,
        /// 1.6, 1.7, 1.7 Extension Level 3, 1.7 Extension Level 5 の
        /// 10 種類です。Adobe Extension Level の値は Build プロパティで
        /// 保持する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Version Version { get; set; } = new Version(0, 0);

        /* ----------------------------------------------------------------- */
        ///
        /// Author
        ///
        /// <summary>
        /// 著者を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Author { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// タイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Subtitle
        ///
        /// <summary>
        /// サブタイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Subtitle { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Keywords
        /// 
        /// <summary>
        /// キーワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Keywords { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Creator
        ///
        /// <summary>
        /// PDF の作成、編集を行うアプリケーション名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Creator { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Producer
        ///
        /// <summary>
        /// PDF の作成・編集を行う際に使用したプリンタドライバ、ライブラリ等
        /// の名前を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Producer { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// ViewMode
        ///
        /// <summary>
        /// PDF ファイルの表示方法を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewMode ViewMode { get; set; } = ViewMode.None;

        /* ----------------------------------------------------------------- */
        ///
        /// ViewLayout
        ///
        /// <summary>
        /// PDF ファイルの各ページを表示する際のレイアウトを取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewLayout ViewLayout { get; set; } = ViewLayout.SinglePage;

        /* ----------------------------------------------------------------- */
        ///
        /// ViewPreferences
        ///
        /// <summary>
        /// PDF ファイルの表示方法に関する値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ViewPreferences
        {
            get
            {
                var mode   = (int)ViewMode;
                var layout = (int)ViewLayout;
                return mode & layout;
            }

            set
            {
                var layout = value & 0x003f;
                if (Enum.IsDefined(typeof(ViewLayout), layout)) ViewLayout = (ViewLayout)layout;

                var mode = value & 0x0fc0;
                if (Enum.IsDefined(typeof(ViewMode), mode)) ViewMode = (ViewMode)mode;
            }
        }

        #endregion
    }
}
