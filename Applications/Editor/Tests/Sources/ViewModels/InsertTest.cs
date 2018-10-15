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
using Cube.Pdf.Pdfium;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Tests.Editor.ViewModels
{
    /* --------------------------------------------------------------------- */
    ///
    /// RemoveTest
    ///
    /// <summary>
    /// Tests for Remove commands and the InsertViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class InsertTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Executes the test for inserting a new PDF document behind the
        /// selected index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Insert() => Create("SampleRotation.pdf", "", 9, vm =>
        {
            vm.Data.Images.Skip(2).First().IsSelected = true;
            Source = GetExamplesWith("Sample.pdf");
            Assert.That(vm.Ribbon.Insert.Command.CanExecute(), Is.True);
            vm.Ribbon.Insert.Command.Execute();
            Assert.That(Wait.For(() => vm.Data.Count.Value == 11), "Timeout (Insert)");

            var dest = vm.Data.Images.ToList();

            Assert.That(dest[ 0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[ 1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[ 2].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[ 3].RawObject.Number, Is.EqualTo(1)); // Insert
            Assert.That(dest[ 4].RawObject.Number, Is.EqualTo(2)); // Insert
            Assert.That(dest[ 5].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[ 6].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[ 7].RawObject.Number, Is.EqualTo(6));
            Assert.That(dest[ 8].RawObject.Number, Is.EqualTo(7));
            Assert.That(dest[ 9].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[10].RawObject.Number, Is.EqualTo(9));

            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Insert_DragDrop
        ///
        /// <summary>
        /// Executes the test for inserting PDF pages through
        /// Drag&amp;Drop operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Inser_DragDrop() => Create("SampleRotation.pdf", "", 9, vm =>
        {
            var f = GetExamplesWith("Sample.pdf");
            var pages = new List<Page>();
            using (var r = new DocumentReader(f)) pages.AddRange(r.Pages);

            vm.InsertOrMove.Execute(new DragDropObject(-1, 0)
            {
                DropIndex = 4,
                Pages     = pages,
            });

            Assert.That(Wait.For(() => vm.Data.Count.Value == 11), "Timeout (Insert)");

            var dest = vm.Data.Images.ToList();

            Assert.That(dest[ 0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[ 1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[ 2].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[ 3].RawObject.Number, Is.EqualTo(4));
            Assert.That(dest[ 4].RawObject.Number, Is.EqualTo(5));
            Assert.That(dest[ 5].RawObject.Number, Is.EqualTo(1)); // Insert
            Assert.That(dest[ 6].RawObject.Number, Is.EqualTo(2)); // Insert
            Assert.That(dest[ 7].RawObject.Number, Is.EqualTo(6));
            Assert.That(dest[ 8].RawObject.Number, Is.EqualTo(7));
            Assert.That(dest[ 9].RawObject.Number, Is.EqualTo(8));
            Assert.That(dest[10].RawObject.Number, Is.EqualTo(9));

            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        });

        #region InsertWindow

        /* ----------------------------------------------------------------- */
        ///
        /// Properties_English
        ///
        /// <summary>
        /// Executes the test for showing the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Properties_English() => Create("SampleRotation.pdf", "", 9, vm =>
        {
            vm.Register<InsertViewModel>(this, e =>
            {
                Assert.That(e.Title.Text,     Is.EqualTo("Insertion details"));
                Assert.That(e.Data,           Is.Not.Null);
                Assert.That(e.Add.Text,       Is.EqualTo("Add ..."));
                Assert.That(e.Up.Text,        Is.EqualTo("Up"));
                Assert.That(e.Down.Text,      Is.EqualTo("Down"));
                Assert.That(e.Remove.Text,    Is.EqualTo("Remove"));
                Assert.That(e.Clear.Text,     Is.EqualTo("Clear"));
                Assert.That(e.OK.Text,        Is.EqualTo("OK"));
                Assert.That(e.OK.Command,     Is.Not.Null);
                Assert.That(e.Cancel.Text,    Is.EqualTo("Cancel"));
                Assert.That(e.Cancel.Command, Is.Not.Null);

                var pos = e.Position;
                Assert.That(pos.Text,                     Is.EqualTo("Insert position"));
                Assert.That(pos.Command,                  Is.Not.Null);
                Assert.That(pos.First.Text,               Is.EqualTo("Beginning"));
                Assert.That(pos.Last.Text,                Is.EqualTo("End"));
                Assert.That(pos.Selected.Text,            Is.EqualTo("Selected position"));
                Assert.That(pos.Selected.Value,           Is.False);
                Assert.That(pos.UserSpecified.Text,       Is.EqualTo("Behind the number of"));
                Assert.That(pos.UserSpecified.Value,      Is.EqualTo(1));
                Assert.That(pos.UserSpecifiedSuffix.Text, Is.EqualTo("/ 9 pages"));

                Assert.That(e.Cancel.Command.CanExecute(), Is.True);
                e.Cancel.Command.Execute();
            });

            vm.Data.Settings.Language = Language.English;
            Assert.That(vm.Ribbon.InsertOthers.Command.CanExecute(), Is.True);
            vm.Ribbon.InsertOthers.Command.Execute();
        });

        #endregion

        #endregion
    }
}
