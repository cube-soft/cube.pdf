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
using Cube.FileSystem.TestService;
using Cube.Pdf.App.Editor;
using Cube.Pdf.Mixin;
using Cube.Pdf.Pdfium;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System;

namespace Cube.Pdf.Tests.Editor.ViewModels
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
        /// Executes the test to set the metadata.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Metadata")]
        [TestCase("日本語のメタ情報")]
        public void Set(string value)
        {
            var cmp = new Metadata
            {
                Title    = value,
                Author   = value,
                Subject  = value,
                Keywords = value,
                Creator  = value,
                Producer = value,
                Version  = new Version(1, 6),
                Viewer   = ViewerPreferences.TwoColumnRight,
            };

            Create("Sample.pdf", 2, vm =>
            {
                using (var _ = Register(vm, cmp))
                {
                    Assert.That(vm.Ribbon.Metadata.Command.CanExecute(), Is.True);
                    vm.Ribbon.Metadata.Command.Execute();
                }

                Assert.That(vm.Data.History.Undoable, Is.True);
                Assert.That(vm.Data.History.Redoable, Is.False);

                Destination = Path(Args(value));
                Execute(vm, vm.Ribbon.SaveAs);
                Assert.That(Wait.For(() => IO.Exists(Destination)));
            });

            using (var r = new DocumentReader(Destination)) AssertMetadata(r.Metadata, cmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Executes the test to cancel the MetadataWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel() => Create("Sample.pdf", 2, vm =>
        {
            var cmp = vm.Data.Metadata.Value.Copy();
            using (var _ = vm.Register<MetadataViewModel>(this, e =>
            {
                e.Document.Value = "dummy";
                Assert.That(e.Cancel.Command.CanExecute(), Is.True);
                e.Cancel.Command.Execute();
            })) vm.Ribbon.Metadata.Command.Execute();

            Assert.That(vm.Data.History.Undoable, Is.False);
            Assert.That(vm.Data.History.Redoable, Is.False);
            AssertMetadata(vm.Data.Metadata.Value, cmp);
        });

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Sets the operation corresponding to the MetadataViewModel
        /// message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable Register(MainViewModel vm, Metadata src) =>
            vm.Register<MetadataViewModel>(this, e =>
        {
            Assert.That(e.Filename.Value,      Is.Not.Null.And.Not.Empty);
            Assert.That(e.Producer.Value,      Is.Not.Null.And.Not.Empty);
            Assert.That(e.Length.Value,        Is.GreaterThan(0));
            Assert.That(e.CreationTime.Value,  Is.GreaterThan(DateTime.MinValue));
            Assert.That(e.LastWriteTime.Value, Is.GreaterThan(DateTime.MinValue));

            e.Document.Value = src.Title;
            e.Author.Value   = src.Author;
            e.Subject.Value  = src.Subject;
            e.Keywords.Value = src.Keywords;
            e.Creator.Value  = src.Creator;
            e.Viewer.Value   = src.Viewer;
            e.Version.Value  = src.Version;

            Assert.That(e.OK.Command.CanExecute(), Is.True);
            e.OK.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// AssertMetadata
        ///
        /// <summary>
        /// Confirms that properties of the specified objects are equal.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertMetadata(Metadata src, Metadata cmp)
        {
            Assert.That(src.Title,         Is.EqualTo(cmp.Title),    nameof(src.Title));
            Assert.That(src.Author,        Is.EqualTo(cmp.Author),   nameof(src.Author));
            Assert.That(src.Subject,       Is.EqualTo(cmp.Subject),  nameof(src.Subject));
            Assert.That(src.Keywords,      Is.EqualTo(cmp.Keywords), nameof(src.Keywords));
            Assert.That(src.Creator,       Is.EqualTo(cmp.Creator),  nameof(src.Creator));
            Assert.That(src.Viewer,        Is.EqualTo(cmp.Viewer));
            Assert.That(src.Version.Major, Is.EqualTo(cmp.Version.Major));
            Assert.That(src.Version.Minor, Is.EqualTo(cmp.Version.Minor));
        }

        #endregion
    }
}
