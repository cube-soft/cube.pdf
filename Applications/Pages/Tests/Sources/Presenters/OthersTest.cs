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
        /// Create_ArgumentNullException
        ///
        /// <summary>
        /// Tests the constructor with an invalid context.
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

                vm.Add();
                Assert.That(Wait.For(() => !vm.Busy), "Timeout (Add)");
                vm.Move(new[] { 0, 1 }, offset);
                Assert.That(Wait.For(() => !vm.Busy), "Timeout (Move)");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_ArgumentNullException
        ///
        /// <summary>
        /// Tests the constructor with an invalid context.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_ArgumentNullException()
        {
            Assert.That(
                () => { using (new MainViewModel()) { } },
                Throws.ArgumentNullException
            );
        }

        #endregion
    }
}
