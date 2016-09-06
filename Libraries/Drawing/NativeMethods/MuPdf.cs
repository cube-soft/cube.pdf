/* ------------------------------------------------------------------------- */
///
/// MuPdf.cs
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
using System.Text;
using System.Runtime.InteropServices;

namespace Cube.Pdf.Drawing.MuPdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// NativeMethods
    /// 
    /// <summary>
    /// MuPDF.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        /* ----------------------------------------------------------------- */
        /// Create
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_Create",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern IntPtr Create();

        /* ----------------------------------------------------------------- */
        /// Dispose
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_Dispose",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern void Dispose(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// LoadFile
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_LoadFile",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int LoadFile(IntPtr core, string filename, string password);

        /* ----------------------------------------------------------------- */
        /// LoadFileFromStream
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_LoadFileFromStream",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int LoadFileFromStream(IntPtr core, byte[] buffer, int size, string password);


        /* ----------------------------------------------------------------- */
        /// SetAlphaBits
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_SetAlphaBits",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int SetAlphaBits(IntPtr core, int bits);

        /* ----------------------------------------------------------------- */
        /// SetPage
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_SetPage",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int SetPage(IntPtr core, int pagenum);

        /* ----------------------------------------------------------------- */
        /// GetBitmap
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetBitmap",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetBitmap(IntPtr core,
            out int width, out int height,
            float dpiX, float dpiY, int rotation, int colorspace,
            bool rotateLandscapePages, bool convertToLetter,
            out int length, int maxSize);

        /* ----------------------------------------------------------------- */
        /// GetWidth
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetWidth",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetWidth(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// GetHeight
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetHeight",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetHeight(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// GetRotation
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetRotation",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetRotation(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// GetTitle
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetTitle",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetTitle(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetAuthor
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetAutor",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetAuthor(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetSubject
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetSubject",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetSubject(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetKeywords
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetKeywords",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetKeywords(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetCreator
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetCreator",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetCreator(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetProducer
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetProducer",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetProducer(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetCreationDate
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetCreationDate",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetCreationDate(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetLastWriteDate
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetLastUpdateDate",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetLastWriteDate(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetFormat
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetFormat",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetFormat(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetEncryption
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_GetEncryption",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern int GetEncryption(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// HasPrintPermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasPrintPermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasPrintPermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasHighResolutionPrintPermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasHighResolutionPrintPermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasHighResolutionPrintPermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasCopyPermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasCopyPermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasCopyPermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasEditPermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasEditPermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasEditPermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasAnnotatePermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasAnnotatePermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasAnnotatePermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasFillFormPermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasFillFormPermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasFillFormPermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasAccessibilityPermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasAccessibilityPermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasAccessibilityPermission(IntPtr core);

        /* ----------------------------------------------------------------- */
        /// HasAssemblePermission
        /* ----------------------------------------------------------------- */
        [DllImport(
            LibName,
            EntryPoint = "MuPDF_HasAssemblePermission",
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi
        )]
        public static extern bool HasAssemblePermission(IntPtr core);

        #region Others

        /* ----------------------------------------------------------------- */
        /// GetPropertyAsStringBuilder
        /* ----------------------------------------------------------------- */
        public delegate int GetPropertyAsStringBuilder(IntPtr core, StringBuilder buffer, int size);

        /* ----------------------------------------------------------------- */
        /// GetStringProperty
        /* ----------------------------------------------------------------- */
        public static string GetStringProperty(IntPtr pObject, GetPropertyAsStringBuilder getter)
        {
            var size   = 256;
            var buffer = new StringBuilder(size);
            var result = getter(pObject, buffer, size);

            if (result > size)
            {
                buffer = new StringBuilder(result);
                getter(pObject, buffer, result);
            }
            return buffer.ToString();
        }

        #endregion

        #region Fields
        private const string LibName = "MuPdf.dll";
        #endregion
    }
}
