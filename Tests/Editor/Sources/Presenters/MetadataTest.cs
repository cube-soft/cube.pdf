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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cube.Mixin.Commands;
using Cube.Tests;
using Cube.Xui;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionTest
    ///
    /// <summary>
    /// Tests for the EncryptionViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MetadataTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Executes the test for setting the PDF metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Set(int id, Metadata cmp) => Open("Sample.pdf", "", vm =>
        {
            var cts = new CancellationTokenSource();
            vm.Value.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.Value.Metadata)) cts.Cancel();
            };

            using (Subscribe(vm, cmp))
            {
                Assert.That(vm.Value.Metadata, Is.Not.Null);
                Assert.That(Test(vm.Ribbon.Metadata, cts), $"Timeout (No.{id})");
                AssertEquals(vm.Value.Metadata, cmp);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Executes the test for selecting the cancel button in the
        /// MetadataWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel() => Open("Sample.pdf", "", vm =>
        {
            var cts = new CancellationTokenSource();
            using (vm.Subscribe<MetadataViewModel>(e => {
                e.Document.Value = "dummy";
                using (e.Subscribe<CloseMessage>(z => cts.Cancel())) e.Cancel.Command.Execute();
            })) {
                Assert.That(vm.Value.Metadata, Is.Not.Null);
                Assert.That(Test(vm.Ribbon.Metadata, cts), "Timeout");
                Assert.That(vm.Value.Metadata.Title, Is.Not.EqualTo("dummy"));
            };
        });

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
                var n = 0;

                yield return new TestCaseData(n++, new Metadata
                {
                    Title    = "Test title",
                    Author   = "Test author",
                    Subject  = "Test subject",
                    Keywords = "Test keywords",
                    Creator  = "Test creator",
                    Producer = "Test producer",
                    Version  = new PdfVersion(1, 6),
                    Options  = ViewerOption.TwoColumnRight,
                });

                yield return new TestCaseData(n++, new Metadata
                {
                    Title    = "日本語のタイトル",
                    Author   = "日本語の著者",
                    Subject  = "日本語のサブタイトル",
                    Keywords = "日本語のキーワード",
                    Creator  = "日本語のアプリケーション",
                    Producer = "日本語の PDF 変換",
                    Version  = new PdfVersion(1, 7),
                    Options  = ViewerOption.OneColumn,
                });
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Subscribe
        ///
        /// <summary>
        /// Sets the operation corresponding to the MetadataViewModel
        /// message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable Subscribe(MainViewModel vm, Metadata src) =>
            vm.Subscribe<MetadataViewModel>(e =>
        {
            vm.Value.Settings.Language = Language.English;
            Assert.That(e.Title,               Is.EqualTo("PDF Metadata"));
            Assert.That(e.Versions.Count(),    Is.EqualTo(6), nameof(e.Versions));
            Assert.That(e.Options.Count(),     Is.EqualTo(6), nameof(e.Options));
            Assert.That(e.Document.Text,       Is.EqualTo("Title"));
            Assert.That(e.Author.Text,         Is.EqualTo("Author"));
            Assert.That(e.Subject.Text,        Is.EqualTo("Subject"));
            Assert.That(e.Keywords.Text,       Is.EqualTo("Keywords"));
            Assert.That(e.Creator.Text,        Is.EqualTo("Creator"));
            Assert.That(e.Option.Text,         Is.EqualTo("Layout"));
            Assert.That(e.Version.Text,        Is.EqualTo("Version"));
            Assert.That(e.Filename.Text,       Is.EqualTo("Filename"));
            Assert.That(e.Filename.Value,      Is.Not.Null.And.Not.Empty);
            Assert.That(e.Producer.Text,       Is.EqualTo("Producer"));
            Assert.That(e.Producer.Value,      Is.Not.Null.And.Not.Empty);
            Assert.That(e.Length.Text,         Is.EqualTo("Filesize"));
            Assert.That(e.Length.Value,        Is.GreaterThan(0));
            Assert.That(e.CreationTime.Text,   Is.EqualTo("Creation"));
            Assert.That(e.CreationTime.Value,  Is.GreaterThan(DateTime.MinValue));
            Assert.That(e.LastWriteTime.Text,  Is.EqualTo("Last updated"));
            Assert.That(e.LastWriteTime.Value, Is.GreaterThan(DateTime.MinValue));
            Assert.That(e.Summary.Text,        Is.EqualTo("Summary"));
            Assert.That(e.Details.Text,        Is.EqualTo("Details"));

            e.Document.Value = src.Title;
            e.Author.Value   = src.Author;
            e.Subject.Value  = src.Subject;
            e.Keywords.Value = src.Keywords;
            e.Creator.Value  = src.Creator;
            e.Option.Value  = src.Options;
            e.Version.Value  = src.Version;
            Assert.That(e.OK.Command.CanExecute(), Is.True);
            e.OK.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// AssertEquals
        ///
        /// <summary>
        /// Confirms that properties of the specified objects are equal.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertEquals(Metadata src, Metadata cmp)
        {
            Assert.That(src.Title,         Is.EqualTo(cmp.Title),    nameof(src.Title));
            Assert.That(src.Author,        Is.EqualTo(cmp.Author),   nameof(src.Author));
            Assert.That(src.Subject,       Is.EqualTo(cmp.Subject),  nameof(src.Subject));
            Assert.That(src.Keywords,      Is.EqualTo(cmp.Keywords), nameof(src.Keywords));
            Assert.That(src.Creator,       Is.EqualTo(cmp.Creator),  nameof(src.Creator));
            Assert.That(src.Options,       Is.EqualTo(cmp.Options));
            Assert.That(src.Version.Major, Is.EqualTo(cmp.Version.Major));
            Assert.That(src.Version.Minor, Is.EqualTo(cmp.Version.Minor));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the command of the specified element.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool Test(IElement src, CancellationTokenSource cts)
        {
            Assert.That(src.Command.CanExecute(), Is.True);
            src.Command.Execute();
            return Wait.For(cts.Token);
        }

        #endregion
    }
}
