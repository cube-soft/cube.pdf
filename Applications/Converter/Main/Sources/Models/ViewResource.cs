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
using System.Text.RegularExpressions;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewResource
    ///
    /// <summary>
    /// Provides resources for display.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ViewResource
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// ProductUri
        ///
        /// <summary>
        /// Gets the URL of the product Web page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri ProductUri { get; } = new Uri("https://www.cube-soft.jp/cubepdf/");

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentUri
        ///
        /// <summary>
        /// Gets the URL of the document Web page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Uri DocumentUri { get; } = new Uri("https://docs.cube-soft.jp/entry/cubepdf");

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a Format pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Format>> Formats { get; } = new[]
        {
            Make("PDF",  Format.Pdf),
            Make("PS",   Format.Ps),
            Make("EPS",  Format.Eps),
            Make("PNG",  Format.Png),
            Make("JPEG", Format.Jpeg),
            Make("BMP",  Format.Bmp),
            Make("TIFF", Format.Tiff),
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
        public static IList<KeyValuePair<string, int>> PdfVersions { get; } = new[]
        {
            Make("PDF 1.7", 7),
            Make("PDF 1.6", 6),
            Make("PDF 1.5", 5),
            Make("PDF 1.4", 4),
            Make("PDF 1.3", 3),
            Make("PDF 1.2", 2),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOptions
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a SaveOption pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, SaveOption>> SaveOptions => new[]
        {
            Make(Properties.Resources.MenuOverwrite, SaveOption.Overwrite),
            Make(Properties.Resources.MenuMergeHead, SaveOption.MergeHead),
            Make(Properties.Resources.MenuMergeTail, SaveOption.MergeTail),
            Make(Properties.Resources.MenuRename,    SaveOption.Rename),
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
        public static IList<KeyValuePair<string, ViewerOption>> ViewerOptions => new[]
        {
            Make(Properties.Resources.MenuSinglePage,     ViewerOption.SinglePage),
            Make(Properties.Resources.MenuOneColumn,      ViewerOption.OneColumn),
            Make(Properties.Resources.MenuTwoPageLeft,    ViewerOption.TwoPageLeft),
            Make(Properties.Resources.MenuTwoPageRight,   ViewerOption.TwoPageRight),
            Make(Properties.Resources.MenuTwoColumnLeft,  ViewerOption.TwoColumnLeft),
            Make(Properties.Resources.MenuTwoColumnRight, ViewerOption.TwoColumnRight),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcesses
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a PostProcess pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, PostProcess>> PostProcesses => new []
        {
            Make(Properties.Resources.MenuOpen,          PostProcess.Open),
            Make(Properties.Resources.MenuOpenDirectory, PostProcess.OpenDirectory),
            Make(Properties.Resources.MenuNone,          PostProcess.None),
            Make(Properties.Resources.MenuOthers,        PostProcess.Others),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Orientations
        ///
        /// <summary>
        /// Gets a collection in which each item consists of a display
        /// string and a Orientation pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Orientation>> Orientations => new []
        {
            Make(Properties.Resources.MenuPortrait,  Orientation.Portrait),
            Make(Properties.Resources.MenuLandscape, Orientation.Landscape),
            Make(Properties.Resources.MenuAuto,      Orientation.Auto),
        };

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
        public static IList<KeyValuePair<string, Language>> Languages => new []
        {
            Make(Properties.Resources.MenuAuto,     Language.Auto),
            Make(Properties.Resources.MenuEnglish,  Language.English),
            Make(Properties.Resources.MenuJapanese, Language.Japanese),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// SourceFilters
        ///
        /// <summary>
        /// Gets a collection of OpenFileDialog filters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<ExtensionFilter> SourceFilters => new[]
        {
            new ExtensionFilter(Properties.Resources.FilterPs,  ".ps"),
            new ExtensionFilter(Properties.Resources.FilterEps, ".eps"),
            new ExtensionFilter(Properties.Resources.FilterPdf, ".pdf"),
            new ExtensionFilter(Properties.Resources.FilterAll, ".*"),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// DestinationFilters
        ///
        /// <summary>
        /// Gets a collection of SaveFileDialog filters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<ExtensionFilter> DestinationFilters => new[]
        {
            new ExtensionFilter(Properties.Resources.FilterPdf,  ".pdf"),
            new ExtensionFilter(Properties.Resources.FilterPs,   ".ps"),
            new ExtensionFilter(Properties.Resources.FilterEps,  ".eps"),
            new ExtensionFilter(Properties.Resources.FilterPng,  ".png"),
            new ExtensionFilter(Properties.Resources.FilterJpeg, ".jpg", ".jpeg"),
            new ExtensionFilter(Properties.Resources.FilterBmp,  ".bmp"),
            new ExtensionFilter(Properties.Resources.FilterTiff, ".tiff", ".tif"),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// UserProgramFilters
        ///
        /// <summary>
        /// Gets a collection of OpenFileDialog filters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<ExtensionFilter> UserProgramFilters => new[]
        {
            new ExtensionFilter(Properties.Resources.FilterExecutable, ".exe", ".bat"),
            new ExtensionFilter(Properties.Resources.FilterAll, ".*"),
        };

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// Invokes the configuration of the ViewResource related operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure() => _core.Invoke();

        /* ----------------------------------------------------------------- */
        ///
        /// WordWrap
        ///
        /// <summary>
        /// Inserts a new line and splits words.
        /// </summary>
        ///
        /// <param name="src">Source string.</param>
        /// <param name="n">Number of characters to split words.</param>
        ///
        /// <returns>Converted string.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string WordWrap(this string src, int n) =>
            Regex.Replace(src, $@"(?<=\G.{{{n}}})(?!$)", Environment.NewLine);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Make(T, U)
        ///
        /// <summary>
        /// Creates a new instance of the KeyValuePair(T, U) class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static KeyValuePair<K, V> Make<K, V>(K key, V value) => new(key, value);

        #endregion

        #region Fields
        private static readonly OnceAction _core = new(() =>
            Locale.Subscribe(e => Properties.Resources.Culture = e.ToCultureInfo()
        ));
        #endregion
    }
}
