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
using Cube.Pdf.Itext;
using Cube.Pdf.Mixin;
using Cube.Xui;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Cube.Pdf.Tests.Editor.ViewModels
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionTest
    ///
    /// <summary>
    /// Tests for the EncryptionViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class EncryptionTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Executes the test for setting the encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Set(int index, Encryption cmp)
        {
            Create("Sample.pdf", "", 2, vm =>
            {
                Assert.That(vm.Data.Encryption, Is.Not.Null);
                var cts = new CancellationTokenSource();
                vm.Data.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(vm.Data.Encryption)) cts.Cancel();
                };

                Register(vm, cmp, false);
                Assert.That(vm.Ribbon.Encryption.Command.CanExecute(), Is.True);
                vm.Ribbon.Encryption.Command.Execute();
                Assert.That(Wait.For(cts.Token), $"Timeout (Encryption)");

                Destination = Path(Args(index, cmp.Method));
                vm.Ribbon.SaveAs.Command.Execute();
                Assert.That(Wait.For(() => IO.Exists(Destination)), $"Timeout (SaveAs)");
            });

            AssertEncryption(Destination, cmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Executes the test for selecting the cancel button in the
        /// EncryptionWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel() => Create("Sample.pdf", "", 2, vm =>
        {
            var cts = new CancellationTokenSource();
            vm.Register<EncryptionViewModel>(this, e =>
            {
                e.OwnerPassword.Value = "dummy";
                e.Register<CloseMessage>(this, z => cts.Cancel());
                Assert.That(e.Cancel.Command.CanExecute(), Is.True);
                e.Cancel.Command.Execute();
            });
            vm.Ribbon.Encryption.Command.Execute();

            Assert.That(Wait.For(cts.Token), "Timeout");
            Assert.That(vm.Data.History.Undoable, Is.False);
            Assert.That(vm.Data.History.Redoable, Is.False);
            Assert.That(vm.Data.Encryption.OwnerPassword, Is.Not.EqualTo("dummy"));
        });

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var n = 0;

                yield return new TestCaseData(n++, new Encryption
                {
                    OwnerPassword    = "owner",
                    UserPassword     = "user",
                    OpenWithPassword = true,
                    Method           = EncryptionMethod.Aes128,
                    Enabled          = true,
                    Permission       = new Permission(0xfffff0c0L),
                });
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Sets the operation corresponding to the EncryptionViewModel
        /// message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable Register(MainViewModel vm, Encryption src, bool share) =>
            vm.Register<EncryptionViewModel>(this, e =>
        {
            e.Enabled.Value            = src.Enabled;
            e.OwnerPassword.Value      = src.OwnerPassword;
            e.OwnerConfirm.Value       = src.OwnerPassword;
            e.Method.Value             = src.Method;
            e.IsOpenPassword.Value     = src.OpenWithPassword;
            e.IsSharePassword.Value    = share;
            e.UserPassword.Value       = src.UserPassword;

            var p = src.Permission;

            e.UserConfirm.Value        = src.UserPassword;
            e.AllowPrint.Value         = p.Print.IsAllowed();
            e.AllowCopy.Value          = p.CopyContents.IsAllowed();
            e.AllowModify.Value        = p.ModifyContents.IsAllowed();
            e.AllowAnnotation.Value    = p.ModifyAnnotations.IsAllowed();
            e.AllowForm.Value          = p.InputForm.IsAllowed();
            e.AllowAccessibility.Value = p.Accessibility.IsAllowed();

            Assert.That(e.OK.Command.CanExecute(), Is.True);
            e.OK.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// AssertEncryption
        ///
        /// <summary>
        /// Confirms that properties of the specified objects are equal.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertEncryption(string src, Encryption cmp)
        {
            using (var r = new DocumentReader(src, cmp.OwnerPassword))
            {
                AssertEncryption(r.Encryption, cmp);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AssertEncryption
        ///
        /// <summary>
        /// Confirms that properties of the specified objects are equal.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertEncryption(Encryption src, Encryption cmp)
        {
            Assert.That(src.Enabled,          Is.EqualTo(cmp.Enabled), nameof(src.Enabled));
            Assert.That(src.OwnerPassword,    Is.EqualTo(cmp.OwnerPassword));
            Assert.That(src.Method,           Is.EqualTo(cmp.Method));
            //Assert.That(src.OpenWithPassword, Is.EqualTo(cmp.OpenWithPassword), nameof(src.OpenWithPassword));
            //Assert.That(src.UserPassword,     Is.EqualTo(cmp.UserPassword));

            var x = src.Permission;
            var y = cmp.Permission;

            Assert.That(x.Print,             Is.EqualTo(y.Print),             nameof(x.Print));
            Assert.That(x.CopyContents,      Is.EqualTo(y.CopyContents),      nameof(x.CopyContents));
            Assert.That(x.ModifyContents,    Is.EqualTo(y.ModifyContents),    nameof(x.ModifyContents));
            Assert.That(x.ModifyAnnotations, Is.EqualTo(y.ModifyAnnotations), nameof(x.ModifyAnnotations));
            Assert.That(x.InputForm,         Is.EqualTo(y.InputForm),         nameof(x.InputForm));
            Assert.That(x.Accessibility,     Is.EqualTo(y.Accessibility),     nameof(x.Accessibility));
        }

        #endregion
    }
}
