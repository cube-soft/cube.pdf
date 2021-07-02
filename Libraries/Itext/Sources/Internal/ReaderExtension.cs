/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Cube.Logging;
using Cube.Mixin.String;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReaderExtension
    ///
    /// <summary>
    /// Provides extended methods of the PdfReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ReaderExtension
    {
        #region Methods

        #region Get

        /* ----------------------------------------------------------------- */
        ///
        /// GetFile
        ///
        /// <summary>
        /// Gets the PdfFile object from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="file">Path of the source PDF file.</param>
        /// <param name="password">Password of the source PDF file.</param>
        ///
        /// <returns>Page object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfFile GetFile(this PdfReader src, string file, string password) => new(file, password)
        {
            Count      = src.NumberOfPages,
            FullAccess = src.IsOpenedWithFullPermissions
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// Gets the Page object from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="file">PDF file information.</param>
        /// <param name="pagenum">Page number</param>
        ///
        /// <returns>Page object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Page GetPage(this PdfReader src, PdfFile file, int pagenum) => new(
            file,                                    // File
            pagenum,                                 // Number
            GetPageSize(src, pagenum),               // Size
            new Angle(src.GetPageRotation(pagenum)), // Rotation
            file.Resolution                          // Resolution
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetMetadata
        ///
        /// <summary>
        /// Gets the Metadata object from the specified reader.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        ///
        /// <returns>Metadata object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Metadata GetMetadata(this PdfReader src) => new()
        {
            Version  = new PdfVersion(1, src.PdfVersion - '0'),
            Author   = src.Info.TryGetValue("Author",   out var s0) ? s0 : string.Empty,
            Title    = src.Info.TryGetValue("Title",    out var s1) ? s1 : string.Empty,
            Subject  = src.Info.TryGetValue("Subject",  out var s2) ? s2 : string.Empty,
            Keywords = src.Info.TryGetValue("Keywords", out var s3) ? s3 : string.Empty,
            Creator  = src.Info.TryGetValue("Creator",  out var s4) ? s4 : string.Empty,
            Producer = src.Info.TryGetValue("Producer", out var s5) ? s5 : string.Empty,
            Options  = ViewerOptionFactory.Create(src.SimpleViewerPreferences),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryption
        ///
        /// <summary>
        /// Gets the Encryption object from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="file">PDF file information.</param>
        ///
        /// <returns>Encryption object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption GetEncryption(this PdfReader src, PdfFile file)
        {
            if (file.FullAccess && !file.Password.HasValue()) return new();

            var user  = src.GetUserPassword(file);
            var value = (uint)src.Permissions;
            src.GetType().LogDebug($"Permission:0x{value:X}", $"Mode:{src.GetCryptoMode()}");

            return new()
            {
                Enabled          = true,
                Method           = src.GetEncryptionMethod(),
                Permission       = new Permission(value),
                OwnerPassword    = file.FullAccess ? file.Password : string.Empty,
                UserPassword     = user,
                OpenWithPassword = user.HasValue(),
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryptionMethod
        ///
        /// <summary>
        /// Gets the encryption method from the specified reader.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        ///
        /// <returns>Encryption method.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static EncryptionMethod GetEncryptionMethod(this PdfReader src) => src.GetCryptoMode() switch
        {
            PdfWriter.STANDARD_ENCRYPTION_40  => EncryptionMethod.Standard40,
            PdfWriter.STANDARD_ENCRYPTION_128 => EncryptionMethod.Standard128,
            PdfWriter.ENCRYPTION_AES_128      => EncryptionMethod.Aes128,
            PdfWriter.ENCRYPTION_AES_256      => EncryptionMethod.Aes256,
            _                                 => EncryptionMethod.Unknown,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserPassword
        ///
        /// <summary>
        /// Gets the user password from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="file">PDF file information.</param>
        ///
        /// <returns>User password.</returns>
        ///
        /// <remarks>
        /// If the encryption method is AES256, the analysis of the user
        /// password will fail, so it is excluded.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetUserPassword(this PdfReader src, PdfFile file)
        {
            if (file.FullAccess)
            {
                var method = src.GetEncryptionMethod();
                if (method == EncryptionMethod.Aes256) return string.Empty; // see remarks

                var bytes = src.ComputeUserPassword();
                if (bytes?.Length > 0) return Encoding.UTF8.GetString(bytes);
            }
            return file.Password;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetBookmarks
        ///
        /// <summary>
        /// Gets the collection of bookmarks embedded in the specified
        /// PDF document.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="pagenum">Page number.</param>
        /// <param name="delta">
        /// Difference in page numbers between PDF documents.
        /// </param>
        /// <param name="dest">Container for the result.</param>
        ///
        /// <remarks>
        /// Invokes processing on the bookmark information retrieved from
        /// the PdfReader object after shifting the page number by delta.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void GetBookmarks(this PdfReader src, int pagenum, int delta, Bookmark dest)
        {
            var cmp = $"^{pagenum} (XYZ|Fit|FitH|FitBH)";
            var bookmarks = SimpleBookmark.GetBookmark(src);
            if (bookmarks == null) return;

            SimpleBookmark.ShiftPageNumbers(bookmarks, delta, null);
            foreach (var b in bookmarks)
            {
                var found = b.TryGetValue("Page", out object obj);
                if (found && Regex.IsMatch(obj.ToString(), cmp)) dest.Add(b);
            }
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Updates the rotation information of the specified PdfReader
        /// object according to the specified Page object.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="page">Page object.</param>
        ///
        /// <remarks>
        /// When rotating a PDF page, it is easiest to update the content of
        /// the PdfReader object and then copy it with a PdfCopy object, etc.,
        /// so this method is used.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Rotate(this PdfReader src, Page page)
        {
            var rot = (page.Rotation + page.Delta).Degree;
            var cmp = src.GetPageRotation(page.Number);
            var dic = src.GetPageN(page.Number);
            if (rot != cmp) dic.Put(PdfName.ROTATE, new PdfNumber(rot));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts the specified exception object to the corresponding
        /// object.
        /// </summary>
        ///
        /// <param name="src">Exception object.</param>
        ///
        /// <returns>Converted exception object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Exception Convert(this Exception src) =>
            src is BadPasswordException obj ? new EncryptionException(obj.Message, obj) : src;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageSize
        ///
        /// <summary>
        /// Gets the page size of the specified page number.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SizeF GetPageSize(PdfReader src, int pagenum)
        {
            var obj = src.GetPageSize(pagenum);
            return new(obj.Width, obj.Height);
        }

        #endregion
    }
}
