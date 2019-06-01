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
using Cube.Mixin.String;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        public RemoveViewModel(Action<IEnumerable<int>> callback,
            int n,
            SynchronizationContext context
        ) : base(() => Properties.Resources.TitleRemove, new Aggregator(), context)
        {
            _count = n;
            Range = new Bindable<string>(string.Empty, GetDispatcher(false));
            OK.Command = new DelegateCommand(
                () => Track(() =>
                {
                    callback(new Range(Range.Value, _count).Select(i => i - 1));
                    Send<CloseMessage>();
                }),
                () => Range.Value.HasValue()
            ).Observe(Range);
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
        public Bindable<string> Range { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RangeCaption
        ///
        /// <summary>
        /// Gets a menu that represents the caption of the removal range.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> RangeCaption => Get(() => new BindableElement<string>(
            () => Properties.Resources.MessageRemoveRange,
            () => Properties.Resources.MenuRemoveRange,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// PageCaption
        ///
        /// <summary>
        /// Gets a menu that represents the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> PageCaption => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuPageCount,
            () => string.Format(Properties.Resources.MessagePage, _count),
            GetDispatcher(false)
        ));

        #endregion

        #region Fields
        private readonly int _count;
        #endregion
    }
}
