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
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Drawing.MuPdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Operations
    ///
    /// <summary>
    /// MuPDF の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class Operations
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreatePage
        ///
        /// <summary>
        /// Page オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Page CreatePage(this IntPtr core, MediaFile file, int pagenum)
        {
            if (NativeMethods.SetPage(core, pagenum) < 0) return null;

            var width  = NativeMethods.GetWidth(core);
            var height = NativeMethods.GetHeight(core);

            return new Page
            {
                File        = file,
                Number      = pagenum,
                Size        = new Size(width, height),
                Rotation    = NativeMethods.GetRotation(core),
                Resolution  = new Point(72, 72)
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
        /* ----------------------------------------------------------------- */
        public static Metadata CreateMetadata(this IntPtr core)
            => new Metadata
        {
            Title    = NativeMethods.GetStringProperty(core, NativeMethods.GetTitle),
            Author   = NativeMethods.GetStringProperty(core, NativeMethods.GetAuthor),
            Subtitle = NativeMethods.GetStringProperty(core, NativeMethods.GetSubject),
            Keywords = NativeMethods.GetStringProperty(core, NativeMethods.GetKeywords),
            Creator  = NativeMethods.GetStringProperty(core, NativeMethods.GetCreator),
            Producer = NativeMethods.GetStringProperty(core, NativeMethods.GetProducer)
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEncryption
        ///
        /// <summary>
        /// Encryption オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Encryption CreateEncryption(this IntPtr core, string password)
        {
            var dest = new Encryption
            {
                IsEnabled     = string.IsNullOrEmpty(password),
                OwnerPassword = password
            };

            var basic = NativeMethods.HasPrintPermission(core);
            var high  = NativeMethods.HasHighResolutionPrintPermission(core);

            dest.Permission.Assemble          = ConvertTo(NativeMethods.HasAssemblePermission(core));
            dest.Permission.ModifyContents    = ConvertTo(NativeMethods.HasEditPermission(core));
            dest.Permission.CopyContents      = ConvertTo(NativeMethods.HasCopyPermission(core));
            dest.Permission.FillInFormFields  = ConvertTo(NativeMethods.HasFillFormPermission(core));
            dest.Permission.ModifyAnnotations = ConvertTo(NativeMethods.HasAnnotatePermission(core));
            dest.Permission.Accessibility     = ConvertTo(NativeMethods.HasAccessibilityPermission(core));
            dest.Permission.Print             = high  ? PermissionMethod.Allow :
                                                basic ? PermissionMethod.Restrict :
                                                        PermissionMethod.Deny;

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImage
        ///
        /// <summary>
        /// Image オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Image CreateImage(this IntPtr core, Page page, double power)
        {
            var width  = 0;
            var height = 0;
            var length = 0;
            var raw    = NativeMethods.GetBitmap(
                core,
                out width,
                out height,
                (float)(power * page.Resolution.X),
                (float)(power * page.Resolution.Y),
                0,
                0,
                false,
                false,
                out length,
                0
            );

            if (raw == IntPtr.Zero) return null;

            var dest = ConvertTo(raw, width, height);
            NativeMethods.FreeRenderedPage(core);
            return dest;
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// ConvertTo
        ///
        /// <summary>
        /// C で生成されたイメージを Image オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static unsafe Image ConvertTo(IntPtr raw, int width, int height)
        {
            var dest = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var bits = dest.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, dest.PixelFormat);
            var ptrSrc  = (byte*)raw;
            var ptrDest = (byte*)bits.Scan0;
            for (var y = 0; y < height; y++)
            {
                var pl = ptrDest;
                var sl = ptrSrc;
                for (var x = 0; x < width; x++)
                {
                    // Swap these here instead of in MuPDF
                    // because most pdf images will be rgb or cmyk.
                    // Since we are going through the pixels one by one
                    // anyway swap here to save a conversion from rgb to bgr.

                    pl[2] = sl[0]; //b-r
                    pl[1] = sl[1]; //g-g
                    pl[0] = sl[2]; //r-b
                    //pl[3] = sl[3]; //alpha
                    pl += 3;
                    sl += 4;
                }
                ptrDest += bits.Stride;
                ptrSrc += width * 4;
            }
            dest.UnlockBits(bits);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ConvertTo
        ///
        /// <summary>
        /// 真偽値を PermissionMethod オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static PermissionMethod ConvertTo(bool value)
            => value ?
               PermissionMethod.Allow :
               PermissionMethod.Deny;

        #endregion
    }
}
