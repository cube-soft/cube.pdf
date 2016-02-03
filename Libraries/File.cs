/* ------------------------------------------------------------------------- */
///
/// File.cs
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
using System.Drawing;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// File
    /// 
    /// <summary>
    /// PDF のファイル情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class File : FileBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File(string path)
            : this(path, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File(string path, string password)
            : this(path, password, IconSize.Small) { }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public File(string path, string password, IconSize size)
            : base(path, size)
        {
            Password = password;
            Resolution = new Point(72, 72);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        /// 
        /// <summary>
        /// オーナパスワードまたはユーザパスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// FullAccess
        /// 
        /// <summary>
        /// ファイルの全ての内容にアクセス可能かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// このプロパティは、PDF ファイルにパスワードによって暗号化されて
        /// おり、かつユーザパスワードを用いてファイルを開いた場合 false に
        /// 設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool FullAccess { get; set; } = false;

        #endregion
    }
}
