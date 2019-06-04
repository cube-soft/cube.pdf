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
using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Mixin.Pdf;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReaderFactory
    ///
    /// <summary>
    /// Provices functionality to create a PdfReader instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ReaderFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the PdfReader class.
        /// </summary>
        ///
        /// <param name="src">PDF document path.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader Create(string src) => new PdfReader(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the PdfReader class.
        /// </summary>
        ///
        /// <param name="src">PDF document path.</param>
        /// <param name="qv">Password string or query.</param>
        /// <param name="fullaccess">Requires full access.</param>
        /// <param name="partial">Partial mode.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader Create(string src,
            QueryMessage<IQuery<string, string>, string> qv,
            bool fullaccess,
            bool partial
        ) {
            while (true)
            {
                try
                {
                    var bytes  = qv.Value.HasValue() ? Encoding.UTF8.GetBytes(qv.Value) : null;
                    var dest   = new PdfReader(src, bytes, partial);
                    var denied = fullaccess && !dest.IsOpenedWithFullPermissions;
                    if (denied)
                    {
                        dest.Dispose();
                        throw new BadPasswordException("Requires full access");
                    }
                    else return dest;
                }
                catch (BadPasswordException)
                {
                    var args = qv.Query.RequestPassword(src);
                    if (!args.Cancel) qv.Value = args.Value;
                    else throw new OperationCanceledException();
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFromImage
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
        public static PdfReader CreateFromImage(string src, IO io)
        {
            using (var ms = new System.IO.MemoryStream())
            using (var ss = io.OpenRead(src))
            using (var image = Image.FromStream(ss))
            {
                Debug.Assert(image != null);
                Debug.Assert(image.FrameDimensionsList != null);
                Debug.Assert(image.FrameDimensionsList.Length > 0);

                var doc = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var guid = image.FrameDimensionsList[0];
                var dim  = new FrameDimension(guid);
                for (var i = 0; i < image.GetFrameCount(dim); ++i)
                {
                    image.SelectActiveFrame(dim, i);

                    var scale = PdfFile.Point / image.HorizontalResolution;
                    var w = image.Width  * scale;
                    var h = image.Height * scale;

                    doc.SetPageSize(new iTextSharp.text.Rectangle(w, h));
                    doc.NewPage();
                    doc.Add(image.GetItextImage());
                }

                doc.Close();
                writer.Close();

                return new PdfReader(ms.ToArray());
            }
        }

        #endregion
    }
}