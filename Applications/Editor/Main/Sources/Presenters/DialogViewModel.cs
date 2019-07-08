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
    /// DialogViewModel(TModel)
    ///
    /// <summary>
    /// Represents the ViewModel for a dialog window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class DialogViewModel<TModel> : ViewModelBase<TModel>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DialogViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the DialogViewModel with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="getTitle">Function to get title.</param>
        /// <param name="model">Model object.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected DialogViewModel(Getter<string> getTitle,
            TModel model,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(model, aggregator, context)
        {
            Title = Get(() => new BindableElement(getTitle, GetDispatcher(false)), nameof(Title));
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
        public IElement Title { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// OK
        ///
        /// <summary>
        /// Gets or sets the binding object for the OK button or menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement OK => Get(() => new BindableElement(
            () => Properties.Resources.MenuOk,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Gets or sets the binding object for the Cancel button or menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Cancel => Get(() => new BindableElement(
            () => Properties.Resources.MenuCancel,
            GetDispatcher(false)
        ) { Command = new DelegateCommand(() => Send<CloseMessage>()) });

        #endregion
    }
}
