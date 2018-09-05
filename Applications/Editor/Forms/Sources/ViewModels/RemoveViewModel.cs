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
using System;
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
        /// <param name="n">Number of pages.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RemoveViewModel(int n, SynchronizationContext context) :
            base(() => Properties.Resources.TitleRemove, new Messenger(), context)
        {
            PageCount = new BindableElement<string>(
                () => string.Format(Properties.Resources.TooltipPageCount, n),
                e  => throw new InvalidOperationException(),
                () => Properties.Resources.MenuPageCount
            );
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PageCount
        ///
        /// <summary>
        /// Gets the menu that represents the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> PageCount { get; }

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
            () => Properties.Resources.TooltipRemoveRange,
            e  => throw new InvalidOperationException(),
            () => Properties.Resources.MenuRemoveRange
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Range
        ///
        /// <summary>
        /// Gets or sets the value that reprensetns the removal range.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bindable<string> Range { get; } = new Bindable<string>(string.Empty);

        #endregion
    }
}
