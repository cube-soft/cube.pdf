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
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertPosViewModel
    ///
    /// <summary>
    /// Represents the ViewModel of the insertion position menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class InsertPosViewModel : ViewModelBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PositionElement
        ///
        /// <summary>
        /// Initializes a new instance of the InsertPosition class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source data.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertPosViewModel(InsertBindable src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(aggregator, context) { _model = src; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// Gets a menu to execute main operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Main => Get(() => new BindableElement(
            () => Properties.Resources.MenuInsertPosition,
            GetDispatcher(false)
        ) { Command = new BindableCommand<int>(e => _model.Index.Value = e) });

        /* ----------------------------------------------------------------- */
        ///
        /// First
        ///
        /// <summary>
        /// Gets the menu that represents the begging of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement First => Get(() => new BindableElement(
            () => Properties.Resources.MenuPositionFirst,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Last
        ///
        /// <summary>
        /// Gets the menu that represents the end of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Last => Get(() => new BindableElement(
            () => Properties.Resources.MenuPositionLast,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Selected
        ///
        /// <summary>
        /// Gets the menu that represents the selected position of the
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<bool> Selected => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuPositionSelected,
            () => _model.SelectedIndex >= 0,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// UserSpecified
        ///
        /// <summary>
        /// Gets the menu that represents the user specified position
        /// of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<int> UserSpecified => Get(() => new BindableElement<int>(
            () => Properties.Resources.MenuPositionSpecified,
            () => _model.UserSpecifiedIndex.Value + 1,
            e  => _model.UserSpecifiedIndex.Value = e - 1,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// UserSpecifiedSuffix
        ///
        /// <summary>
        /// Gets the text that represents the suffix of UserSpecified
        /// menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement UserSpecifiedSuffix => Get(() => new BindableElement(
            () => string.Format($"/ {Properties.Resources.MessagePage}", _model.Count),
            GetDispatcher(false)
        ));

        #endregion

        #region Fields
        private readonly InsertBindable _model;
        #endregion
    }
}
