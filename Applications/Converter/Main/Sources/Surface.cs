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
using System.Linq;
using Cube.Forms;
using Cube.Globalization;
using Cube.Pdf.Ghostscript;
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
        { "Simplified Chinese",      Language.SimplifiedChinese },
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
        { Texts.General_Overwrite, SaveOption.Overwrite },
        { Texts.General_MergeHead, SaveOption.MergeHead },
        { Texts.General_MergeTail, SaveOption.MergeTail },
        { Texts.General_Rename,    SaveOption.Rename    },
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
        { Texts.Metadata_SinglePage,     ViewerOption.SinglePage     },
        { Texts.Metadata_OneColumn,      ViewerOption.OneColumn      },
        { Texts.Metadata_TwoPageLeft,    ViewerOption.TwoPageLeft    },
        { Texts.Metadata_TwoPageRight,   ViewerOption.TwoPageRight   },
        { Texts.Metadata_TwoColumnLeft,  ViewerOption.TwoColumnLeft  },
        { Texts.Metadata_TwoColumnRight, ViewerOption.TwoColumnRight },
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
        { Texts.General_Open,          PostProcess.Open          },
        { Texts.General_OpenDirectory, PostProcess.OpenDirectory },
        { Texts.General_None,          PostProcess.None          },
        { Texts.General_UserProgram,   PostProcess.Others        },
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
        { Texts.General_Portrait,  Orientation.Portrait  },
        { Texts.General_Landscape, Orientation.Landscape },
        { Texts.General_Auto,      Orientation.Auto      },
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
        { Texts.General_Auto,       ColorMode.SameAsSource },
        { Texts.General_Rgb,        ColorMode.Rgb          },
        { Texts.General_Grayscale,  ColorMode.Grayscale    },
        { Texts.General_Monochrome, ColorMode.Monochrome   },
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
        new(Texts.Filter_Ps,  src.GetCandidates(Format.Ps ).ToArray()),
        new(Texts.Filter_Eps, src.GetCandidates(Format.Eps).ToArray()),
        new(Texts.Filter_Pdf, src.GetCandidates(Format.Pdf).ToArray()),
        new(Texts.Filter_All, ".*"),
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
        new(Texts.Filter_Pdf,  src.GetCandidates(Format.Pdf ).ToArray()),
        new(Texts.Filter_Ps,   src.GetCandidates(Format.Ps  ).ToArray()),
        new(Texts.Filter_Eps,  src.GetCandidates(Format.Eps ).ToArray()),
        new(Texts.Filter_Png,  src.GetCandidates(Format.Png ).ToArray()),
        new(Texts.Filter_Jpeg, src.GetCandidates(Format.Jpeg).ToArray()),
        new(Texts.Filter_Bmp,  src.GetCandidates(Format.Bmp ).ToArray()),
        new(Texts.Filter_Tiff, src.GetCandidates(Format.Tiff).ToArray()),
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
        new(Texts.Filter_Exe, ".exe", ".bat"),
        new(Texts.Filter_All, ".*"),
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
        new Uri(src).With("lang", Locale.GetCurrentLanguage().ToCultureInfo().Name.ToLowerInvariant());

    #endregion
}
