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
using Cube.FileSystem.Tests;
using Cube.Pdf.Mixin;
using Cube.Pdf.Pdfium;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReaderTest
    ///
    /// <summary>
    /// DocumentReader のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentReaderTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// 各ページの情報を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf", 1, 595.0f, 842.0f,   0)]
        [TestCase("SampleRotation.pdf", 2, 595.0f, 842.0f,  90)]
        [TestCase("SampleRotation.pdf", 3, 595.0f, 842.0f, 180)]
        [TestCase("SampleRotation.pdf", 4, 595.0f, 842.0f, 270)]
        [TestCase("SampleRotation.pdf", 5, 595.0f, 842.0f,   0)]
        public void GetPage(string filename, int n, float w, float h, int degree)
        {
            using (var reader = new DocumentReader(GetExamplesWith(filename)))
            {
                var dest = reader.GetPage(n);

                Assert.That(dest.Resolution.X, Is.EqualTo(72.0f));
                Assert.That(dest.Resolution.Y, Is.EqualTo(72.0f));
                Assert.That(dest.Size.Width,   Is.EqualTo(w));
                Assert.That(dest.Size.Height,  Is.EqualTo(h));
                Assert.That(dest.Rotation,     Is.EqualTo(degree));
            }
        }

        #endregion
    }
}
