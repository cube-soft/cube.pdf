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
using Cube.Tests;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Clip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModelTest
    ///
    /// <summary>
    /// Tests the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MainViewModelTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Tests the Attach and related methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Attach(int id, string filename, IEnumerable<string> clips)
        {
            var dest = Get($"{nameof(Attach)}-{id}.pdf");
            IO.Copy(GetSource(filename), dest);

            using (var vm = new MainViewModel(new SynchronizationContext()))
            {
                _ = vm.Subscribe<OpenSourceMessage>(e => e.Value = new[] { dest });
                _ = vm.Subscribe<AttachFileMessage>(e => e.Value = clips.Select(f => GetSource(f)));

                Assert.That(vm.Test(vm.Open), nameof(vm.Open));
                Assert.That(vm.Test(vm.Attach), nameof(vm.Attach));
                Assert.That(vm.Test(vm.Save), nameof(vm.Save));
            }

            Assert.That(IO.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// Tests the Detach and related methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Detach()
        {
            var dest = Get($"{nameof(Detach)}.pdf");
            IO.Copy(GetSource("SampleAttachmentCjk.pdf"), dest);

            using (var vm = new MainViewModel(new SynchronizationContext()))
            {
                _ = vm.Subscribe<OpenSourceMessage>(e => e.Value = new[] { dest });

                Assert.That(vm.Test(vm.Open), nameof(vm.Open));
                Assert.That(vm.Test(() => vm.Detach(new[] { 0 })));
                Assert.That(vm.Test(vm.Save), nameof(vm.Save));
            }

            Assert.That(IO.Exists(dest), Is.True);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets the test cases.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                var n = 0;
                yield return new TestCaseData(n++, "Sample.pdf", new[] { "Sample.jpg" });
            }
        }

        #endregion
    }
}
