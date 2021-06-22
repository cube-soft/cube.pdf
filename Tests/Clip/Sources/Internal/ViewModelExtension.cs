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
using Cube.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Cube.Pdf.Clip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelExtension
    ///
    /// <summary>
    /// Provides extended methods of the MainViewModel class for testing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class ViewModelExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the specified action.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        /// <param name="action">Target action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static bool Test(this MainViewModel vm, Action action)
        {
            Assert.That(vm.Busy, Is.False, nameof(Test));
            var cs = new CancellationTokenSource();
            void observe(object s, EventArgs e)
            {
                if (vm.Busy) return;
                vm.PropertyChanged -= observe;
                cs.Cancel();
            }

            vm.PropertyChanged += observe;
            try
            {
                action();
                return Wait.For(cs.Token);
            }
            finally { vm.PropertyChanged -= observe; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetClips
        ///
        /// <summary>
        /// Gets the collection of attached files.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        ///
        /// <returns>Collection of attached files.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<ClipItem> GetClips(this MainViewModel vm) => (IEnumerable<ClipItem>)vm.Clips.DataSource;

        #endregion
    }
}
