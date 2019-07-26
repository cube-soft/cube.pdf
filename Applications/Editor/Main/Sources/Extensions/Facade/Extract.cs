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
using Cube.Mixin.Iteration;
using Cube.Pdf.Itext;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractExtension
    ///
    /// <summary>
    /// Represents the extended methods to extract PDF pages.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ExtractExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractAs
        ///
        /// <summary>
        /// Extracts the selected PDF pages and saves to the specified
        /// file.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Path to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void ExtractAs(this MainFacade src, string dest) =>
            src.Extract(new SaveOption(src.Value.IO)
        {
            Destination = dest,
            Format      = SaveFormat.Pdf,
            Target      = SaveTarget.Selected,
            Split       = false,
        });

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractAs
        ///
        /// <summary>
        /// Extracts PDF pages according to the specified options.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="options">Extract options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void ExtractAs(this MainFacade src, SaveOption options)
        {
            if (options.Format == SaveFormat.Pdf)
            {
                if (options.Split) src.SplitAsDocument(options);
                else src.ExtractAsDocument(options);
            }
            else src.SplitAsImage(options);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractAsDocument
        ///
        /// <summary>
        /// Extracts PDF pages according to the specified options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void ExtractAsDocument(this MainFacade src, SaveOption option)
        {
            using (var writer = new DocumentWriter())
            {
                writer.Add(src.GetTarget(option)
                              .OrderBy(i => i)
                              .Select(i => src.Value.Images[i].RawObject));
                writer.Save(option.Destination);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SplitAsDocument
        ///
        /// <summary>
        /// Extracts PDF pages according to the specified options and
        /// saves as a file per page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SplitAsDocument(this MainFacade src, SaveOption option)
        {
            throw new System.NotImplementedException();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SplitAsImage
        ///
        /// <summary>
        /// Extracts PDF pages according to the specified options and
        /// saves as an image file per page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SplitAsImage(this MainFacade src, SaveOption option)
        {
            throw new System.NotImplementedException();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTarget
        ///
        /// <summary>
        /// Gets the target indices according to the specified options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<int> GetTarget(this MainFacade src, SaveOption e) =>
            e.Target == SaveTarget.All      ? src.Value.Count.Make(i => i) :
            e.Target == SaveTarget.Selected ? src.Value.Images.GetSelectedIndices() :
            new Range(e.Range, src.Value.Count).Select(i => i - 1);

        #endregion
    }
}
