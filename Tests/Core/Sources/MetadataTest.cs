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
using System.Collections.Generic;
using Cube.Backports;
using NUnit.Framework;
using VO = Cube.Pdf.ViewerOption;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataTest
    ///
    /// <summary>
    /// Tests the Metadata class through various IDocumentReader
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
        /// Tests the properties of Metadata object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Get(string klass, string filename, Metadata cmp)
        {
            using var src = Create(klass, GetSource(filename), "");
            var dest = src.Metadata;

            Assert.That(dest.Title,         Is.EqualTo(cmp.Title),    nameof(dest.Title));
            Assert.That(dest.Author,        Is.EqualTo(cmp.Author),   nameof(dest.Author));
            Assert.That(dest.Subject,       Is.EqualTo(cmp.Subject),  nameof(dest.Subject));
            Assert.That(dest.Keywords,      Is.EqualTo(cmp.Keywords), nameof(dest.Keywords));
            Assert.That(dest.Creator,       Is.EqualTo(cmp.Creator),  nameof(dest.Creator));
            Assert.That(dest.Producer,      Does.StartWith(cmp.Producer));
            Assert.That(dest.Version.Major, Is.EqualTo(cmp.Version.Major));
            Assert.That(dest.Version.Minor, Is.EqualTo(cmp.Version.Minor));

            // TODO: Implementation of PDFium is incomplete.
            // Assert.That(dest.Options, Is.EqualTo(cmp.Options));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetViewerOption
        ///
        /// <summary>
        /// Tests the Options property of the Metadata object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetViewerOption()
        {
            var src = GetSource("SampleViewerOption.pdf");
            using var dest = Create(nameof(Pdf.Itext), src, "");

            var pl = dest.Metadata.Options.ToPageLayout();
            Assert.That(pl.HasFlag(VO.TwoColumnLeft), Is.True, nameof(VO.TwoColumnLeft));
            Assert.That(pl.HasFlag(VO.TwoColumnRight), Is.False, nameof(VO.TwoColumnRight));
            Assert.That(pl.HasFlag(VO.TwoPageLeft), Is.False, nameof(VO.TwoPageLeft));
            Assert.That(pl.HasFlag(VO.TwoPageRight), Is.False, nameof(VO.TwoPageRight));
            Assert.That(pl.HasFlag(VO.SinglePage), Is.False, nameof(VO.SinglePage));
            Assert.That(pl.HasFlag(VO.OneColumn), Is.False, nameof(VO.OneColumn));
            Assert.That(pl.HasFlag(VO.Outline), Is.False, nameof(VO.Outline));

            var pm = dest.Metadata.Options.ToPageMode();
            Assert.That(pm.HasFlag(VO.Outline), Is.True, nameof(VO.Outline));
            Assert.That(pm.HasFlag(VO.None), Is.True, nameof(VO.None));
            Assert.That(pm.HasFlag(VO.Thumbnail), Is.False, nameof(VO.Thumbnail));
            Assert.That(pm.HasFlag(VO.FullScreen), Is.False, nameof(VO.FullScreen));
            Assert.That(pm.HasFlag(VO.Attachment), Is.False, nameof(VO.Attachment));
            Assert.That(pm.HasFlag(VO.TwoColumnLeft), Is.False, nameof(VO.TwoColumnLeft));
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
                    yield return new(klass, "Sample.pdf", new Metadata
                    {
                        Version  = new PdfVersion(1, 7),
                        Title    = "README",
                        Author   = "株式会社キューブ・ソフト",
                        Subject  = "",
                        Keywords = "",
                        Creator  = "CubePDF",
                        Producer = "GPL Ghostscript",
                        Options  = VO.None,
                    });

                    yield return new(klass, "SampleRotation.pdf", new Metadata
                    {
                        Version  = new PdfVersion(1, 7),
                        Title    = "テスト用文書",
                        Author   = "株式会社キューブ・ソフト",
                        Subject  = "Cube.Pdf.Tests",
                        Keywords = "CubeSoft,PDF,Test",
                        Creator  = "CubePDF",
                        Producer = "iTextSharp",
                        Options   = VO.TwoPageLeft | VO.Thumbnail,
                    });
                }
            }
        }

        #endregion
    }
}
