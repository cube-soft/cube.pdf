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
using Cube.Generics;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

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
        public static Page GetPage(this PdfReader src, PdfFile file, int pagenum) => new Page(
            file,                                    // File
            pagenum,                                 // Number
            src.GetPageSize(pagenum).ToSize(),       // Size
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
        public static Metadata GetMetadata(this PdfReader src) => new Metadata
        {
            Version  = new PdfVersion(1, src.PdfVersion - '0'),
            Author   = src.Info.TryGetValue("Author",   out var s0) ? s0 : string.Empty,
            Title    = src.Info.TryGetValue("Title",    out var s1) ? s1 : string.Empty,
            Subject  = src.Info.TryGetValue("Subject",  out var s2) ? s2 : string.Empty,
            Keywords = src.Info.TryGetValue("Keywords", out var s3) ? s3 : string.Empty,
            Creator  = src.Info.TryGetValue("Creator",  out var s4) ? s4 : string.Empty,
            Producer = src.Info.TryGetValue("Producer", out var s5) ? s5 : string.Empty,
            Options  = ViewerOptionsFactory.Create(src.SimpleViewerPreferences),
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
            if (file.FullAccess && string.IsNullOrEmpty(file.Password)) return new Encryption();

            var password = src.GetUserPassword(file);
            return new Encryption
            {
                Enabled          = true,
                Method           = src.GetEncryptionMethod(),
                Permission       = new Permission(src.Permissions),
                OwnerPassword    = file.FullAccess ? file.Password : string.Empty,
                UserPassword     = password,
                OpenWithPassword = password.HasValue(),
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
        public static EncryptionMethod GetEncryptionMethod(this PdfReader src)
        {
            var dic = new Dictionary<int, EncryptionMethod>
            {
                { PdfWriter.STANDARD_ENCRYPTION_40,  EncryptionMethod.Standard40 },
                { PdfWriter.STANDARD_ENCRYPTION_128, EncryptionMethod.Standard128 },
                { PdfWriter.ENCRYPTION_AES_128,      EncryptionMethod.Aes128 },
                { PdfWriter.ENCRYPTION_AES_256,      EncryptionMethod.Aes256 },
            };

            return dic.TryGetValue(src.GetCryptoMode(), out var dest) ? dest : EncryptionMethod.Unknown;
        }

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
        /// 暗号化方式が AES256 の場合、ユーザパスワードの解析に
        /// 失敗するので除外しています。AES256 の場合の解析方法を要検討。
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
        /// GetContentParser
        ///
        /// <summary>
        /// Gets the PdfReaderContentParser object from the specified
        /// reader.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        ///
        /// <returns>PdfReaderContentParser object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReaderContentParser GetContentParser(this PdfReader src) =>
            new PdfReaderContentParser(src);

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
        /// PdfReader オブジェクトから取得されたしおり情報に対して、
        /// ページ番号を delta だけずらした後に処理を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void GetBookmarks(this PdfReader src, int pagenum, int delta,
            IList<Dictionary<string, object>> dest)
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
        /// 更新します。
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="page">Page object.</param>
        ///
        /// <remarks>
        /// PDF ページを回転させる場合、いったん PdfReader オブジェクトの
        /// 内容を更新した後に PdfCopy オブジェクト等でコピーする方法が
        /// もっとも容易に実現できるため、この方法を採用しています。
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

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ToSize
        ///
        /// <summary>
        /// Converts from iTextSharp.text.Rectable to SizeF.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SizeF ToSize(this iTextSharp.text.Rectangle src) =>
            new SizeF(src.Width, src.Height);

        #endregion
    }
}
