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
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using Cube.FileSystem;
using Cube.Text.Extensions;
using iText.IO.Image;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Exceptions;
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
            src is PdfFile   f0 ? From(f0.FullName, new(null, f0.Password), options) :
            src is ImageFile f1 ? FromImage(f1.FullName) :
            default;

        /* ----------------------------------------------------------------- */
        ///
        /// From
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
        public static PdfDocument From(string src, Password password, OpenOption options)
        {
            while (true)
            {
                try
                {
                    using var ss = Io.Open(src);
                    var obj = new PdfReader(ss, GetOptions(password.Value));
                    var dest = new PdfDocument(obj.SetMemorySavingMode(options.SaveMemory));

                    if (options.FullAccess && !dest.GetReader().IsOpenedWithFullPermission())
                    {
                        dest.Close();
                        throw new BadPasswordException("PdfReader is not opened with owner password");
                    }

                    return dest;
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
            var doc = new PdfDocument(new PdfWriter(ms));
            var kv  = GetImageAttributes(src);

            for (var i = 0; i < kv.Value; ++i)
            {
                var obj = kv.Key.Equals(ImageFormat.Tiff) ?
                          ImageDataFactory.CreateTiff(UrlUtil.ToURL(src), false, i + 1, false) :
                          ImageDataFactory.Create(src);

                var dx = obj.GetDpiX() > 0 ? obj.GetDpiX() : 96;
                var dy = obj.GetDpiY() > 0 ? obj.GetDpiY() : 96;
                var w  = obj.GetWidth()  * (PdfFile.Point / dx);
                var h  = obj.GetHeight() * (PdfFile.Point / dy);

                _ = new PdfCanvas(doc.AddNewPage(new PageSize(w, h)))
                    .AddImageFittedIntoRectangle(obj, new(w, h), false);
            }

            doc.Close();
            var dest = new RandomAccessSourceFactory().CreateSource(ms.ToArray());
            return new(new PdfReader(dest, new()));
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

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageAttributes
        ///
        /// <summary>
        /// Gets the type and number of pages of the specified image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static KeyValuePair<ImageFormat, int> GetImageAttributes(string src)
        {
            using var ss = Io.Open(src);
            using var dest = Image.FromStream(ss);
            return new(dest.RawFormat, dest.GetFrameCount(new(dest.FrameDimensionsList[0])));
        }

        #endregion
    }
}