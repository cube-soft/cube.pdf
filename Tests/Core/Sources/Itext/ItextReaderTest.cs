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
using System.Linq;
using Cube.Pdf.Itext;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// ItextReaderTest
    ///
    /// <summary>
    /// Tests the ITest.DocumentReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ItextReaderTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments_Count
        ///
        /// <summary>
        /// Tests the Count property of the embedded file collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf", ExpectedResult = 0)]
        [TestCase("SampleAttachment.pdf", ExpectedResult = 2)]
        [TestCase("SampleAttachmentEmpty.pdf", ExpectedResult = 3)]
        public int Attachments_Count(string filename)
        {
            using var reader = new DocumentReader(GetSource(filename));
            foreach (var obj in reader.Attachments)
            {
                Assert.That(obj.Data, Is.Not.Null);
                Assert.That(obj.Checksum.Length, Is.EqualTo(32));
            }
            return reader.Attachments.Count();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attachments_Length
        ///
        /// <summary>
        /// Tests the Length property of the specified embedded file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleAttachment.pdf", "CubePDF.png", ExpectedResult = 3765L)]
        [TestCase("SampleAttachment.pdf", "CubeICE.png", ExpectedResult = 165524L)]
        [TestCase("SampleAttachmentEmpty.pdf", "Empty", ExpectedResult = 0L)]
        [TestCase("SampleAttachmentCjk.pdf", "日本語のサンプル.md", ExpectedResult = 12843L)]
        public long Attachments_Length(string filename, string key)
        {
            using var src = new DocumentReader(GetSource(filename));
            var dest = src.Attachments.First(x => x.Name == key);
            Assert.That(dest.Data, Is.Not.Null);
            Assert.That(dest.Checksum.Length, Is.EqualTo(32));
            return dest.Length;
        }

        #endregion
    }
}
