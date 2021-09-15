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
using System.Linq;
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
    internal static class VmExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Boot
        ///
        /// <summary>
        /// Hooks some messages, opens the specified PDF file, and makes it
        /// ready for testing.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel object.</param>
        /// <param name="vp">Dataset for testing.</param>
        ///
        /// <returns>Disposable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable Boot(this MainViewModel vm, VmParam vp)
        {
            var dest = Hook(vm, vp);

            vm.Test(vm.Ribbon.Open);
            vm.Test(vm.Ribbon.Redraw);

            var obj = vm.Value.Images.First();
            var cmp = vm.Value.Images.Preferences.Dummy;

            Assert.That(Wait.For(() => obj.Image != cmp), "Timeout (Boot)");

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the command of the specified element.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel object.</param>
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
            Assert.That(src.Command.CanExecute(), $"CanExecute ({src.Text})");
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
        /// <param name="vm">Bindable source.</param>
        /// <param name="vp">Dataset for testing.</param>
        ///
        /// <returns>Disposable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable Hook(this IBindable vm, VmParam vp) => new DisposableContainer(
            vm.Subscribe<DialogMessage    >(e => vm.GetType().LogDebug($"{e.Icon}", e.Text)),
            vm.Subscribe<OpenFileMessage  >(e => e.Value = new[] { vp.Source }),
            vm.Subscribe<SaveFileMessage  >(e => e.Value = vp.Save),
            vm.Subscribe<PasswordViewModel>(e => {
                Assert.That(e.Title, Is.Not.Null.And.Not.Empty);
                Assert.That(e.Password.Text, Is.Not.Null.And.Not.Empty);
                Assert.That(e.Password.Value, Is.Null);

                e.Password.Value = vp.Password;
                var dest = vp.Password.HasValue() ? e.OK : e.Cancel;
                Assert.That(dest.Command.CanExecute(), Is.True, dest.Text);
                dest.Command.Execute();
            })
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Hook
        ///
        /// <summary>
        /// Sets some dummy callbacks to the specified Messenger.
        /// </summary>
        ///
        /// <param name="vm">Bindable source.</param>
        ///
        /// <returns>Disposable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable Hook(this IBindable vm) => Hook(vm, new());

        #endregion
    }
}
