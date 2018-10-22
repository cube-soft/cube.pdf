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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task Set(int index, Encryption cmp)
        {
            await CreateAsync("Sample.pdf", "", 2, async (vm) =>
            {
                var cts = new CancellationTokenSource();
                using (var _ = Register(vm, cmp, false, cts))
                {
                    Assert.That(vm.Ribbon.Encryption.Command.CanExecute(), Is.True);
                    vm.Ribbon.Encryption.Command.Execute();
                    var done = await Wait.ForAsync(cts.Token).ConfigureAwait(false);
                    Assert.That(done, $"Timeout (Encryption)");
                }

                Assert.That(vm.Data.History.Undoable, Is.True);
                Assert.That(vm.Data.History.Redoable, Is.False);

                Destination = Path(Args(index, cmp.Method));
                await ExecuteAsync(vm, vm.Ribbon.SaveAs);
                var save = await Wait.ForAsync(() => IO.Exists(Destination)).ConfigureAwait(false);
                Assert.That(save, $"Timeout (SaveAs)");
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
        public void Cancel() => CreateAsync("Sample.pdf", "", 2, async (vm) =>
        {
            var cts = new CancellationTokenSource();
            var dp  = vm.Register<EncryptionViewModel>(this, e =>
            {
                e.OwnerPassword.Value = "dummy";
                Assert.That(e.Cancel.Command.CanExecute(), Is.True);
                e.Cancel.Command.Execute();
                cts.Cancel();
            });

            vm.Ribbon.Encryption.Command.Execute();
            await Wait.ForAsync(cts.Token);
            dp.Dispose();

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
                var index = 0;

                yield return new TestCaseData(++index, new Encryption
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
        private IDisposable Register(MainViewModel vm, Encryption src, bool share,
            CancellationTokenSource cts) => vm.Register<EncryptionViewModel>(this, e =>
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
            cts.Cancel(); // done
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
