/* ------------------------------------------------------------------------- */
///
/// Translator.cs
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
    /// Translator
    /// 
    /// <summary>
    /// Cube.Pdf の各データ型と iTextSharp 内部で使用されている型（または値）
    /// の相互変換を補助するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Translator
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

            if (value.Printing)          dest |= PdfWriter.AllowPrinting;
            // if (value.DePrinting)     dest |= PdfWriter.AllowDegradedPrinting;
            if (value.Assembly)          dest |= PdfWriter.AllowAssembly;
            if (value.ModifyContents)    dest |= PdfWriter.AllowModifyContents;
            if (value.CopyContents)      dest |= PdfWriter.AllowCopy;
            if (value.InputFormFields)   dest |= PdfWriter.AllowFillIn;
            if (value.ModifyAnnotations) dest |= PdfWriter.AllowModifyAnnotations;
            if (value.Accessibility)     dest |= PdfWriter.AllowScreenReaders;
            // if (value.ExtractPage)    dest |= ???
            // if (value.Signature)      dest |= ???
            // if (value.TemplatePage)   dest |= ???

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
            if ((value & PdfWriter.AllowPrinting) != 0) dest.Printing = true;
            // if ((value & PdfWriter.AllowDegradedPrinting) != 0) dest.DePrinting = true;
            if ((value & PdfWriter.AllowAssembly) != 0) dest.Assembly = true;
            if ((value & PdfWriter.AllowModifyContents) != 0) dest.ModifyContents = true;
            if ((value & PdfWriter.AllowCopy) != 0) dest.CopyContents = true;
            if ((value & PdfWriter.AllowFillIn) != 0) dest.InputFormFields = true;
            if ((value & PdfWriter.AllowModifyAnnotations) != 0) dest.ModifyAnnotations = true;
            if ((value & PdfWriter.AllowScreenReaders) != 0) dest.Accessibility = true;
            // if ((value & ???) != 0) dest.ExtractPage = false;
            // if ((value & ???) != 0) dest.Signature = true;
            // if ((value & ???) != 0) dest.TemplatePage = true;

            return dest;
        }

        #endregion
    }
}