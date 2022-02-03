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
using Cube.Mixin.String;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;

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

        /* ----------------------------------------------------------------- */
        ///
        /// GetFile
        ///
        /// <summary>
        /// Gets the PdfFile object from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfDocument object.</param>
        /// <param name="path">Path of the source PDF file.</param>
        /// <param name="password">Password of the source PDF file.</param>
        ///
        /// <returns>Page object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfFile GetFile(this PdfDocument src, string path, string password) => new(path)
        {
            Count      = src.GetNumberOfPages(),
            Password   = password,
            FullAccess = src.GetReader().IsOpenedWithFullPermission(),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// Gets the Page object from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfDocument object.</param>
        /// <param name="file">PDF file information.</param>
        /// <param name="pagenum">Page number</param>
        ///
        /// <returns>Page object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PageBase GetPage(this PdfDocument src, PdfFile file, int pagenum) => new()
        {
            File        = file,
            Number      = pagenum,
            Size        = GetPageSize(src, pagenum),
            Rotation    = new(src.GetPage(pagenum).GetRotation()),
            Resolution  = file.Resolution,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetMetadata
        ///
        /// <summary>
        /// Gets the Metadata object from the specified reader.
        /// </summary>
        ///
        /// <param name="src">PdfDocument object.</param>
        ///
        /// <returns>Metadata object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Metadata GetMetadata(this PdfDocument src)
        {
            var info = src.GetDocumentInfo();
            return new()
            {
                Version  = GetVersion(src),
                Author   = info.GetAuthor(),
                Title    = info.GetTitle(),
                Subject  = info.GetSubject(),
                Keywords = info.GetKeywords(),
                Creator  = info.GetCreator(),
                Producer = info.GetProducer(),
                Options  = GetViewerOption(src.GetCatalog()),
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryption
        ///
        /// <summary>
        /// Gets the Encryption object from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfDocument object.</param>
        /// <param name="file">PDF file information.</param>
        ///
        /// <returns>Encryption object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption GetEncryption(this PdfDocument src, PdfFile file)
        {
            if (file.FullAccess && !file.Password.HasValue()) return new();

            var user  = GetUserPassword(src, file);
            var value = (uint)src.GetReader().GetPermissions();
            typeof(ReaderExtension).LogDebug($"Permission:0x{value:X}", $"Mode:{src.GetReader().GetCryptoMode()}");

            return new()
            {
                Enabled          = true,
                Method           = GetEncryptionMethod(src),
                Permission       = new Permission(value),
                OwnerPassword    = file.FullAccess ? file.Password : string.Empty,
                UserPassword     = user,
                OpenWithPassword = user.HasValue(),
            };
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
        /// GetVersion
        ///
        /// <summary>
        /// Gets the PDF version of the specified document.
        /// </summary>
        ///
        /// <remarks>
        /// The method throws an exception if the specified PdfVersion
        /// object is not in major.minor notation.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static PdfVersion GetVersion(PdfDocument src)
        {
            var s = src.GetPdfVersion().ToPdfName().GetValue();
            if (s.Length == 3 && s[1] == '.') return new(s[0] - '0', s[2] - '0');
            throw new ArgumentException($"{s}:Unexpected PDF version");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPageSize
        ///
        /// <summary>
        /// Gets the page size of the specified page number.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SizeF GetPageSize(PdfDocument src, int pagenum)
        {
            var obj = src.GetPage(pagenum).GetPageSize();
            return new(obj.GetWidth(), obj.GetHeight());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewerOption
        ///
        /// <summary>
        /// Gets the viewer options from the specified object.
        /// </summary>
        ///
        /// <param name="src">PdfCatalog object.</param>
        ///
        /// <returns>ViewerOption value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static ViewerOption GetViewerOption(PdfCatalog src) => ViewerOptionFactory.Create(
            src.GetPageLayout()?.GetValue() ?? string.Empty,
            src.GetPageMode()?.GetValue()   ?? string.Empty
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryptionMethod
        ///
        /// <summary>
        /// Gets the encryption method from the specified reader.
        /// </summary>
        ///
        /// <param name="src">PdfDocument object.</param>
        ///
        /// <returns>Encryption method.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static EncryptionMethod GetEncryptionMethod(PdfDocument src) =>
            src.GetReader().GetCryptoMode() switch
        {
            EncryptionConstants.STANDARD_ENCRYPTION_40  => EncryptionMethod.Standard40,
            EncryptionConstants.STANDARD_ENCRYPTION_128 => EncryptionMethod.Standard128,
            EncryptionConstants.ENCRYPTION_AES_128      => EncryptionMethod.Aes128,
            EncryptionConstants.ENCRYPTION_AES_256      => EncryptionMethod.Aes256,
            _                                           => EncryptionMethod.Unknown,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserPassword
        ///
        /// <summary>
        /// Gets the user password from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">PdfDocument object.</param>
        /// <param name="file">PDF file information.</param>
        ///
        /// <returns>User password.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetUserPassword(PdfDocument src, PdfFile file)
        {
            if (file.FullAccess)
            {
                var bytes = src.GetReader().ComputeUserPassword();
                if (bytes?.Length > 0) return Encoding.UTF8.GetString(bytes);
            }
            return file.Password;
        }

        #endregion
    }
}
