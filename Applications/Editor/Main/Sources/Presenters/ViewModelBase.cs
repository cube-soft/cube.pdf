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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelBase(TModel)
    ///
    /// <summary>
    /// Represents the base class of ViewModels.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ViewModelBase<TModel> : Presentable<TModel>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// GenericViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the GenericViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="model">Model object.</param>
        /// <param name="aggregator">Messenger object.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ViewModelBase(TModel model, Aggregator aggregator, SynchronizationContext context) :
            base(model, aggregator, context) { }

        #endregion
    }
}
