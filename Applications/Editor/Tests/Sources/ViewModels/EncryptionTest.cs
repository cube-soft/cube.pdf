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
using Cube.Xui.Mixin;
using NUnit.Framework;
using System;

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
        /// Executes the test to set the encryption information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(EncryptionMethod.Aes128, 0xfffff0c0L)]
        public void Set(EncryptionMethod method, long permission)
        {
            var cmp  = new Encryption
            {
                OwnerPassword    = "owner",
                UserPassword     = "user",
                OpenWithPassword = true,
                Method           = method,
                Enabled          = true,
                Permission       = new Permission(permission),
            };

            Create("Sample.pdf", 2, vm =>
            {
                using (var _ = Register(vm, cmp, false))
                {
                    Assert.That(vm.Ribbon.Encryption.Command.CanExecute(), Is.True);
                    vm.Ribbon.Encryption.Command.Execute();
                }

                Assert.That(vm.Data.History.Undoable, Is.True);
                Assert.That(vm.Data.History.Redoable, Is.False);

                Destination = Path(Args(method, permission));
                Execute(vm, vm.Ribbon.SaveAs);
                Assert.That(Wait.For(() => IO.Exists(Destination)));
            });

            using (var r = new DocumentReader(Destination, cmp.OwnerPassword))
            {
                AssertEncryption(r.Encryption, cmp);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Executes the test to cancel the EncryptionWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel() => Create("Sample.pdf", 2, vm =>
        {
            using (var _ = vm.Register<EncryptionViewModel>(this, e =>
            {
                e.OwnerPassword.Value = "dummy";
                Assert.That(e.Cancel.Command.CanExecute(), Is.True);
                e.Cancel.Command.Execute();
            })) vm.Ribbon.Encryption.Command.Execute();

            Assert.That(vm.Data.History.Undoable, Is.False);
            Assert.That(vm.Data.History.Redoable, Is.False);
            Assert.That(vm.Data.Encryption.Value.OwnerPassword, Is.Not.EqualTo("dummy"));
        });

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
            var pm = src.Permission;

            e.Enabled.Value            = src.Enabled;
            e.OwnerPassword.Value      = src.OwnerPassword;
            e.OwnerConfirm.Value       = src.OwnerPassword;
            e.Method.Value             = src.Method;
            e.IsOpenPassword.Value     = src.OpenWithPassword;
            e.IsSharePassword.Value    = share;
            e.UserPassword.Value       = src.UserPassword;
            e.UserConfirm.Value        = src.UserPassword;
            e.AllowPrint.Value         = pm.Print.IsAllowed();
            e.AllowCopy.Value          = pm.CopyContents.IsAllowed();
            e.AllowModify.Value        = pm.ModifyContents.IsAllowed();
            e.AllowAnnotation.Value    = pm.ModifyAnnotations.IsAllowed();
            e.AllowForm.Value          = pm.InputForm.IsAllowed();
            e.AllowAccessibility.Value = pm.Accessibility.IsAllowed();

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
