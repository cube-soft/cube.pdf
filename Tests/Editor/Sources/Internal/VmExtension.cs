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
using System.ComponentModel;
using System.Threading;
using Cube.Logging;
using Cube.Mixin.Commands;
using Cube.Mixin.String;
using Cube.Tests;
using Cube.Xui;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// VmExtension
    ///
    /// <summary>
    /// Provides extended methods of the MainViewModel class for testing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class VmExtension
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
            var cts = new CancellationTokenSource();
            void observe(object s, PropertyChangedEventArgs e)
            {
                if (e.PropertyName != nameof(vm.Value.Busy) || vm.Value.Busy) return;
                vm.Value.PropertyChanged -= observe;
                cts.Cancel();
            }

            Assert.That(vm.Value.Busy, Is.False, $"Busy ({src.Text})");
            vm.Value.PropertyChanged += observe;
            Assert.That(src.Command.CanExecute(), Is.True, $"CanExecute ({src.Text})");
            src.Command.Execute();
            Assert.That(Wait.For(cts.Token), $"Timeout ({src.Text})");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Hook
        ///
        /// <summary>
        /// Sets some dummy callbacks to the specified Messenger.
        /// </summary>
        ///
        /// <param name="src">Bindable source.</param>
        ///
        /// <returns>Disposable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable Hook(this IBindable src) => Hook(src, new());

        /* ----------------------------------------------------------------- */
        ///
        /// Hook
        ///
        /// <summary>
        /// Sets some dummy callbacks to the specified Messenger.
        /// </summary>
        ///
        /// <param name="src">Bindable source.</param>
        /// <param name="data">Dataset for testing.</param>
        ///
        /// <returns>Disposable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable Hook(this IBindable src, VmParam data) => new DisposableContainer(
            src.Subscribe<DialogMessage    >(e => src.GetType().LogDebug($"{e.Icon}", e.Text)),
            src.Subscribe<OpenFileMessage  >(e => e.Value = new[] { data.Source }),
            src.Subscribe<SaveFileMessage  >(e => e.Value = data.Save),
            src.Subscribe<PasswordViewModel>(e => {
                Assert.That(e.Title, Is.Not.Null.And.Not.Empty);
                Assert.That(e.Password.Text, Is.Not.Null.And.Not.Empty);
                Assert.That(e.Password.Value, Is.Null);

                e.Password.Value = data.Password;
                var dest = data.Password.HasValue() ? e.OK : e.Cancel;
                Assert.That(dest.Command.CanExecute(), Is.True, dest.Text);
                dest.Command.Execute();
            })
        );

        #endregion
    }
}
