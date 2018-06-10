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
    /// PdfWriter の拡張用クラスです。
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
        /// メタ情報を設定します。
        /// </summary>
        ///
        /// <param name="src">PdfStamper オブジェクト</param>
        /// <param name="data">メタ情報</param>
        /// <param name="original">オリジナルのメタ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(this PdfStamper src, Metadata data, IDictionary<string, string> original)
        {
            original.Update("Title",    data.Title);
            original.Update("Subject",  data.Subtitle);
            original.Update("Keywords", data.Keywords);
            original.Update("Creator",  data.Creator);
            original.Update("Author",   data.Author);

            src.MoreInfo = original;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 暗号化情報を設定します。
        /// </summary>
        ///
        /// <param name="src">PdfWriter オブジェクト</param>
        /// <param name="data">暗号化情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(this PdfWriter src, Encryption data)
        {
            if (data == null || !data.Enabled ||
                string.IsNullOrEmpty(data.OwnerPassword)) return;

            var m = (int)data.Method;
            var p = (int)data.Permission.Value;

            var owner = data.OwnerPassword;
            var user  = !data.OpenWithPassword ? string.Empty :
                        !string.IsNullOrEmpty(data.UserPassword) ? data.UserPassword :
                        owner;

            src.SetEncryption(m, user, owner, p);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 添付ファイルを設定します。
        /// </summary>
        ///
        /// <param name="src">PdfCopy オブジェクト</param>
        /// <param name="data">添付ファイル一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(this PdfCopy src, IEnumerable<Attachment> data)
        {
            var done = new List<Attachment>();

            foreach (var item in data)
            {
                if (done.Any(e =>
                    e.Name.ToLower() == item.Name.ToLower() &&
                    e.Length == item.Length &&
                    e.Checksum.SequenceEqual(item.Checksum)
                )) continue;

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
        /// Dictionary オブジェクトの内容を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Update(this IDictionary<string, string> src, string key, string value)
        {
            if (src.ContainsKey(key)) src[key] = value;
            else src.Add(key, value);
        }

        #endregion
    }
}
