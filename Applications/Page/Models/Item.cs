/* ------------------------------------------------------------------------- */
///
/// MainForm.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Drawing;
using IoEx = System.IO;

namespace Cube.Pdf.App.Page
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.App.Page.Item
    ///
    /// <summary>
    /// 各項目の情報を保持するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Item : IEquatable<Item>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Item
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Item() { }

        /* ----------------------------------------------------------------- */
        ///
        /// Item
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Item(PageType type, string path)
        {
            Type = type;

            var info = new IoEx.FileInfo(path);
            Path = info.FullName;
            FileSize = info.Length;
            LastWriteTime = info.LastWriteTime;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Type
        /// 
        /// <summary>
        /// 項目の種類を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageType Type { get; set; } = PageType.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        /// 
        /// <summary>
        /// オリジナルのファイルへのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        /// 
        /// <summary>
        /// Type に応じたオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object Value { get; set; } = null;

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        /// 
        /// <summary>
        /// 表示サイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Size ViewSize { get; set; } = Size.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// FileSize
        /// 
        /// <summary>
        /// ファイルサイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long FileSize { get; set; } = 0;

        /* ----------------------------------------------------------------- */
        ///
        /// PageCount
        /// 
        /// <summary>
        /// ファイルに含まれるページ数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int PageCount { get; set; } = 0;

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        /// 
        /// <summary>
        /// 現在のファイルに最後に書き込みが行われた時刻を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime { get; set; } = DateTime.MinValue;

        #endregion

        #region IEquatable<Item> methods

        /* ----------------------------------------------------------------- */
        ///
        /// Equals
        ///
        /// <summary>
        /// 引数に指定されたオブジェクトと等しいかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Equals(Item other)
        {
            return Path == other.Path;
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

            var other = obj as Item;
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
