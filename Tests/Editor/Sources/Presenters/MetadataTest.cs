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
using Cube.Mixin.Commands;
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
    class MetadataTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the normal case of the MetadataViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Test(int _, Metadata cmp)
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = Subscribe(vm, cmp);

            Assert.That(vm.Ribbon.Metadata.Command.CanExecute());
            vm.Ribbon.Metadata.Command.Execute();
            Assert.That(vm.Value.Metadata, Is.Not.Null);
            AssertEquals(vm.Value.Metadata, cmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Tests the Cancel command in the MetadataViewModel.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = vm.Subscribe<MetadataViewModel>(mvm => {
                mvm.Document.Value = "dummy";
                mvm.Cancel.Command.Execute();
            });

            Assert.That(vm.Ribbon.Metadata.Command.CanExecute());
            vm.Ribbon.Metadata.Command.Execute();
            Assert.That(vm.Value.Metadata, Is.Not.Null);
            Assert.That(vm.Value.Metadata.Title, Is.Not.EqualTo("dummy"));
        }

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
        public static IEnumerable<TestCaseData> TestCases { get
        {
            var n = 0;

            yield return new(n++, new Metadata
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

            yield return new(n++, new Metadata
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
        }}

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
            vm.Subscribe<MetadataViewModel>(mvm =>
        {
            vm.Value.Settings.Language = Language.English;
            Assert.That(mvm.Title,               Is.EqualTo("PDF Metadata"));
            Assert.That(mvm.Versions.Count(),    Is.EqualTo(6), nameof(mvm.Versions));
            Assert.That(mvm.Options.Count(),     Is.EqualTo(6), nameof(mvm.Options));
            Assert.That(mvm.Document.Text,       Is.EqualTo("Title"));
            Assert.That(mvm.Author.Text,         Is.EqualTo("Author"));
            Assert.That(mvm.Subject.Text,        Is.EqualTo("Subject"));
            Assert.That(mvm.Keywords.Text,       Is.EqualTo("Keywords"));
            Assert.That(mvm.Creator.Text,        Is.EqualTo("Creator"));
            Assert.That(mvm.Option.Text,         Is.EqualTo("Layout"));
            Assert.That(mvm.Version.Text,        Is.EqualTo("Version"));
            Assert.That(mvm.Filename.Text,       Is.EqualTo("Filename"));
            Assert.That(mvm.Filename.Value,      Is.Not.Null.And.Not.Empty);
            Assert.That(mvm.Producer.Text,       Is.EqualTo("Producer"));
            Assert.That(mvm.Producer.Value,      Is.Not.Null.And.Not.Empty);
            Assert.That(mvm.Length.Text,         Is.EqualTo("Filesize"));
            Assert.That(mvm.Length.Value,        Is.GreaterThan(0));
            Assert.That(mvm.CreationTime.Text,   Is.EqualTo("Creation"));
            Assert.That(mvm.CreationTime.Value,  Is.GreaterThan(DateTime.MinValue));
            Assert.That(mvm.LastWriteTime.Text,  Is.EqualTo("Last updated"));
            Assert.That(mvm.LastWriteTime.Value, Is.GreaterThan(DateTime.MinValue));
            Assert.That(mvm.Summary.Text,        Is.EqualTo("Summary"));
            Assert.That(mvm.Details.Text,        Is.EqualTo("Details"));

            mvm.Document.Value = src.Title;
            mvm.Author.Value   = src.Author;
            mvm.Subject.Value  = src.Subject;
            mvm.Keywords.Value = src.Keywords;
            mvm.Creator.Value  = src.Creator;
            mvm.Option.Value   = src.Options;
            mvm.Version.Value  = src.Version;

            Assert.That(mvm.OK.Command.CanExecute());
            mvm.OK.Command.Execute();
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
            Assert.That(src.Title,    Is.EqualTo(cmp.Title),    nameof(src.Title));
            Assert.That(src.Author,   Is.EqualTo(cmp.Author),   nameof(src.Author));
            Assert.That(src.Subject,  Is.EqualTo(cmp.Subject),  nameof(src.Subject));
            Assert.That(src.Keywords, Is.EqualTo(cmp.Keywords), nameof(src.Keywords));
            Assert.That(src.Creator,  Is.EqualTo(cmp.Creator),  nameof(src.Creator));
            Assert.That(src.Options,  Is.EqualTo(cmp.Options));
            Assert.That(src.Version.Major, Is.EqualTo(cmp.Version.Major));
            Assert.That(src.Version.Minor, Is.EqualTo(cmp.Version.Minor));
        }

        #endregion
    }
}
