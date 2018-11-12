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
    /// PrinterDriverTest
    ///
    /// <summary>
    /// Represents tests of the PrinterDriver class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PrinterDriverTest : DeviceFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test to create a new instance of the PrinterDriver
        /// class with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Dummy Driver",                ExpectedResult = false)]
        [TestCase("Microsoft Shared Fax Driver", ExpectedResult = true )]
        public bool Create(string name) => Invoke(() =>
        {
            var src = new PrinterDriver(name);
            Assert.That(src.Name.Unify(),             Is.EqualTo(name));
            Assert.That(src.Environment.HasValue(),   Is.True, nameof(src.Environment));
            Assert.That(src.DirectoryName.HasValue(), Is.True, nameof(src.DirectoryName));
            return src.Exists;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForce
        ///
        /// <summary>
        /// Executes the test to create a new instance of the PrinterDriver
        /// class with a force option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CreateForce()
        {
            var name = "Dummy Driver";
            var src  = new PrinterDriver(name, true);
            Assert.That(src.Name,                     Is.EqualTo(name));
            Assert.That(src.Exists,                   Is.False, nameof(src.Exists));
            Assert.That(src.MonitorName.HasValue(),   Is.False, nameof(src.MonitorName));
            Assert.That(src.FileName.HasValue(),      Is.False, nameof(src.FileName));
            Assert.That(src.Config.HasValue(),        Is.False, nameof(src.Config));
            Assert.That(src.Data.HasValue(),          Is.False, nameof(src.Data));
            Assert.That(src.Help.HasValue(),          Is.False, nameof(src.Help));
            Assert.That(src.Dependencies.HasValue(),  Is.False, nameof(src.Dependencies));
            Assert.That(src.Environment.HasValue(),   Is.True, nameof(src.Environment));
            Assert.That(src.DirectoryName.HasValue(), Is.True, nameof(src.DirectoryName));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Install_Throws
        ///
        /// <summary>
        /// Confirms the behavior to install the invalid printer driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Install_Throws()
        {
            var src = new PrinterDriver("Dummy Driver", true);
            Assert.That(src.Exists, Is.False);

            src.MonitorName  = "Dummy Monitor";
            src.FileName     = "Dummy.dll";
            src.Config       = "DummyUi.dll";
            src.Data         = "Dummy.ppd";
            src.Help         = "Dummy.hlp";
            src.Dependencies = "Dummy.ntf";
            Assert.That(() => src.Install(), Throws.InstanceOf<Exception>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall_Ignore
        ///
        /// <summary>
        /// Confirms the behavior to uninstall the inexistent printer
        /// driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Uninstall_Ignore()
        {
            var src = new PrinterDriver("Dummy Driver", true);
            Assert.That(src.Exists, Is.False);

            src.MonitorName  = "Dummy Monitor";
            src.FileName     = "Dummy.dll";
            src.Config       = "DummyUi.dll";
            src.Data         = "Dummy.ppd";
            src.Help         = "Dummy.hlp";
            src.Dependencies = "Dummy.ntf";
            Assert.DoesNotThrow(() => src.Uninstall());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Executes the test to get the collection of printer drivers.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetElements() => Invoke(() =>
        {
            var src = PrinterDriver.GetElements();
            Assert.That(src.Count(), Is.AtLeast(1));

            foreach (var e in src)
            {
                e.Log();
                Assert.That(e.Name.HasValue(),          Is.True, nameof(e.Name));
                Assert.That(e.FileName.HasValue(),      Is.True, nameof(e.FileName));
                Assert.That(e.Environment.HasValue(),   Is.True, nameof(e.Environment));
                Assert.That(e.DirectoryName.HasValue(), Is.True, nameof(e.DirectoryName));
                Assert.That(e.Exists,                   Is.True, nameof(e.Exists));
            }
        });

        #endregion
    }
}
