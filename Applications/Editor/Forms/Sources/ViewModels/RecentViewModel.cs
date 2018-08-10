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
using Cube.Xui;
using GalaSoft.MvvmLight.Messaging;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RecentViewModel
    ///
    /// <summary>
    /// Provides binding properties and commands for the recently used
    /// PDF files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RecentViewModel : MessengerViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// HeroViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the RecentViewModel with the
        /// specified argumetns.
        /// </summary>
        ///
        /// <param name="items">Recently used PDF files.</param>
        /// <param name="messenger">Messenger object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RecentViewModel(DirectoryMonitor items, IMessenger messenger) : base(messenger)
        {
            Items = items;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the collection of the recently used PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DirectoryMonitor Items { get; }

        #endregion
    }
}
