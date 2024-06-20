﻿/* ------------------------------------------------------------------------- */
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
using System.Threading;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PositionViewModel
    ///
    /// <summary>
    /// Represents the ViewModel of the insertion position menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PositionViewModel : PresentableBase<InsertBindableValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PositionViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the PositionViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source data.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PositionViewModel(InsertBindableValue src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(src, aggregator, context) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Gets a menu to select the position.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Select => Get(() => new BindableElement(
            () => Surface.Texts.Insert_Position,
            GetDispatcher(false)
        ) { Command = new DelegateCommand<int>(e => Facade.Index = e) });

        /* ----------------------------------------------------------------- */
        ///
        /// First
        ///
        /// <summary>
        /// Gets the menu that represents the begging of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement First => Get(() => new BindableElement(
            () => Surface.Texts.Insert_Position_Head,
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
        public IElement Last => Get(() => new BindableElement(
            () => Surface.Texts.Insert_Position_Tail,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndex
        ///
        /// <summary>
        /// Gets the menu that represents the selected position of the
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<int> SelectedIndex => Get(() => new BindableElement<int>(
            () => Surface.Texts.Insert_Position_Select,
            () => Facade.SelectedIndex,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// UserIndex
        ///
        /// <summary>
        /// Gets the menu that represents the user specified position
        /// of the document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<int> UserIndex => Get(() => new BindableElement<int>(
            () => Surface.Texts.Insert_Position_Custom,
            () => Facade.UserIndex + 1,
            e  => Facade.UserIndex = e - 1,
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
        public IElement<int> Count => Get(() => new BindableElement<int>(
            () => string.Format($"/ {Surface.Texts.Message_Pages}", Facade.Count),
            () => Facade.Count,
            GetDispatcher(false)
        ));

        #endregion
    }
}
