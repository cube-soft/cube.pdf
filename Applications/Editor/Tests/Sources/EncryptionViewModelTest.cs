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
using Cube.Pdf.Mixin;
using Cube.Xui.Mixin;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionViewModelTest
    ///
    /// <summary>
    /// Tests for the EncryptionViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class EncryptionViewModelTest : ViewModelFixture
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Executes the test to set the encryption information
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Set()
        {
            var cmp  = new Encryption
            {
                OwnerPassword    = "owner",
                UserPassword     = "user",
                OpenWithPassword = true,
                Method           = EncryptionMethod.Aes128,
                Enabled          = true,
                Permission       = new Permission(0xfffff0c0L),
            };

            Create("Sample.pdf", 2, vm =>
            {
                var dp = vm.Register<EncryptionViewModel>(this, e =>
                {
                    var pm = cmp.Permission;

                    e.Enabled.Value            = cmp.Enabled;
                    e.OwnerPassword.Value      = cmp.OwnerPassword;
                    e.OwnerConfirm.Value       = cmp.OwnerPassword;
                    e.Method.Value             = cmp.Method;
                    e.IsOpenPassword.Value     = cmp.OpenWithPassword;
                    e.IsSharePassword.Value    = false;
                    e.UserPassword.Value       = cmp.UserPassword;
                    e.UserConfirm.Value        = cmp.UserPassword;
                    e.AllowPrint.Value         = pm.Print.IsAllowed();
                    e.AllowCopy.Value          = pm.CopyContents.IsAllowed();
                    e.AllowModify.Value        = pm.ModifyContents.IsAllowed();
                    e.AllowAnnotation.Value    = pm.ModifyAnnotations.IsAllowed();
                    e.AllowForm.Value          = pm.InputForm.IsAllowed();
                    e.AllowAccessibility.Value = pm.Accessibility.IsAllowed();

                    Assert.That(e.OK.Command.CanExecute(), Is.True);
                    e.OK.Command.Execute();
                });

                Assert.That(vm.Ribbon.Encryption.Command.CanExecute(), Is.True);
                vm.Ribbon.Encryption.Command.Execute();

                Assert.That(vm.Data.History.Undoable, Is.True);
                Assert.That(vm.Data.History.Redoable, Is.False);

                Destination = GetResultsWith($"Encryption_Sample.pdf");
                Execute(vm, vm.Ribbon.SaveAs);
                Assert.That(Wait.For(() => IO.Exists(Destination)));
            });
        }
    }
}
