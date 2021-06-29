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
using System.Windows;
using Cube.FileSystem;
using Cube.Mixin.Commands;
using Cube.Pdf.Pdfium;
using Cube.Tests;
using GongSolutions.Wpf.DragDrop;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// InsertTest
    ///
    /// <summary>
    /// Tests the Insert commands and the InsertViewModel class.
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
        [TestCase("Sample.jpg",  10)]
        public void Insert(string filename, int n) => Open("SampleRotation.pdf", "", vm =>
        {
            vm.Value.Images.Skip(2).First().Selected = true;
            Source = GetSource(filename);
            Assert.That(vm.Ribbon.Insert.Command.CanExecute(), Is.True);
            vm.Ribbon.Insert.Command.Execute();
            Assert.That(Wait.For(() => vm.Value.Count == n), "Timeout (Insert)");

            var dest = vm.Value.Images.ToList();
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(1)); // Insert
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));

            Destination = Get(Args(filename.Replace('.', '_')));
            vm.Ribbon.SaveAs.Command.Execute();
            Assert.That(Wait.For(() => Io.Exists(Destination)), "Timeout (Save)");
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
        public void InsertOthers() => Open("SampleRotation.pdf", "", vm =>
        {
            _ = vm.Subscribe<InsertViewModel>(ivm =>
            {
                _ = ivm.Subscribe<OpenFileMessage>(e => {
                    e.Value  = new[] { GetSource("Sample.pdf") };
                    e.Cancel = false;
                });
                ivm.Add.Command.Execute();
                ivm.OK.Command.Execute();
            });

            Assert.That(vm.Ribbon.InsertOthers.Command.CanExecute(), Is.True);
            vm.Ribbon.InsertOthers.Command.Execute();
            Assert.That(Wait.For(() => vm.Value.Count == 11), "Timeout");
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
        public void Inser_DragDrop() => Open("SampleRotation.pdf", "", vm =>
        {
            var f = GetSource("Sample.pdf");
            var pages = new List<Page>();
            using (var r = new DocumentReader(f)) pages.AddRange(r.Pages);

            vm.InsertOrMove.Execute(new DragDropObject(-1, 0)
            {
                DropIndex = 4,
                Pages     = pages,
            });

            Assert.That(Wait.For(() => vm.Value.Count == 11), "Timeout (Insert)");

            var dest = vm.Value.Images.ToList();

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
        /// Show
        ///
        /// <summary>
        /// Tests to show the InsertWindow dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Show() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            Assert.That(ivm.Value,                 Is.Not.Null);
            Assert.That(ivm.Value.Count,           Is.EqualTo(9));
            Assert.That(ivm.Value.SelectedIndex,   Is.EqualTo(-1));

            Assert.That(ivm.Title,                 Is.EqualTo("Insertion details"));
            Assert.That(ivm.OK.Text,               Is.EqualTo("OK"));
            Assert.That(ivm.OK.Command,            Is.Not.Null);
            Assert.That(ivm.Cancel.Text,           Is.EqualTo("Cancel"));
            Assert.That(ivm.Cancel.Command,        Is.Not.Null);

            Assert.That(ivm.Add.Text,              Is.EqualTo("Add ..."));
            Assert.That(ivm.Add.Command,           Is.Not.Null);
            Assert.That(ivm.Preview.Text,          Is.EqualTo("Preview"));
            Assert.That(ivm.Preview.Command,       Is.Not.Null);
            Assert.That(ivm.Up.Text,               Is.EqualTo("Up"));
            Assert.That(ivm.Up.Command,            Is.Not.Null);
            Assert.That(ivm.Down.Text,             Is.EqualTo("Down"));
            Assert.That(ivm.Down.Command,          Is.Not.Null);
            Assert.That(ivm.Remove.Text,           Is.EqualTo("Remove"));
            Assert.That(ivm.Remove.Command,        Is.Not.Null);
            Assert.That(ivm.Clear.Text,            Is.EqualTo("Clear"));
            Assert.That(ivm.Clear.Command,         Is.Not.Null);
            Assert.That(ivm.FileName.Text,         Is.EqualTo("Filename"));
            Assert.That(ivm.FileName.Command,      Is.Null);
            Assert.That(ivm.FileType.Text,         Is.EqualTo("Type"));
            Assert.That(ivm.FileType.Command,      Is.Null);
            Assert.That(ivm.FileLength.Text,       Is.EqualTo("Filesize"));
            Assert.That(ivm.FileLength.Command,    Is.Null);
            Assert.That(ivm.LastWriteTime.Text,    Is.EqualTo("Last updated"));
            Assert.That(ivm.LastWriteTime.Command, Is.Null);
            Assert.That(ivm.DragAdd,               Is.Not.Null);
            Assert.That(ivm.DragMove,              Is.Not.Null);

            var file = ivm.Value.Files[0];
            Assert.That(ivm.Value.Files.Count,     Is.EqualTo(4));
            Assert.That(file.Name,                 Is.EqualTo("Sample.pdf"));
            Assert.That(file.FullName,             Does.EndWith("Sample.pdf"));
            Assert.That(file.Length,               Is.AtLeast(60000));
            Assert.That(file.LastWriteTime,        Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(file.Icon,                 Is.Not.Null);
            Assert.That(file.Selected,             Is.False);

            var it = ivm.Position;
            Assert.That(it.Select.Text,            Is.EqualTo("Insert position"));
            Assert.That(it.Select.Command,         Is.Not.Null);
            Assert.That(it.First.Text,             Is.EqualTo("Beginning"));
            Assert.That(it.First.Command,          Is.Null);
            Assert.That(it.Last.Text,              Is.EqualTo("End"));
            Assert.That(it.Last.Command,           Is.Null);
            Assert.That(it.SelectedIndex.Text,     Is.EqualTo("Selected position"));
            Assert.That(it.SelectedIndex.Value,    Is.EqualTo(-1));
            Assert.That(it.SelectedIndex.Command,  Is.Null);
            Assert.That(it.UserIndex.Text,         Is.EqualTo("Behind the number of"));
            Assert.That(it.UserIndex.Value,        Is.EqualTo(1));
            Assert.That(it.UserIndex.Command,      Is.Null);
            Assert.That(it.Count.Text,             Is.EqualTo("/ 9 pages"));
            Assert.That(it.Count.Value,            Is.EqualTo(9));
            Assert.That(it.Count.Command,          Is.Null);

            Assert.That(ivm.Cancel.Command.CanExecute(), Is.True);
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SelectClear
        ///
        /// <summary>
        /// Tests to clear the selection in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SelectClear() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            Assert.That(ivm.Value.Selection.Count, Is.EqualTo(0));
            ivm.Value.Files[0].Selected = true;
            Assert.That(ivm.Value.Selection.Count, Is.EqualTo(1));
            ivm.SelectClear.Execute();
            Assert.That(ivm.Value.Selection.Count, Is.EqualTo(0));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Tests to clear items in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Clear() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            Assert.That(ivm.Clear.Command.CanExecute(), Is.True);
            ivm.Clear.Command.Execute();
            Assert.That(ivm.Value.Files.Count, Is.EqualTo(0));
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Tests to remove the selected item in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Remove() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            Assert.That(ivm.Remove.Command.CanExecute(), Is.False);
            ivm.Value.Files[0].Selected = true;
            Assert.That(ivm.Remove.Command.CanExecute(), Is.True);
            ivm.Remove.Command.Execute();
            Assert.That(ivm.Value.Files.Count, Is.EqualTo(3));
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Tests to Move the selected item in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Move() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            Assert.That(ivm.Down.Command.CanExecute(), Is.False);
            ivm.Value.Files[0].Selected = true;
            Assert.That(ivm.Down.Command.CanExecute(), Is.True);
            ivm.Down.Command.Execute();
            Assert.That(ivm.Value.Files[0].Name, Is.EqualTo("SampleAes128.pdf"));
            Assert.That(ivm.Value.Files[1].Name, Is.EqualTo("Sample.pdf"));
            ivm.Cancel.Command.Execute();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// DragUp
        ///
        /// <summary>
        /// Tests to move the selected items through the Drag&amp;Drop
        /// operation in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragUp() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            ivm.Value.Files[2].Selected = true;
            ivm.Value.Files[3].Selected = true;

            var obj = new MockDropInfo
            {
                DragInfo    = new MockDragInfo(ivm.Value.Files[3], 3),
                Data        = ivm.Value.Files[3],
                TargetItem  = ivm.Value.Files[1],
                InsertIndex = 1,
            };

            ivm.DragMove.DragOver(obj);
            Assert.That(obj.NotHandled,        Is.False);
            Assert.That(obj.Effects,           Is.EqualTo(DragDropEffects.Move));
            Assert.That(obj.DropTargetAdorner, Is.EqualTo(DropTargetAdorners.Insert));
            Assert.That(obj.Data,              Is.EqualTo(obj.DragInfo.Data));
            Assert.That(obj.DragInfo.Data,     Is.EqualTo(obj.DragInfo.SourceItem));
            ivm.DragMove.Drop(obj);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// DragDown
        ///
        /// <summary>
        /// Tests to move the selected items through the Drag&amp;Drop
        /// operation in the InsertWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDown() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            ivm.Value.Files[0].Selected = true;
            ivm.Value.Files[2].Selected = true;

            var obj = new MockDropInfo
            {
                DragInfo    = new MockDragInfo(ivm.Value.Files[0], 0),
                Data        = ivm.Value.Files[0],
                TargetItem  = ivm.Value.Files[2],
                InsertIndex = 2,
            };

            ivm.DragMove.DragOver(obj);
            ivm.DragMove.Drop(obj);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// DragCancel
        ///
        /// <summary>
        /// Confirms the behavior when the dragged index equals to the
        /// dropped index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragCancel() => CreateIvm("SampleRotation.pdf", "", ivm =>
        {
            ivm.Value.Files[2].Selected = true;

            var obj = new MockDropInfo
            {
                DragInfo    = new MockDragInfo(ivm.Value.Files[2], 2),
                Data        = ivm.Value.Files[2],
                TargetItem  = ivm.Value.Files[2],
                InsertIndex = 2,
            };

            ivm.DragMove.DragOver(obj);
            Assert.That(obj.NotHandled,            Is.True);
            Assert.That(obj.Effects,               Is.EqualTo(DragDropEffects.None));
            Assert.That(obj.DragInfo.Effects,      Is.EqualTo(DragDropEffects.Move));
            Assert.That(obj.DropTargetAdorner,     Is.Null);
            Assert.That(obj.UnfilteredInsertIndex, Is.EqualTo(2));
            Assert.That(obj.DestinationText,       Is.Empty);
            ivm.DragMove.Drop(obj);
        });

        #endregion

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// CreateIvm
        ///
        /// <summary>
        /// Gets a new instance of the InsertViewModel class and runs the
        /// specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void CreateIvm(string filename, string password, Action<InsertViewModel> action) =>
            Open(filename, password, vm =>
        {
            var cts = new CancellationTokenSource();
            _ = vm.Subscribe<InsertViewModel>(ivm =>
            {
                _ = ivm.Subscribe<OpenFileMessage>(e => {
                    e.Cancel = false;
                    e.Value  = new[]
                    {
                        GetSource("Sample.pdf"),
                        GetSource("SampleAes128.pdf"),
                        GetSource("Sample.jpg"),
                        GetSource("Loading.png"),
                    };
                });
                ivm.Add.Command.Execute();
                action(ivm);
                ivm.Dispose();
                cts.Cancel();
            });

            vm.Value.Settings.Language = Language.English;
            Assert.That(vm.Ribbon.InsertOthers.Command.CanExecute(), Is.True);
            vm.Ribbon.InsertOthers.Command.Execute();
            Assert.That(Wait.For(cts.Token), "Timeout");
        });

        #endregion
    }
}
