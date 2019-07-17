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
using Cube.FileSystem;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// OpenExtension
    ///
    /// <summary>
    /// Represents the extended methods to open documents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class OpenExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Sets properties of the specified IDocumentReader.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="doc">Document information.</param>
        ///
        /// <remarks>
        /// PDFium は Metadata や Encryption の情報取得が不完全なため、
        /// これらの情報は、必要になったタイミングで iTextSharp を用いて
        /// 取得します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Open(this MainFacade src, IDocumentReader doc)
        {
            src.Value.Source = doc.File;
            if (!doc.Encryption.Enabled) src.Value.Encryption = doc.Encryption;
            src.Value.Images.Add(doc.Pages);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens the first item of the specified collection.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="files">File collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Open(this MainFacade src, IEnumerable<string> files)
        {
            var path = files.FirstPdf();
            if (path.HasValue()) src.Open(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenLink
        ///
        /// <summary>
        /// Opens a PDF document with the specified link.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="link">Information for the link.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void OpenLink(this MainFacade src, Entity link)
        {
            try { src.Open(Shortcut.Resolve(link?.FullName)?.Target); }
            catch (Exception err)
            {
                var cancel = err is OperationCanceledException ||
                             err is TwiceException;
                if (!cancel) _ = src.Value.IO.TryDelete(link?.FullName);
                throw;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenProcess
        ///
        /// <summary>
        /// Starts a new process with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="args">User arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void OpenProcess(this MainFacade src, string args) =>
            Process.Start(new ProcessStartInfo
        {
            FileName  = Assembly.GetExecutingAssembly().Location,
            Arguments = args
        });

        /* ----------------------------------------------------------------- */
        ///
        /// ReOpen
        ///
        /// <summary>
        /// Resets some properties with the specified new PDF document.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="doc">New PDF document.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void ReOpen(this MainFacade src, IDocumentReader doc)
        {
            var items = doc.Pages.Select((v, i) => new { Value = v, Index = i });
            foreach (var e in items) src.Value.Images[e.Index].RawObject = e.Value;
            src.Value.Source = doc.File;
            src.Value.History.Clear();
        }

        #endregion
    }
}
