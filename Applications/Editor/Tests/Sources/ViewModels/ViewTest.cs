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
using Cube.Mixin.Commands;
using Cube.Tests;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Pdf.Editor.Tests.ViewModels
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewTest
    ///
    /// <summary>
    /// Tests for viewing operations of the MainViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ViewTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// Executes the test for showing the PreviewWindow of the selected
        /// item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Preview() => Open("Sample.pdf", "", vm =>
        {
            var cts = new CancellationTokenSource();
            var dp  = vm.Subscribe<PreviewViewModel>(e =>
            {
                Assert.That(e.Title.Text,        Is.Not.Null.And.Not.Empty);
                Assert.That(e.Data.File.Value,   Is.Not.Null);
                Assert.That(e.Data.Width.Value,  Is.GreaterThan(0));
                Assert.That(e.Data.Height.Value, Is.GreaterThan(0));

                Assert.That(Wait.For(() => !e.Data.Busy.Value), "Timeout (PreviewImage)");
                Assert.That(e.Data.Image.Value,  Is.Not.Null);

                e.Cancel.Command.Execute();
                cts.Cancel(); // done
            });

            vm.Test(vm.Ribbon.Select);
            Assert.That(vm.Ribbon.Preview.Command.CanExecute(), Is.True);
            Task.Run(() => vm.Ribbon.Preview.Command.Execute());
            Assert.That(Wait.For(cts.Token), "Timeout (Preview)");
            dp.Dispose();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Executes the test for selecting some items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Select() => Open("SampleRotation.pdf", "", vm =>
        {
            var unit    = 3; // Number of PropertyChanged events per action.
            var changed = 0;
            var dest    = vm.Data.Images.Selection;
            dest.PropertyChanged += (s, e) => ++changed;

            Assert.That(dest.Count,   Is.EqualTo(0));
            Assert.That(dest.Indices, Is.Not.Null);
            Assert.That(dest.Last,    Is.EqualTo(-1));

            vm.Data.Images.First().IsSelected = true;
            Assert.That(Wait.For(() => !vm.Data.Busy.Value));
            Assert.That(changed,    Is.EqualTo(1 * unit));
            Assert.That(dest.Count, Is.EqualTo(1), nameof(dest.Count));
            Assert.That(dest.Last,  Is.EqualTo(0), nameof(dest.Last));

            vm.Test(vm.Ribbon.SelectFlip);
            Assert.That(changed,    Is.EqualTo(10 * unit));
            Assert.That(dest.Count, Is.EqualTo(8), nameof(dest.Count));
            Assert.That(dest.Last,  Is.EqualTo(8), nameof(dest.Last));

            vm.Test(vm.Ribbon.Select); // SelectAll
            Assert.That(changed,    Is.EqualTo(11 * unit));
            Assert.That(dest.Count, Is.EqualTo(9), nameof(dest.Count));
            Assert.That(dest.Last,  Is.EqualTo(8), nameof(dest.Last));

            vm.Test(vm.Ribbon.Select); // SelectClear
            Assert.That(changed, Is.EqualTo(20 * unit));
            Assert.That(dest.Count, Is.EqualTo(0));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Zoom
        ///
        /// <summary>
        /// Executes the test for changing the item size.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Zoom() => Open("Sample.pdf", "", vm =>
        {
            var ip = vm.Data.Images.Preferences;
            Assert.That(ip.ItemSizeOptions.Count, Is.EqualTo(9));
            Assert.That(ip.ItemSizeIndex,         Is.EqualTo(3));
            Assert.That(ip.ItemSize,              Is.EqualTo(250));

            vm.Data.ItemSize.Value = 325;
            Wait.For(() => !vm.Data.Busy.Value);

            Assert.That(ip.ItemSizeIndex, Is.EqualTo(4));
            Assert.That(ip.ItemSize,      Is.EqualTo(300));
        });

        /* ----------------------------------------------------------------- */
        ///
        /// FrameOnly
        ///
        /// <summary>
        /// Executes the test for changing the FrameOnly setting.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void FrameOnly() => Open("Sample.pdf", "", vm =>
        {
            Assert.That(vm.Ribbon.FrameOnly.Value, Is.False);
            vm.Ribbon.FrameOnly.Value = true;
            foreach (var item in vm.Data.Images) Assert.That(item.Image, Is.Null);
        });

        #endregion
    }
}
