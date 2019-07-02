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
namespace Cube.Pdf.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipItem
    ///
    /// <summary>
    /// Represents an attached item to a PDF.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ClipItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ClipItem
        ///
        /// <summary>
        /// Initializes a new instance of the ClipItem class with the
        /// specified object.
        /// </summary>
        ///
        /// <param name="raw">Raw attachment data.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ClipItem(Attachment raw)
        {
            RawObject = raw;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the name of the attachment.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name => RawObject.Name;

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// Gets the file size of the attachment.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length => RawObject.Length;

        /* ----------------------------------------------------------------- */
        ///
        /// Status
        ///
        /// <summary>
        /// Gets a value that represents the current condition.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Status { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// Gets the raw object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Attachment RawObject { get; }

        #endregion
    }
}
