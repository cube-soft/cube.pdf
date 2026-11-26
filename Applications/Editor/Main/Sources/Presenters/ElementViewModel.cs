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
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ElementViewModel(TModel)
    ///
    /// <summary>
    /// Represents a ViewModel that owns its bindable elements.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ElementViewModel<TModel> : PresentableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ElementViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ElementViewModel class.
        /// </summary>
        ///
        /// <param name="model">Model object.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        /// <param name="owner">Whether to dispose the model with the ViewModel.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ElementViewModel(TModel model, Aggregator aggregator,
            SynchronizationContext context, bool owner = true
        ) : base(aggregator, context)
        {
            Facade = model;
            if (owner && model is IDisposable e) Assets.Add(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Facade
        ///
        /// <summary>
        /// Gets the model used by the ViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected TModel Facade { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the value of the specified property. The value is
        /// created only once and released together with the ViewModel.
        /// </summary>
        ///
        /// <param name="creator">Function to create the value.</param>
        /// <param name="name">Name of the property.</param>
        ///
        /// <returns>Value of the property.</returns>
        ///
        /// <remarks>
        /// Bindable elements keep a subscription to the language setting
        /// until they are disposed, so they must not outlive the dialog.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected new T Get<T>(Func<T> creator, [CallerMemberName] string name = null) => base.Get(() =>
        {
            var dest = creator();
            if (dest is IDisposable e) Assets.Add(e);
            return dest;
        }, name);

        #endregion
    }
}
