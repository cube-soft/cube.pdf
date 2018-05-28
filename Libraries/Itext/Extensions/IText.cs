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
using Cube.Pdf.Itext.Images;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ItextExtension
    ///
    /// <summary>
    /// iTextSharp に関する拡張メソッドを定義するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ItextExtension
    {
        #region PdfReader

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePdfReader
        ///
        /// <summary>
        /// 画像ファイルから PdfReader オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">画像ファイルの情報</param>
        ///
        /// <returns>PdfReader オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfReader CreatePdfReader(this ImageFile src)
        {
            using (var ms = new System.IO.MemoryStream())
            using (var image = Image.FromFile(src.FullName))
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                var guid = image.FrameDimensionsList[0];
                var dimension = new FrameDimension(guid);
                for (var i = 0; i < image.GetFrameCount(dimension); ++i)
                {
                    image.SelectActiveFrame(dimension, i);

                    var scale = 72.0 / image.HorizontalResolution;
                    var w = (float)(image.Width * scale);
                    var h = (float)(image.Height * scale);

                    document.SetPageSize(new iTextSharp.text.Rectangle(w, h));
                    document.NewPage();
                    document.Add(image.Convert());
                }

                document.Close();
                writer.Close();

                return new PdfReader(ms.ToArray());
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePage
        ///
        /// <summary>
        /// Page オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static Page CreatePage(this PdfReader src, MediaFile file, int pagenum)
        {
            var size = src.GetPageSize(pagenum);

            return new Page()
            {
                File       = file,
                Number     = pagenum,
                Size       = new Size((int)size.Width, (int)size.Height),
                Rotation   = src.GetPageRotation(pagenum),
                Resolution = new Point(72, 72)
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMetadata
        ///
        /// <summary>
        /// Metadata オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        ///
        /// <returns>Metadata オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Metadata CreateMetadata(this PdfReader src)
            => new Metadata
        {
            Version         = new Version(1, src.PdfVersion - '0', 0, 0),
            Author          = src.Info.ContainsKey("Author")   ? src.Info["Author"]   : "",
            Title           = src.Info.ContainsKey("Title")    ? src.Info["Title"]    : "",
            Subtitle        = src.Info.ContainsKey("Subject")  ? src.Info["Subject"]  : "",
            Keywords        = src.Info.ContainsKey("Keywords") ? src.Info["Keywords"] : "",
            Creator         = src.Info.ContainsKey("Creator")  ? src.Info["Creator"]  : "",
            Producer        = src.Info.ContainsKey("Producer") ? src.Info["Producer"] : "",
            ViewPreferences = src.SimpleViewerPreferences
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEncryption
        ///
        /// <summary>
        /// Encryption オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="file">PDF のファイル情報</param>
        ///
        /// <returns>Encryption オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption CreateEncryption(this PdfReader src, PdfFile file)
        {
            if (file.FullAccess && string.IsNullOrEmpty(file.Password)) return new Encryption();

            var password = src.GetUserPassword(file);
            return new Encryption
            {
                IsEnabled             = true,
                Method                = src.GetEncryptionMethod(),
                Permission            = new Permission(src.Permissions),
                OwnerPassword         = file.FullAccess ? file.Password : string.Empty,
                UserPassword          = password,
                IsUserPasswordEnabled = !string.IsNullOrEmpty(password),
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
                default: break;
            }
            return EncryptionMethod.Unknown;
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
                if (bytes?.Length > 0) return System.Text.Encoding.UTF8.GetString(bytes);
            }
            return file.Password;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Page オブジェクトの情報にしたがって回転します。
        /// </summary>
        ///
        /// <remarks>
        /// PDF ページを回転させる場合、いったん PdfReader オブジェクトの
        /// 内容を改変した後に PdfCopy オブジェクト等でコピーする方法が
        /// もっとも容易に実現できます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Rotate(this PdfReader src, Page page)
        {
            var rot = src.GetPageRotation(page.Number);
            var dic = src.GetPageN(page.Number);
            if (rot != page.Rotation) dic.Put(PdfName.ROTATE, new PdfNumber(page.Rotation));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// 文書プロパティを結合します。
        /// </summary>
        ///
        ///
        /// <param name="src">PdfReader オブジェクト</param>
        /// <param name="data">文書プロパティ</param>
        ///
        /// <returns>結合結果</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDictionary<string, string> Merge(this PdfReader src, Metadata data)
        {
            var dest = src.Info;

            dest.Update("Title",    data.Title);
            dest.Update("Subject",  data.Subtitle);
            dest.Update("Keywords", data.Keywords);
            dest.Update("Creator",  data.Creator);
            dest.Update("Author",   data.Author);

            return dest;
        }

        #endregion

        #region PdfWriter

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
            if (data == null || !data.IsEnabled ||
                string.IsNullOrEmpty(data.OwnerPassword)) return;

            var m = (int)data.Method;
            var p = (int)data.Permission.Value;

            var owner = data.OwnerPassword;
            var user  = !data.IsUserPasswordEnabled ? string.Empty :
                        !string.IsNullOrEmpty(data.UserPassword) ? data.UserPassword :
                        owner;

            src.SetEncryption(m, user, owner, p);
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
