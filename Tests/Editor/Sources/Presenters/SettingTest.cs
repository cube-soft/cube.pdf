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
using System.Linq;
using Cube.Xui.Commands.Extensions;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingTest
    ///
    /// <summary>
    /// Tests the Setting commands and the SettingViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the Setting command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Test()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = vm.Subscribe<SettingViewModel>(svm =>
            {
                AssertObject(svm);
                Assert.That(svm.OK.Command.CanExecute());
                Assert.That(svm.Cancel.Command.CanExecute());
                svm.Cancel.Command.Execute();
            });

            Assert.That(vm.Ribbon.Setting.Command.CanExecute());
            vm.Ribbon.Setting.Command.Execute();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Check
        ///
        /// <summary>
        /// Checks the default settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Check()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            var dest = vm.Value.Settings;
            Assert.That(dest.Width,           Is.EqualTo(800));
            Assert.That(dest.Height,          Is.EqualTo(600));
            Assert.That(dest.ItemSize,        Is.EqualTo(250));
            Assert.That(dest.FrameOnly,       Is.False);
            Assert.That(dest.ShrinkResources, Is.True);
            Assert.That(dest.RecentVisible,   Is.True);
            Assert.That(dest.Temp,            Is.Empty);

            vm.Value.Settings.Width  = 1024;
            vm.Value.Settings.Height = 768;
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// AssertObject
        ///
        /// <summary>
        /// Confirms the properties of the specified object.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /// <remarks>
        /// The NewVM method sets the Language.English to the generated
        /// MainViewModel object. Therefore, Language.Value will be English
        /// instead of Auto.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertObject(SettingViewModel src)
        {
            Assert.That(src.Title,                 Is.Not.Null.And.Not.Empty);
            Assert.That(src.Version.Text,          Is.Not.Null.And.Not.Empty);
            Assert.That(src.Version.Value,         Does.StartWith("CubePDF Utility 2.1.1 "));
            Assert.That(src.Windows.Text,          Does.StartWith("Microsoft Windows"));
            Assert.That(src.Framework.Text,        Does.StartWith("Microsoft .NET Framework"));
            Assert.That(src.Link.Text,             Is.EqualTo("Copyright © 2013 CubeSoft, Inc."));
            Assert.That(src.Link.Value.ToString(), Does.StartWith("https://www.cube-soft.jp/cubepdfutility/?lang="));
            Assert.That(src.CheckUpdate.Text,      Is.Not.Null.And.Not.Empty);
            Assert.That(src.Language.Text,         Is.Not.Null.And.Not.Empty);
            Assert.That(src.Language.Value,        Is.EqualTo(Language.English)); // see remarks.
            Assert.That(src.Languages.Count(),     Is.EqualTo(3));
        }

        #endregion
    }
}
