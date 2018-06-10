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
        /// PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        ///
        /// <returns>PdfReader オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader Create(string src) => new PdfReader(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PDF ファイルのパス</param>
        /// <param name="query">パスワード用オブジェクト</param>
        /// <param name="denyUserPassword">
        /// ユーザパスワードの入力を拒否するかどうか
        /// </param>
        /// <param name="password">入力されたパスワード</param>
        ///
        /// <returns>PdfReader オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader Create(string src, IQuery<string> query,
            bool denyUserPassword, out string password)
        {
            password = string.Empty;

            while (true)
            {
                try { return CreateCore(src, password, denyUserPassword); }
                catch (EncryptionException)
                {
                    var e = QueryEventArgs.Create(src);
                    query.Request(e);
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

                    var scale = 72.0 / image.HorizontalResolution;
                    var w = (float)(image.Width * scale);
                    var h = (float)(image.Height * scale);

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

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateCore
        ///
        /// <summary>
        /// PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader CreateCore(string src, string password, bool denyUserPassword)
        {
            var bytes = !string.IsNullOrEmpty(password) ? Encoding.UTF8.GetBytes(password) : null;
            var dest  = new PdfReader(src, bytes, true);
            var deny  = denyUserPassword && !dest.IsOpenedWithFullPermissions;

            if (deny)
            {
                dest.Dispose();
                var msg = Properties.Resources.ErrorDenyUserPassword;
                throw new BadPasswordException(msg);
            }
            return dest;
        }

        #endregion
    }
}