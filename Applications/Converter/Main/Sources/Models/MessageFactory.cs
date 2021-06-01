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
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MessageFactory
    ///
    /// <summary>
    /// Provides functionality to create message objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class MessageFactory
    {
        #region DialogMessage

        /* ----------------------------------------------------------------- */
        ///
        /// Create
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
        /* ----------------------------------------------------------------- */
        public static DialogMessage Create(Exception src) =>
            CreateError(GetErrorMessage(src));

        /* ----------------------------------------------------------------- */
        ///
        /// Create
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
        /* ----------------------------------------------------------------- */
        public static DialogMessage Create(string src, SaveOption option) =>
            CreateWarn(GetWarnMessage(src, option));

        /* ----------------------------------------------------------------- */
        ///
        /// CreateError
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
        /* ----------------------------------------------------------------- */
        public static DialogMessage CreateError(string src) => new DialogMessage
        {
            Text    = src,
            Title   = Properties.Resources.TitleError,
            Icon    = DialogIcon.Error,
            Buttons = DialogButtons.Ok,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateWarn
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
        /* ----------------------------------------------------------------- */
        public static DialogMessage CreateWarn(string src) => new DialogMessage
        {
            Text    = src,
            Title   = Properties.Resources.TitleWarning,
            Icon    = DialogIcon.Warning,
            Buttons = DialogButtons.YesNo,
        };

        #endregion

        #region OpenOrSaveFileMessage

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForSource
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
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage CreateForSource(this SettingFolder src)
        {
            var path = src.Value.Source;
            var dest = new OpenFileMessage
            {
                Text        = Properties.Resources.TitleSelectSource,
                Value       = GetFileNames(path, src.IO),
                Multiselect = false,
                Filter      = ViewResource.SourceFilters.GetFilter(),
                FilterIndex = ViewResource.SourceFilters.GetFilterIndex(path, src.IO),
            };

            if (src.Value.ExplicitDirectory) dest.InitialDirectory = GetDirectoryName(path, src.IO);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForDestination
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
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage CreateForDestination(this SettingFolder src)
        {
            var path = src.Value.Destination;
            var dest = new SaveFileMessage
            {
                Text            = Properties.Resources.TitleSelectDestination,
                Value           = GetFileName(path, src.IO),
                OverwritePrompt = false,
                Filter          = ViewResource.DestinationFilters.GetFilter(),
                FilterIndex     = ViewResource.DestinationFilters.GetFilterIndex(path, src.IO),
            };

            if (src.Value.ExplicitDirectory) dest.InitialDirectory = GetDirectoryName(path, src.IO);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForUserProgram
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
        /* ----------------------------------------------------------------- */
        public static OpenFileMessage CreateForUserProgram(this SettingFolder src)
        {
            var path = src.Value.UserProgram;
            var dest = new OpenFileMessage
            {
                Text        = Properties.Resources.TitleSelectUserProgram,
                Value       = GetFileNames(path, src.IO),
                Multiselect = false,
                Filter      = ViewResource.UserProgramFilters.GetFilter(),
            };

            if (src.Value.ExplicitDirectory) dest.InitialDirectory = GetDirectoryName(path, src.IO);
            return dest;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetErrorMessage
        ///
        /// <summary>
        /// Gets an error message from the specified exception.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetErrorMessage(Exception src)
        {
            if (src is CryptographicException) return Properties.Resources.MessageDigest;
            if (src is EncryptionException) return Properties.Resources.MessageMergePassword;
            if (src is GsApiException gs) return string.Format(Properties.Resources.MessageGhostscript, gs.Status);
            if (src is ArgumentException e) return e.Message;
            return $"{src.Message} ({src.GetType().Name})";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetWarnMessage
        ///
        /// <summary>
        /// Gets an warning message from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetWarnMessage(string src, SaveOption option)
        {
            var s0 = string.Format(Properties.Resources.MessageExists, src);
            var ok = new Dictionary<SaveOption, string>
            {
                { SaveOption.Overwrite, Properties.Resources.MessageOverwrite },
                { SaveOption.MergeHead, Properties.Resources.MessageMergeHead },
                { SaveOption.MergeTail, Properties.Resources.MessageMergeTail },
            }.TryGetValue(option, out var s1);

            Debug.Assert(ok);
            return $"{s0} {s1}";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets a filename without extension.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetFileName(string src, IO io) =>
            src.HasValue() ? io.Get(src).BaseName : string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileNames
        ///
        /// <summary>
        /// Gets a sequence of filenames.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<string> GetFileNames(string src, IO io)
        {
            if (src.HasValue()) yield return io.Get(src).BaseName;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// Gets a directory name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetDirectoryName(string src, IO io) =>
            src.HasValue() ? io.Get(src).DirectoryName : string.Empty;

        #endregion
    }
}
