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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cube.FileSystem;
using Cube.Tests;
using NUnit.Framework;

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
            Io.Copy(GetSource(filename), dest, true);

            var f0 = new[] { dest };
            var f1 = clips.Select(f => GetSource(f));

            using (var vm = new MainViewModel(new SynchronizationContext()))
            using (vm.Subscribe<OpenFileMessage>(e => e.Value = e.Multiselect ? f1 : f0))
            {
                Assert.That(vm.Source, Is.Null);
                Assert.That(vm.Test(vm.Open), nameof(vm.Open));
                Assert.That(vm.Source, Is.EqualTo(dest));
                Assert.That(vm.Test(vm.Attach), nameof(vm.Attach));
                Assert.That(vm.Test(vm.Save), nameof(vm.Save));
            }

            Assert.That(Io.Exists(dest), Is.True);
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
            var dest = Get($"{nameof(Reset)}.pdf");
            Io.Copy(GetSource("SampleAttachmentCjk.pdf"), dest, true);

            using (var vm = new MainViewModel(new SynchronizationContext()))
            using (vm.Subscribe<OpenFileMessage>(e => e.Value = new[] { dest }))
            {
                Assert.That(vm.Test(vm.Open), nameof(vm.Open));
                Assert.That(vm.GetClips().Count(), Is.EqualTo(3));
                Assert.That(vm.Test(() => vm.Detach(new[] { 0 })));
                Assert.That(vm.GetClips().Count(), Is.EqualTo(2));
                Assert.That(vm.Test(vm.Save), nameof(vm.Save));
            }

            Assert.That(Io.Exists(dest), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Tests the Reset and related methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Reset()
        {
            var dest = Get($"{nameof(Detach)}.pdf");
            Io.Copy(GetSource("SampleAttachmentCjk.pdf"), dest, true);

            var f0 = new[] { dest };
            var f1 = new[] { GetSource("Sample.jpg") };

            using (var vm = new MainViewModel(new SynchronizationContext()))
            using (vm.Subscribe<OpenFileMessage>(e => e.Value = e.Multiselect ? f1 : f0))
            {
                Assert.That(vm.Test(vm.Open), nameof(vm.Open));
                Assert.That(vm.GetClips().Count(), Is.EqualTo(3));

                Assert.That(vm.Test(() => vm.Detach(new[] { 0, 2 })));
                Assert.That(vm.GetClips().Count(), Is.EqualTo(1));

                Assert.That(vm.Test(vm.Attach), nameof(vm.Attach));
                Assert.That(vm.GetClips().Count(), Is.EqualTo(2));

                Assert.That(vm.Test(vm.Reset), nameof(vm.Reset));
                Assert.That(vm.GetClips().Count(), Is.EqualTo(3));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Throws
        ///
        /// <summary>
        /// Tests the constructor with an invalid context.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Throws()
        {
            Assert.That(() => { using (new MainViewModel()) { } }, Throws.ArgumentNullException);
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
        public static IEnumerable<TestCaseData> TestCases { get
        {
            var n = 0;
            yield return new TestCaseData(n++, "Sample.pdf", new[] { "Sample.jpg" });
        }}

        #endregion
    }
}
