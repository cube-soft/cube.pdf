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
using Cube.Pdf.Mixin;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

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
        /// Get
        ///
        /// <summary>
        /// Executes the test for getting page information of the
        /// specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, int n, float w, float h, int degree)
        {
            var src = GetExamplesWith(filename);

            using (var reader = Create(klass, src, ""))
            {
                var dest = reader.GetPage(n);

                Assert.That(dest.Resolution.X,    Is.EqualTo(72.0f));
                Assert.That(dest.Resolution.Y,    Is.EqualTo(72.0f));
                Assert.That(dest.Size.Width,      Is.EqualTo(w));
                Assert.That(dest.Size.Height,     Is.EqualTo(h));
                Assert.That(dest.Rotation.Degree, Is.EqualTo(degree));

                dest.Delta = new Angle(90);
                dest.Reset();
                Assert.That(dest.Delta.Degree, Is.EqualTo(0));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get_Image
        ///
        /// <summary>
        /// Executes the test for getting page information of the
        /// specified image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Get_Image()
        {
            var src  = GetExamplesWith("SampleImage02.png");
            var dest = IO.GetImagePage(src, 0);

            Assert.That(dest.Resolution.X,    Is.EqualTo(96.0f));
            Assert.That(dest.Resolution.Y,    Is.EqualTo(96.0f));
            Assert.That(dest.Size.Width,      Is.EqualTo(137));
            Assert.That(dest.Size.Height,     Is.EqualTo(157));
            Assert.That(dest.Rotation.Degree, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get_Image_Throws
        ///
        /// <summary>
        /// Executes the test for confirming the result when the specified
        /// index is wrong.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Get_Image_Throws() => Assert.That(
            () => IO.GetImagePage(GetExamplesWith("SampleImage02.png"), 10),
            Throws.TypeOf<ExternalException>()
        );

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
            var src = new Page(
                null,                      // File
                1,                         // Number
                new SizeF(595.0f, 842.0f), // Size
                new Angle(),               // Rotation
                new PointF(72, 72)         // Resolution
            ) { Delta = new Angle(degree) };

            var dest = src.GetDisplaySize();
            Assert.That(dest.Value.Width,  Is.EqualTo(w).Within(1.0));
            Assert.That(dest.Value.Height, Is.EqualTo(h).Within(1.0));
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
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (var klass in GetClassIds())
                {
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 1, 595.0f, 842.0f,   0);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 2, 595.0f, 842.0f,  90);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 3, 595.0f, 842.0f, 180);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 4, 595.0f, 842.0f, 270);
                    yield return new TestCaseData(klass, "SampleRotation.pdf", 5, 595.0f, 842.0f,   0);
                }
            }
        }

        #endregion
    }
}
