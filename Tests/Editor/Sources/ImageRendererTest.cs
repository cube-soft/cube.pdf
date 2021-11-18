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
using System.Drawing.Imaging;
using Cube.FileSystem;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageRendererTest
    ///
    /// <summary>
    /// Tests the ImageRenderer class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ImageRendererTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Render_WithImage
        ///
        /// <summary>
        /// Tests the Render method with an image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Loading.png", 1,   0)]
        [TestCase("Sample.jpg",  1,   0)]
        [TestCase("Sample.jpg",  1,  90)]
        [TestCase("Sample.jpg",  1, 180)]
        [TestCase("Sample.jpg",  1, 270)]
        [TestCase("Sample.tiff", 1,   0)]
        [TestCase("Sample.tiff", 2,  90)]
        [TestCase("Sample.tiff", 3, 180)]
        public void Render_WithImage(string filename, int pagenum, int angle)
        {
            var src  = new ImagePageCollection(GetSource(filename));
            var page = src[pagenum - 1];

            page.Delta = new(angle);
            using var obj = new ImageRenderer().Render(page, new(500, 500));

            var dest = Get($"Render-{filename}-{pagenum}-{angle}.png");
            obj.Save(dest, ImageFormat.Png);
            Assert.That(Io.Exists(dest), dest);
        }

        #endregion
    }
}
