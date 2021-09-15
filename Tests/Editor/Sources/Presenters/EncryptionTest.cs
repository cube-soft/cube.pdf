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
using Cube.Mixin.Commands;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionTest
    ///
    /// <summary>
    /// Tests the Encryption commands.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class EncryptionTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the normal case of the EncryptionViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Test()
        {
            var cmp = new Encryption
            {
                OwnerPassword    = "owner",
                UserPassword     = "user",
                OpenWithPassword = true,
                Method           = EncryptionMethod.Aes128,
                Enabled          = true,
                Permission       = new Permission(0xfffff0c0L),
            };

            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = Subscribe(vm, cmp);

            Assert.That(vm.Ribbon.Encryption.Command.CanExecute());
            vm.Ribbon.Encryption.Command.Execute();
            Assert.That(vm.Value.Encryption, Is.Not.Null);
            AssertEquals(vm.Value.Encryption, cmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Tests the Cancel command in the EncryptionViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = vm.Subscribe<EncryptionViewModel>(evm => {
                evm.OwnerPassword.Value = "dummy";
                evm.Cancel.Command.Execute();
            });

            Assert.That(vm.Ribbon.Encryption.Command.CanExecute());
            vm.Ribbon.Encryption.Command.Execute();
            Assert.That(vm.Value.Encryption, Is.Not.Null);
            Assert.That(vm.Value.Encryption.OwnerPassword, Is.Not.EqualTo("dummy"));
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Subscribe
        ///
        /// <summary>
        /// Sets the operation corresponding to the EncryptionViewModel
        /// message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable Subscribe(MainViewModel vm, Encryption src) =>
            vm.Subscribe<EncryptionViewModel>(evm =>
        {
            vm.Value.Settings.Language = Language.English;
            evm.Enabled.Value            = src.Enabled;
            evm.OwnerPassword.Value      = src.OwnerPassword;
            evm.OwnerConfirm.Value       = src.OwnerPassword;
            evm.Method.Value             = src.Method;
            evm.OpenPassword.Value       = src.OpenWithPassword;
            evm.SharePassword.Value      = false;
            evm.UserPassword.Value       = src.UserPassword;
            evm.UserConfirm.Value        = src.UserPassword;
            evm.AllowPrint.Value         = src.Permission.Print.IsAllowed();
            evm.AllowCopy.Value          = src.Permission.CopyContents.IsAllowed();
            evm.AllowModify.Value        = src.Permission.ModifyContents.IsAllowed();
            evm.AllowAnnotation.Value    = src.Permission.ModifyAnnotations.IsAllowed();
            evm.AllowForm.Value          = src.Permission.InputForm.IsAllowed();
            evm.AllowAccessibility.Value = src.Permission.Accessibility.IsAllowed();

            Assert.That(evm.Title,           Is.EqualTo("Encryption"));
            Assert.That(evm.Methods.Count(), Is.EqualTo(4));
            Assert.That(evm.Operation.Value);
            Assert.That(evm.OK.Command.CanExecute());

            evm.OK.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// AssertEquals
        ///
        /// <summary>
        /// Confirms that properties of the specified objects are equal.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertEquals(Encryption src, Encryption cmp)
        {
            Assert.That(src.Enabled,          Is.EqualTo(cmp.Enabled), nameof(src.Enabled));
            Assert.That(src.OwnerPassword,    Is.EqualTo(cmp.OwnerPassword));
            Assert.That(src.Method,           Is.EqualTo(cmp.Method));

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
