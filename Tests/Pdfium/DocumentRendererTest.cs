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
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Pdf.Tests.Pdfium
{
    /* --------------------------------------------------------------------- */
    ///
    /// MetadataTest
    ///
    /// <summary>
    /// Metadata のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentRendererTest : DocumentReaderFixture
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
        [TestCase("SampleRotation.pdf", 1)]
        [TestCase("SampleRotation.pdf", 2)]
        [TestCase("SampleRotation.pdf", 3)]
        [TestCase("SampleRotation.pdf", 4)]
        public void Render(string filename, int pagenum)
        {
            try
            {
                var src = GetExamplesWith(filename);
                var dest = GetResultsWith($"{IO.Get(src).NameWithoutExtension}-{pagenum}.png");

                using (var reader = new DocumentReader(src))
                {
                    var page = reader.GetPage(pagenum);
                    var vs   = page.GetViewSize();
                    var w    = (int)(vs.Width * 1.0);
                    var h    = (int)(vs.Height * 1.0);

                    using (var image = new Bitmap(w, h))
                    using (var gs = Graphics.FromImage(image))
                    {
                        reader.Render(gs, pagenum, new Point(0, 0), new Size(w, h), 0);
                        image.Save(dest, ImageFormat.Png);
                    }
                }

                Assert.That(IO.Exists(dest), Is.True);
            }
            catch (System.Exception err) { Log.Logger.Warn(GetType(), err.ToString()); throw; }
        }
    }
}
