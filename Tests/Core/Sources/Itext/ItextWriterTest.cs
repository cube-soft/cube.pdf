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
    /// ItextWriterTest
    ///
    /// <summary>
    /// DocumentWriter および DocumentSplitter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ItextWriterTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Executes the test for saving the PDF document as a new file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",           "",           0, ExpectedResult = 2)]
        [TestCase("SampleAnnotation.pdf", "",          90, ExpectedResult = 2)]
        [TestCase("SampleBookmark.pdf",   "",         180, ExpectedResult = 9)]
        [TestCase("SampleAttachment.pdf", "",         270, ExpectedResult = 9)]
        [TestCase("SampleRc128.pdf",      "password", 180, ExpectedResult = 2)]
        [TestCase("SampleAes256.pdf",     "password",  90, ExpectedResult = 9)]
        public int Save(string filename, string password, int degree)
        {
            var src  = GetSource(filename);
            var dest = Path(Args(filename));

            using (var w = new DocumentWriter(new() { Smart = true }))
            using (var r = new DocumentReader(src, password))
            {
                w.Set(r.Metadata);
                w.Set(r.Encryption);
                w.Add(Rotate(r.Pages, degree));
                w.Save(dest);
            }
            return Count(dest, password, degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// Executes the test for overwriting the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",      "",          0, ExpectedResult = 2)]
        [TestCase("SampleRc128.pdf", "password", 90, ExpectedResult = 2)]
        public int Overwrite(string filename, string password, int degree)
        {
            var dest = Path(Args(filename));
            Io.Copy(GetSource(filename), dest, true);

            var op = new OpenOption { SaveMemory = false };
            var r  = new DocumentReader(dest, password, op);
            using (var w = new DocumentWriter(new() { Smart = true }))
            {
                w.Set(r.Metadata);
                w.Set(r.Encryption);
                w.Add(Rotate(r.Pages, degree), r);
                w.Save(dest);
            }
            return Count(dest, password, degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// Executes the test for merging PDF documents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", "SampleBookmark.pdf", 90, ExpectedResult = 11)]
        [TestCase("Sample.pdf", "Sample.pdf",          0, ExpectedResult =  4)]
        public int Merge(string f0, string f1, int degree)
        {
            var op   = new OpenOption { SaveMemory = false };
            var r0   = new DocumentReader(GetSource(f0), "", op);
            var r1   = new DocumentReader(GetSource(f1), "", op);
            var dest = Path(Args(r0.File.BaseName, r1.File.BaseName));

            using (var w = new DocumentWriter(new() { Smart = true }))
            {
                foreach (var p in r0.Pages) w.Add(Rotate(p, degree), r0);
                w.Add(Rotate(r1.Pages, degree), r1);
                w.Save(dest);
            }
            return Count(dest, "", degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Merge_Image
        ///
        /// <summary>
        /// Executes the test for merging a PDF document and an image file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleBookmark.pdf", "SampleImage01.png", 90, ExpectedResult = 10)]
        public int Merge_Image(string doc, string image, int degree)
        {
            var op   = new OpenOption { SaveMemory = false };
            var r0   = new DocumentReader(GetSource(doc), "", op);
            var dest = Path(Args(r0.File.BaseName, Io.Get(image).BaseName));

            using (var w = new DocumentWriter(new() { Smart = true }))
            using (var r = new DocumentReader(GetSource(doc), "", op))
            {
                foreach (var p in r0.Pages) w.Add(Rotate(p, degree));
                w.Add(Rotate(new ImagePageCollection(GetSource(image)), degree));
                w.Save(dest);
            }
            return Count(dest, "", degree);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Executes the test for splitting a PDF document in page by page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleBookmark.pdf", "",         ExpectedResult = 9)]
        [TestCase("SampleRc128.pdf",    "password", ExpectedResult = 2)]
        public int Split(string filename, string password)
        {
            var src  = GetSource(filename);
            var info = Io.Get(src);
            var name = info.BaseName;
            var ext  = info.Extension;
            var dest = Path(Args(name));

            Io.Copy(src, Io.Combine(dest, $"{name}-01{ext}"), true);

            using var w = new DocumentSplitter(new());
            var op = new OpenOption { SaveMemory = false };
            w.Add(new DocumentReader(src, password, op));
            w.Save(dest);

            var n = w.Results.Count;
            var cmp = Io.GetFiles(dest).Count();
            Assert.That(cmp, Is.EqualTo(n + 1));
            Assert.That(Io.Exists(Io.Combine(dest, $"{name}-01(1){ext}")));

            w.Reset();
            Assert.That(w.Results.Count, Is.EqualTo(0));

            return n;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Executes the test for attaching a file to a PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleRotation.pdf",   "SampleImage01.png",   ExpectedResult = 1)]
        [TestCase("SampleAttachment.pdf", "SampleImage02.png",   ExpectedResult = 3)]
        [TestCase("SampleAttachment.pdf", "日本語のサンプル.md", ExpectedResult = 3)]
        public int Attach(string doc, string file)
        {
            var op   = new OpenOption { SaveMemory = false };
            var src  = GetSource(doc);
            var r0   = new DocumentReader(src, "", op);
            var r1   = Io.Get(GetSource(file));
            var dest = Path(Args(r0.File.BaseName, r1.BaseName));

            using (var w = new DocumentWriter())
            {
                w.Add(r0);
                w.Add(r0.Attachments);
                w.Attach(new Attachment(r1.FullName));
                w.Attach(new Attachment(r1.FullName)); // Skip duplicated object.
                w.Save(dest);
            }

            using var r = new DocumentReader(dest, "", op);
            var items = r.Attachments;
            Assert.That(items.Any(x => x.Name.FuzzyEquals(file)), Is.True);
            foreach (var obj in items) Assert.That(obj.Length, Is.AtLeast(1));
            return items.Count();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetMetadata
        ///
        /// <summary>
        /// Executes the test for setting PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Metadata")]
        [TestCase("日本語のテスト")]
        public void SetMetadata(string value)
        {
            var src  = GetSource("SampleViewerOption.pdf");
            var dest = Path(Args(value));
            var op   = new OpenOption { SaveMemory = true };
            var cmp  = new Metadata
            {
                Title    = value,
                Author   = value,
                Subject  = value,
                Keywords = value,
                Creator  = value,
                Producer = value,
                Version  = new PdfVersion(1, 5),
                Options  = ViewerOption.TwoColumnLeft,
            };

            using (var w = new DocumentWriter(new()))
            {
                w.Set(cmp);
                w.Add(new DocumentReader(src, "", op));
                w.Save(dest);
            }

            using var r = new DocumentReader(dest, "", op);
            var m = r.Metadata;
            Assert.That(m.Title,         Is.EqualTo(cmp.Title), nameof(m.Title));
            Assert.That(m.Author,        Is.EqualTo(cmp.Author), nameof(m.Author));
            Assert.That(m.Subject,       Is.EqualTo(cmp.Subject), nameof(m.Subject));
            Assert.That(m.Keywords,      Is.EqualTo(cmp.Keywords), nameof(m.Keywords));
            Assert.That(m.Creator,       Is.EqualTo(cmp.Creator), nameof(m.Creator));
            Assert.That(m.Producer,      Does.StartWith("iText"));
            Assert.That(m.Version.Major, Is.EqualTo(cmp.Version.Major));
            Assert.That(m.Version.Minor, Is.EqualTo(cmp.Version.Minor));
            Assert.That(m.Options,       Is.EqualTo(cmp.Options));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetEncryption
        ///
        /// <summary>
        /// Executes the test for setting the encryption settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(EncryptionMethod.Aes128, 0xfffff0c0L)]
        public void SetEncryption(EncryptionMethod method, long permission)
        {
            var src  = GetSource("Sample.pdf");
            var dest = Path(Args(method, permission));
            var op   = new OpenOption { SaveMemory = false };
            var cmp  = new Encryption
            {
                OwnerPassword    = "owner",
                UserPassword     = "user",
                OpenWithPassword = true,
                Method           = method,
                Enabled          = true,
                Permission       = new Permission(permission),
            };

            using (var w = new DocumentWriter(new()))
            {
                w.Set(cmp);
                w.Add(new DocumentReader(src, "", op));
                w.Save(dest);
            }

            using var r = new DocumentReader(dest, cmp.OwnerPassword);
            Assert.That(r.Encryption.Enabled,       Is.True);
            Assert.That(r.Encryption.OwnerPassword, Is.EqualTo(cmp.OwnerPassword));
            Assert.That(r.Encryption.Method,        Is.EqualTo(cmp.Method));

            var x = r.Encryption.Permission;
            var y = cmp.Permission;
            Assert.That(x.Print,             Is.EqualTo(y.Print),             nameof(x.Print));
            Assert.That(x.CopyContents,      Is.EqualTo(y.CopyContents),      nameof(x.CopyContents));
            Assert.That(x.ModifyContents,    Is.EqualTo(y.ModifyContents),    nameof(x.ModifyContents));
            Assert.That(x.ModifyAnnotations, Is.EqualTo(y.ModifyAnnotations), nameof(x.ModifyAnnotations));
            Assert.That(x.InputForm,         Is.EqualTo(y.InputForm),         nameof(x.InputForm));
            Assert.That(x.Accessibility,     Is.EqualTo(y.Accessibility),     nameof(x.Accessibility));
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
           Get($"{name}_{string.Join("_", parts)}.pdf");

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
