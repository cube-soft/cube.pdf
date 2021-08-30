/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Runtime.InteropServices;

namespace Cube.Pdf.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileFactory
    ///
    /// <summary>
    /// Provides factory methods of the PdfFile class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class FileFactory
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a PdfFile object from the specified arguments.
        /// </summary>
        ///
        /// <param name="core">PDFium object.</param>
        /// <param name="password">Password of the file.</param>
        /// <param name="fullaccess">
        /// Value indicating whether the loaded PDF document can be fully
        /// accessible.
        /// </param>
        ///
        /// <returns>PdfFile object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PdfFile Create(PdfiumReader core, string password, bool fullaccess) =>
            new(core.Source)
        {
            Count      = core.Invoke(NativeMethods.FPDF_GetPageCount),
            Password   = password,
            FullAccess = fullaccess,
        };
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FileAccess
    ///
    /// <summary>
    /// Represents the data structure for PDFium to access files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal class FileAccess
    {
        public uint Length;
        public IntPtr GetBlock;
        public IntPtr Parameter;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetBlockHandler
    ///
    /// <summary>
    /// Represents the delegate to read data from the specified stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int GetBlockHandler(IntPtr param, uint pos, IntPtr dest, uint size);
}
