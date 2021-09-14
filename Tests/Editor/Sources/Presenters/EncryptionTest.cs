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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cube.Mixin.Commands;
using Cube.Tests;
using Cube.Xui;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
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
    class EncryptionTest : VmFixture
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
        public void Set(int id, Encryption cmp)
        {
            using var vm = NewVM();
            using var d0 = vm.Hook(new() { Source = GetSource("Sample.pdf") });

            vm.Test(vm.Ribbon.Open);

            var cts = new CancellationTokenSource();
            vm.Value.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.Value.Encryption)) cts.Cancel();
            };

            using (Subscribe(vm, cmp, false))
            {
                Assert.That(vm.Value.Encryption, Is.Not.Null);
                Assert.That(Test(vm.Ribbon.Encryption, cts), $"Timeout (No.{id})");
                AssertEquals(vm.Value.Encryption, cmp);
            }
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
        public void Cancel()
        {
            using var vm = NewVM();
            using var d0 = vm.Hook(new() { Source = GetSource("Sample.pdf") });

            vm.Test(vm.Ribbon.Open);

            var cts = new CancellationTokenSource();
            using (vm.Subscribe<EncryptionViewModel>(e => {
                e.OwnerPassword.Value = "dummy";
                using (e.Subscribe<CloseMessage>(z => cts.Cancel())) e.Cancel.Command.Execute();
            })) {
                Assert.That(vm.Value.Encryption, Is.Not.Null);
                Assert.That(Test(vm.Ribbon.Encryption, cts), "Timeout");
                Assert.That(vm.Value.Encryption.OwnerPassword, Is.Not.EqualTo("dummy"));
            }
        }

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
        /// Subscribe
        ///
        /// <summary>
        /// Sets the operation corresponding to the EncryptionViewModel
        /// message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable Subscribe(MainViewModel vm, Encryption src, bool share) =>
            vm.Subscribe<EncryptionViewModel>(e =>
        {
            vm.Value.Settings.Language = Language.English;
            var pm = src.Permission;

            e.Enabled.Value            = src.Enabled;
            e.OwnerPassword.Value      = src.OwnerPassword;
            e.OwnerConfirm.Value       = src.OwnerPassword;
            e.Method.Value             = src.Method;
            e.OpenPassword.Value       = src.OpenWithPassword;
            e.SharePassword.Value      = share;
            e.UserPassword.Value       = src.UserPassword;
            e.UserConfirm.Value        = src.UserPassword;
            e.AllowPrint.Value         = pm.Print.IsAllowed();
            e.AllowCopy.Value          = pm.CopyContents.IsAllowed();
            e.AllowModify.Value        = pm.ModifyContents.IsAllowed();
            e.AllowAnnotation.Value    = pm.ModifyAnnotations.IsAllowed();
            e.AllowForm.Value          = pm.InputForm.IsAllowed();
            e.AllowAccessibility.Value = pm.Accessibility.IsAllowed();

            Assert.That(e.Title,           Is.EqualTo("Encryption"));
            Assert.That(e.Methods.Count(), Is.EqualTo(4));
            Assert.That(e.Operation.Value, Is.True);
            Assert.That(e.OK.Command.CanExecute(), Is.True);
            e.OK.Command.Execute();
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

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the command of the specified element.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool Test(IElement src, CancellationTokenSource cts)
        {
            Assert.That(src.Command.CanExecute(), Is.True);
            src.Command.Execute();
            return Wait.For(cts.Token);
        }

        #endregion
    }
}
