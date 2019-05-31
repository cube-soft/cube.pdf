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
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

namespace Cube.Pdf.Editor
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
    public class RecentViewModel : ViewModelBase
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
        /// <param name="Aggregator">Messenger object.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RecentViewModel(DirectoryMonitor items, Aggregator Aggregator,
            SynchronizationContext context) : base(Aggregator, context)
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

        /* ----------------------------------------------------------------- */
        ///
        /// Menu
        ///
        /// <summary>
        /// Gets the menu for recently used PDF files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Menu => Get(() => new BindableElement(
            () => Properties.Resources.MenuRecent, GetDispatcher(false)
        ) { Command = new BindableCommand(() => Track(() => Process.Start(Items.Directory))) });

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Gets the command to open the specified link.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Open { get; set; }

        #endregion
    }
}
