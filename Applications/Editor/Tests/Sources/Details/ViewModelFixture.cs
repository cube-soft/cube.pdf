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
using Cube.Mixin.String;
using Cube.Tests;
using Cube.Xui;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.Editor.Tests
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
            Source   = GetSource(filename);
            Password = password;
            Execute(vm, vm.Ribbon.Open);
            Assert.That(Wait.For(() => vm.Data.Images.Count == n), "Timeout (Open)");
            action(vm);
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
        protected void Execute(MainViewModel vm, BindableElement src)
        {
            var cts = new CancellationTokenSource();
            void action(object s, EventArgs e)
            {
                if (vm.Data.Busy.Value) return;
                vm.Data.Busy.PropertyChanged -= action;
                cts.Cancel();
            }

            Assert.That(vm.Data.Busy.Value, Is.False, $"Busy ({src.Text})");
            vm.Data.Busy.PropertyChanged += action;
            Assert.That(src.Command.CanExecute(), Is.True, $"CanExecute ({src.Text})");
            src.Command.Execute();
            Assert.That(Wait.For(cts.Token), $"Timeout ({src.Text})");
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
           Get($"{name}_{string.Join("_", parts)}.pdf");

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
            var dummy = new BitmapImage(new Uri(GetSource("Loading.png")));
            var asm   = Assembly.GetExecutingAssembly();
            var fmt   = DataContract.Format.Registry;
            var path  = @"CubeSoft\Cube.Pdf.Editor.Tests";
            var src   = new SettingsFolder(asm, fmt, path, IO) { AutoSave = false };
            var dest  = new MainViewModel(src, new SynchronizationContext());

            dest.Data.Images.Preferences.Dummy = dummy;
            dest.Data.Images.Preferences.VisibleFirst = 0;
            dest.Data.Images.Preferences.VisibleLast = 10;

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
        private DialogStatus Select(DialogButtons src)
        {
            var dic = new Dictionary<DialogButtons, DialogStatus>
            {
                { DialogButtons.Ok,          DialogStatus.Ok  },
                { DialogButtons.OkCancel,    DialogStatus.Ok  },
                { DialogButtons.YesNo,       DialogStatus.Yes },
                { DialogButtons.YesNoCancel, DialogStatus.Yes },
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
        private IEnumerable<IDisposable> Register(IPresentable src)
        {
            void dialog(DialogMessage e)
            {
                e.Status = Select(e.Buttons);
            }

            void open(OpenFileMessage e)
            {
                e.Value  = new[] { Source };
                e.Cancel = false;
            }

            void save(SaveFileMessage e)
            {
                e.Value  = Destination;
                e.Cancel = false;
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
                src.Subscribe<DialogMessage>(dialog),
                src.Subscribe<OpenFileMessage>(open),
                src.Subscribe<SaveFileMessage>(save),
                src.Subscribe<PasswordViewModel>(pass),
            };
        }

        #endregion
    }
}
