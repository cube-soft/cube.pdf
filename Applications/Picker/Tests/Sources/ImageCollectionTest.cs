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
using System;
using System.Collections.Generic;

namespace Cube.Pdf.Picker.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacadeTest
    ///
    /// <summary>
    /// Tests the MainFacade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    public class ImageCollectionTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the constructor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public int Extract(int id, string filename)
        {
            var dest = Get($"{nameof(Extract)}-{id}");
            using (var src = new ImageCollection(GetSource(filename), IO))
            {
                src.ExtractAsync(new Progress<Message<int>>()).Wait();
                src.Save(dest);
            }
            return IO.GetFiles(dest).Length;
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
                yield return new TestCaseData(n++, "Sample.pdf").Returns(2);
                yield return new TestCaseData(n++, "SampleAlpha.pdf").Returns(2);
                yield return new TestCaseData(n++, "SampleEmpty.pdf").Returns(0);
            }
        }

        #endregion
    }
}
