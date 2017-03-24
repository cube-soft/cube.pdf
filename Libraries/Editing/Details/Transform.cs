/* ------------------------------------------------------------------------- */
///
/// Transform.cs
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
using System.Drawing;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// Transform
    /// 
    /// <summary>
    /// Cube.Pdf の各データ型と iTextSharp 内部で使用されている型（または値）
    /// の相互変換を補助するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Transform
    {
        #region Translate Cube.Pdf to iText object

        /* ----------------------------------------------------------------- */
        ///
        /// ToIText
        /// 
        /// <summary>
        /// Cube.Pdf.EncryptionMethod オブジェクトを対応する値に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int ToIText(EncryptionMethod value)
        {
            switch (value)
            {
                case EncryptionMethod.Standard40:  return PdfWriter.STANDARD_ENCRYPTION_40;
                case EncryptionMethod.Standard128: return PdfWriter.STANDARD_ENCRYPTION_128;
                case EncryptionMethod.Aes128:      return PdfWriter.ENCRYPTION_AES_128;
                case EncryptionMethod.Aes256:      return PdfWriter.ENCRYPTION_AES_256;
                default: break;
            }
            return -1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToIText
        /// 
        /// <summary>
        /// Cube.Pdf.Permission オブジェクトを対応する値に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static int ToIText(Permission value)
        {
            int dest = 0;

            if (value.Print.IsAllowed())          dest |= PdfWriter.AllowPrinting;
            if (!value.Print.IsDenied())          dest |= PdfWriter.AllowDegradedPrinting;
            if (value.Assemble.IsAllowed())          dest |= PdfWriter.AllowAssembly;
            if (value.ModifyContents.IsAllowed())    dest |= PdfWriter.AllowModifyContents;
            if (value.CopyContents.IsAllowed())      dest |= PdfWriter.AllowCopy;
            if (value.FillInFormFields.IsAllowed())   dest |= PdfWriter.AllowFillIn;
            if (value.ModifyAnnotations.IsAllowed()) dest |= PdfWriter.AllowModifyAnnotations;
            if (value.Accessibility.IsAllowed())     dest |= PdfWriter.AllowScreenReaders;
            // if (value.ExtractPage.IsAllow())    dest |= ???
            // if (value.Signature.IsAllow())      dest |= ???
            // if (value.TemplatePage.IsAllow())   dest |= ???

            return dest;
        }

        #endregion

        #region Translate iText to Cube.Pdf or System object

        /* ----------------------------------------------------------------- */
        ///
        /// ToSize
        /// 
        /// <summary>
        /// iTextSharp.text.Rectangle オブジェクトを System.Drawing.Size
        /// オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Size ToSize(iTextSharp.text.Rectangle value)
        {
            return new Size((int)value.Width, (int)value.Height);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToEncryptionMethod
        /// 
        /// <summary>
        /// 値を Cube.Pdf.EncryptionMethod オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static EncryptionMethod ToEncryptionMethod(int value)
        {
            switch (value)
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
        /// ToPermission
        /// 
        /// <summary>
        /// 値を Cube.Pdf.Permission オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Permission ToPermission(long value)
        {
            var dest = new Permission();

            dest.Print          = ToPermissionMethod(value, PdfWriter.AllowPrinting);
            dest.Assemble          = ToPermissionMethod(value, PdfWriter.AllowAssembly);
            dest.ModifyContents    = ToPermissionMethod(value, PdfWriter.AllowModifyContents);
            dest.CopyContents      = ToPermissionMethod(value, PdfWriter.AllowCopy);
            dest.FillInFormFields   = ToPermissionMethod(value, PdfWriter.AllowFillIn);
            dest.ModifyAnnotations = ToPermissionMethod(value, PdfWriter.AllowModifyAnnotations);
            dest.Accessibility     = ToPermissionMethod(value, PdfWriter.AllowScreenReaders);
            // dest.ExtractPage    = ToPermissionMethod(value, ???);
            // dest.Signature      = ToPermissionMethod(value, ???);
            // dest.TemplatePage   = ToPermissionMethod(value, ???);

            if (dest.Print.IsDenied() && (value & PdfWriter.AllowDegradedPrinting) != 0)
            {
                dest.Print = PermissionMethod.Restrict;
            }
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToPermissionMethod
        /// 
        /// <summary>
        /// 値を Cube.Pdf.PermissionMethod オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static PermissionMethod ToPermissionMethod(long value, int mask)
        {
            return ((value & mask) != 0) ? PermissionMethod.Allow : PermissionMethod.Deny;
        }

        #endregion
    }
}