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
using Cube.Mixin.Syntax;
using System;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveExtension
    ///
    /// <summary>
    /// Represents the extended methods to save documents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class SaveExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the PDF document to the specified file path.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Path to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainFacade src, string dest) => src.Save(dest, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the PDF document to the specified file path.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Path to save.</param>
        /// <param name="reopen">
        /// Value indicating whether to reopen the document.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainFacade src, string dest, bool reopen) => src.Save(
            dest,
            e => { src.Backup.Invoke(e); src.Cache?.Clear(); },
            e => { if (reopen) src.ReOpen(e.FullName); }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Save the PDF document
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Saving file information.</param>
        /// <param name="prev">Action to be invoked before saving.</param>
        /// <param name="next">Action to be invoked after saving.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainFacade src, string dest, Action<Entity> prev, Action<Entity> next)
        {
            var obj   = src.Value;
            var itext = obj.Source.GetItexReader(obj.Query, obj.IO);
            obj.Set(itext.Metadata, itext.Encryption);

            src.Save(itext, new SaveOption(obj.IO)
            {
                Target      = SaveTarget.All,
                Split       = false,
                Destination = dest,
                Metadata    = obj.Metadata,
                Encryption  = obj.Encryption,
                Attachments = itext.Attachments,
            }, prev, next);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// Overwrites the PDF document.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Overwrite(this MainFacade src) =>
            src.Value.History.Undoable.Then(() => src.Save(src.Value.Source.FullName));

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts PDF pages saves to a file with the specified options.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="options">Save options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this MainFacade src, SaveOption options) =>
            src.Save(null, options, e => src.Backup.Invoke(e), e => { });

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
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
        public static void Extract(this MainFacade src, string dest) =>
            src.Extract(new SaveOption(src.Value.IO)
        {
            Target      = SaveTarget.Selected,
            Split       = false,
            Destination = dest,
        });

        #endregion
    }
}
