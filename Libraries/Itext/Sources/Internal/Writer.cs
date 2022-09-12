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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cube.FileSystem;
using Cube.Text.Extensions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.Utils;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// Writer
    ///
    /// <summary>
    /// Represents the components to save the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class Writer : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Writer
        ///
        /// <summary>
        /// Initializes a new instance of the Writer class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="path">Path to save.</param>
        /// <param name="options">Save options.</param>
        /// <param name="metadata">PDF metadata.</param>
        /// <param name="encryption">PDF encryption settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Writer(string path, SaveOption options, Metadata metadata, Encryption encryption)
        {
            var op = new WriterProperties();
            _ = op.SetPdfVersion(GetVersion(metadata));
            _ = op.SetFullCompressionMode(metadata.Version.Minor >= 5);
            if (options.ShrinkResources) _ = op.UseSmartMode();
            SetEncryption(encryption, op);

            _document = new(new PdfWriter(Io.Create(path), op));
            SetMetadata(metadata, _document);

            var keep = options.KeepOutlines;
            _merger = new(_document, keep, keep);
            _ = _merger.SetCloseSourceDocuments(false);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified page to the writer.
        /// </summary>
        ///
        /// <param name="src">iText reader.</param>
        /// <param name="page">Page information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IDisposable src, Page page) => Add(src, new[] { page });

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified page to the writer.
        /// </summary>
        ///
        /// <param name="src">iText reader.</param>
        /// <param name="pages">Page collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IDisposable src, IEnumerable<Page> pages)
        {
            var obj = Reader.From(src);

            foreach (var page in pages)
            {
                var pp     = obj.GetPage(page.Number);
                var cmp    = pp.GetRotation();
                var degree = (page.Rotation + page.Delta).Degree;
                if (degree != cmp) _ = pp.SetRotation(degree);
            }
            _ = _merger.Merge(obj, pages.Select(e => e.Number).ToArray());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified collection as the embedded files.
        /// </summary>
        ///
        /// <param name="src">Embedded files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Attachment> src)
        {
            foreach (var e in src) _document.AddFileAttachment(e.Name,
                PdfFileSpec.CreateEmbeddedFileSpec(_document,
                    e.Data, string.Empty, e.Name,
                    null, null, null
                )
            );
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the DocumentReader
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) => _merger?.Close();

        /* ----------------------------------------------------------------- */
        ///
        /// SetMetadata
        ///
        /// <summary>
        /// Sets the specified PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetMetadata(Metadata src, PdfDocument dest)
        {
            _ = dest.GetDocumentInfo()
                    .SetAuthor(src.Author)
                    .SetTitle(src.Title)
                    .SetSubject(src.Subject)
                    .SetKeywords(src.Keywords)
                    .SetCreator(src.Creator);

            var pl = src.Options.ToPageLayout();
            if (pl != ViewerOption.None) _ = dest.GetCatalog().SetPageLayout(new(pl.ToName()));

            var pm = src.Options.ToPageMode();
            if (pm != ViewerOption.None) _ = dest.GetCatalog().SetPageMode(new(pm.ToName()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetEncryption
        ///
        /// <summary>
        /// Sets the specified encryption information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetEncryption(Encryption src, WriterProperties dest)
        {
            if (src == null || !src.Enabled || !src.OwnerPassword.HasValue()) return;

            var owner = src.OwnerPassword;
            var user  = !src.OpenWithPassword ? string.Empty :
                        src.UserPassword.HasValue() ? src.UserPassword :
                        owner;

            _ = dest.SetStandardEncryption(
                Encoding.UTF8.GetBytes(user),
                Encoding.UTF8.GetBytes(owner),
                (int)src.Permission.Value,
                GetEncryptionMethod(src.Method)
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetVersion
        ///
        /// <summary>
        /// Gets the PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private iText.Kernel.Pdf.PdfVersion GetVersion(Metadata src)
        {
            if (src.Version.Major > 1) return iText.Kernel.Pdf.PdfVersion.PDF_2_0;
            if (src.Version.Major < 1) return iText.Kernel.Pdf.PdfVersion.PDF_1_7;
            return src.Version.Minor switch
            {
                0 => iText.Kernel.Pdf.PdfVersion.PDF_1_0,
                1 => iText.Kernel.Pdf.PdfVersion.PDF_1_1,
                2 => iText.Kernel.Pdf.PdfVersion.PDF_1_2,
                3 => iText.Kernel.Pdf.PdfVersion.PDF_1_3,
                4 => iText.Kernel.Pdf.PdfVersion.PDF_1_4,
                5 => iText.Kernel.Pdf.PdfVersion.PDF_1_5,
                6 => iText.Kernel.Pdf.PdfVersion.PDF_1_6,
                7 => iText.Kernel.Pdf.PdfVersion.PDF_1_7,
                _ => iText.Kernel.Pdf.PdfVersion.PDF_1_7,
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryptionMethod
        ///
        /// <summary>
        /// Gets the value corresponding to the specified method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetEncryptionMethod(EncryptionMethod src) => src switch
        {
            EncryptionMethod.Standard40  => EncryptionConstants.STANDARD_ENCRYPTION_40,
            EncryptionMethod.Standard128 => EncryptionConstants.STANDARD_ENCRYPTION_128,
            EncryptionMethod.Aes128      => EncryptionConstants.ENCRYPTION_AES_128,
            EncryptionMethod.Aes256      => EncryptionConstants.ENCRYPTION_AES_256,
            _                            => EncryptionConstants.STANDARD_ENCRYPTION_40,
        };

        #endregion

        #region Fields
        private readonly PdfDocument _document;
        private readonly PdfMerger _merger;
        #endregion
    }
}
