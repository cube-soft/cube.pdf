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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Pdf.Itext;
using Cube.Pdf.Mixin;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// BitmapTest
    ///
    /// <summary>
    /// Tests the DocumentWriter class with bitmap image files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class BitmapTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Tests to convert the specified bitmap file to the PDF.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.png",   30000L)]
        [TestCase("Sample.jpg",   30000L)]
        [TestCase("Sample.bmp",   30000L)]
        [TestCase("Sample.tiff", 300000L)]
        public void Save(string filename, long threshold)
        {
            var dest = Path(Args(filename));

            using (var w = new DocumentWriter())
            {
                w.Add(new ImagePageCollection(GetSource(filename)));
                w.Save(dest);
            }

            Assert.That(Io.Get(dest).Length, Is.LessThan(threshold));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge_Image
        ///
        /// <summary>
        /// Tests to merge a PDF document and an image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.png", 90, ExpectedResult = 10)]
        public int Merge(string filename, int degree)
        {
            var op = new OpenOption { SaveMemory = false };
            var r0 = new DocumentReader(GetSource("SampleBookmark.pdf"), "", op);
            var dest = Path(Args(r0.File.BaseName, Io.Get(filename).BaseName));

            using (var w = new DocumentWriter())
            {
                foreach (var p in r0.Pages) w.Add(Rotate(p, degree));
                w.Add(Rotate(new ImagePageCollection(GetSource(filename)), degree));
                w.Save(dest);
            }
            return Count(dest, "", degree);
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Args
        ///
        /// <summary>
        /// Converts params to an IEnumerable(object) object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<object> Args(params object[] src) => src;

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// Creates the path by using the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Path(IEnumerable<object> parts, [CallerMemberName] string name = null) =>
           Get($"{name}_{string.Join("_", parts.Select(e => e.ToString()).ToArray())}.pdf");

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Sets the Delta property of all specified Page objects
        /// so that the rotation of the pages become the specified
        /// degree.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Page> Rotate(IEnumerable<Page> src, int degree) =>
            src.Select(e => Rotate(e, degree));

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Sets the Delta property of the specified Page object
        /// so that the rotation of the page becomes the specified
        /// degree.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Page Rotate(Page src, int degree)
        {
            src.Delta = new Angle(degree - src.Rotation.Degree);
            return src;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int Count(string src, string password, int degree)
        {
            using var reader = new DocumentReader(src, password);
            Assert.That(reader.File.Count, Is.EqualTo(reader.Pages.Count()));
            Assert.That(
                reader.Pages.Select(e => e.Rotation.Degree),
                Is.EquivalentTo(Enumerable.Repeat(degree, reader.File.Count)),
                nameof(Page.Rotation)
            );
            return reader.File.Count;
        }

        #endregion
    }
}
