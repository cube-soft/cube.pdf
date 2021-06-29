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
using Cube.FileSystem;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Pages.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// MergeTest
    ///
    /// <summary>
    /// Tests the Merge method of the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MergeTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Tests the Merge method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Merge(int id, IEnumerable<string> files)
        {
            var dest = Get($"{nameof(Merge)}-{id}.pdf");

            using (var vm = new MainViewModel(Enumerable.Empty<string>(), new()))
            using (vm.Subscribe<OpenFileMessage>(e => e.Value = files.Select(f => GetSource(f))))
            using (vm.Subscribe<SaveFileMessage>(e => e.Value = dest))
            {
                Assert.That(vm.Invokable, Is.False);
                Assert.That(vm.Test(vm.Add), nameof(vm.Add));
                Assert.That(vm.Invokable, Is.True);
                Assert.That(vm.Test(vm.Merge), nameof(vm.Merge));
                Assert.That(vm.GetFiles().Count(), Is.EqualTo(0));
            }

            Assert.That(Io.Exists(dest), Is.True);
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
                yield return new TestCaseData(n++, new[] { "Sample.pdf", "SampleRotation.pdf" });
                yield return new TestCaseData(n++, new[] { "Sample.pdf", "Sample.jpg" });
                yield return new TestCaseData(n++, new[] { "Sample.pdf", "Sample.pdf", "Sample.pdf" });
            }
        }

        #endregion
    }
}
