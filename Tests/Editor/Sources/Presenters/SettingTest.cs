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
using Cube.Tests;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingTest
    ///
    /// <summary>
    /// Tests the settings related classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Confirms the values of settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create() => Make(vm =>
        {
            Assert.That(vm.Value.Settings.Width,       Is.EqualTo(800));
            Assert.That(vm.Value.Settings.Height,      Is.EqualTo(600));
            Assert.That(vm.Value.Settings.CheckUpdate, Is.True);

            vm.Value.Settings.Width       = 1024;
            vm.Value.Settings.Height      = 768;
            vm.Value.Settings.CheckUpdate = false;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Confirms properties of the SettingViewModel class and invokes
        /// the Cancel command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel() => Open("Sample.pdf", "", vm =>
        {
            var cts = new CancellationTokenSource();
            _ = vm.Subscribe<SettingViewModel>(e =>
            {
                Assert.That(e.Title,             Is.Not.Null.And.Not.Empty);
                Assert.That(e.Version.Text,      Is.Not.Null.And.Not.Empty);
                Assert.That(e.Version.Value,     Does.StartWith("Cube.Pdf.Editor.Tests 0.6.3β "));
                Assert.That(e.Windows.Text,      Does.StartWith("Microsoft Windows"));
                Assert.That(e.Framework.Text,    Does.StartWith("Microsoft .NET Framework"));
                Assert.That(e.Link.Text,         Is.EqualTo("Copyright © 2013 CubeSoft, Inc."));
                Assert.That(e.Link.Value,        Is.EqualTo(new Uri("https://www.cube-soft.jp/cubepdfutility/")));
                Assert.That(e.Update.Text,       Is.Not.Null.And.Not.Empty);
                Assert.That(e.Update.Value,      Is.True);
                Assert.That(e.Language.Text,     Is.Not.Null.And.Not.Empty);
                Assert.That(e.Language.Value,    Is.EqualTo(Language.Auto));
                Assert.That(e.Languages.Count(), Is.EqualTo(3));

                Assert.That(e.OK.Command.CanExecute(),     Is.True);
                Assert.That(e.Cancel.Command.CanExecute(), Is.True);

                e.Cancel.Command.Execute();
                cts.Cancel(); // done
            });

            Assert.That(vm.Ribbon.Setting.Command.CanExecute(), Is.True);
            vm.Ribbon.Setting.Command.Execute();
            Assert.That(Wait.For(cts.Token), Is.True, "Timeout (Cancel)");
        });

        #endregion
    }
}
