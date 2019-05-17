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
using Cube.Mixin.String;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// WriterExtension
    ///
    /// <summary>
    /// Provides extended methods of PdfWriter and its inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class WriterExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the PDF metadata to the specified writer.
        /// </summary>
        ///
        /// <param name="src">PdfStamper object.</param>
        /// <param name="data">PDf metadata.</param>
        /// <param name="info">Original PDF metadata.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(this PdfStamper src, Metadata data, IDictionary<string, string> info)
        {
            info.Update("Title",    data.Title);
            info.Update("Subject",  data.Subject);
            info.Update("Keywords", data.Keywords);
            info.Update("Creator",  data.Creator);
            info.Update("Author",   data.Author);

            src.MoreInfo = info;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the encryption settings to the specified writer.
        /// </summary>
        ///
        /// <param name="src">PdfWriter object.</param>
        /// <param name="data">Encryption settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(this PdfWriter src, Encryption data)
        {
            if (data == null || !data.Enabled || !data.OwnerPassword.HasValue()) return;

            var m = GetMethod(data.Method);
            var p = (int)data.Permission.Value;

            var owner = data.OwnerPassword;
            var user  = !data.OpenWithPassword ? string.Empty :
                        data.UserPassword.HasValue() ? data.UserPassword :
                        owner;

            src.SetEncryption(m, user, owner, p);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets attachments to the specified writer.
        /// </summary>
        ///
        /// <param name="src">PdfCopy object.</param>
        /// <param name="data">Collection of attachments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(this PdfCopy src, IEnumerable<Attachment> data)
        {
            var done = new List<Attachment>();

            foreach (var item in data)
            {
                var dup = done.Any(e =>
                    e.Name.ToLower() == item.Name.ToLower() &&
                    e.Length == item.Length &&
                    e.Checksum.SequenceEqual(item.Checksum)
                );

                if (dup) continue;

                var fs = item is EmbeddedAttachment ?
                         PdfFileSpecification.FileEmbedded(src, null, item.Name, item.Data) :
                         PdfFileSpecification.FileEmbedded(src, item.Source, item.Name, null);

                fs.SetUnicodeFileName(item.Name, true);
                src.AddFileAttachment(fs);
                done.Add(item);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the specified Dictionary object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Update(this IDictionary<string, string> src, string key, string value)
        {
            if (src.ContainsKey(key)) src[key] = value;
            else src.Add(key, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethod
        ///
        /// <summary>
        /// Gets the value corresponding to the specified method.
        /// </summary>
        ///
        /// <remarks>
        /// iTextSharp は AES256 Revision 6 に対応していないため、
        /// 暫定的に AES256 を割り当てています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetMethod(EncryptionMethod src)
        {
            var dic = new Dictionary<EncryptionMethod, int>
            {
                { EncryptionMethod.Standard40,  PdfWriter.STANDARD_ENCRYPTION_40  },
                { EncryptionMethod.Standard128, PdfWriter.STANDARD_ENCRYPTION_128 },
                { EncryptionMethod.Aes128,      PdfWriter.ENCRYPTION_AES_128      },
                { EncryptionMethod.Aes256,      PdfWriter.ENCRYPTION_AES_256      },
                { EncryptionMethod.Aes256r6,    PdfWriter.ENCRYPTION_AES_256      },
            };

            return dic.TryGetValue(src, out var dest) ? dest : PdfWriter.STANDARD_ENCRYPTION_40;
        }

        #endregion
    }
}
