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
using Cube.Generics;
using Cube.Xui;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RemoveViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a RemoveWindow instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RemoveViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the RemoveViewModel with the
        /// specified argumetns.
        /// </summary>
        ///
        /// <param name="callback">Callback method when applied.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RemoveViewModel(Action<IEnumerable<int>> callback, int n, SynchronizationContext context) :
            base(() => Properties.Resources.TitleRemove, new Messenger(), context)
        {
            PageCaption = new BindableElement<string>(
                () => string.Format(Properties.Resources.MessagePage, n),
                () => Properties.Resources.MenuPageCount
            );

            OK.Command = new BindableCommand(
                () => Post(() => Execute(callback, n)),
                () => Range.Value.HasValue(),
                Range
            );
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Range
        ///
        /// <summary>
        /// Gets the removal range.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Range { get; } = new Bindable<string>(string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// RangeCaption
        ///
        /// <summary>
        /// Gets the menu that represents the caption of the removal
        /// range.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> RangeCaption { get; } = new BindableElement<string>(
            () => Properties.Resources.MessageRemoveRange,
            () => Properties.Resources.MenuRemoveRange
        );

        /* ----------------------------------------------------------------- */
        ///
        /// PageCaption
        ///
        /// <summary>
        /// Gets the menu that represents the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> PageCaption { get; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Executes the main command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Execute(Action<IEnumerable<int>> callback, int n)
        {
            var dest = new Range(Range.Value, n).Select(i => i - 1);
            callback(dest);
            Send<CloseMessage>();
        }

        #endregion
    }
}
