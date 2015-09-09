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
using Size = System.Drawing.Size;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Page
    /// 
    /// <summary>
    /// PDF のページを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Page : IPage
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Type
        /// 
        /// <summary>
        /// オブジェクトの種類を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageType Type
        {
            get { return PageType.Pdf; }
        }

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
        public string FileName { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        /// 
        /// <summary>
        /// リソースとなる PDF ファイルのパスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// PageNumber
        /// 
        /// <summary>
        /// 対象としているページのページ番号を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint PageNumber { get; set; } = 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        /// 
        /// <summary>
        /// 対象としているページのサイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Size Size { get; set; } = Size.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        /// 
        /// <summary>
        /// 該当ページを表示する際の回転角を取得または設定します (degree)。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Rotation { get; set; } = 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Power
        /// 
        /// <summary>
        /// 該当ページを表示する際の倍率を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public double Power { get; set; } = 1.0;

        /* ----------------------------------------------------------------- */
        ///
        /// ViewSize
        /// 
        /// <summary>
        /// 該当ページを表示する際のサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Size ViewSize
        {
            get
            {
                var degree = Rotation;
                if (degree < 0) degree += 360;
                else if (degree >= 360) degree -= 360;

                var radian = Math.PI * degree / 180.0;
                var sin = Math.Abs(Math.Sin(radian));
                var cos = Math.Abs(Math.Cos(radian));
                var width = Size.Width * cos + Size.Height * sin;
                var height = Size.Width * sin + Size.Height * cos;
                return new Size((int)(width * Power), (int)(height * Power));
            }
        }

        #endregion

        #region Implementations for IEquatable<IPage>

        /* ----------------------------------------------------------------- */
        ///
        /// Equals
        ///
        /// <summary>
        /// 引数に指定されたオブジェクトと等しいかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Equals(IPage obj)
        {
            var other = obj as Page;
            if (other == null) return false;
            return FileName == other.FileName && PageNumber == other.PageNumber;
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

            var other = obj as IPage;
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
