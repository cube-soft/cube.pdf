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
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataTest
    ///
    /// <summary>
    /// Tests for the Metadata class through various IDocumentReader
    /// implementations.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PdfVersionTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetString
        ///
        /// <summary>
        /// Executes the test to get string that represents PDF version.
        /// </summary>
        ///
        /// <remarks>
        /// 既知の PDF バージョンおよびサブセットを列挙します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("",      1, 2, 0, ExpectedResult = "PDF 1.2")]
        [TestCase("",      1, 3, 0, ExpectedResult = "PDF 1.3")]
        [TestCase("",      1, 4, 0, ExpectedResult = "PDF 1.4")]
        [TestCase("",      1, 5, 0, ExpectedResult = "PDF 1.5")]
        [TestCase("",      1, 6, 0, ExpectedResult = "PDF 1.6")]
        [TestCase("",      1, 7, 0, ExpectedResult = "PDF 1.7")]
        [TestCase("",      1, 7, 1, ExpectedResult = "PDF 1.7 ExtensionLevel 1")]
        [TestCase("",      1, 7, 3, ExpectedResult = "PDF 1.7 ExtensionLevel 3")]
        [TestCase("",      1, 7, 5, ExpectedResult = "PDF 1.7 ExtensionLevel 5")]
        [TestCase("",      1, 7, 6, ExpectedResult = "PDF 1.7 ExtensionLevel 6")]
        [TestCase("",      1, 7, 8, ExpectedResult = "PDF 1.7 ExtensionLevel 8")]
        [TestCase("",      2, 0, 0, ExpectedResult = "PDF 2.0")]
        [TestCase("A-1a",  1, 4, 0, ExpectedResult = "PDF/A-1a 1.4")]
        [TestCase("A-1b",  1, 4, 0, ExpectedResult = "PDF/A-1b 1.4")]
        [TestCase("A-2",   1, 7, 0, ExpectedResult = "PDF/A-2 1.7")]
        [TestCase("A-3",   1, 7, 0, ExpectedResult = "PDF/A-3 1.7")]
        [TestCase("E-1",   1, 6, 0, ExpectedResult = "PDF/E-1 1.6")]
        [TestCase("UA-1",  1, 7, 0, ExpectedResult = "PDF/UA-1 1.7")]
        [TestCase("VT-1",  1, 6, 0, ExpectedResult = "PDF/VT-1 1.6")]
        [TestCase("VT-2",  1, 6, 0, ExpectedResult = "PDF/VT-2 1.6")]
        [TestCase("VT-2s", 1, 6, 0, ExpectedResult = "PDF/VT-2s 1.6")]
        [TestCase("X-1a",  1, 3, 0, ExpectedResult = "PDF/X-1a 1.3")]
        [TestCase("X-1a",  1, 4, 0, ExpectedResult = "PDF/X-1a 1.4")]
        [TestCase("X-3",   1, 3, 0, ExpectedResult = "PDF/X-3 1.3")]
        [TestCase("X-3",   1, 4, 0, ExpectedResult = "PDF/X-3 1.4")]
        [TestCase("X-4",   1, 6, 0, ExpectedResult = "PDF/X-4 1.6")]
        [TestCase("X-4p",  1, 6, 0, ExpectedResult = "PDF/X-4p 1.6")]
        [TestCase("X-5",   1, 6, 0, ExpectedResult = "PDF/X-5 1.6")]
        public string GetString(string subset, int major, int minor, int extension) =>
            new PdfVersion(subset, major, minor, extension).ToString();

        /* ----------------------------------------------------------------- */
        ///
        /// GetString_Default
        ///
        /// <summary>
        /// Executes the test to get string that represents PDF version.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetString_Default() =>
            Assert.That(new PdfVersion().ToString(), Is.EqualTo("PDF 1.0"));

        #endregion
    }
}
