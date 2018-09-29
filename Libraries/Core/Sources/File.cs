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
    /// Represents information of PDF and image files.
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
        /// Initializes a new instance of the File class with the specified
        /// file path.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public File(string src) : base(src) { }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Initializes a new instance of the File class with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="refreshable">
        /// Object to refresh file information.
        /// </param>
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
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// Gets the resolution of the PDF or image object.
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
    /// Represents information of a PDF file.
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
        /// Initializes a new instance of the PdfFile class with the
        /// specified file path.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PdfFile(string src) : this(src, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PdfFile
        ///
        /// <summary>
        /// Initializes a new instance of the PdfFile class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password to open the PDF file.</param>
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
        /// Initializes a new instance of the PdfFile class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password to open the PDF file.</param>
        /// <param name="refreshable">
        /// Object to refresh file information.
        /// </param>
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
        /// Point
        ///
        /// <summary>
        /// Gets the DPI value of the "Point" unit.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int Point => 72;

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the owner or user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// FullAccess
        ///
        /// <summary>
        /// Gets or sets the value indicating whether you can access
        /// all contents of the PDF document.
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
        /// Initializes properties of the PdfFile class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Initialize(string password)
        {
            Password   = password;
            Resolution = new PointF(Point, Point);
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
    /// Represents information of an image file.
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
        /// Initializes a new instance of the ImageFile class with the
        /// specified file path.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string src) : base(src) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageFile
        ///
        /// <summary>
        /// Initializes a new instance of the ImageFile class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the image file.</param>
        /// <param name="refreshable">
        /// Object to refresh file information.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageFile(string src, IRefreshable refreshable) : base(src, refreshable) { }

        #endregion
    }

    #endregion
}
