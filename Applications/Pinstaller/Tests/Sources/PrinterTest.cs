/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Generics;
using Cube.Pdf.App.Pinstaller;
using Cube.Pdf.App.Pinstaller.Debug;
using NUnit.Framework;
using System;
using System.Linq;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PrinterTest
    ///
    /// <summary>
    /// Represents tests of the Printer class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PrinterTest : DeviceFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test to create a new instance of the Printer
        /// class with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Dummy Printr",                  ExpectedResult = false)]
        [TestCase("Microsoft XPS Document Writer", ExpectedResult = true )]
        public bool Create(string name) => Invoke(() =>
        {
            var src = new Printer(name);
            Assert.That(src.Name.Unify(),           Is.EqualTo(name));
            Assert.That(src.Environment.HasValue(), Is.True);
            return src.Exists;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForce
        ///
        /// <summary>
        /// Executes the test to create a new instance of the Printer
        /// class with a force option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CreateForce()
        {
            var name = "Dummy Printer";
            var src  = new Printer(name, true);
            Assert.That(src.Name,                   Is.EqualTo(name));
            Assert.That(src.ShareName,              Is.EqualTo(name));
            Assert.That(src.Exists,                 Is.False, nameof(src.Exists));
            Assert.That(src.CanInstall(),           Is.False, nameof(src.CanInstall));
            Assert.That(src.PortName.HasValue(),    Is.False, nameof(src.PortName));
            Assert.That(src.DriverName.HasValue(),  Is.False, nameof(src.DriverName));
            Assert.That(src.Environment.HasValue(), Is.True,  nameof(src.Environment));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Install_Throws
        ///
        /// <summary>
        /// Confirms the behavior to install the invalid printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Install_Throws()
        {
            var src = new Printer("Dummy Printer", true);
            Assert.That(src.Exists, Is.False);

            src.ShareName  = "Dummy SharePrinter";
            src.DriverName = "Dummy Driver";
            src.PortName   = "Dummy Port";
            Assert.That(src.CanInstall(), Is.True);
            Assert.That(() => src.Install(), Throws.InstanceOf<Exception>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall_Ignore
        ///
        /// <summary>
        /// Confirms the behavior to uninstall the inexistent printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Uninstall_Ignore()
        {
            var src = new Printer("Dummy Printer", true);
            Assert.That(src.Exists, Is.False);

            src.ShareName  = "Dummy SharePrinter";
            src.DriverName = "Dummy Driver";
            src.PortName   = "Dummy Port";
            Assert.DoesNotThrow(() => src.Uninstall());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Executes the test to get the collection of printers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetElements() => Invoke(() =>
        {
            var src = Printer.GetElements();
            Assert.That(src.Count(), Is.AtLeast(1));

            foreach (var e in src)
            {
                e.Log();
                Assert.That(e.Name.HasValue(),         Is.True, nameof(e.Name));
                Assert.That(e.DriverName.HasValue(),   Is.True, nameof(e.DriverName));
                Assert.That(e.PortName.HasValue(),     Is.True, nameof(e.PortName));
                Assert.That(e.Environment.HasValue(),  Is.True, nameof(e.Environment));
                Assert.That(e.Exists,                  Is.True, nameof(e.Exists));
            }
        });

        #endregion
    }
}
