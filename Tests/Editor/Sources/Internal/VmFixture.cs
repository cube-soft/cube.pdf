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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Cube.Collections.Extensions;
using Cube.DataContract;
using Cube.Tests;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// VmFixture
    ///
    /// <summary>
    /// Provides functionality to test the ViewModel classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class VmFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// NewVM
        ///
        /// <summary>
        /// Creates a new instance of the MainViewModel class.
        /// </summary>
        ///
        /// <returns>MainViewModel object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected MainViewModel NewVM()
        {
            var src   = new SettingFolder(Format.Json, Get("Settings.json"));
            var dest  = new MainViewModel(src, new());
            var dummy = new BitmapImage(new(GetSource("Loading.png")));

            dest.Value.Settings.Language = Language.English;
            dest.Value.Images.Preferences.Dummy = dummy;
            dest.Value.Images.Preferences.VisibleFirst = 0;
            dest.Value.Images.Preferences.VisibleLast = 10;

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets a path with the specified arguments.
        /// </summary>
        ///
        /// <param name="args">Arguments to determine the path.</param>
        /// <param name="name">Name used for a part of the path.</param>
        ///
        /// <returns>Full path.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Get(IEnumerable<object> args, [CallerMemberName] string name = null) =>
           Get($"{name}-{args.Join("", e => e.ToString())}.pdf");

        /* ----------------------------------------------------------------- */
        ///
        /// Args
        ///
        /// <summary>
        /// Converts from the specified argument to a collection.
        /// </summary>
        ///
        /// <param name="src">Source arguments.</param>
        ///
        /// <returns>Collection object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected IEnumerable<object> Args(params object[] src) => src;

        #endregion
    }
}
