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
using Cube.FileSystem.Tests;
using Cube.Pdf.App.Editor;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelFixture
    ///
    /// <summary>
    /// Provides functionality to test throw ViewModel classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewModelFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Gets a new MainViewModel instance.
        /// </summary>
        ///
        /// <returns>MainViewModel object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected MainViewModel Create()
        {
            var dest = new MainViewModel();
            dest.Images.Loading = new BitmapImage(new Uri(GetExamplesWith("Loading.png")));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Wait
        ///
        /// <summary>
        /// Waits for the result of the specified predicate to be true.
        /// </summary>
        ///
        /// <param name="predicate">Predicate.</param>
        ///
        /// <returns>false for the timeout.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected bool Wait(Func<bool> predicate) => WaitAsync(predicate).Result;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WaitAsync
        ///
        /// <summary>
        /// Waits for the result of the specified predicate to be true
        /// as an asynchronous operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async Task<bool> WaitAsync(Func<bool> predicate)
        {
            for (var i = 0; i < 100; ++i)
            {
                if (predicate()) return true;
                await Task.Delay(100).ConfigureAwait(false);
            }
            return false;
        }

        #endregion
    }
}
