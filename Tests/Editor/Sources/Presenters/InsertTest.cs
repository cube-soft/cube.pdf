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
using System.Runtime.CompilerServices;
using System.Windows;
using Cube.FileSystem;
using Cube.Pdf.Pdfium;
using Cube.Tests;
using Cube.Xui.Commands.Extensions;
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
    class InsertTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Tests to insert a new PDF document behind the selected index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleAnnotation.pdf", 11)]
        [TestCase("Loading.png",          10)]
        [TestCase("Sample.jpg",           10)]
        public void Insert(string file, int n)
        {
            var vp = new VmParam
            {
                Source = GetSource("Sample.pdf"),
                Save   = Get(Args(file.Replace('.', '-')))
            };

            using var vm = NewVM();
            using var z0 = vm.Boot(vp);

            vp.Source = GetSource(file);
            vm.Select(2);
            vm.Test(vm.Ribbon.Insert);
            Assert.That(vm.Value.Count, Is.EqualTo(n));

            var dest = vm.Value.Images.ToList();
            Assert.That(dest[0].RawObject.Number, Is.EqualTo(1));
            Assert.That(dest[1].RawObject.Number, Is.EqualTo(2));
            Assert.That(dest[2].RawObject.Number, Is.EqualTo(3));
            Assert.That(dest[3].RawObject.Number, Is.EqualTo(1)); // Insert
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));

            vm.Test(vm.Ribbon.SaveAs);
            Assert.That(Wait.For(() => Io.Exists(vp.Save)), vp.Save);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Insert_DragDrop
        ///
        /// <summary>
        /// Tests to insert PDF pages through Drag&amp;Drop operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Inser_DragDrop()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });

            var pages = new List<Page2>();
            var f = GetSource("SampleAnnotation.pdf");
            using (var r = new DocumentReader(f)) pages.AddRange(r.Pages);

            vm.Test(() => vm.InsertOrMove.Execute(new DragDropObject(-1, 0)
            {
                DropIndex = 4,
                Pages     = pages,
            }));

            var dest = vm.Value.Images.ToList();
            Assert.That(dest.Count,                Is.EqualTo(11));
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
        }

        #endregion

        #region Tests (InsertViewModel)

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_SelectClear
        ///
        /// <summary>
        /// Tests the SelectClear command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_SelectClear() => Boot(vm =>
        {
            Assert.That(vm.Value.Selection.Count, Is.EqualTo(0));
            vm.Value.Files[0].Selected = true;
            Assert.That(vm.Value.Selection.Count, Is.EqualTo(1));
            vm.SelectClear.Execute();
            Assert.That(vm.Value.Selection.Count, Is.EqualTo(0));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_Clear
        ///
        /// <summary>
        /// Tests the Clear command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Clear() => Boot(vm =>
        {
            Assert.That(vm.Clear.Command.CanExecute(), Is.True);
            vm.Clear.Command.Execute();
            Assert.That(vm.Value.Files.Count, Is.EqualTo(0));
        }, false);

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_Remove
        ///
        /// <summary>
        /// Tests the Remove command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Remove() => Boot(vm =>
        {
            Assert.That(vm.Remove.Command.CanExecute(), Is.False);
            vm.Value.Files[0].Selected = true;
            Assert.That(vm.Remove.Command.CanExecute(), Is.True);
            vm.Remove.Command.Execute();
            Assert.That(vm.Value.Files.Count, Is.EqualTo(3));
        }, false);

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_Move
        ///
        /// <summary>
        /// Tests the Move command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_Move() => Boot(vm =>
        {
            Assert.That(vm.Down.Command.CanExecute(), Is.False);
            vm.Value.Files[0].Selected = true;
            Assert.That(vm.Down.Command.CanExecute(), Is.True);
            vm.Down.Command.Execute();
            Assert.That(vm.Value.Files[0].Name, Is.EqualTo("Sample.jpg"));
            Assert.That(vm.Value.Files[1].Name, Is.EqualTo("Loading.png"));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_DragUp
        ///
        /// <summary>
        /// Tests to move the selected items through the Drag&amp;Drop
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_DragUp() => Boot(vm =>
        {
            vm.Value.Files[2].Selected = true;
            vm.Value.Files[3].Selected = true;

            var obj = new MockDropInfo
            {
                DragInfo    = new MockDragInfo(vm.Value.Files[3], 3),
                Data        = vm.Value.Files[3],
                TargetItem  = vm.Value.Files[1],
                InsertIndex = 1,
            };

            vm.DragMove.DragOver(obj);
            Assert.That(obj.NotHandled,        Is.False);
            Assert.That(obj.Effects,           Is.EqualTo(DragDropEffects.Move));
            Assert.That(obj.DropTargetAdorner, Is.EqualTo(DropTargetAdorners.Insert));
            Assert.That(obj.Data,              Is.EqualTo(obj.DragInfo.Data));
            Assert.That(obj.DragInfo.Data,     Is.EqualTo(obj.DragInfo.SourceItem));
            vm.DragMove.Drop(obj);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_DragDown
        ///
        /// <summary>
        /// Tests to move the selected items through the Drag&amp;Drop
        /// operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_DragDown() => Boot(vm =>
        {
            vm.Value.Files[0].Selected = true;
            vm.Value.Files[2].Selected = true;

            var obj = new MockDropInfo
            {
                DragInfo    = new MockDragInfo(vm.Value.Files[0], 0),
                Data        = vm.Value.Files[0],
                TargetItem  = vm.Value.Files[2],
                InsertIndex = 2,
            };

            vm.DragMove.DragOver(obj);
            vm.DragMove.Drop(obj);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Ivm_DragCancel
        ///
        /// <summary>
        /// Confirms the behavior when the dragged index equals to the
        /// dropped index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Ivm_DragSame() => Boot(vm =>
        {
            vm.Value.Files[2].Selected = true;

            var obj = new MockDropInfo
            {
                DragInfo    = new MockDragInfo(vm.Value.Files[2], 2),
                Data        = vm.Value.Files[2],
                TargetItem  = vm.Value.Files[2],
                InsertIndex = 2,
            };

            vm.DragMove.DragOver(obj);
            Assert.That(obj.NotHandled,            Is.True);
            Assert.That(obj.Effects,               Is.EqualTo(DragDropEffects.None));
            Assert.That(obj.DragInfo.Effects,      Is.EqualTo(DragDropEffects.Move));
            Assert.That(obj.DropTargetAdorner,     Is.Null);
            Assert.That(obj.UnfilteredInsertIndex, Is.EqualTo(2));
            Assert.That(obj.DestinationText,       Is.Empty);
            vm.DragMove.Drop(obj);
        });

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Boot
        ///
        /// <summary>
        /// Creates a new instance of the InsertViewModel class and invokes
        /// the specified action.
        /// </summary>
        ///
        /// <param name="callback">Callback action.</param>
        /// <param name="apply">
        /// Value indicating whether or not the result of the user callback
        /// action should be applied, i.e., false means the cancel.
        /// </param>
        /// <param name="name">Caller method name.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void Boot(Action<InsertViewModel> callback, bool apply = true, [CallerMemberName] string name = null)
        {
            var vp = new VmParam
            {
                Source = GetSource("Sample.pdf"),
                Save   = Get($"{name}.pdf"),
            };

            using var vm = NewVM();
            using var z0 = vm.Boot(vp);
            using var z1 = vm.Subscribe<InsertViewModel>(ivm =>
            {
                using var z2 = ivm.Subscribe<OpenFileMessage>(e => {
                    e.Cancel = false;
                    e.Value  = new[]
                    {
                        GetSource("SampleAnnotation.pdf"),
                        GetSource("SampleRotation.pdf"),
                        GetSource("Sample.jpg"),
                        GetSource("Loading.png"),
                    };
                });

                Assert.That(ivm.Add.Command.CanExecute());
                ivm.Add.Command.Execute();
                AssertObject(ivm);
                callback(ivm);

                var cmd = apply ? ivm.OK.Command : ivm.Cancel.Command;
                Assert.That(cmd.CanExecute());
                cmd.Execute();
            });

            Assert.That(vm.Ribbon.InsertOthers.Command.CanExecute());
            vm.Ribbon.InsertOthers.Command.Execute();

            var n = apply ? 22 : 9;
            Assert.That(Wait.For(() => vm.Value.Count == n), "Timeout");

            vm.Test(vm.Ribbon.SaveAs);
            Assert.That(Wait.For(() => Io.Exists(vp.Save)), vp.Save);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AssertObject
        ///
        /// <summary>
        /// Confirms the properties of the specified object.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void AssertObject(InsertViewModel src)
        {
            Assert.That(src.Value,                 Is.Not.Null);
            Assert.That(src.Value.Count,           Is.EqualTo(9));
            Assert.That(src.Value.SelectedIndex,   Is.EqualTo(-1));

            Assert.That(src.Title,                 Is.EqualTo("Insertion details"));
            Assert.That(src.OK.Text,               Is.EqualTo("OK"));
            Assert.That(src.OK.Command,            Is.Not.Null);
            Assert.That(src.Cancel.Text,           Is.EqualTo("Cancel"));
            Assert.That(src.Cancel.Command,        Is.Not.Null);

            Assert.That(src.Add.Text,              Is.EqualTo("Add ..."));
            Assert.That(src.Add.Command,           Is.Not.Null);
            Assert.That(src.Preview.Text,          Is.EqualTo("Preview"));
            Assert.That(src.Preview.Command,       Is.Not.Null);
            Assert.That(src.Up.Text,               Is.EqualTo("Up"));
            Assert.That(src.Up.Command,            Is.Not.Null);
            Assert.That(src.Down.Text,             Is.EqualTo("Down"));
            Assert.That(src.Down.Command,          Is.Not.Null);
            Assert.That(src.Remove.Text,           Is.EqualTo("Remove"));
            Assert.That(src.Remove.Command,        Is.Not.Null);
            Assert.That(src.Clear.Text,            Is.EqualTo("Clear"));
            Assert.That(src.Clear.Command,         Is.Not.Null);
            Assert.That(src.FileName.Text,         Is.EqualTo("Filename"));
            Assert.That(src.FileName.Command,      Is.Null);
            Assert.That(src.FileType.Text,         Is.EqualTo("Type"));
            Assert.That(src.FileType.Command,      Is.Null);
            Assert.That(src.FileLength.Text,       Is.EqualTo("Filesize"));
            Assert.That(src.FileLength.Command,    Is.Null);
            Assert.That(src.LastWriteTime.Text,    Is.EqualTo("Last updated"));
            Assert.That(src.LastWriteTime.Command, Is.Null);
            Assert.That(src.DragAdd,               Is.Not.Null);
            Assert.That(src.DragMove,              Is.Not.Null);

            var file = src.Value.Files[0];
            Assert.That(src.Value.Files.Count,     Is.EqualTo(4));
            Assert.That(file.Name,                 Is.EqualTo("Loading.png"));
            Assert.That(file.FullName,             Does.EndWith("Loading.png"));
            Assert.That(file.Length,               Is.AtLeast(1200));
            Assert.That(file.LastWriteTime,        Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(file.Icon,                 Is.Not.Null);
            Assert.That(file.Selected,             Is.False);

            var pos = src.Position;
            Assert.That(pos.Select.Text,            Is.EqualTo("Insert position"));
            Assert.That(pos.Select.Command,         Is.Not.Null);
            Assert.That(pos.First.Text,             Is.EqualTo("Beginning"));
            Assert.That(pos.First.Command,          Is.Null);
            Assert.That(pos.Last.Text,              Is.EqualTo("End"));
            Assert.That(pos.Last.Command,           Is.Null);
            Assert.That(pos.SelectedIndex.Text,     Is.EqualTo("Selected position"));
            Assert.That(pos.SelectedIndex.Value,    Is.EqualTo(-1));
            Assert.That(pos.SelectedIndex.Command,  Is.Null);
            Assert.That(pos.UserIndex.Text,         Is.EqualTo("Behind the number of"));
            Assert.That(pos.UserIndex.Value,        Is.EqualTo(1));
            Assert.That(pos.UserIndex.Command,      Is.Null);
            Assert.That(pos.Count.Text,             Is.EqualTo("/ 9 pages"));
            Assert.That(pos.Count.Value,            Is.EqualTo(9));
            Assert.That(pos.Count.Command,          Is.Null);
        }

        #endregion
    }
}
