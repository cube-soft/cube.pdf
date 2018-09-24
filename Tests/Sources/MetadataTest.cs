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
using System;
using System.Collections.Generic;

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
    class MetadataTest : DocumentReaderFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Executes the test to get metadata of the specified PDF
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, Metadata cmp)
        {
            using (var r = Create(klass, GetExamplesWith(filename), ""))
            {
                var dest = r.Metadata;

                Assert.That(dest.Title,         Is.EqualTo(cmp.Title),    nameof(dest.Title));
                Assert.That(dest.Author,        Is.EqualTo(cmp.Author),   nameof(dest.Author));
                Assert.That(dest.Subject,       Is.EqualTo(cmp.Subject),  nameof(dest.Subject));
                Assert.That(dest.Keywords,      Is.EqualTo(cmp.Keywords), nameof(dest.Keywords));
                Assert.That(dest.Creator,       Is.EqualTo(cmp.Creator),  nameof(dest.Creator));
                Assert.That(dest.Producer,      Does.StartWith(cmp.Producer));
                Assert.That(dest.Version.Major, Is.EqualTo(cmp.Version.Major));
                Assert.That(dest.Version.Minor, Is.EqualTo(cmp.Version.Minor));

                // TODO: Implementation of PDFium is incomplete.
                // Assert.That(dest.Viewer, Is.EqualTo(cmp.Viewer));
            }
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
                    yield return new TestCaseData(klass, "Sample.pdf", new Metadata
                    {
                        Version  = new Version(1, 7, 0, 0),
                        Title    = "README",
                        Author   = "株式会社キューブ・ソフト",
                        Subject  = "",
                        Keywords = "",
                        Creator  = "CubePDF",
                        Producer = "GPL Ghostscript",
                        Viewer   = ViewerPreferences.None,
                    });

                    yield return new TestCaseData(klass, "SampleRotation.pdf", new Metadata
                    {
                        Version  = new Version(1, 7, 0, 0),
                        Title    = "テスト用文書",
                        Author   = "株式会社キューブ・ソフト",
                        Subject  = "Cube.Pdf.Tests",
                        Keywords = "CubeSoft,PDF,Test",
                        Creator  = "CubePDF",
                        Producer = "iTextSharp",
                        Viewer   = ViewerPreferences.TwoPageLeft | ViewerPreferences.Thumbnail,
                    });
                }
            }
        }

        #endregion
    }
}
