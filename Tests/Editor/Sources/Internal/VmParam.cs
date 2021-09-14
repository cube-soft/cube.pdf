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
namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// VmParam
    ///
    /// <summary>
    /// Represents the data to be set for the properties of the message
    /// published by the ViewModel.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class VmParam
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets or sets the loading path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the saving path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; }
    }
}
