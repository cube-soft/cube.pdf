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
using System.Drawing.Imaging;
using System.Text;
using Cube.FileSystem;
using Cube.Mixin.String;
using iText.IO.Image;
using iText.IO.Source;
using iText.Kernel.Crypto;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// Reader
    ///
    /// <summary>
    /// Provides factory and other static methods about PdfReader.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Reader
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// From
        ///
        /// <summary>
        /// Converts the specified object to the PdfReader object.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfDocument From(object src) => src as PdfDocument;

        /* ----------------------------------------------------------------- */
        ///
        /// FromPdf
        ///
        /// <summary>
        /// Creates a new instance of the PdfReader class with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="src">Source File object.</param>
        /// <param name="options">Open options.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfDocument From(File src, OpenOption options) =>
            src is PdfFile   f0 ? FromPdf(f0.FullName, new(null, f0.Password), options) :
            src is ImageFile f1 ? FromImage(f1.FullName) :
            default;

        /* ----------------------------------------------------------------- */
        ///
        /// FromPdf
        ///
        /// <summary>
        /// Creates a new instance of the PdfReader class.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfDocument FromPdf(string src) => new(new PdfReader(src));

        /* ----------------------------------------------------------------- */
        ///
        /// FromPdf
        ///
        /// <summary>
        /// Creates a new instance of the PdfReader class.
        /// </summary>
        ///
        /// <param name="src">Path of the PDF file.</param>
        /// <param name="password">Password string or query.</param>
        /// <param name="options">Open options.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfDocument FromPdf(string src, Password password, OpenOption options)
        {
            while (true)
            {
                try
                {
                    using var ss = Io.Open(src);
                    var dest = new PdfReader(ss, GetOptions(password.Value))
                        .SetMemorySavingMode(options.SaveMemory);
                    if (options.FullAccess && !dest.IsOpenedWithFullPermission())
                    {
                        dest.Close();
                        throw new BadPasswordException("PdfReader is not opened with owner password");
                    }
                    return new(dest);
                }
                catch (BadPasswordException)
                {
                    var msg = password.Source.Request(src);
                    if (!msg.Cancel) password.Value = msg.Value;
                    else throw new OperationCanceledException();
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromImage
        ///
        /// <summary>
        /// Creates a new instance of the PdfReader class from the
        /// specified image.
        /// </summary>
        ///
        /// <param name="src">Path of the image.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfDocument FromImage(string src)
        {
            using var ms = new System.IO.MemoryStream();
            using var ss = Io.Open(src);
            using var image = Image.FromStream(ss);

            var doc  = new PdfDocument(new PdfWriter(ms));
            var guid = image.FrameDimensionsList[0];
            var dim  = new FrameDimension(guid);

            for (var i = 0; i < image.GetFrameCount(dim); ++i)
            {
                _ = image.SelectActiveFrame(dim, i);

                var scale = PdfFile.Point / image.HorizontalResolution;
                var w = image.Width * scale;
                var h = image.Height * scale;
                var page = doc.AddNewPage(new PageSize(w, h));

                _ = new PdfCanvas(page).AddImageFittedIntoRectangle(
                    ImageDataFactory.Create(image, null),
                    new(w, h),
                    false
                );
            }

            doc.Close();

            var bytes = new RandomAccessSourceFactory().CreateSource(ms.ToArray());
            return new(new PdfReader(bytes, new()));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        ///
        /// <summary>
        /// Requests the password of the specified PDF file.
        /// </summary>
        ///
        /// <param name="query">Query object.</param>
        /// <param name="src">Path of the PDF file.</param>
        ///
        /// <returns>Query result.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static QueryMessage<string, string> Request(this IQuery<string> query, string src)
        {
            try
            {
                var dest = Query.NewMessage(src);
                query.Request(dest);
                if (dest.Cancel || dest.Value.HasValue()) return dest;
                throw new ArgumentException("Password is empty.");
            }
            catch (Exception e) { throw new EncryptionException("Input password may be incorrect.", e); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetOptions
        ///
        /// <summary>
        /// Creates a new instance of the ReaderProperties class with the
        /// specified password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ReaderProperties GetOptions(string password)
        {
            var dest = new ReaderProperties();
            if (password.HasValue()) _ = dest.SetPassword(Encoding.UTF8.GetBytes(password));
            return dest;
        }

        #endregion
    }
}