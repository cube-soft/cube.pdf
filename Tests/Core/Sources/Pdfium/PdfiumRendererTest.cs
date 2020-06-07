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
using Cube.Mixin.Pdf;
using Cube.Pdf.Pdfium;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
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
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// RenderImage
        ///
        /// <summary>
        /// Executes the test to render the specified page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Render(int id, string filename, int pagenum, int width, int height, RenderOption options)
        {
            using (var src = new DocumentRenderer(GetSource(filename)))
            {
                src.RenderOption = options;
                using (var bmp = src.Render(src.GetPage(pagenum)))
                {
                    Assert.That(bmp.Width, Is.EqualTo(width));
                    Assert.That(bmp.Height, Is.EqualTo(height));
                    bmp.Save(Get($"{nameof(Render)}-{id:D3}.png"), ImageFormat.Png);
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Render_Graphics
        ///
        /// <summary>
        /// Executes the test to render the specified page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Render_Graphics(int id, string filename, int pagenum, int width, int height, RenderOption options)
        {
            using (var src = new DocumentRenderer(GetSource(filename)))
            using (var bmp = new Bitmap(width, height))
            using (var gs  = Graphics.FromImage(bmp))
            {
                src.RenderOption = options;
                src.Render(gs, src.GetPage(pagenum));
                bmp.Save(Get($"{nameof(Render_Graphics)}-{id:D3}.png"), ImageFormat.Png);
            }
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
                yield return new TestCaseData(n++, "SampleRotation.pdf",   1, 595, 842, new RenderOption());
                yield return new TestCaseData(n++, "SampleRotation.pdf",   2, 842, 595, new RenderOption());
                yield return new TestCaseData(n++, "SampleRotation.pdf",   3, 595, 842, new RenderOption());
                yield return new TestCaseData(n++, "SampleRotation.pdf",   4, 842, 595, new RenderOption());
                yield return new TestCaseData(n++, "SampleImage.pdf",      1, 595, 842, new RenderOption());
                yield return new TestCaseData(n++, "SampleImage.pdf",      1, 595, 842, new RenderOption { Print = true });
                yield return new TestCaseData(n++, "SampleImage.pdf",      1, 595, 842, new RenderOption { Grayscale = true });
                yield return new TestCaseData(n++, "SampleImage.pdf",      1, 595, 842, new RenderOption { AntiAlias = false });
                yield return new TestCaseData(n++, "SampleImage.pdf",      1, 595, 842, new RenderOption { Background = Color.Black });
                yield return new TestCaseData(n++, "SampleAlpha.pdf",      1, 595, 841, new RenderOption());
                yield return new TestCaseData(n++, "SampleAnnotation.pdf", 1, 595, 842, new RenderOption { Annotation = true,  Background = Color.White });
                yield return new TestCaseData(n++, "SampleAnnotation.pdf", 1, 595, 842, new RenderOption { Annotation = false, Background = Color.White });
                yield return new TestCaseData(n++, "SampleForm.pdf",       1, 613, 859, new RenderOption { Annotation = true,  Background = Color.White });
                yield return new TestCaseData(n++, "SampleForm.pdf",       1, 613, 859, new RenderOption { Annotation = false, Background = Color.White });
                yield return new TestCaseData(n++, "SampleFormSign.pdf",   1, 595, 842, new RenderOption { Annotation = true,  Background = Color.White });
                yield return new TestCaseData(n++, "SampleFormSign.pdf",   1, 595, 842, new RenderOption { Annotation = false, Background = Color.White });
            }
        }

        #endregion
    }
}
