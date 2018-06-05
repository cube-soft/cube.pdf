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
using System;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewLayout
    ///
    /// <summary>
    /// PDF ファイルを表示する際の各ページのレイアウトを定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum ViewLayout
    {
        /// <summary>単一ページ</summary>
        SinglePage = 0x0001,
        /// <summary>連続ページ</summary>
        OneColumn = 0x0002,
        /// <summary>連続見開きページ（左綴じ）</summary>
        TwoColumnLeft = 0x0004,
        /// <summary>連続見開きページ（右綴じ）</summary>
        TwoColumnRight = 0x0008,
        /// <summary>見開きページ（左綴じ）</summary>
        TwoPageLeft = 0x0010,
        /// <summary>見開きページ（右綴じ）</summary>
        TwoPageRight = 0x0020
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ViewOption
    ///
    /// <summary>
    /// PDF ファイルの表示方法を定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum ViewOption
    {
        /// <summary>なし</summary>
        None = 0x0040,
        /// <summary>アウトライン</summary>
        Outline = 0x0080,
        /// <summary>サムネイル</summary>
        Thumbnail = 0x0100,
        /// <summary>全画面表示</summary>
        FullScreen = 0x0200,
        /// <summary>付加的なコンテンツ</summary>
        OptionalContent = 0x0400,
        /// <summary>添付オブジェクト</summary>
        Attachment = 0x0800
    }
}
