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
using GalaSoft.MvvmLight;
using System;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelExtension
    ///
    /// <summary>
    /// Provides extended methods for ViewModel classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ViewModelExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new menu with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">ViewModel object.</param>
        /// <param name="getter">Function to get value.</param>
        /// <param name="gettext">Function to get text.</param>
        ///
        /// <returns>BindableElement(T) object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static BindableElement<T> Create<T>(this ViewModelBase src,
            Getter<T> getter, Getter<string> gettext) =>
            new BindableElement<T>(getter, gettext);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new menu with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">ViewModel object.</param>
        /// <param name="getter">Function to get value.</param>
        /// <param name="setter">Action to set value.</param>
        /// <param name="gettext">Function to get text.</param>
        ///
        /// <returns>BindableElement(T) object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static BindableElement<T> Create<T>(this ViewModelBase src,
            Getter<T> getter, Action<T> setter, Getter<string> gettext) =>
            new BindableElement<T>(getter, e => { setter(e); return true; }, gettext);

        #endregion
    }
}
