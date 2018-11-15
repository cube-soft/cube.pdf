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
using Cube.DataContract;
using Cube.Pdf.App.Pinstaller;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// InstallerTest
    ///
    /// <summary>
    /// Represents tests of the Installer class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class InstallerTest : DeviceFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test to create a new instance of the Installer
        /// class with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Create(Format format, string filename, DeviceConfig cmp) => Invoke(() =>
        {
            var src    = GetExamplesWith(filename);
            var engine = new Installer(format, src);
            var dest   = engine.Config;
            Assert.That(engine,          Is.Not.Null);
            Assert.That(engine.Location, Is.EqualTo(src));
            Assert.That(engine.IO,       Is.Not.Null);

            // PortMonitors
            Assert.That(dest.PortMonitors.Count, Is.EqualTo(cmp.PortMonitors.Count));
            for (var i = 0; i < dest.PortMonitors.Count; ++i)
            {
                var x = dest.PortMonitors[i];
                var y =  cmp.PortMonitors[i];
                var m = $"{nameof(dest.PortMonitors)}[{i}]";
                Assert.That(x.Name,     Is.EqualTo(y.Name),     $"{m}.{nameof(x.Name)}");
                Assert.That(x.FileName, Is.EqualTo(y.FileName), $"{m}.{nameof(x.FileName)}");
                Assert.That(x.Config,   Is.EqualTo(y.Config),   $"{m}.{nameof(x.Config)}");
            }

            // Ports
            Assert.That(dest.Ports.Count, Is.EqualTo(cmp.Ports.Count));
            for (var i = 0; i < dest.Ports.Count; ++i)
            {
                var x = dest.Ports[i];
                var y =  cmp.Ports[i];
                var m = $"{nameof(dest.Ports)}[{i}]";
                Assert.That(x.Name,        Is.EqualTo(y.Name),        $"{m}.{nameof(x.Name)}");
                Assert.That(x.MonitorName, Is.EqualTo(y.MonitorName), $"{m}.{nameof(x.MonitorName)}");
                Assert.That(x.Application, Is.EqualTo(y.Application), $"{m}.{nameof(x.Application)}");
                Assert.That(x.Arguments,   Is.EqualTo(y.Arguments),   $"{m}.{nameof(x.Arguments)}");
                Assert.That(x.Temp,        Is.EqualTo(y.Temp),        $"{m}.{nameof(x.Temp)}");
                Assert.That(x.WaitForExit, Is.EqualTo(y.WaitForExit), $"{m}.{nameof(x.WaitForExit)}");
            }

            // PrinterDrivers
            Assert.That(dest.PrinterDrivers.Count, Is.EqualTo(cmp.PrinterDrivers.Count));
            for (var i = 0; i < dest.PrinterDrivers.Count; ++i)
            {
                var x = dest.PrinterDrivers[i];
                var y =  cmp.PrinterDrivers[i];
                var m = $"{nameof(dest.PrinterDrivers)}[{i}]";
                Assert.That(x.Name,         Is.EqualTo(y.Name),         $"{m}.{nameof(x.Name)}");
                Assert.That(x.MonitorName,  Is.EqualTo(y.MonitorName),  $"{m}.{nameof(x.MonitorName)}");
                Assert.That(x.FileName,     Is.EqualTo(y.FileName),     $"{m}.{nameof(x.FileName)}");
                Assert.That(x.Config,       Is.EqualTo(y.Config),       $"{m}.{nameof(x.Config)}");
                Assert.That(x.Data,         Is.EqualTo(y.Data),         $"{m}.{nameof(x.Data)}");
                Assert.That(x.Help,         Is.EqualTo(y.Help),         $"{m}.{nameof(x.Help)}");
                Assert.That(x.Dependencies, Is.EqualTo(y.Dependencies), $"{m}.{nameof(x.Dependencies)}");
            }

            // Printers
            Assert.That(dest.Printers.Count, Is.EqualTo(cmp.Printers.Count));
            for (var i = 0; i < dest.Printers.Count; ++i)
            {
                var x = dest.Printers[i];
                var y =  cmp.Printers[i];
                var m = $"{nameof(dest.Printers)}[{i}]";
                Assert.That(x.Name,       Is.EqualTo(y.Name),       $"{m}.{nameof(x.Name)}");
                Assert.That(x.ShareName,  Is.EqualTo(y.ShareName),  $"{m}.{nameof(x.ShareName)}");
                Assert.That(x.DriverName, Is.EqualTo(y.DriverName), $"{m}.{nameof(x.DriverName)}");
                Assert.That(x.PortName,   Is.EqualTo(y.PortName),   $"{m}.{nameof(x.PortName)}");
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Executes the test to uninstall devices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("SampleDummy.json")]
        public void Uninstall(string filename) => Invoke(() =>
            new Installer(Format.Json, GetExamplesWith(filename)).Uninstall()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Install_Throws
        ///
        /// <summary>
        /// Confirms the behavior to install with the invalid settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Install_Throws()
        {
            var src  = GetExamplesWith("SampleDummy.json");
            var dest = new Installer(Format.Json, src);

            Assert.That(() => dest.Install(Examples, true), Throws.InstanceOf<Exception>());
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(Format.Json, "SampleEmpty.json", new DeviceConfig());
                yield return new TestCaseData(Format.Json, "Sample.json", new DeviceConfig
                {
                    PortMonitors = new[] { new PortMonitorConfig
                    {
                        Name         = "CubeMon",
                        FileName     = "cubemon.dll",
                        Config       = "cubemonui.dll",
                    }},
                    Ports = new[] { new PortConfig
                    {
                        Name         = "CubePDF:",
                        MonitorName  = "CubeMon",
                        Application  = @"C:\Program Files\CubePDF\CubeProxy.exe",
                        Arguments    = @"/Exec ""C:\Program Files\CubePDF\cubepdf.exe""",
                        Temp         = @"CubeSoft\CubePDF",
                        WaitForExit  = false,
                    }},
                    PrinterDrivers = new[] { new PrinterDriverConfig
                    {
                        Name         = "CubePDF",
                        MonitorName  = "CubeMon",
                        FileName     = "cubeps5.dll",
                        Config       = "cubeps5ui.dll",
                        Data         = "cubepdf.ppd",
                        Help         = "cubeps.hlp",
                        Dependencies = "cubeps.ntf",
                    }},
                    Printers = new[] { new PrinterConfig
                    {
                        Name         = "CubePDF",
                        ShareName    = "CubePDF",
                        DriverName   = "CubePDF",
                        PortName     = "CubePDF:",
                    }}
                });
            }
        }

        #endregion
    }
}
