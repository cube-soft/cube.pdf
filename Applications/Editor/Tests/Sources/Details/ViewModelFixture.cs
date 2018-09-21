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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelFixture
    ///
    /// <summary>
    /// Provides functionality to test ViewModel classes.
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

        #region Create

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Gets a new instance of the MainViewModel class and execute
        /// the specified action.
        /// </summary>
        ///
        /// <param name="callback">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Create(Action<MainViewModel> callback)
        {
            //using (var src = Create())
            var src = Create();
            {
                var dps = Register(src);
                callback(src);
                foreach (var e in dps) e.Dispose();
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
            Assert.That(Wait.For(() => vm.Data.Images.Count == n), "Timeout (Open)");
            action(vm);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// CreateAsync
        ///
        /// <summary>
        /// Gets a new instance of the MainViewModel class and execute
        /// the specified callback as an asynchronous operation.
        /// </summary>
        ///
        /// <param name="callback">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected async Task CreateAsync(Func<MainViewModel, Task> callback)
        {
            //using (var src = Create())
            var src = Create();
            {
                var dps = Register(src);
                await callback(src);
                foreach (var e in dps) e.Dispose();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateAsync
        ///
        /// <summary>
        /// Gets a new instance of the MainViewModel class, executes
        /// the Open command, and runs the specified action as an
        /// asynchronous operation.
        /// </summary>
        ///
        /// <param name="filename">Filename of the source.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="callback">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected Task CreateAsync(string filename, int n,
            Func<MainViewModel, Task> callback) => CreateAsync(async (vm) =>
        {
            Source = GetExamplesWith(filename);
            await ExecuteAsync(vm, vm.Ribbon.Open).ConfigureAwait(false);
            var open = await Wait.ForAsync(() => vm.Data.Images.Count == n).ConfigureAwait(false);
            Assert.That(open, "Timeout (Open)");
            await callback(vm).ConfigureAwait(false);
        });

        #endregion

        #region Execute

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Execute the specified command.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        /// <param name="src">Bindable element.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Execute(MainViewModel vm, BindableElement src) =>
            ExecuteAsync(vm, src).Wait();

        /* ----------------------------------------------------------------- */
        ///
        /// ExecuteAsync
        ///
        /// <summary>
        /// Execute the specified command as an asynchronous operation.
        /// </summary>
        ///
        /// <param name="vm">MainViewModel instance.</param>
        /// <param name="src">Bindable element.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected async Task ExecuteAsync(MainViewModel vm, BindableElement src)
        {
            var data  = vm.Data;
            var ready = await Wait.ForAsync(() => !data.Busy.Value).ConfigureAwait(false);
            Assert.That(ready, $"NotReady ({src.Text})");

            data.SetMessage(string.Empty);
            Assert.That(src.Command.CanExecute(), nameof(src.Command.CanExecute));
            src.Command.Execute();

            var done = await Wait.ForAsync(() => !data.Busy.Value).ConfigureAwait(false);
            Assert.That(done, $"Timeout ({src.Text})");
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Args
        ///
        /// <summary>
        /// Converts params to an object array.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected object[] Args(params object[] src) => src;

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// Creates the path by using the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Path(object[] parts, [CallerMemberName] string name = null) =>
           GetResultsWith($"{name}_{string.Join("_", parts)}.pdf");

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
        protected virtual void Setup()
        {
            Source      = string.Empty;
            Destination = string.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Gets a new instance of the MainViewModel class.
        /// </summary>
        ///
        /// <returns>MainViewModel object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private MainViewModel Create()
        {
            var dummy = new BitmapImage(new Uri(GetExamplesWith("Loading.png")));
            var dest  = new MainViewModel();

            dest.Data.Preferences.Dummy = dummy;
            dest.Data.Preferences.VisibleFirst = 0;
            dest.Data.Preferences.VisibleLast = 10;

            return dest;
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
        private IEnumerable<IDisposable> Register(IMessengerViewModel src)
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

            return new List<IDisposable>
            {
                src.Register<OpenFileMessage>(this, open),
                src.Register<SaveFileMessage>(this, save),
            };
        }

        #endregion
    }
}
