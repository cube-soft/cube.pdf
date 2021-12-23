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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// OtherViewModelTest
    ///
    /// <summary>
    /// Tests properties and methods of ViewModel classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class OtherViewModelTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// Confirms the properties of the MainViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Main() => Invoke(vm =>
        {
            Assert.That(vm.Title,   Does.StartWith(nameof(Main)));
            Assert.That(vm.Title,   Does.Contain("CubePDF 1.6.1"));
            Assert.That(vm.Version, Does.StartWith("1.6.1 (").And.EndsWith(")"));
            Assert.That(vm.Uri.ToString(), Does.StartWith("https://www.cube-soft.jp/cubepdf/?lang="));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// General
        ///
        /// <summary>
        /// Tests the properties of the SettingViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void General() => Invoke(vm =>
        {
            var dest = vm.General;
            GetType().LogDebug($"CheckUpdate:{dest.CheckUpdate}");
            Assert.That(dest.Resolution,           Is.EqualTo(600));
            Assert.That(dest.Language,             Is.EqualTo(Language.Auto));
            Assert.That(dest.IsAutoOrientation,    Is.True,  nameof(dest.IsAutoOrientation));
            Assert.That(dest.IsPortrait,           Is.False, nameof(dest.IsPortrait));
            Assert.That(dest.IsLandscape,          Is.False, nameof(dest.IsLandscape));
            Assert.That(dest.Grayscale,            Is.False, nameof(dest.Grayscale));
            Assert.That(dest.ImageFilter,          Is.True,  nameof(dest.ImageFilter));
            Assert.That(dest.Linearization,        Is.False, nameof(dest.Linearization));
            Assert.That(dest.IsPdf,                Is.True,  nameof(dest.IsPdf));
            Assert.That(dest.UserProgramEditable,  Is.False, nameof(dest.UserProgramEditable));
            Assert.That(dest.SourceEditable,       Is.False, nameof(dest.SourceEditable));
            Assert.That(dest.SourceVisible,        Is.False, nameof(dest.SourceVisible));

            dest.Format = Format.Png;
            Assert.That(dest.IsPdf, Is.False, nameof(dest.IsPdf));

            dest.PostProcess = PostProcess.Others;
            Assert.That(dest.UserProgramEditable,  Is.True,  nameof(dest.UserProgramEditable));

            dest.IsPortrait = true;
            Assert.That(dest.IsAutoOrientation,  Is.False, nameof(dest.IsAutoOrientation));
            Assert.That(dest.IsPortrait,         Is.True,  nameof(dest.IsPortrait));
            Assert.That(dest.IsLandscape,        Is.False, nameof(dest.IsLandscape));

            dest.IsLandscape = true;
            Assert.That(dest.IsAutoOrientation,  Is.False, nameof(dest.IsAutoOrientation));
            Assert.That(dest.IsPortrait,         Is.False, nameof(dest.IsPortrait));
            Assert.That(dest.IsLandscape,        Is.True,  nameof(dest.IsLandscape));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Tests the properties of the MetadataViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Metadata() => Invoke(vm =>
        {
            var dest = vm.Metadata;
            Assert.That(dest.Title,    Is.Empty, nameof(dest.Title));
            Assert.That(dest.Author,   Is.Empty, nameof(dest.Author));
            Assert.That(dest.Subject,  Is.Empty, nameof(dest.Subject));
            Assert.That(dest.Keywords, Is.Empty, nameof(dest.Keywords));
            Assert.That(dest.Creator,  Is.EqualTo("CubePDF"));
            Assert.That(dest.Options,  Is.EqualTo(ViewerOption.OneColumn));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Tests the properties of the EncryptionViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Encryption() => Invoke(vm =>
        {
            var dest = vm.Encryption;
            Assert.That(dest.Enabled,            Is.False, nameof(dest.Enabled));
            Assert.That(dest.OwnerPassword,      Is.Empty, nameof(dest.OwnerPassword));
            Assert.That(dest.OwnerConfirm,       Is.Empty, nameof(dest.OwnerConfirm));
            Assert.That(dest.OpenWithPassword,   Is.False, nameof(dest.OpenWithPassword));
            Assert.That(dest.SharePassword,      Is.False, nameof(dest.SharePassword));
            Assert.That(dest.DividePassword,     Is.False, nameof(dest.DividePassword));
            Assert.That(dest.UserPassword,       Is.Empty, nameof(dest.UserPassword));
            Assert.That(dest.UserConfirm,        Is.Empty, nameof(dest.UserConfirm));
            Assert.That(dest.AllowCopy,          Is.False, nameof(dest.AllowCopy));
            Assert.That(dest.AllowForm,          Is.False, nameof(dest.AllowForm));
            Assert.That(dest.AllowModify,        Is.False, nameof(dest.AllowModify));
            Assert.That(dest.AllowPrint,         Is.False, nameof(dest.AllowPrint));
            Assert.That(dest.PermissionEditable, Is.True,  nameof(dest.PermissionEditable));

            dest.Enabled = true;
            Assert.That(dest.OwnerCorrect, Is.False);
            dest.OwnerPassword = "Password";
            Assert.That(dest.OwnerCorrect, Is.False);
            dest.OwnerConfirm = "Password";
            Assert.That(dest.OwnerCorrect, Is.True);

            Assert.That(dest.UserCorrect, Is.True);
            dest.OpenWithPassword = true;
            Assert.That(dest.UserCorrect, Is.False);
            dest.UserPassword = "User";
            Assert.That(dest.UserCorrect, Is.False);
            dest.UserConfirm = "User";
            Assert.That(dest.UserCorrect, Is.True);

            Assert.That(dest.Enabled,            Is.True,  nameof(dest.Enabled));
            Assert.That(dest.OpenWithPassword,   Is.True,  nameof(dest.OpenWithPassword));
            Assert.That(dest.SharePassword,      Is.False, nameof(dest.SharePassword));
            Assert.That(dest.DividePassword,     Is.True,  nameof(dest.DividePassword));
            Assert.That(dest.PermissionEditable, Is.True,  nameof(dest.PermissionEditable));

            dest.SharePassword = true;

            Assert.That(dest.SharePassword,      Is.True,  nameof(dest.SharePassword));
            Assert.That(dest.DividePassword,     Is.False, nameof(dest.DividePassword));
            Assert.That(dest.PermissionEditable, Is.False, nameof(dest.PermissionEditable));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectSource
        ///
        /// <summary>
        /// Tests the SelectSource method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SelectSource() => Invoke(vm =>
        {
            var done = $"{nameof(SelectSource)}_Done.pdf";

            _ = vm.Subscribe<OpenFileMessage>(e =>
            {
                Assert.That(e.Text,             Is.EqualTo("入力ファイルを選択"));
                Assert.That(e.InitialDirectory, Is.Empty);
                Assert.That(e.Value.Count(),    Is.EqualTo(1));
                Assert.That(e.CheckPathExists,  Is.True);
                Assert.That(e.GetFilterText(),  Is.Not.Null.And.Not.Empty);
                Assert.That(e.GetFilterIndex(), Is.EqualTo(0));

                e.Value  = new[] { done };
                e.Cancel = false;
            });

            vm.General.Language = Language.Japanese;
            vm.SelectSource();
            Assert.That(Wait.For(() => vm.General.Source.FuzzyEquals(done)), "Timeout");
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectDestination
        ///
        /// <summary>
        /// Tests the SelectDestination method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SelectDestination() => Invoke(vm =>
        {
            var done = $"{nameof(SelectDestination)}_Done.pdf";

            _ = vm.Subscribe<SaveFileMessage>(e =>
            {
                Assert.That(e.Text,             Is.EqualTo("名前を付けて保存"));
                Assert.That(e.InitialDirectory, Is.Empty);
                Assert.That(e.Value,            Is.EqualTo(nameof(SelectDestination)));
                Assert.That(e.OverwritePrompt,  Is.False);
                Assert.That(e.CheckPathExists,  Is.False);
                Assert.That(e.GetFilterText(),  Is.Not.Null.And.Not.Empty);
                Assert.That(e.GetFilterIndex(), Is.EqualTo(0));

                e.Value  = done;
                e.Cancel = false;
            });

            vm.General.Language = Language.Japanese;
            vm.SelectDestination();
            Assert.That(Wait.For(() => vm.General.Destination.FuzzyEquals(done)), "Timeout");
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectUserProgram
        ///
        /// <summary>
        /// Tests the SelectUserProgram method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SelectUserProgram() => Invoke(vm =>
        {
            var done = $"{nameof(SelectUserProgram)}_Done.pdf";

            _ = vm.Subscribe<OpenFileMessage>(e =>
            {
                Assert.That(e.Text,             Is.EqualTo("変換完了時に実行するプログラムを選択"));
                Assert.That(e.InitialDirectory, Is.Empty);
                Assert.That(e.Value.Count(),    Is.EqualTo(0));
                Assert.That(e.CheckPathExists,  Is.True);
                Assert.That(e.GetFilterText(),  Is.Not.Null.And.Not.Empty);
                Assert.That(e.GetFilterIndex(), Is.EqualTo(0));

                e.Value  = new[] { done };
                e.Cancel = false;
            });

            vm.General.Language = Language.Japanese;
            vm.SelectUserProgram();
            Assert.That(Wait.For(() => vm.General.UserProgram.FuzzyEquals(done)), "Timeout");
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Validate_OwnerPassword
        ///
        /// <summary>
        /// Tests the process of checking the input of the owner password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Validate_OwnerPassword() => Invoke(vm =>
        {
            vm.General.Language         = Language.English;
            vm.Encryption.Enabled       = true;
            vm.Encryption.OwnerPassword = nameof(Validate_OwnerPassword);

            Assert.That(TestError(vm), Is.True, "Timeout (Empty)");
            vm.Encryption.OwnerConfirm = "Dummy";
            Assert.That(TestError(vm), Is.True, "Timeout (NotMatch)");
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Validate_UserPassword
        ///
        /// <summary>
        /// Tests the process of checking the input of the user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Validate_UserPassword() => Invoke(vm =>
        {
            vm.General.Language            = Language.English;
            vm.Encryption.Enabled          = true;
            vm.Encryption.OwnerPassword    = nameof(Validate_OwnerPassword);
            vm.Encryption.OwnerConfirm     = nameof(Validate_OwnerPassword);
            vm.Encryption.OpenWithPassword = true;
            vm.Encryption.UserPassword     = nameof(Validate_UserPassword);

            Assert.That(TestError(vm), Is.True, "Timeout (Empty)");
            vm.Encryption.UserConfirm = "Dummy";
            Assert.That(TestError(vm), Is.True, "Timeout (NotMatch)");
        });

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Creates a new instance of the MainViewModel class and invokes
        /// the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action<MainViewModel> action, [CallerMemberName] string name = null)
        {
            var args = GetArgs(name);
            var dest = Create(Combine(args, "Sample.ps"));

            using (Locale.Subscribe(SetUiCulture))
            using (var vm = new MainViewModel(dest, new SynchronizationContext()))
            using (vm.Subscribe<DialogMessage>(SetMessage))
            {
                action(vm);
            }
        }

        #endregion
    }
}
