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

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertPosition
    ///
    /// <summary>
    /// Represents insert position menus of the InsertWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class InsertPosition : BindableElement
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
        /// <param name="data">Data object.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public InsertPosition(InsertBindable data, IDispatcher dispatcher) :
            base(() => Properties.Resources.MenuInsertPosition, dispatcher)
        {
            Command = new RelayCommand<int>(e => data.Index.Value = e);

            First = new BindableElement(() => Properties.Resources.MenuPositionFirst, Dispatcher);
            Last  = new BindableElement(() => Properties.Resources.MenuPositionLast, Dispatcher);

            Selected = new BindableElement<bool>(
                () => data.SelectedIndex >= 0,
                () => Properties.Resources.MenuPositionSelected,
                Dispatcher);

            UserSpecified = new BindableElement<int>(
                () => data.UserSpecifiedIndex.Value + 1,
                e => { data.UserSpecifiedIndex.Value = e - 1; return true; },
                () => Properties.Resources.MenuPositionSpecified,
                Dispatcher);

            UserSpecifiedSuffix = new BindableElement(
                () => string.Format($"/ {Properties.Resources.MessagePage}", data.Count),
                Dispatcher);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// First
        ///
        /// <summary>
        /// Gets the menu that represents the begging of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement First { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Last
        ///
        /// <summary>
        /// Gets the menu that represents the end of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement Last { get; }

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
        public BindableElement<bool> Selected { get; }

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
        public BindableElement<int> UserSpecified { get; }

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
        public BindableElement UserSpecifiedSuffix { get; }

        #endregion
    }
}
