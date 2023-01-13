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
using System.Globalization;
using Cube.Forms;
using Cube.Web.Extensions;

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
        public static ComboListSource<Language> Languages => new()
        {
            { nameof(Language.Auto),     Language.Auto     },
            { nameof(Language.English),  Language.English  },
            { nameof(Language.Japanese), Language.Japanese },
            { nameof(Language.German),   Language.German   },
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
        public static ComboListSource<int> PdfVersions => new()
        {
            { "PDF 1.7", 7 },
            { "PDF 1.6", 6 },
            { "PDF 1.5", 5 },
            { "PDF 1.4", 4 },
            { "PDF 1.3", 3 },
            { "PDF 1.2", 2 },
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
        public static ComboListSource<ViewerOption> ViewerOptions => new()
        {
            { Properties.Resources.MenuSinglePage,     ViewerOption.SinglePage },
            { Properties.Resources.MenuOneColumn,      ViewerOption.OneColumn },
            { Properties.Resources.MenuTwoPageLeft,    ViewerOption.TwoPageLeft },
            { Properties.Resources.MenuTwoPageRight,   ViewerOption.TwoPageRight },
            { Properties.Resources.MenuTwoColumnLeft,  ViewerOption.TwoColumnLeft },
            { Properties.Resources.MenuTwoColumnRight, ViewerOption.TwoColumnRight },
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
