/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Drawing;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Editing.IText
{
    /* --------------------------------------------------------------------- */
    ///
    /// IText.Operations
    /// 
    /// <summary>
    /// iTextSharp に関する拡張メソッドを定義するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Operations
    {
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
                Method                = Transform.ToEncryptionMethod(src.GetCryptoMode()),
                Permission            = Transform.ToPermission(src.Permissions),
                OwnerPassword         = file.FullAccess ? file.Password : string.Empty,
                UserPassword          = password,
                IsUserPasswordEnabled = !string.IsNullOrEmpty(password),
            };
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
                var method = Transform.ToEncryptionMethod(src.GetCryptoMode());
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
    }
}
