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
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using Cube.Pdf.Pinstaller.Debug;
using NUnit.Framework;
using System;
using System.Linq;

namespace Cube.Pdf.Pinstaller.Tests
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
        public bool Create(string name)
        {
            var src = new PrinterDriver(name);
            Assert.That(src.Name.Unify(),               Is.EqualTo(name));
            Assert.That(src.Environment.HasValue(),     Is.True, nameof(src.Environment));
            Assert.That(src.TargetDirectory.HasValue(), Is.True, nameof(src.TargetDirectory));
            return src.Exists;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForce
        ///
        /// <summary>
        /// Executes the test to create a new instance of the PrinterDriver
        /// class with an empty collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CreateForce()
        {
            var name = "Dummy Driver";
            var src  = new PrinterDriver(name, Enumerable.Empty<PrinterDriver>());
            Assert.That(src.Name,                       Is.EqualTo(name));
            Assert.That(src.Exists,                     Is.False, nameof(src.Exists));
            Assert.That(src.CanInstall(),               Is.False, nameof(src.CanInstall));
            Assert.That(src.MonitorName.HasValue(),     Is.False, nameof(src.MonitorName));
            Assert.That(src.FileName.HasValue(),        Is.False, nameof(src.FileName));
            Assert.That(src.Config.HasValue(),          Is.False, nameof(src.Config));
            Assert.That(src.Data.HasValue(),            Is.False, nameof(src.Data));
            Assert.That(src.Help.HasValue(),            Is.False, nameof(src.Help));
            Assert.That(src.Environment.HasValue(),     Is.True, nameof(src.Environment));
            Assert.That(src.TargetDirectory.HasValue(), Is.True, nameof(src.TargetDirectory));
            Assert.That(src.Dependencies.Count(),       Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Executes the test of Copy extende method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Copy() => Invoke(() =>
        {
            var dest = new PrinterDriverConfig
            {
                Name         = "CubeTestPrinterDriver",
                MonitorName  = "",
                FileName     = "pscript5.dll",
                Config       = "ps5ui.dll",
                Data         = "cubepdf.ppd",
                Help         = "pscript.hlp",
                Dependencies = new[] { "pscript.ntf", "pscrptfe.ntf", "ps_schm.gdl" },
                Repository   = "ntprint",
            }.Convert(PrinterDriver.GetElements());

            dest.Copy(Examples, IO);
            var dir = dest.TargetDirectory;
            Assert.That(IO.Exists(IO.Combine(dir, dest.FileName)), dest.FileName);
            Assert.That(IO.Exists(IO.Combine(dir, dest.Config)),   dest.Config);
            Assert.That(IO.Exists(IO.Combine(dir, dest.Data)),     dest.Data);
            Assert.That(IO.Exists(IO.Combine(dir, dest.Help)),     dest.Help);
            foreach (var s in dest.Dependencies) Assert.That(IO.Exists(IO.Combine(dir, s)), s);
        });

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
            var src = new PrinterDriver("Dummy Driver");
            Assert.That(src.Exists, Is.False);

            src.MonitorName  = "Dummy Monitor";
            src.FileName     = "Dummy.dll";
            src.Config       = "DummyUi.dll";
            src.Data         = "Dummy.ppd";
            src.Help         = "Dummy.hlp";
            src.Dependencies = new[] { "Dummy.ntf" };
            Assert.That(src.CanInstall(), Is.True);
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
            var src = new PrinterDriver("Dummy Driver");
            Assert.That(src.Exists, Is.False);

            src.MonitorName  = "Dummy Monitor";
            src.FileName     = "Dummy.dll";
            src.Config       = "DummyUi.dll";
            src.Data         = "Dummy.ppd";
            src.Help         = "Dummy.hlp";
            src.Dependencies = new[] { "Dummy.ntf" };
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
        public void GetElements()
        {
            var src = PrinterDriver.GetElements();
            Assert.That(src.Count(), Is.AtLeast(1));

            foreach (var e in src)
            {
                e.Log();
                Assert.That(e.Name.HasValue(),            Is.True, nameof(e.Name));
                Assert.That(e.FileName.HasValue(),        Is.True, nameof(e.FileName));
                Assert.That(e.Environment.HasValue(),     Is.True, nameof(e.Environment));
                Assert.That(e.TargetDirectory.HasValue(), Is.True, nameof(e.TargetDirectory));
                Assert.That(e.Exists,                     Is.True, nameof(e.Exists));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRepository
        ///
        /// <summary>
        /// Executes the test of the GetRepository extended method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("ntprint", ExpectedResult = true)]
        [TestCase("dummy",   ExpectedResult = false)]
        [TestCase("",        ExpectedResult = false)]
        public bool GetRepository(string src)
        {
            var driver = new PrinterDriver("Dummy", Enumerable.Empty<PrinterDriver>()) { Repository = src };
            var dest   = driver.GetRepository(IO);
            this.LogDebug($"[{nameof(GetRepository)}] {src} -> {dest.Quote()}");
            return dest.HasValue() && IO.Exists(dest);
        }

        #endregion
    }
}
