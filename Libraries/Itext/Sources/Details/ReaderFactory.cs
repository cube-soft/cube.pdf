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
using Cube.Pdf.Mixin;
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
    /// PdfReader を生成するためのクラスです。
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
        /// <param name="query">Password query.</param>
        /// <param name="partial">true for partial mode.</param>
        /// <param name="password">Password input by user.</param>
        ///
        /// <returns>PdfReader object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader Create(string src, IQuery<string> query, bool partial, out string password)
        {
            password = string.Empty;

            while (true)
            {
                try
                {
                    var bytes = !string.IsNullOrEmpty(password) ? Encoding.UTF8.GetBytes(password) : null;
                    return new PdfReader(src, bytes, partial);
                }
                catch (BadPasswordException)
                {
                    var e = query.RequestPassword(src);
                    if (!e.Cancel) password = e.Result;
                    else throw new OperationCanceledException();
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFromImage
        ///
        /// <summary>
        /// 画像ファイルから PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">画像ファイルのパス</param>
        /// <param name="io">入出力用オブジェクト</param>
        ///
        /// <returns>PdfReader オブジェクト</returns>
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