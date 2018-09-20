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
using Cube.Pdf.Pdfium;
using NUnit.Framework;
using System.Drawing.Imaging;

namespace Cube.Pdf.Tests.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumRendererTest
    ///
    /// <summary>
    /// Tests for the DocumentRenderer class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PdfiumRendererTest : DocumentReaderFixture
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Render
        ///
        /// <summary>
        /// PDF の描画テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf", 1, 0.5, 297, 421)]
        [TestCase("SampleRotation.pdf", 2, 0.5, 421, 297)]
        [TestCase("SampleRotation.pdf", 3, 0.5, 297, 421)]
        [TestCase("SampleRotation.pdf", 4, 0.5, 421, 297)]
        public void Render(string filename, int pagenum, double ratio, int width, int height)
        {
            var src  = GetExamplesWith(filename);
            var dest = GetResultsWith($"{IO.Get(src).NameWithoutExtension}-{pagenum}.png");

            using (var reader = new DocumentReader(src))
            {
                var page = reader.GetPage(pagenum);
                using (var image = reader.GetImage(page, ratio))
                {
                    Assert.That(image.Width,  Is.EqualTo(width));
                    Assert.That(image.Height, Is.EqualTo(height));
                    image.Save(dest, ImageFormat.Png);
                }
            }

            Assert.That(IO.Exists(dest), Is.True);
        }
    }
}
