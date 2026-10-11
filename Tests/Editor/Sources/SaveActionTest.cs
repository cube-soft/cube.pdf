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
using System.Drawing;
using Cube.FileSystem;
using Cube.Tests;
using NUnit.Framework;
using iText.Kernel.Pdf;

namespace Cube.Pdf.Editor.Tests;

/* ------------------------------------------------------------------------- */
///
/// SaveActionTest
///
/// <summary>
/// Tests PNG dimensions produced by the save action.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class SaveActionTest : FileFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveAsPng
    ///
    /// <summary>
    /// Tests exact pixel dimensions and preserves truncation of fractions.
    /// </summary>
    ///
    /// <param name="width">Page width in points.</param>
    /// <param name="height">Page height in points.</param>
    /// <param name="dpi">Output resolution.</param>
    /// <param name="rotation">Original page rotation in degrees.</param>
    /// <param name="delta">Additional rotation in degrees.</param>
    /// <param name="x">Expected image width in pixels.</param>
    /// <param name="y">Expected image height in pixels.</param>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(144, 216, 75, 0, 0, 150, 225)]
    [TestCase(144, 216, 300, 0, 0, 600, 900)]
    [TestCase(144, 216, 600, 0, 0, 1200, 1800)]
    [TestCase(144, 216, 300, 90, 0, 900, 600)]
    [TestCase(144, 216, 300, 180, 0, 600, 900)]
    [TestCase(144, 216, 300, 270, 0, 900, 600)]
    [TestCase(144, 216, 300, 0, 90, 900, 600)]
    [TestCase(144, 216, 300, 0, 180, 600, 900)]
    [TestCase(144, 216, 300, 0, 270, 900, 600)]
    [TestCase(144, 216, 300, 90, 270, 600, 900)]
    [TestCase(100.5f, 200.5f, 72, 0, 0, 100, 200)]
    [TestCase(595, 842, 75, 0, 0, 619, 877)]
    public void SaveAsPng(float width, float height, int dpi, int rotation, int delta, int x, int y)
    {
        var src = Get("Dimension.pdf");
        using (var document = new PdfDocument(new PdfWriter(src)))
        {
            var page = document.AddNewPage(new iText.Kernel.Geom.PageSize(width, height));
            page.SetRotation(rotation);
            new iText.Kernel.Pdf.Canvas.PdfCanvas(page)
                .SetFillColor(iText.Kernel.Colors.ColorConstants.RED)
                .Rectangle(0, 0, width, height)
                .Fill();
        }

        using var renderer = new Pdfium.DocumentRenderer(src);
        var images = new ImageCollection(_ => renderer, Dispatcher.Vanilla);
        images.Add(renderer.Pages);
        images[0].RawObject.Delta = new(delta);
        using var action = new SaveAction(renderer, images, new()
        {
            Destination = Get("Dimension.png"),
            Format = SaveFormat.Png,
            Target = SaveTarget.All,
            Resolution = dpi,
        });
        try
        {
            action.Invoke(_ => { }, e =>
            {
                using var image = new Bitmap(e.FullName);
                Assert.That(image.Width, Is.EqualTo(x));
                Assert.That(image.Height, Is.EqualTo(y));
                Assert.That(image.GetPixel(x / 2, y / 2).ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
            });
        }
        finally { images.Clear(); }
    }
}
