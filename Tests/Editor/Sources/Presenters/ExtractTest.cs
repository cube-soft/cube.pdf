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
using System.Linq;
using System.Threading;
using Cube.Mixin.Commands;
using Cube.Mixin.IO;
using Cube.Pdf.Itext;
using Cube.Pdf.Mixin;
using Cube.Tests;
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
    class ExtractTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests to extract the selected items as a new PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract() => Open("SampleRotation.pdf", "", vm =>
        {
            Destination = Get(Args("Sample"));
            Assert.That(IO.Exists(Destination), Is.False, Destination);

            vm.Value.Images.Skip(1).First().Selected = true;
            vm.Value.Images.First().Selected = true;
            Assert.That(Wait.For(() => vm.Ribbon.Extract.Command.CanExecute()));

            vm.Test(vm.Ribbon.Extract);

            using (var r = new DocumentReader(Destination))
            {
                Assert.That(r.GetPage(1).GetViewSize().Width, Is.EqualTo(595.0f).Within(1.0f));
                Assert.That(r.GetPage(2).GetViewSize().Width, Is.EqualTo(842.0f).Within(1.0f));
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractOthers
        ///
        /// <summary>
        /// Tests the ExtractOthers command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void ExtractOthers() => Open("SampleRotation.pdf", "", vm =>
        {
            vm.Value.Settings.Language = Language.English;
            var cts = new CancellationTokenSource();
            using (vm.Subscribe<ExtractViewModel>(evm =>
            {
                Assert.That(evm.Title,               Is.EqualTo("Extraction details"));
                Assert.That(evm.Formats.Count(),     Is.EqualTo(2));
                Assert.That(evm.Destination.Text,    Is.EqualTo("Save path"));
                Assert.That(evm.Destination.Value,   Is.Null, nameof(evm.Destination));
                Assert.That(evm.Destination.Command, Is.Not.Null);
                Assert.That(evm.Format.Text,         Is.EqualTo("Format"));
                Assert.That(evm.Format.Value,        Is.EqualTo(SaveFormat.Pdf));
                Assert.That(evm.Format.Command,      Is.Null);
                Assert.That(evm.Resolution.Text,     Is.EqualTo("dpi"));
                Assert.That(evm.Resolution.Value,    Is.EqualTo(144));
                Assert.That(evm.Resolution.Command,  Is.Null);
                Assert.That(evm.Count.Text,          Is.EqualTo("Page count"));
                Assert.That(evm.Count.Value,         Is.EqualTo(9), nameof(evm.Count));
                Assert.That(evm.Count.Command,       Is.Null);
                Assert.That(evm.Target.Text,         Is.EqualTo("Target pages"));
                Assert.That(evm.Target.Command,      Is.Null);
                Assert.That(evm.Selected.Text,       Is.EqualTo("Selected pages"));
                Assert.That(evm.Selected.Value,      Is.False, nameof(evm.Selected));
                Assert.That(evm.Selected.Command,    Is.Not.Null);
                Assert.That(evm.All.Text,            Is.EqualTo("All pages"));
                Assert.That(evm.All.Value,           Is.True, nameof(evm.All));
                Assert.That(evm.All.Command,         Is.Null);
                Assert.That(evm.Specified.Text,      Is.EqualTo("Specified range"));
                Assert.That(evm.Specified.Value,     Is.False, nameof(evm.Specified));
                Assert.That(evm.Specified.Command,   Is.Null);
                Assert.That(evm.Range.Text,          Is.EqualTo("e.g. 1,2,4-7,9"));
                Assert.That(evm.Range.Value,         Is.Null, nameof(evm.Range));
                Assert.That(evm.Range.Command,       Is.Null);
                Assert.That(evm.Split.Text,          Is.EqualTo("Save as a separate file per page"));
                Assert.That(evm.Split.Value,         Is.False, nameof(evm.Split));
                Assert.That(evm.Split.Command,       Is.Not.Null);
                Assert.That(evm.Option.Text,         Is.EqualTo("Options"));
                Assert.That(evm.Option.Command,      Is.Null);

                evm.Destination.Value = Results;
                Assert.That(evm.OK.Command.CanExecute(), Is.False);

                evm.Cancel.Command.Execute();
                cts.Cancel();
            })) {
                Assert.That(vm.Ribbon.ExtractOthers.Command.CanExecute(), Is.True);
                vm.Ribbon.ExtractOthers.Command.Execute();
                Assert.That(Wait.For(cts.Token), Is.True, "Timeout (Extract)");
            }
        });

        #endregion
    }
}
