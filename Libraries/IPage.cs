/* ------------------------------------------------------------------------- */
///
/// IPage.cs
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
using System.Drawing;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// IPage
    /// 
    /// <summary>
    /// PDF のページを表すインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IPage : IEquatable<IPage>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Type
        /// 
        /// <summary>
        /// オブジェクトの種類を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        PageType Type { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FilePath
        /// 
        /// <summary>
        /// オブジェクト元となるファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string FilePath { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        /// 
        /// <summary>
        /// オブジェクトのオリジナルサイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        Size Size { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        /// 
        /// <summary>
        /// オブジェクトを表示する際の回転角を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// 値は度単位 (degree) で設定して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        int Rotation { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Power
        /// 
        /// <summary>
        /// 表示倍率を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        double Power { get; set; }
    }
}
