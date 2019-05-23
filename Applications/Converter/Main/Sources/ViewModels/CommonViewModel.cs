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
using Cube.Mixin.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// CommonViewModel
    ///
    /// <summary>
    /// Represents the base class of ViewModel classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class CommonViewModel : PresentableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CommonViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CommonViewModel with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected CommonViewModel(Aggregator aggregator, SynchronizationContext context) :
            base(aggregator, context) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Send
        ///
        /// <summary>
        /// Sends the specified message and invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Send<T>(T message, Action<T> next)
        {
            Send(message);
            Track(() => next(message), MessageFactory.Create, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Confirm
        ///
        /// <summary>
        /// Sends the specified dialog message and determines if the status
        /// is OK.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected bool Confirm(DialogMessage message)
        {
            Confirm(message);
            return message.Status != DialogStatus.Ok;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TrackClose
        ///
        /// <summary>
        /// Invokes the specified action as an asynchronous manner and
        /// sends the close message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Task TrackClose(Action action) => Task.Run(() =>
        {
            try { action(); }
            catch (OperationCanceledException) { /* ignore */ }
            catch (Exception err)
            {
                this.LogError(err);
                Confirm(MessageFactory.Create(err));
            }
            finally { Post<CloseMessage>(); }
        });

        #endregion
    }
}
