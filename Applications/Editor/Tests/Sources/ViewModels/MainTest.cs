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
using Cube.Xui.Mixin;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace Cube.Pdf.Tests.Editor.ViewModels
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainTest
    ///
    /// <summary>
    /// Tests for editing operations of the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class MainTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Executes the test to open a PDF document and create images
        /// as an asynchronous operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",       "",         2)]
        [TestCase("SampleAes128.pdf", "password", 2)]
        public void Open(string filename, string password, int n) =>
            Create(filename, password, n, vm =>
        {
            var pref = vm.Data.Preferences;
            Assert.That(pref.ItemSize,      Is.EqualTo(250));
            Assert.That(pref.ItemSizeIndex, Is.EqualTo(3));
            Assert.That(pref.ItemMargin,    Is.EqualTo(3));
            Assert.That(pref.TextHeight,    Is.EqualTo(25));

            var images = vm.Data.Images.ToList();
            foreach (var item in images) Assert.That(item.Image, Is.Not.Null);

            var dest = images[0];
            var cts  = new CancellationTokenSource();
            dest.PropertyChanged += (s, e) => cts.Cancel();
            Execute(vm, vm.Ribbon.Refresh);
            Assert.That(Wait.For(cts.Token), nameof(vm.Ribbon.Refresh));
            Assert.That(dest.Image, Is.Not.EqualTo(pref.Dummy));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Executes the test to save the PDF document as a new file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf",       "",         2)]
        [TestCase("SampleAes128.pdf", "password", 2)]
        public void Save(string filename, string password, int n) =>
            Create(filename, password, n, vm =>
        {
            var fi = IO.Get(Source);
            Destination = Path(Args(fi.NameWithoutExtension));
            Password    = string.Empty;
            Assert.That(IO.Exists(Destination), Is.False);

            Execute(vm, vm.Ribbon.SaveAs);
            Assert.That(Wait.For(() => IO.Exists(Destination)));
            Assert.That(vm.Data.Source.Value.FullName, Is.EqualTo(Destination));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Executes the test to close the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.pdf", true )]
        [TestCase("Sample.pdf", false)]
        public void Close(string filename, bool modify) => Create(vm =>
        {
            var fi = IO.Get(GetExamplesWith(filename));
            Source = Path(Args(fi.NameWithoutExtension, modify));
            IO.Copy(fi.FullName, Source, true);

            Execute(vm, vm.Ribbon.Open);
            Assert.That(Wait.For(() => vm.Data.Images.Count == 2), $"Timeout (Open)");

            if (modify)
            {
                Execute(vm, vm.Ribbon.Select);
                Execute(vm, vm.Ribbon.RotateLeft);
            }

            Execute(vm, vm.Ribbon.Close);
            Assert.That(Wait.For(() => !vm.Data.IsOpen()), $"Timeout (Close)");
            Assert.That(IO.TryDelete(Source), Is.True);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Executes the test to extract selected items as a new PDF
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract() => Create("Sample.pdf", "", 2, vm =>
        {
            Destination = Path(Args("Sample"));
            Assert.That(IO.Exists(Destination), Is.False);

            Assert.That(vm.Ribbon.Extract.Command.CanExecute(), Is.False);
            vm.Data.Images.First().IsSelected = true;
            Wait.For(() => !vm.Data.Busy.Value);
            Assert.That(vm.Ribbon.Extract.Command.CanExecute(), Is.True);

            Execute(vm, vm.Ribbon.Extract);
            Assert.That(Wait.For(() => IO.Exists(Destination)));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Executes the test to insert a new PDF document behind the
        /// selected index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Insert() => Create("SampleRotation.pdf", "", 9, vm =>
        {
            vm.Data.Images.Skip(2).First().IsSelected = true;
            Source = GetExamplesWith("Sample.pdf");
            Execute(vm, vm.Ribbon.Insert);

            var dest = vm.Data.Images.ToList();
            Assert.That(dest.Count, Is.EqualTo(9));
            for (var i = 0; i < dest.Count; ++i) Assert.That(dest[i].Index, Is.EqualTo(i));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Rotate
        ///
        /// <summary>
        /// Executes the test to rotate selected items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Rotate() => Create("Sample.pdf", "", 2, vm =>
        {
            var images = vm.Data.Images.ToList();
            var dest   = images[0];
            var dummy  = vm.Data.Preferences.Dummy;
            Assert.That(Wait.For(() => dest.Image != dummy), "Timeout");

            Assert.That(vm.Ribbon.RotateLeft.Command.CanExecute(),  Is.False);
            Assert.That(vm.Ribbon.RotateRight.Command.CanExecute(), Is.False);

            var image  = dest.Image;
            var width  = dest.Width;
            var height = dest.Height;
            var count  = 0;
            dest.IsSelected = true;
            dest.PropertyChanged += (s, e) => ++count;

            Execute(vm, vm.Ribbon.RotateLeft);
            Assert.That(Wait.For(() => count >= 4), "Timeout (Left)");
            Assert.That(dest.Image,  Is.Not.EqualTo(image).And.Not.EqualTo(dummy), "Left");
            Assert.That(dest.Width,  Is.Not.EqualTo(width),  nameof(width));
            Assert.That(dest.Height, Is.Not.EqualTo(height), nameof(height));

            Execute(vm, vm.Ribbon.RotateRight);
            Assert.That(Wait.For(() => count >= 8), "Timeout (Right)");
            Assert.That(dest.Image,  Is.Not.EqualTo(image).And.Not.EqualTo(dummy), "Right");
            Assert.That(dest.Width,  Is.EqualTo(width),  nameof(width));
            Assert.That(dest.Height, Is.EqualTo(height), nameof(height));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Undo
        ///
        /// <summary>
        /// Executes the test to undo the last action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Undo() => Create("SampleRotation.pdf", "", 9, vm =>
        {
            Execute(vm, vm.Ribbon.Select);
            Execute(vm, vm.Ribbon.Remove);

            Assert.That(vm.Data.Images.Count, Is.EqualTo(0));
            Assert.That(vm.Data.History.Undoable, Is.True);
            Assert.That(vm.Data.History.Redoable, Is.False);

            Execute(vm, vm.Ribbon.Undo);

            Assert.That(vm.Data.Images.Count, Is.EqualTo(9));
            Assert.That(vm.Data.History.Undoable, Is.False);
            Assert.That(vm.Data.History.Redoable, Is.True);

            Execute(vm, vm.Ribbon.Redo);

            Assert.That(vm.Data.Images.Count, Is.EqualTo(0));
            Assert.That(vm.Data.History.Undoable, Is.True);
            Assert.That(vm.Data.History.Redoable, Is.False);
        });

        #endregion
    }
}
