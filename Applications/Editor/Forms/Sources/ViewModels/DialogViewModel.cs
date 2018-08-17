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
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// DialogViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a dialog window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class DialogViewModel : MessengerViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DialogViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the DialogViewModel with the
        /// specified argumetns.
        /// </summary>
        ///
        /// <param name="title">Title for the dialog.</param>
        /// <param name="messenger">Messenger object.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected DialogViewModel(Func<string> title, IMessenger messenger,
            SynchronizationContext context) : base(messenger, context)
        {
            Title = new MenuEntry(title);
            Cancel.Command = new RelayCommand(() => Send<CloseMessage>());
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets the binding object for the title component.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry Title { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// OK
        ///
        /// <summary>
        /// Gets or sets the binding object for the OK button or menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry OK { get; set; } = new MenuEntry(
            () => Properties.Resources.MenuOk
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Gets or sets the binding object for the Cancel button or menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry Cancel { get; set; } = new MenuEntry(
            () => Properties.Resources.MenuCancel
        );

        #endregion
    }
}
