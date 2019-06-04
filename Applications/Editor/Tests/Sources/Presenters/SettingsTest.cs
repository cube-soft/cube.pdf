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
using System.Threading.Tasks;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsTest
    ///
    /// <summary>
    /// Tests for the SettingsViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingsTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Executes the test for confirming properties and invoking the
        /// Cancel command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel() => Open("Sample.pdf", "", vm =>
        {
            var cts = new CancellationTokenSource();
            var dp  = vm.Subscribe<SettingsViewModel>(e =>
            {
                Assert.That(e.Title.Text,        Is.Not.Null.And.Not.Empty);
                Assert.That(e.Version.Text,      Is.Not.Null.And.Not.Empty);
                Assert.That(e.Version.Value,     Does.StartWith("Cube.Pdf.Editor.Tests 0.5.3β "));
                Assert.That(e.Windows.Text,      Does.StartWith("Microsoft Windows"));
                Assert.That(e.Framework.Text,    Does.StartWith("Microsoft .NET Framework"));
                Assert.That(e.Link.Text,         Is.EqualTo("Copyright © 2010 CubeSoft, Inc."));
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

            Assert.That(vm.Ribbon.Settings.Command.CanExecute(), Is.True);
            TaskEx.Run(() => vm.Ribbon.Settings.Command.Execute());
            Assert.That(Wait.For(cts.Token), Is.True, "Timeout (Cancel)");
            dp.Dispose();
        });

        #endregion
    }
}
