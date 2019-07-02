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
using Cube.Mixin.Syntax;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Pdf.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelBase(T)
    ///
    /// <summary>
    /// Represents the base class for the ViewModels.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ViewModelBase<TFacade> : PresentableBase where TFacade : IDisposable
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ViewModelBase class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade of models.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* --------------------------------------------------------------------- */
        protected ViewModelBase(TFacade facade, Aggregator aggregator, SynchronizationContext context) :
            base(aggregator, context)
        {
            Facade = facade;
        }

        #endregion

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// Facade
        ///
        /// <summary>
        /// Gets the facade of models.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        protected TFacade Facade { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) =>
            disposing.Then(() => Facade?.Dispose());

        /* ----------------------------------------------------------------- */
        ///
        /// Send
        ///
        /// <summary>
        /// Sends the specified message, and invokes the specified action
        /// when not canceled by the user.
        /// </summary>
        ///
        /// <param name="message">Message to send.</param>
        /// <param name="next">
        /// Action to be invoked when not canceled by the user.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected async Task Send(OpenFileMessage message, Action<IEnumerable<string>> next)
        {
            Send(message);
            if (message.Cancel) return;
            await Track(() => next(message.Value));
        }

        #endregion
    }
}
