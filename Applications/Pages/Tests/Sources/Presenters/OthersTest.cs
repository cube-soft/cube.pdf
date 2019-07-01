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
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Pages.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// OthersTest
    ///
    /// <summary>
    /// Tests methods of the MainViewModel class except for the Merge and
    /// Split.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class OthersTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Tests the Move method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase( 1)]
        [TestCase(-1)]
        public void Move(int offset)
        {
            using (var vm = new MainViewModel(new SynchronizationContext()))
            {
                _ = vm.Subscribe<OpenFileMessage>(e => e.Value = new[] { GetSource("SampleRotation.pdf") });

                Assert.That(vm.Files, Is.Not.Null);
                Assert.That(vm.Test(vm.Add), nameof(vm.Add));
                Assert.That(vm.Test(() => vm.Move(new[] { 0, 1 }, offset)), nameof(vm.Move));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Tests the Remove method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Remove()
        {
            var files = new[] { "Sample.pdf", "SampleRotation.pdf", "Sample.jpg" };
            using (var vm = new MainViewModel(new SynchronizationContext()))
            {
                _ = vm.Subscribe<OpenFileMessage>(e => e.Value = files.Select(f => GetSource(f)));

                Assert.That(vm.Test(vm.Add), nameof(vm.Add));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(3));
                Assert.That(vm.Test(() => vm.Remove(new[] { 0, 2 })), nameof(vm.Remove));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(1));
                Assert.That(vm.Test(() => vm.Remove(Enumerable.Empty<int>())), nameof(vm.Remove));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(1));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Tests the Clear method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Clear()
        {
            var files = new[] { "Sample.pdf", "SampleRotation.pdf" };
            using (var vm = new MainViewModel(new SynchronizationContext()))
            {
                _ = vm.Subscribe<OpenFileMessage>(e => e.Value = files.Select(f => GetSource(f)));

                Assert.That(vm.Test(vm.Add), nameof(vm.Add));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(2));
                Assert.That(vm.Test(vm.Clear), nameof(vm.Clear));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(0));
                Assert.That(vm.Test(vm.Clear), nameof(vm.Clear));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(0));
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
    }
}
