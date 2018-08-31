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
using GalaSoft.MvvmLight.Messaging;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a <c>EncryptionWindow</c> instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EncryptionViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the <c>EncryptionViewModel</c>
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel(SynchronizationContext context) :
            base(() => Properties.Resources.TitleEncryption, new Messenger(), context) { }

        #endregion

        #region Properties

        public MenuEntry Enabled { get; } = new MenuEntry(
            () => Properties.Resources.MenuEncryptionEnabled
        );

        public MenuEntry OwnerPassword { get; } = new MenuEntry(
            () => Properties.Resources.MenuOwnerPassword
        );

        public MenuEntry UserPassword { get; } = new MenuEntry(
            () => Properties.Resources.MenuUserPassword
        );

        #endregion
    }
}
