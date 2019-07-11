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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataFacade
    ///
    /// <summary>
    /// Provides functionality to access or update the PDF metadata.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MetadataFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataFacade
        ///
        /// <summary>
        /// Initializes a new instance of the MetadataFacade class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source data.</param>
        /// <param name="file">File information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataFacade(Metadata src, Information file)
        {
            src.Version = Versions.FirstOrDefault(e => e.Minor == src.Version.Minor) ??
                          Versions.First();
            if (src.Options == Pdf.ViewerOption.None) src.Options = Pdf.ViewerOption.OneColumn;

            Value  = src;
            File   = file;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the Metadata settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Metadata Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// File
        ///
        /// <summary>
        /// Gets information of the provided PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Information File { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Versions
        ///
        /// <summary>
        /// Gets a collection of PDF version numbers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<PdfVersion> Versions { get; } = new[]
        {
            new PdfVersion(1, 7),
            new PdfVersion(1, 6),
            new PdfVersion(1, 5),
            new PdfVersion(1, 4),
            new PdfVersion(1, 3),
            new PdfVersion(1, 2),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerOptions
        ///
        /// <summary>
        /// Gets a collection of viewer options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<ViewerOption> ViewerOptions { get; } = new[]
        {
            Pdf.ViewerOption.SinglePage,
            Pdf.ViewerOption.OneColumn,
            Pdf.ViewerOption.TwoColumnLeft,
            Pdf.ViewerOption.TwoColumnRight,
            Pdf.ViewerOption.TwoPageLeft,
            Pdf.ViewerOption.TwoPageRight,
        };

        #endregion
    }
}
