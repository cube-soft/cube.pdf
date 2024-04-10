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
namespace Cube.Pdf.Pages;

using System;
using Cube.Forms;
using Cube.Globalization;
using Cube.Web.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Surface
///
/// <summary>
/// Provides resources for display.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class Surface
{
    /* --------------------------------------------------------------------- */
    ///
    /// Texts
    ///
    /// <summary>
    /// Get texts for UI.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static Text Texts { get; } = new();

    /* --------------------------------------------------------------------- */
    ///
    /// ProductUri
    ///
    /// <summary>
    /// Gets the URL of the product Web page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static Uri ProductUri => MakeUri("https://www.cube-soft.jp/cubepdfpage/");

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentUri
    ///
    /// <summary>
    /// Gets the URL of the document Web page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static Uri DocumentUri => MakeUri("https://docs.cube-soft.jp/entry/cubepdf-page");

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Languages
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a Language pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<Language> Languages => new()
    {
        { nameof(Language.Auto),     Language.Auto     },
        { nameof(Language.English),  Language.English  },
        { nameof(Language.German),   Language.German   },
        { nameof(Language.Japanese), Language.Japanese },
        { "Simplified Chinese",      Language.SimplifiedChinese },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// PdfVersions
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a minor number of PDF version.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<int> PdfVersions => new()
    {
        { "PDF 1.7", 7 },
        { "PDF 1.6", 6 },
        { "PDF 1.5", 5 },
        { "PDF 1.4", 4 },
        { "PDF 1.3", 3 },
        { "PDF 1.2", 2 },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// ViewerOptions
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a ViewerOptions pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<ViewerOption> ViewerOptions => new()
    {
        { Texts.Metadata_SinglePage,     ViewerOption.SinglePage },
        { Texts.Metadata_OneColumn,      ViewerOption.OneColumn },
        { Texts.Metadata_TwoPageLeft,    ViewerOption.TwoPageLeft },
        { Texts.Metadata_TwoPageRight,   ViewerOption.TwoPageRight },
        { Texts.Metadata_TwoColumnLeft,  ViewerOption.TwoColumnLeft },
        { Texts.Metadata_TwoColumnRight, ViewerOption.TwoColumnRight },
    };

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// MakeUri
    ///
    /// <summary>
    /// Gets the Uri object from the specified URL string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Uri MakeUri(string url) =>
        new Uri(url).With("lang", Locale.GetCurrentLanguage().ToCultureInfo().Name.ToLowerInvariant());

    #endregion
}