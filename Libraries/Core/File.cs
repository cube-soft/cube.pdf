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
using Cube.FileSystem;
using System.Drawing;

namespace Cube.Pdf
{
    #region File

    /* --------------------------------------------------------------------- */
    ///
    /// File
    ///
    /// <summary>
    /// PDF や画像等のファイル情報を保持するための基底クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class File : Information
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
        /// <param name="src">ファイルまたはディレクトリのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public File(string src) : base(src) { }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ファイルまたはディレクトリのパス</param>
        /// <param name="refreshable">更新用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public File(string src, IRefreshable refreshable) : base(src, refreshable) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// ページ数に相当する値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// ファイルの解像度を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PointF Resolution { get; set; }

        #endregion
    }

    #endregion

    #region PdfFile

    /* --------------------------------------------------------------------- */
    ///
    /// PdfFile
    ///
    /// <summary>
    /// PDF ファイルの情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PdfFile : File
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PdfFile
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile(string src) : this(src, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfFile
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ファイルのパス</param>
        /// <param name="password">ファイルを開くためのパスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile(string src, string password) : base(src)
        {
            Initialize(password);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfFile
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ファイルのパス</param>
        /// <param name="password">ファイルを開くためのパスワード</param>
        /// <param name="refreshable">情報更新用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile(string src, string password, IRefreshable refreshable) :
            base(src, refreshable)
        {
            Initialize(password);
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
        /// PDF ファイルにパスワードによって暗号化されており、かつユーザ
        /// パスワードを用いてファイルを開いた場合 false に設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool FullAccess { get; set; } = true;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        ///
        /// <summary>
        /// 内部情報を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Initialize(string password)
        {
            Password   = password;
            Resolution = new PointF(72.0f, 72.0f);
        }

        #endregion
    }

    #endregion

    #region ImageFile

    /* --------------------------------------------------------------------- */
    ///
    /// ImageFile
    ///
    /// <summary>
    /// 画像ファイルの情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageFile : File
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ファイルまたはディレクトリのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string src) : base(src) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ファイルまたはディレクトリのパス</param>
        /// <param name="refreshable">更新用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string src, IRefreshable refreshable) : base(src, refreshable) { }

        #endregion
    }

    #endregion
}
