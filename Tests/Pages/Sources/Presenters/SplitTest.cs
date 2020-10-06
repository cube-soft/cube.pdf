/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Pages.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// SplitTest
    ///
    /// <summary>
    /// Tests the Split method of the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SplitTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Tests the Split method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public int Split(int id, string filename)
        {
            var dest = Get($"{nameof(Split)}-{id}");

            using (var vm = new MainViewModel(new SynchronizationContext()))
            using (vm.Subscribe<OpenFileMessage>(e => e.Value = new[] { GetSource(filename) }))
            using (vm.Subscribe<OpenDirectoryMessage>(e => e.Value = dest))
            {
                Assert.That(vm.Test(vm.Add), nameof(vm.Add));
                Assert.That(vm.Test(vm.Split), nameof(vm.Split));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(0));
            }

            return IO.GetFiles(dest).Count();
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
                yield return new TestCaseData(n++, "SampleRotation.pdf").Returns(9);
                yield return new TestCaseData(n++, "Sample.jpg").Returns(1);
            }
        }

        #endregion
    }
}
