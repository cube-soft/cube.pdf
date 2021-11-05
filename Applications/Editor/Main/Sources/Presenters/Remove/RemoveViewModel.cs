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
using System;
using System.Collections.Generic;
using System.Threading;
using Cube.Mixin.Observing;
using Cube.Mixin.String;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RemoveViewModel
    ///
    /// <summary>
    /// Represents the ViewModel associated with the RemoveWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class RemoveViewModel : DialogViewModel<RemoveFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the RemoveViewModel with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="callback">Callback method when applied.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RemoveViewModel(Action<IEnumerable<int>> callback,
            int n,
            SynchronizationContext context
        ) : base(new(n, new ContextDispatcher(context, false)), new(), context)
        {
            OK.Command = new DelegateCommand(
                () => Quit(() => callback(Facade.Get()), false),
                () => Facade.Range.HasValue()
            ).Hook(Facade, nameof(Facade.Range));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the page count menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<int> Count => Get(() => new BindableElement<int>(
            () => Properties.Resources.MenuPageCount,
            () => Facade.Count,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Range
        ///
        /// <summary>
        /// Gets the menu of removal range.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Range => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuTarget,
            () => Facade.Range,
            e  => Facade.Range = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Example
        ///
        /// <summary>
        /// Gets menu of range example.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Example => Get(() => new BindableElement(
            () => Properties.Resources.MessageRangeExample,
            GetDispatcher(false)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        ///
        /// <returns>String value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Properties.Resources.TitleRemove;

        #endregion
    }
}
