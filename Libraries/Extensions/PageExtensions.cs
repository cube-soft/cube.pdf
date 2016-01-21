/* ------------------------------------------------------------------------- */
///
/// PageExtensions.cs
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

namespace Cube.Pdf.Extensions
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageExtensions
    /// 
    /// <summary>
    /// IPage およびその実装クラスの拡張メソッドを定義するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PageExtensions
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ViewSize
        /// 
        /// <summary>
        /// ページオブジェクトを表示する際のサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Size ViewSize(this Page page)
        {
            var degree = page.Rotation;
            if (degree < 0) degree += 360;
            else if (degree >= 360) degree -= 360;

            var radian = Math.PI * degree / 180.0;
            var sin = Math.Abs(Math.Sin(radian));
            var cos = Math.Abs(Math.Cos(radian));
            var width  = page.Size.Width * cos + page.Size.Height * sin;
            var height = page.Size.Width * sin + page.Size.Height * cos;
            return new Size((int)width, (int)height);
        }
    }
}
