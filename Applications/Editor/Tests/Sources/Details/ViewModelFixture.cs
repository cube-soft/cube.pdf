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
using Cube.Generics;
using Cube.Pdf.App.Editor;
using Cube.Xui;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
    public abstract class ViewModelFixture : FileFixture
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ViewModelFixture
        ///
        /// <summary>
        /// Initializes a new instance of the ViewModelFixture class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ViewModelFixture() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets or sets a loading path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Source { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets a saving path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Destination { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets a password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Password { get; set; }

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
            using (var src = Create())
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
        /// <param name="filename">Filename of the source.</param>
        /// <param name="password">Password of the source.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="action">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Create(string filename, string password, int n,
            Action<MainViewModel> action) => Create(vm =>
        {
            Source   = GetExamplesWith(filename);
            Password = password;
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
            using (var src = Create())
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
        /// <param name="password">Password of the source.</param>
        /// <param name="n">Number of pages.</param>
        /// <param name="callback">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected Task CreateAsync(string filename, string password, int n,
            Func<MainViewModel, Task> callback) => CreateAsync(async (vm) =>
        {
            Source   = GetExamplesWith(filename);
            Password = password;

            Assert.That(vm.Ribbon.Open.Command.CanExecute(), Is.True);
            vm.Ribbon.Open.Command.Execute();
            var done = await Wait.ForAsync(() => vm.Data.Images.Count == n).ConfigureAwait(false);
            Assert.That(done, "Timeout (Open)");

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
            var cts = new CancellationTokenSource();
            void action(object s, EventArgs e)
            {
                if (!vm.Data.Busy.Value) return;
                vm.Data.Busy.PropertyChanged -= action;
                cts.Cancel();
            }

            Assert.That(vm.Data.Busy.Value, Is.False);
            vm.Data.Busy.PropertyChanged += action;
            Assert.That(src.Command.CanExecute(), Is.True);
            src.Command.Execute();

            var done = await Wait.ForAsync(cts.Token).ConfigureAwait(false);
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
            Password    = string.Empty;
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
            var asm   = Assembly.GetExecutingAssembly();
            var fmt   = DataContract.Format.Registry;
            var path  = @"CubeSoft\Cube.Pdf.Tests.Editor";
            var src   = new SettingsFolder(asm, fmt, path, IO) { AutoSave = false };
            var dest  = new MainViewModel(src);

            dest.Data.Preferences.Dummy = dummy;
            dest.Data.Preferences.VisibleFirst = 0;
            dest.Data.Preferences.VisibleLast = 10;

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Selects a button.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private MessageBoxResult Select(MessageBoxButton src)
        {
            var dic = new Dictionary<MessageBoxButton, MessageBoxResult>
            {
                { MessageBoxButton.OK,          MessageBoxResult.OK  },
                { MessageBoxButton.OKCancel,    MessageBoxResult.OK  },
                { MessageBoxButton.YesNo,       MessageBoxResult.Yes },
                { MessageBoxButton.YesNoCancel, MessageBoxResult.Yes },
            };

            var check = dic.TryGetValue(src, out var dest);
            Debug.Assert(check);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Sets some dummy callbacks to the specified Messenger.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<IDisposable> Register(IMessengerViewModel src)
        {
            void dialog(DialogMessage e)
            {
                Assert.That(e.Image, Is.Not.EqualTo(MessageBoxImage.Error), e.Content);
                e.Result = Select(e.Button);
                e.Callback?.Invoke(e);
            }

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

            void pass(PasswordViewModel e)
            {
                e.Password.Value = Password;
                var dest = Password.HasValue() ? e.OK : e.Cancel;
                Assert.That(dest.Command.CanExecute(), $"{dest.Text} (Password)");
                dest.Command.Execute();
            }

            return new List<IDisposable>
            {
                src.Register<DialogMessage>(this, dialog),
                src.Register<OpenFileMessage>(this, open),
                src.Register<SaveFileMessage>(this, save),
                src.Register<PasswordViewModel>(this, pass),
            };
        }

        #endregion
    }
}
