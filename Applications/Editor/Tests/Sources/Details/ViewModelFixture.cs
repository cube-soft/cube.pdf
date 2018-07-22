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
using Cube.FileSystem.TestService;
using Cube.Pdf.App.Editor;
using Cube.Xui;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System;
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
        /// Gets a new instance of the MainViewModel class and execute
        /// the specified action.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Create(Action<MainViewModel> action)
        {
            //using (var src = new MainViewModel())
            var src = new MainViewModel();
            {
                var dummy = new BitmapImage(new Uri(GetExamplesWith("Loading.png")));

                src.Data.Preferences.Dummy = dummy;
                src.Data.Preferences.VisibleFirst = 0;
                src.Data.Preferences.VisibleLast = 10;

                action(src);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Gets a new instance of the MainViewModel class, executes
        /// the Open command, and runs the specified action.
        /// </summary>
        ///
        /// <param name="action">User action.</param>
        /// <param name="src">File path to open.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Create(string src, Action<MainViewModel> action) => Create(vm =>
        {
            ExecuteOpen(vm, src);
            action(vm);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Execute the specified command.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        /// <param name="src">RibbonEntry that has the command.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Execute(MainViewModel vm, RibbonEntry src)
        {
            vm.Data.Message.Value = string.Empty;
            Assert.That(vm.Data.IsBusy.Value, Is.False, nameof(vm.Data.IsBusy));
            Assert.That(src.Command.CanExecute(), Is.True, nameof(src.Command.CanExecute));

            src.Command.Execute();
            Assert.That(Wait.For(() => !vm.Data.IsBusy.Value), "Timeout");
            Assert.That(vm.Data.Message.Value, Is.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExecuteOpen
        ///
        /// <summary>
        /// Execute the Open command with the specified file.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        /// <param name="src">File path to open.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void ExecuteOpen(MainViewModel vm, string src)
        {
            void open(OpenFileMessage e)
            {
                e.FileName = src;
                e.Result   = true;
                e.Callback.Invoke(e);
            };

            vm.Messenger.Register<OpenFileMessage>(this, open);
            Execute(vm, vm.Ribbon.Open);
            Assert.That(Wait.For(() => vm.Data.IsOpen.Value), nameof(vm.Ribbon.Open));
            vm.Messenger.Unregister<OpenFileMessage>(this, open);
        }

        #endregion
    }
}
