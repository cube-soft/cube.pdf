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
using Cube.FileSystem;
using Cube.Pdf.Extensions;
using Cube.Pdf.Itext;
using Cube.Xui.Commands.Extensions;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractTest
    ///
    /// <summary>
    /// Tests the Extract commands and the ExtractViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests to extract the selected items as a new PDF file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract()
        {
            var vp = new VmParam
            {
                Source = GetSource("SampleRotation.pdf"),
                Save   = Get(Args("Sample")),
            };

            using var vm = NewVM();
            using var z0 = vm.Boot(vp);

            Assert.That(Io.Exists(vp.Save), Is.False, vp.Save);
            vm.Select(1, 0);
            Assert.That(vm.Ribbon.Extract.Command.CanExecute());
            vm.Test(vm.Ribbon.Extract);

            using var r = new DocumentReader(vp.Save);
            Assert.That(r.GetPage(1).GetViewSize().Width, Is.EqualTo(595.0f).Within(1.0f));
            Assert.That(r.GetPage(2).GetViewSize().Width, Is.EqualTo(842.0f).Within(1.0f));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractOthers
        ///
        /// <summary>
        /// Tests the ExtractOthers command.
        /// </summary>
        ///
        /// <remarks>
        /// TODO: Tests the OK command, instead of the Cancel command.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void ExtractOthers()
        {
            using var vm = NewVM();
            using var z0 = vm.Boot(new() { Source = GetSource("Sample.pdf") });
            using var z1 = vm.Subscribe<ExtractViewModel>(evm => {
                AssertObject(evm);
                evm.Cancel.Command.Execute();
            });

            Assert.That(vm.Ribbon.ExtractOthers.Command.CanExecute());
            vm.Ribbon.ExtractOthers.Command.Execute();
        }

        #endregion

        #region Others

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
        private void AssertObject(ExtractViewModel src)
        {
            Assert.That(src.Title,               Is.EqualTo("Extraction Details"));
            Assert.That(src.Formats.Count(),     Is.EqualTo(2));
            Assert.That(src.Destination.Text,    Is.EqualTo("Save path"));
            Assert.That(src.Destination.Value,   Is.Empty, nameof(src.Destination));
            Assert.That(src.Destination.Command, Is.Not.Null);
            Assert.That(src.Format.Text,         Is.EqualTo("Format"));
            Assert.That(src.Format.Value,        Is.EqualTo(SaveFormat.Pdf));
            Assert.That(src.Format.Command,      Is.Null);
            Assert.That(src.Resolution.Text,     Is.EqualTo("dpi"));
            Assert.That(src.Resolution.Value,    Is.EqualTo(144));
            Assert.That(src.Resolution.Command,  Is.Null);
            Assert.That(src.Count.Text,          Is.EqualTo("Page count"));
            Assert.That(src.Count.Value,         Is.EqualTo(9), nameof(src.Count));
            Assert.That(src.Count.Command,       Is.Null);
            Assert.That(src.Target.Text,         Is.EqualTo("Target pages"));
            Assert.That(src.Target.Command,      Is.Null);
            Assert.That(src.Selected.Text,       Is.EqualTo("Selected pages"));
            Assert.That(src.Selected.Value,      Is.False, nameof(src.Selected));
            Assert.That(src.Selected.Command,    Is.Not.Null);
            Assert.That(src.All.Text,            Is.EqualTo("All pages"));
            Assert.That(src.All.Value,           Is.True, nameof(src.All));
            Assert.That(src.All.Command,         Is.Null);
            Assert.That(src.Specified.Text,      Is.EqualTo("Specified range"));
            Assert.That(src.Specified.Value,     Is.False, nameof(src.Specified));
            Assert.That(src.Specified.Command,   Is.Null);
            Assert.That(src.Range.Text,          Is.EqualTo("e.g. 1,2,4-7,9"));
            Assert.That(src.Range.Value,         Is.Empty, nameof(src.Range));
            Assert.That(src.Range.Command,       Is.Null);
            Assert.That(src.Split.Text,          Is.EqualTo("Save as a separate file per page"));
            Assert.That(src.Split.Value,         Is.False, nameof(src.Split));
            Assert.That(src.Split.Command,       Is.Not.Null);
            Assert.That(src.Option.Text,         Is.EqualTo("Options"));
            Assert.That(src.Option.Command,      Is.Null);
        }

        #endregion
    }
}
