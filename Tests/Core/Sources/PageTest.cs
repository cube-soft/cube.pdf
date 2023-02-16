/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using Cube.Pdf.Extensions;
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageTest
    ///
    /// <summary>
    /// Tests for Page and its inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PageTest : DocumentReaderFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// Executes the test for getting page information of the
        /// specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void GetPage(string klass, string filename, int n, float w, float h, int degree)
        {
            using var src = Create(klass, GetSource(filename), "");
            var dest = src.GetPage(n);

            Assert.That(dest.Resolution.X,    Is.EqualTo(72.0f));
            Assert.That(dest.Resolution.Y,    Is.EqualTo(72.0f));
            Assert.That(dest.Size.Width,      Is.EqualTo(w));
            Assert.That(dest.Size.Height,     Is.EqualTo(h));
            Assert.That(dest.Rotation.Degree, Is.EqualTo(degree));

            dest.Delta = new Angle(90);
            dest.Reset();
            Assert.That(dest.Delta.Degree, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage
        ///
        /// <summary>
        /// Tests to get page information of the specified image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetImagePage()
        {
            var src  = GetSource("SampleImage02.png");
            var dest = new ImagePageCollection(src)[0];

            Assert.That(dest.Resolution.X,    Is.GreaterThan(1.0f));
            Assert.That(dest.Resolution.Y,    Is.GreaterThan(1.0f));
            Assert.That(dest.Size.Width,      Is.EqualTo(137));
            Assert.That(dest.Size.Height,     Is.EqualTo(157));
            Assert.That(dest.Rotation.Degree, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetImagePage_Throws
        ///
        /// <summary>
        /// Tests to confirm the result when the specified index is wrong.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetImagePage_Throws()
        {
            var src = GetSource("SampleImage02.png");
            Assert.That(
                () => new ImagePageCollection(src)[10],
                Throws.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewSize
        ///
        /// <summary>
        /// Executes the test for calculating the displayed size from
        /// the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(  0,  595.0f,  842.0f)]
        [TestCase( 45, 1016.1f, 1016.1f)]
        [TestCase( 90,  842.0f,  595.0f)]
        [TestCase(135, 1016.1f, 1016.1f)]
        [TestCase(180,  595.0f,  842.0f)]
        [TestCase(225, 1016.1f, 1016.1f)]
        [TestCase(270,  842.0f,  595.0f)]
        [TestCase(315, 1016.1f, 1016.1f)]
        public void GetViewSize(int degree, float w, float h)
        {
            var src = new Page2
            {
                File       = null,
                Number     = 1,
                Size       = new(595.0f, 842.0f),
                Rotation   = new(),
                Resolution = new(72, 72),
                Delta      = new(degree),
            };

            var dest = src.GetViewSize();
            Assert.That(dest.Width,  Is.EqualTo(w).Within(1.0));
            Assert.That(dest.Height, Is.EqualTo(h).Within(1.0));
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases { get
        {
            foreach (var klass in GetIds())
            {
                yield return new TestCaseData(klass, "SampleRotation.pdf", 1, 595.0f, 842.0f,   0);
                yield return new TestCaseData(klass, "SampleRotation.pdf", 2, 595.0f, 842.0f,  90);
                yield return new TestCaseData(klass, "SampleRotation.pdf", 3, 595.0f, 842.0f, 180);
                yield return new TestCaseData(klass, "SampleRotation.pdf", 4, 595.0f, 842.0f, 270);
                yield return new TestCaseData(klass, "SampleRotation.pdf", 5, 595.0f, 842.0f,   0);
            }
        }}

        #endregion
    }
}
