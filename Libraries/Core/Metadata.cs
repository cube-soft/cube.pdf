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
using Cube.Generics;
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
    public class Metadata : ObservableProperty
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
        public Version Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Author
        ///
        /// <summary>
        /// 著者を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// タイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Subtitle
        ///
        /// <summary>
        /// サブタイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Subtitle
        {
            get => _subtitle;
            set => SetProperty(ref _subtitle, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Keywords
        ///
        /// <summary>
        /// キーワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Keywords
        {
            get => _keywords;
            set => SetProperty(ref _keywords, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Creator
        ///
        /// <summary>
        /// PDF の作成、編集を行うアプリケーション名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Creator
        {
            get => _creator;
            set => SetProperty(ref _creator, value);
        }

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
        public string Producer
        {
            get => _producer;
            set => SetProperty(ref _producer, value);
        }

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
        public ViewLayout ViewLayout
        {
            get => _layout;
            set => SetProperty(ref _layout, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ViewOption
        ///
        /// <summary>
        /// PDF ファイルの表示方法を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ViewOption ViewOption
        {
            get => _option;
            set => SetProperty(ref _option, value);
        }

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
            get => ViewOption.TryCast<int>() & ViewLayout.TryCast<int>();
            set
            {
                var s0 = SetProperty(ref _layout, ToLayout(value), nameof(ViewLayout));
                var s1 = SetProperty(ref _option, ToMode(value), nameof(ViewOption));
                if (s0 || s1) RaisePropertyChanged(nameof(ViewPreferences));
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ToLayout
        ///
        /// <summary>
        /// ViewLayout に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ViewLayout ToLayout(int src) => (src & 0x003f).TryCast(ViewLayout.SinglePage);

        /* ----------------------------------------------------------------- */
        ///
        /// ToMode
        ///
        /// <summary>
        /// ViewMode に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ViewOption ToMode(int src) => (src & 0x0fc0).TryCast(ViewOption.None);

        #endregion

        #region Fields
        private Version _version = new Version(1, 2);
        private string _author = string.Empty;
        private string _title = string.Empty;
        private string _subtitle = string.Empty;
        private string _keywords = string.Empty;
        private string _creator = string.Empty;
        private string _producer = string.Empty;
        private ViewOption _option = ViewOption.None;
        private ViewLayout _layout = ViewLayout.SinglePage;
        #endregion
    }
}
