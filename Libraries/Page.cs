/* ------------------------------------------------------------------------- */
///
/// Page.cs
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
    /// Page
    /// 
    /// <summary>
    /// ページを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Page : IEquatable<Page>
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// ページオブジェクトが属するファイルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public FileBase File { get; set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Number
        /// 
        /// <summary>
        /// ページ番号を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Number { get; set; } = -1;

        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        /// 
        /// <summary>
        /// ページオブジェクト表示時の回転角を度 (degree) 単位で取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Rotation { get; set; } = 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        /// 
        /// <summary>
        /// 水平方法および垂直方向の解像度（1 インチあたりのピクセル数）を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Point Resolution { get; set; } = Point.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        /// 
        /// <summary>
        /// ページオブジェクトのサイズ（ピクセル単位）を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Size Size { get; set; } = Size.Empty;

        #endregion

        #region IEquatable<PageBase> methods

        /* ----------------------------------------------------------------- */
        ///
        /// Equals
        ///
        /// <summary>
        /// 引数に指定されたオブジェクトと等しいかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Equals(Page other)
        {
            return File == other.File && Number == other.Number;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Equals
        ///
        /// <summary>
        /// 引数に指定されたオブジェクトと等しいかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null)) return false;
            if (object.ReferenceEquals(this, obj)) return true;

            var other = obj as Page;
            if (other == null) return false;

            return Equals(other);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetHashCode
        ///
        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
