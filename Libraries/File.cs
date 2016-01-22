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
        /// IsRestricted
        /// 
        /// <summary>
        /// ファイル内容へのアクセスが制限されているかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// このプロパティは、PDF ファイルにパスワードによる暗号化が
        /// 施されており、かつユーザパスワードを用いてファイルを開いた
        /// 場合に true になります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsRestricted { get; set; } = false;

        #endregion
    }
}
