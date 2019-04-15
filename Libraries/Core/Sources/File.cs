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
using System;
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
    [Serializable]
    public abstract class File : Information
    {
        #region Constructors

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected File(string src, IO io) : base(src, io.GetController()) { }

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
    [Serializable]
    public class PdfFile : File
    {
        #region Constructors

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        internal PdfFile(string src, string password, IO io) : base(src, io)
        {
            Password   = password;
            Resolution = new PointF(Point, Point);
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
        public static float Point => 72.0F;

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
    [Serializable]
    public class ImageFile : File
    {
        #region Constructors

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected internal ImageFile(string src, IO io) : base(src, io) { }

        #endregion
    }

    #endregion
}
