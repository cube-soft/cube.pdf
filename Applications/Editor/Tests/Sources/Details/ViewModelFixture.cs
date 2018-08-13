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
using GalaSoft.MvvmLight.Messaging;
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
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets or sets a file path that uses in the OpenFileDialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets a file path that uses in the SaveFileDialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; set; }

        #endregion

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
            using (var src = new MainViewModel())
            //var src = new MainViewModel();
            {
                var dummy = new BitmapImage(new Uri(GetExamplesWith("Loading.png")));

                src.Data.Preferences.Dummy = dummy;
                src.Data.Preferences.VisibleFirst = 0;
                src.Data.Preferences.VisibleLast = 10;

                Register(src.Messenger);
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
        /// <param name="n">Number of pages.</param>
        /// <param name="filename">Filename of the source.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Create(string filename, int n, Action<MainViewModel> action) => Create(vm =>
        {
            Source = GetExamplesWith(filename);
            Execute(vm, vm.Ribbon.Open);
            Assert.That(Wait.For(() => vm.Data.Images.Count == n), nameof(vm.Ribbon.Open));
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
        protected void Execute(MainViewModel vm, MenuEntry src)
        {
            Assert.That(Wait.For(() => !vm.Data.IsBusy.Value), "Timeout");
            vm.Data.Message.Value = string.Empty;
            Assert.That(src.Command.CanExecute(), Is.True, nameof(src.Command.CanExecute));
            src.Command.Execute();
            Assert.That(Wait.For(() => !vm.Data.IsBusy.Value), "Timeout");
            Assert.That(vm.Data.Message.Value, Is.Empty);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Executes before each test.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [SetUp]
        private void Setup()
        {
            Source      = string.Empty;
            Destination = string.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Register some dummy callbacks to the specified Messenger.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Register(IMessenger src)
        {
            void open(OpenFileMessage e)
            {
                e.FileName = Source;
                e.Result   = true;
                e.Callback.Invoke(e);
            }

            void save(SaveFileMessage e)
            {
                e.FileName = Destination;
                e.Result   = true;
                e.Callback.Invoke(e);
            }

            src.Register<OpenFileMessage>(this, open);
            src.Register<SaveFileMessage>(this, save);
        }

        #endregion
    }
}
