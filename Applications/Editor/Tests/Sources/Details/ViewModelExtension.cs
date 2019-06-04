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
using Cube.Mixin.Commands;
using Cube.Tests;
using Cube.Xui;
using NUnit.Framework;
using System;
using System.Threading;

namespace Cube.Pdf.Editor.Tests
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
        /// Tests the command of the specified element.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        /// <param name="src">Target element.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test(this MainViewModel vm, BindableElement src)
        {
            var cs = new CancellationTokenSource();
            void observe(object s, EventArgs e)
            {
                if (vm.Data.Busy.Value) return;
                vm.Data.Busy.PropertyChanged -= observe;
                cs.Cancel();
            }

            Assert.That(vm.Data.Busy.Value, Is.False, $"Busy ({src.Text})");
            vm.Data.Busy.PropertyChanged += observe;
            Assert.That(src.Command.CanExecute(), Is.True, $"CanExecute ({src.Text})");
            src.Command.Execute();
            Assert.That(Wait.For(cs.Token), $"Timeout ({src.Text})");
        }

        #endregion
    }
}
