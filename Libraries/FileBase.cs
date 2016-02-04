/* ------------------------------------------------------------------------- */
///
/// FileBase.cs
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
    /// FileBase
    /// 
    /// <summary>
    /// ファイル情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileBase : Cube.FileSystem.FileInfo, IEquatable<FileBase>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileBase
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected FileBase(string path, IconSize size) : base(path, size) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PageCount
        /// 
        /// <summary>
        /// ページ数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int PageCount { get; set; } = 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        /// 
        /// <summary>
        /// ファイルの解像度を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Point Resolution { get; set; } = Point.Empty;

        #endregion

        #region IEquatable<FileBase> methods

        /* ----------------------------------------------------------------- */
        ///
        /// Equals
        ///
        /// <summary>
        /// 引数に指定されたオブジェクトと等しいかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Equals(FileBase other)
        {
            return FullName == other.FullName;
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
