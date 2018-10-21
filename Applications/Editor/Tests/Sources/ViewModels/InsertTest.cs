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
using Cube.Xui;
using Cube.Xui.Mixin;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        [TestCase("Sample.pdf",  11)]
        [TestCase("Loading.png", 10)]
        public void Insert(string filename, int n) => Create("SampleRotation.pdf", "", 9, vm =>
        {
            vm.Data.Images.Skip(2).First().IsSelected = true;
            Source = GetExamplesWith(filename);
            Assert.That(vm.Ribbon.Insert.Command.CanExecute(), Is.True);
            vm.Ribbon.Insert.Command.Execute();
            Assert.That(Wait.For(() => vm.Data.Count.Value == n), "Timeout (Insert)");

            var dest = vm.Data.Images.ToList();

            Assert.That(dest[ 0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[ 1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[ 2].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[ 3].RawObject.Number, Is.EqualTo(1)); // Insert

            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOthers
        ///
        /// <summary>
        /// Executes the test to insert files through the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void InsertOthers() => Create("SampleRotation.pdf", "", 9, vm =>
        {
            vm.Register<InsertViewModel>(this, ivm =>
            {
                ivm.Register<OpenFileMessage>(this, e => {
                    e.FileNames = new[] { GetExamplesWith("Sample.pdf") };
                    e.Result    = true;
                    e.Callback.Invoke(e);
                });
                ivm.Add.Command.Execute();
                ivm.OK.Command.Execute();
            });

            Assert.That(vm.Ribbon.InsertOthers.Command.CanExecute(), Is.True);
            vm.Ribbon.InsertOthers.Command.Execute();
            Assert.That(Wait.For(() => vm.Data.Count.Value == 11), "Timeout");
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
        /// Ivm_Properties
        ///
        /// <summary>
        /// Executes the test to show the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Properties() => CreateInsert("SampleRotation.pdf", "", 9, ivm =>
        {
            Assert.That(ivm.Data,               Is.Not.Null);
            Assert.That(ivm.Data.Count,         Is.EqualTo(9));
            Assert.That(ivm.Data.SelectedIndex, Is.EqualTo(-1));

            Assert.That(ivm.Title.Text,         Is.EqualTo("Insertion details"));
            Assert.That(ivm.Add.Text,           Is.EqualTo("Add ..."));
            Assert.That(ivm.Preview.Text,       Is.EqualTo("Preview"));
            Assert.That(ivm.Up.Text,            Is.EqualTo("Up"));
            Assert.That(ivm.Down.Text,          Is.EqualTo("Down"));
            Assert.That(ivm.Remove.Text,        Is.EqualTo("Remove"));
            Assert.That(ivm.Clear.Text,         Is.EqualTo("Clear"));
            Assert.That(ivm.OK.Text,            Is.EqualTo("OK"));
            Assert.That(ivm.OK.Command,         Is.Not.Null);
            Assert.That(ivm.Cancel.Text,        Is.EqualTo("Cancel"));
            Assert.That(ivm.Cancel.Command,     Is.Not.Null);
            Assert.That(ivm.DragAdd,            Is.Not.Null);
            Assert.That(ivm.DragMove,           Is.Not.Null);

            var src = ivm.Data.Files[0];
            Assert.That(ivm.Data.Files.Count,   Is.EqualTo(3));
            Assert.That(src.Name,               Is.EqualTo("Sample.pdf"));
            Assert.That(src.FullName,           Does.EndWith("Sample.pdf"));
            Assert.That(src.Length,             Is.AtLeast(60000));
            Assert.That(src.LastWriteTime,      Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(src.Icon,               Is.Not.Null);
            Assert.That(src.IsSelected,         Is.False);

            var pos = ivm.Position;
            Assert.That(pos.Text,                     Is.EqualTo("Insert position"));
            Assert.That(pos.Command,                  Is.Not.Null);
            Assert.That(pos.First.Text,               Is.EqualTo("Beginning"));
            Assert.That(pos.Last.Text,                Is.EqualTo("End"));
            Assert.That(pos.Selected.Text,            Is.EqualTo("Selected position"));
            Assert.That(pos.Selected.Value,           Is.False);
            Assert.That(pos.UserSpecified.Text,       Is.EqualTo("Behind the number of"));
            Assert.That(pos.UserSpecified.Value,      Is.EqualTo(1));
            Assert.That(pos.UserSpecifiedSuffix.Text, Is.EqualTo("/ 9 pages"));

            Assert.That(ivm.Cancel.Command.CanExecute(), Is.True);
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_SelectClear
        ///
        /// <summary>
        /// Executes the test to clear the selection in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_SelectClear() => CreateInsert("SampleRotation.pdf", "", 9, ivm =>
        {
            Assert.That(ivm.Data.Selection.Count, Is.EqualTo(0));
            ivm.Data.Files[0].IsSelected = true;
            Assert.That(ivm.Data.Selection.Count, Is.EqualTo(1));
            ivm.SelectClear.Execute();
            Assert.That(ivm.Data.Selection.Count, Is.EqualTo(0));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_Clear
        ///
        /// <summary>
        /// Executes the test to clear items in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Clear() => CreateInsert("SampleRotation.pdf", "", 9, ivm =>
        {
            Assert.That(ivm.Clear.Command.CanExecute(), Is.True);
            ivm.Clear.Command.Execute();
            Assert.That(ivm.Data.Files.Count, Is.EqualTo(0));
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_Remove
        ///
        /// <summary>
        /// Executes the test to remove the selected item in the
        /// InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Remove() => CreateInsert("SampleRotation.pdf", "", 9, ivm =>
        {
            Assert.That(ivm.Remove.Command.CanExecute(), Is.False);
            ivm.Data.Files[0].IsSelected = true;
            Assert.That(ivm.Remove.Command.CanExecute(), Is.True);
            ivm.Remove.Command.Execute();
            Assert.That(ivm.Data.Files.Count, Is.EqualTo(2));
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_Move
        ///
        /// <summary>
        /// Executes the test to Move the selected item in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Move() => CreateInsert("SampleRotation.pdf", "", 9, ivm =>
        {
            Assert.That(ivm.Down.Command.CanExecute(), Is.False);
            ivm.Data.Files[0].IsSelected = true;
            Assert.That(ivm.Down.Command.CanExecute(), Is.True);
            ivm.Down.Command.Execute();
            Assert.That(ivm.Data.Files[0].Name, Is.EqualTo("SampleAes128.pdf"));
            Assert.That(ivm.Data.Files[1].Name, Is.EqualTo("Sample.pdf"));
            ivm.Cancel.Command.Execute();
        });

        #endregion

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Gets a new instance of the InsertViewModel class and runs the
        /// specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void CreateInsert(string filename, string password, int n,
            Action<InsertViewModel> action) => Create(filename, password, n, vm =>
        {
            var cts = new CancellationTokenSource();
            vm.Register<InsertViewModel>(this, ivm =>
            {
                ivm.Register<OpenFileMessage>(this, e => {
                    e.Result    = true;
                    e.FileNames = new[]
                    {
                        GetExamplesWith("Sample.pdf"),
                        GetExamplesWith("SampleAes128.pdf"),
                        GetExamplesWith("Loading.png"),
                    };
                    e.Callback.Invoke(e);
                });
                ivm.Add.Command.Execute();
                action(ivm);
                ivm.Dispose();
                cts.Cancel();
            });

            vm.Data.Settings.Language = Language.English;
            Assert.That(vm.Ribbon.InsertOthers.Command.CanExecute(), Is.True);
            vm.Ribbon.InsertOthers.Command.Execute();
            Assert.That(Wait.For(cts.Token), "Timeout");
        });

        #endregion
    }
}
