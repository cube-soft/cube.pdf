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
using System.Collections.Generic;
using System.Globalization;
using Cube.Mixin.Uri;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// Resource
    ///
    /// <summary>
    /// Provides resources for display.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Resource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ProductUri
        ///
        /// <summary>
        /// Gets the URL of the product Web page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri ProductUri => GetUri("https://www.cube-soft.jp/cubepdfpage/");

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Languages
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a Language pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, Language>> Languages => new()
        {
            new("Auto",     Language.Auto),
            new("English",  Language.English),
            new("Japanese", Language.Japanese),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// PdfVersions
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a minor number of PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, int>> PdfVersions => new()
        {
            new("PDF 1.7", 7),
            new("PDF 1.6", 6),
            new("PDF 1.5", 5),
            new("PDF 1.4", 4),
            new("PDF 1.3", 3),
            new("PDF 1.2", 2),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// ViewerOptions
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a ViewerOptions pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, ViewerOption>> ViewerOptions => new()
        {
            new(Properties.Resources.MenuSinglePage,     ViewerOption.SinglePage),
            new(Properties.Resources.MenuOneColumn,      ViewerOption.OneColumn),
            new(Properties.Resources.MenuTwoPageLeft,    ViewerOption.TwoPageLeft),
            new(Properties.Resources.MenuTwoPageRight,   ViewerOption.TwoPageRight),
            new(Properties.Resources.MenuTwoColumnLeft,  ViewerOption.TwoColumnLeft),
            new(Properties.Resources.MenuTwoColumnRight, ViewerOption.TwoColumnRight),
        };

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateCulture
        ///
        /// <summary>
        /// Updates the culture information of resource objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void UpdateCulture(Language src) => Properties.Resources.Culture = src.ToCultureInfo();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetUri
        ///
        /// <summary>
        /// Gets the Uri object from the specified URL string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Uri GetUri(string url) =>
            new Uri(url).With("lang", CultureInfo.CurrentCulture.Name.ToLowerInvariant());

        #endregion
    }
}
