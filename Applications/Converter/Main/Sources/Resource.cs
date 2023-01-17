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
namespace Cube.Pdf.Converter;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cube.Forms;
using Cube.Pdf.Ghostscript;
using Cube.Web.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Resource
///
/// <summary>
/// Provides resources for display.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class Resource
{
    #region ComboListSource

    /* --------------------------------------------------------------------- */
    ///
    /// Formats
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a Format pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<Format> Formats { get; } = new()
    {
        { "PDF",  Format.Pdf  },
        { "PS",   Format.Ps   },
        { "EPS",  Format.Eps  },
        { "PNG",  Format.Png  },
        { "JPEG", Format.Jpeg },
        { "BMP",  Format.Bmp  },
        { "TIFF", Format.Tiff },
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
    public static ComboListSource<int> PdfVersions { get; } = new()
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
    };

    /* --------------------------------------------------------------------- */
    ///
    /// SaveOptions
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a SaveOption pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<SaveOption> SaveOptions => new()
    {
        { Properties.Resources.MenuOverwrite, SaveOption.Overwrite },
        { Properties.Resources.MenuMergeHead, SaveOption.MergeHead },
        { Properties.Resources.MenuMergeTail, SaveOption.MergeTail },
        { Properties.Resources.MenuRename,    SaveOption.Rename    },
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
        { Properties.Resources.MenuSinglePage,     ViewerOption.SinglePage     },
        { Properties.Resources.MenuOneColumn,      ViewerOption.OneColumn      },
        { Properties.Resources.MenuTwoPageLeft,    ViewerOption.TwoPageLeft    },
        { Properties.Resources.MenuTwoPageRight,   ViewerOption.TwoPageRight   },
        { Properties.Resources.MenuTwoColumnLeft,  ViewerOption.TwoColumnLeft  },
        { Properties.Resources.MenuTwoColumnRight, ViewerOption.TwoColumnRight },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcesses
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a PostProcess pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<PostProcess> PostProcesses => new()
    {
        { Properties.Resources.MenuOpen,          PostProcess.Open          },
        { Properties.Resources.MenuOpenDirectory, PostProcess.OpenDirectory },
        { Properties.Resources.MenuNone,          PostProcess.None          },
        { Properties.Resources.MenuOthers,        PostProcess.Others        },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// Orientations
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a Orientation pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<Orientation> Orientations => new()
    {
        { Properties.Resources.MenuPortrait,  Orientation.Portrait  },
        { Properties.Resources.MenuLandscape, Orientation.Landscape },
        { Properties.Resources.MenuAuto,      Orientation.Auto      },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// ColorModes
    ///
    /// <summary>
    /// Gets a collection in which each item consists of a display
    /// string and a ColorMode pair.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static ComboListSource<ColorMode> ColorModes => new()
    {
        { Properties.Resources.MenuAuto,       ColorMode.SameAsSource },
        { Properties.Resources.MenuRgb,        ColorMode.Rgb          },
        { Properties.Resources.MenuGrayscale,  ColorMode.Grayscale    },
        { Properties.Resources.MenuMonochrome, ColorMode.Monochrome   },
    };

    #endregion

    #region FileDialogFilter

    /* --------------------------------------------------------------------- */
    ///
    /// GetSourceFilters
    ///
    /// <summary>
    /// Gets a collection of OpenFileDialog filters.
    /// </summary>
    ///
    /// <param name="src">Extension list.</param>
    ///
    /// <returns>Collection of dialog filter objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static IList<FileDialogFilter> GetSourceFilters(this ExtensionList src) => new FileDialogFilter[]
    {
        new(Properties.Resources.FilterPs,  src.GetCandidates(Format.Ps ).ToArray()),
        new(Properties.Resources.FilterEps, src.GetCandidates(Format.Eps).ToArray()),
        new(Properties.Resources.FilterPdf, src.GetCandidates(Format.Pdf).ToArray()),
        new(Properties.Resources.FilterAll, ".*"),
    };

    /* --------------------------------------------------------------------- */
    ///
    /// GetDestinationFilters
    ///
    /// <summary>
    /// Gets a collection of SaveFileDialog filters.
    /// </summary>
    ///
    /// <param name="src">Extension list.</param>
    ///
    /// <returns>Collection of dialog filter objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static IList<FileDialogFilter> GetDestinationFilters(this ExtensionList src) => new FileDialogFilter[]
    {
        new(Properties.Resources.FilterPdf,  src.GetCandidates(Format.Pdf ).ToArray()),
        new(Properties.Resources.FilterPs,   src.GetCandidates(Format.Ps  ).ToArray()),
        new(Properties.Resources.FilterEps,  src.GetCandidates(Format.Eps ).ToArray()),
        new(Properties.Resources.FilterPng,  src.GetCandidates(Format.Png ).ToArray()),
        new(Properties.Resources.FilterJpeg, src.GetCandidates(Format.Jpeg).ToArray()),
        new(Properties.Resources.FilterBmp,  src.GetCandidates(Format.Bmp ).ToArray()),
        new(Properties.Resources.FilterTiff, src.GetCandidates(Format.Tiff).ToArray()),
    };

    /* --------------------------------------------------------------------- */
    ///
    /// GetProgramFilters
    ///
    /// <summary>
    /// Gets a collection of OpenFileDialog filters.
    /// </summary>
    ///
    /// <param name="src">Extension list.</param>
    ///
    /// <returns>Collection of dialog filter objects.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static IList<FileDialogFilter> GetProgramFilters(this ExtensionList src) => new FileDialogFilter[]
    {
        new(Properties.Resources.FilterExecutable, ".exe", ".bat"),
        new(Properties.Resources.FilterAll, ".*"),
    };

    #endregion

    #region Uri

    /* --------------------------------------------------------------------- */
    ///
    /// ProductUri
    ///
    /// <summary>
    /// Gets the URL of the product Web page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static Uri ProductUri => MakeUri("https://www.cube-soft.jp/cubepdf/");

    /* --------------------------------------------------------------------- */
    ///
    /// DocumentUri
    ///
    /// <summary>
    /// Gets the URL of the document Web page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static Uri DocumentUri => MakeUri("https://docs.cube-soft.jp/entry/cubepdf");

    /* --------------------------------------------------------------------- */
    ///
    /// MakeUri
    ///
    /// <summary>
    /// Gets the Uri object from the specified URL string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Uri MakeUri(string src) =>
        new Uri(src).With("lang", CultureInfo.CurrentCulture.Name.ToLowerInvariant());

    #endregion
}
