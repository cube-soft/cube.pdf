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
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReaderExtension
    ///
    /// <summary>
    /// PdfReader の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ReaderExtension
    {
        #region Methods

        #region Get

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// Page オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="file">PDF ファイル情報</param>
        /// <param name="pagenum">ページ番号</param>
        ///
        /// <returns>Page オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Page GetPage(this PdfReader src, PdfFile file, int pagenum) => new Page(
            file,                                    // File
            pagenum,                                 // Number
            src.GetPageSize(pagenum).ToSize(),       // Size
            new Angle(src.GetPageRotation(pagenum)), // Rotation
            file.Resolution                          // Resolution
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetMetadata
        ///
        /// <summary>
        /// Metadata オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        ///
        /// <returns>Metadata オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Metadata GetMetadata(this PdfReader src) => new Metadata
        {
            Version        = new Version(1, src.PdfVersion - '0', 0, 0),
            Author         = src.Info.ContainsKey("Author")   ? src.Info["Author"]   : string.Empty,
            Title          = src.Info.ContainsKey("Title")    ? src.Info["Title"]    : string.Empty,
            Subject        = src.Info.ContainsKey("Subject")  ? src.Info["Subject"]  : string.Empty,
            Keywords       = src.Info.ContainsKey("Keywords") ? src.Info["Keywords"] : string.Empty,
            Creator        = src.Info.ContainsKey("Creator")  ? src.Info["Creator"]  : string.Empty,
            Producer       = src.Info.ContainsKey("Producer") ? src.Info["Producer"] : string.Empty,
            DisplayOptions = DisplayOptionsFactory.Create(src.SimpleViewerPreferences),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryption
        ///
        /// <summary>
        /// Encryption オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="file">PDF ファイル情報</param>
        ///
        /// <returns>Encryption オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption GetEncryption(this PdfReader src, PdfFile file)
        {
            if (file.FullAccess && string.IsNullOrEmpty(file.Password)) return new Encryption();

            var password = src.GetUserPassword(file);
            return new Encryption
            {
                Enabled          = true,
                Method           = src.GetEncryptionMethod(),
                Permission       = new Permission(src.Permissions),
                OwnerPassword    = file.FullAccess ? file.Password : string.Empty,
                UserPassword     = password,
                OpenWithPassword = !string.IsNullOrEmpty(password),
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEncryptionMethod
        ///
        /// <summary>
        /// 暗号化方式を取得します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        ///
        /// <returns>暗号化方式</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static EncryptionMethod GetEncryptionMethod(this PdfReader src)
        {
            switch (src.GetCryptoMode())
            {
                case PdfWriter.STANDARD_ENCRYPTION_40:  return EncryptionMethod.Standard40;
                case PdfWriter.STANDARD_ENCRYPTION_128: return EncryptionMethod.Standard128;
                case PdfWriter.ENCRYPTION_AES_128:      return EncryptionMethod.Aes128;
                case PdfWriter.ENCRYPTION_AES_256:      return EncryptionMethod.Aes256;
                default:                                return EncryptionMethod.Unknown;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserPassword
        ///
        /// <summary>
        /// ユーザパスワードを取得します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="file">PDF のファイル情報</param>
        ///
        /// <returns>ユーザパスワード</returns>
        ///
        /// <remarks>
        /// 暗号化方式が AES256 の場合、ユーザパスワードの解析に
        /// 失敗するので除外しています。AES256 の場合の解析方法を要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetUserPassword(this PdfReader src, PdfFile file)
        {
            if (file.FullAccess)
            {
                var method = src.GetEncryptionMethod();
                if (method == EncryptionMethod.Aes256) return string.Empty; // see remarks

                var bytes = src.ComputeUserPassword();
                if (bytes?.Length > 0) return Encoding.UTF8.GetString(bytes);
            }
            return file.Password;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetContentParser
        ///
        /// <summary>
        /// PdfReaderContentParser オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        ///
        /// <returns>PdfReaderContentParser オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReaderContentParser GetContentParser(this PdfReader src) =>
            new PdfReaderContentParser(src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetBookmarks
        ///
        /// <summary>
        /// PDF ファイルに存在するしおり情報を取得します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="pagenum">ページ番号</param>
        /// <param name="delta">ページ番号の差分</param>
        /// <param name="dest">結果の格納用オブジェクト</param>
        ///
        /// <remarks>
        /// PdfReader オブジェクトから取得されたしおり情報に対して、
        /// ページ番号を delta だけずらした後に処理を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void GetBookmarks(this PdfReader src, int pagenum, int delta,
            IList<Dictionary<string, object>> dest)
        {
            var cmp = $"^{pagenum} (XYZ|Fit|FitH|FitBH)";
            var bookmarks = SimpleBookmark.GetBookmark(src);
            if (bookmarks == null) return;

            SimpleBookmark.ShiftPageNumbers(bookmarks, delta, null);
            foreach (var b in bookmarks)
            {
                var found = b.TryGetValue("Page", out object obj);
                if (found && Regex.IsMatch(obj.ToString(), cmp)) dest.Add(b);
            }
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Page の回転情報にしたがって PdfReader オブジェクトの内容を
        /// 更新します。
        /// </summary>
        ///
        /// <remarks>
        /// PDF ページを回転させる場合、いったん PdfReader オブジェクトの
        /// 内容を更新した後に PdfCopy オブジェクト等でコピーする方法が
        /// もっとも容易に実現できるため、この方法を採用しています。
        /// </remarks>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="page">Page オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Rotate(this PdfReader src, Page page)
        {
            var rot = (page.Rotation + page.Delta).Degree;
            var cmp = src.GetPageRotation(page.Number);
            var dic = src.GetPageN(page.Number);
            if (rot != cmp) dic.Put(PdfName.ROTATE, new PdfNumber(rot));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ToSize
        ///
        /// <summary>
        /// iTextSharp の Rectable オブジェクトを SizeF オブジェクトに
        /// 変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SizeF ToSize(this iTextSharp.text.Rectangle src) =>
            new SizeF(src.Width, src.Height);

        #endregion
    }
}
