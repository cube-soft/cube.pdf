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
using System;
using System.Linq;

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
        #region Save

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
        /// Save the PDF document
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Saving file information.</param>
        /// <param name="callback">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainFacade src, Entity dest, Action callback)
        {
            var data = src.Value;
            var tmp  = data.IO.Combine(dest.DirectoryName, Guid.NewGuid().ToString("D"));

            try
            {
                var reader = data.Source.GetItexReader(data.Query, data.IO);
                data.Set(reader.Metadata, reader.Encryption);

                using (var writer = new Itext.DocumentWriter())
                {
                    writer.Add(reader.Attachments);
                    writer.Add(data.Images.Select(e => e.RawObject), reader);
                    writer.Set(data.Metadata);
                    writer.Set(data.Encryption);
                    writer.Save(tmp);
                }

                callback();
                src.Backup.Invoke(dest);
                data.IO.Copy(tmp, dest.FullName, true);
            }
            finally { _ = data.IO.TryDelete(tmp); }
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
        public static void Overwrite(this MainFacade src)
        {
            if (src.Value.History.Undoable) src.Save(src.Value.Source.FullName);
        }

        #endregion
    }
}
