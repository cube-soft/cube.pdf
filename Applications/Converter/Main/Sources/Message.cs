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
using System.Security.Cryptography;
using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// Message
///
/// <summary>
/// Provides functionality to create message objects.
/// </summary>
///
/* ------------------------------------------------------------------------- */
static class Message
{
    #region DialogMessage

    /* --------------------------------------------------------------------- */
    ///
    /// From
    ///
    /// <summary>
    /// Create a message to show a DialogBox with an error icon
    /// and OK button.
    /// </summary>
    ///
    /// <param name="src">Occurred exception.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static DialogMessage From(Exception src) => Error(GetMessage(src));

    /* --------------------------------------------------------------------- */
    ///
    /// From
    ///
    /// <summary>
    /// Create a message to show a DialogBox with a warning icon
    /// and OK/Cancel buttons.
    /// </summary>
    ///
    /// <param name="src">Path to save.</param>
    /// <param name="option">Save option.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static DialogMessage From(string src, SaveOption option) =>
        Warn(GetMessage(src, option));

    /* --------------------------------------------------------------------- */
    ///
    /// Error
    ///
    /// <summary>
    /// Create a message to show a DialogBox with an error icon
    /// and OK button.
    /// </summary>
    ///
    /// <param name="src">Error message.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static DialogMessage Error(string src) => new()
    {
        Text    = src,
        Title   = "CubePDF",
        Icon    = DialogIcon.Error,
        Buttons = DialogButtons.Ok,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// Warn
    ///
    /// <summary>
    /// Create a message to show a DialogBox with a warning icon
    /// and OK/Cancel buttons.
    /// </summary>
    ///
    /// <param name="src">Warning message.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static DialogMessage Warn(string src) => new()
    {
        Text    = src,
        Title   = "CubePDF",
        Icon    = DialogIcon.Warning,
        Buttons = DialogButtons.YesNo,
        CancelCandidates = new[] { DialogStatus.No, DialogStatus.Cancel },
    };

    #endregion

    #region OpenOrSaveFileMessage

    /* --------------------------------------------------------------------- */
    ///
    /// ForSource
    ///
    /// <summary>
    /// Creates a message to show an OpenFileDialog dialog for
    /// selecting the source path.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /// <returns>OpenFileMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static OpenFileMessage ForSource(SettingFolder src)
    {
        var path = src.Value.Source;
        var dest = new OpenFileMessage
        {
            Text        = Properties.Resources.TitleSelectSource,
            Value       = GetFileNames(path),
            Multiselect = false,
            Filters     = Resource.SourceFilters,
        };

        if (src.Value.Appendix.ExplicitDirectory) dest.InitialDirectory = GetDirectoryName(path);
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ForDestination
    ///
    /// <summary>
    /// Creates a message to show an OpenFileDialog dialog for
    /// selecting the destination path.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /// <returns>SaveFileMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static SaveFileMessage ForDestination(SettingFolder src)
    {
        var path = src.Value.Destination;
        var dest = new SaveFileMessage
        {
            Text            = Properties.Resources.TitleSelectDestination,
            Value           = GetFileName(path),
            OverwritePrompt = false,
            Filters         = Resource.DestinationFilters,
        };

        if (src.Value.Appendix.ExplicitDirectory) dest.InitialDirectory = GetDirectoryName(path);
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ForUserProgram
    ///
    /// <summary>
    /// Creates a message to show an OpenFileDialog dialog for
    /// selecting the user program.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /// <returns>OpenFileMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static OpenFileMessage ForUserProgram(SettingFolder src)
    {
        var path = src.Value.UserProgram;
        var dest = new OpenFileMessage
        {
            Text        = Properties.Resources.TitleSelectUserProgram,
            Value       = GetFileNames(path),
            Multiselect = false,
            Filters     = Resource.UserProgramFilters,
        };

        if (src.Value.Appendix.ExplicitDirectory) dest.InitialDirectory = GetDirectoryName(path);
        return dest;
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetMessage
    ///
    /// <summary>
    /// Gets an error message from the specified exception.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetMessage(Exception src) => src switch
    {
        CryptographicException => Properties.Resources.ErrorDigest,
        EncryptionException    => Properties.Resources.ErrorMergePassword,
        GsApiException e       => string.Format(Properties.Resources.ErrorGhostscript, e.Status),
        ArgumentException      => src.Message,
        _                      => $"{src.Message} ({src.GetType().Name})",
    };

    /* --------------------------------------------------------------------- */
    ///
    /// GetMessage
    ///
    /// <summary>
    /// Gets an warning message from the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetMessage(string src, SaveOption option)
    {
        var s0 = string.Format(Properties.Resources.WarnExists, src);
        return $"{s0} {GetMessage(option)}";
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetMessage
    ///
    /// <summary>
    /// Gets the message from the specified save option.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetMessage(SaveOption src) => src switch
    {
        SaveOption.Overwrite => Properties.Resources.WarnOverwrite,
        SaveOption.MergeHead => Properties.Resources.WarnMergeHead,
        SaveOption.MergeTail => Properties.Resources.WarnMergeTail,
        _                    => string.Empty,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// GetFileName
    ///
    /// <summary>
    /// Gets a filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetFileName(string src) =>
        src.HasValue() ? Io.Get(src).Name : string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// GetFileNames
    ///
    /// <summary>
    /// Gets a sequence of filenames.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<string> GetFileNames(string src)
    {
        if (src.HasValue()) yield return Io.Get(src).Name;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDirectoryName
    ///
    /// <summary>
    /// Gets a directory name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetDirectoryName(string src) =>
        src.HasValue() ? Io.Get(src).DirectoryName : string.Empty;

    #endregion
}
