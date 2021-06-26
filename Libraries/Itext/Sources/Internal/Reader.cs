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
using Cube.Mixin.Drawing;
using Cube.Mixin.String;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;

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
        public static PdfReader From(object src) => src as PdfReader;

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
        public static PdfReader From(File src, OpenOption options) =>
            src is PdfFile   f0 ? FromPdf(f0.FullName, new(null, f0.Password), options) :
            src is ImageFile f1 ? FromImage(f1.FullName, options.IO) :
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
        public static PdfReader FromPdf(string src) => new(src);

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
        public static PdfReader FromPdf(string src, Password password, OpenOption options)
        {
            while (true)
            {
                try
                {
                    var bytes = password.Value.HasValue() ? Encoding.UTF8.GetBytes(password.Value) : null;
                    var dest  = new PdfReader(src, bytes, options.SaveMemory);
                    if (dest.IsOpenedWithFullPermissions || !options.FullAccess) return dest;
                    dest.Dispose();
                    throw new BadPasswordException("Owner password is required.");
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
        /// <param name="io">I/O handler.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader FromImage(string src, IO io)
        {
            using var ms = new System.IO.MemoryStream();
            using var ss = io.OpenRead(src);
            using var image = Image.FromStream(ss);

            var doc = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            var guid = image.FrameDimensionsList[0];
            var dim  = new FrameDimension(guid);
            for (var i = 0; i < image.GetFrameCount(dim); ++i)
            {
                _ = image.SelectActiveFrame(dim, i);

                var scale = PdfFile.Point / image.HorizontalResolution;
                var w = image.Width  * scale;
                var h = image.Height * scale;

                _ = doc.SetPageSize(new iTextSharp.text.Rectangle(w, h));
                _ = doc.NewPage();
                _ = doc.Add(Convert(image));
            }

            doc.Close();
            writer.Close();

            return new(ms.ToArray());
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
        /// Convert
        ///
        /// <summary>
        /// Converts from System.Drawing.Image to iTextSharp.text.Image.
        /// </summary>
        ///
        /// <param name="src">Source image.</param>
        ///
        /// <returns>Converted image.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static iTextSharp.text.Image Convert(Image src)
        {
            var scale = PdfFile.Point / src.HorizontalResolution;
            var format = src.GetImageFormat();
            if (!new List<ImageFormat> {
                ImageFormat.Bmp,
                ImageFormat.Gif,
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Tiff,
            }.Contains(format)) format = ImageFormat.Png;

            var dest = iTextSharp.text.Image.GetInstance(src, format);
            dest.SetAbsolutePosition(0, 0);
            dest.ScalePercent(scale * 100);

            return dest;
        }

        #endregion
    }
}