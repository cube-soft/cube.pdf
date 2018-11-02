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
            var src  = GetExamplesWith(filename);
            var dest = new Installer(format, src);
            Assert.That(dest,          Is.Not.Null);
            Assert.That(dest.Location, Is.EqualTo(src));
            Assert.That(dest.IO,       Is.Not.Null);

            var x0 = dest.Config.PortMonitor;
            var y0 = cmp.PortMonitor;
            var m0 = nameof(dest.Config.PortMonitor);
            Assert.That(x0.Name,     Is.EqualTo(y0.Name),     $"{m0}.{nameof(x0.Name)}");
            Assert.That(x0.FileName, Is.EqualTo(y0.FileName), $"{m0}.{nameof(x0.FileName)}");

            var x1 = dest.Config.Port;
            var y1 = cmp.Port;
            var m1 = nameof(dest.Config.Port);
            Assert.That(x1.Name,             Is.EqualTo(y1.Name),             $"{m1}.{nameof(x1.Name)}");
            Assert.That(x1.FileName,         Is.EqualTo(y1.FileName),         $"{m1}.{nameof(x1.FileName)}");
            Assert.That(x1.Arguments,        Is.EqualTo(y1.Arguments),        $"{m1}.{nameof(x1.Arguments)}");
            Assert.That(x1.WorkingDirectory, Is.EqualTo(y1.WorkingDirectory), $"{m1}.{nameof(x1.WorkingDirectory)}");
            Assert.That(x1.WaitForExit,      Is.EqualTo(y1.WaitForExit),      $"{m1}.{nameof(x1.WaitForExit)}");

            var x2 = dest.Config.PrinterDriver;
            var y2 = cmp.PrinterDriver;
            var m2 = nameof(dest.Config.PrinterDriver);
            Assert.That(x2.Name,         Is.EqualTo(y2.Name),         $"{m2}.{nameof(x2.Name)}");
            Assert.That(x2.MonitorName,  Is.EqualTo(y2.MonitorName),  $"{m2}.{nameof(x2.MonitorName)}");
            Assert.That(x2.Config,       Is.EqualTo(y2.Config),       $"{m2}.{nameof(x2.Config)}");
            Assert.That(x2.Data,         Is.EqualTo(y2.Data),         $"{m2}.{nameof(x2.Data)}");
            Assert.That(x2.Help,         Is.EqualTo(y2.Help),         $"{m2}.{nameof(x2.Help)}");
            Assert.That(x2.Dependencies, Is.EqualTo(y2.Dependencies), $"{m2}.{nameof(x2.Dependencies)}");

            var x3 = dest.Config.Printer;
            var y3 = cmp.Printer;
            var m3 = nameof(dest.Config.Printer);
            Assert.That(x3.Name,       Is.EqualTo(y3.Name),       $"{m3}.{nameof(x3.Name)}");
            Assert.That(x3.ShareName,  Is.EqualTo(y3.ShareName),  $"{m3}.{nameof(x3.ShareName)}");
            Assert.That(x3.DriverName, Is.EqualTo(y3.DriverName), $"{m3}.{nameof(x3.DriverName)}");
            Assert.That(x3.PortName,   Is.EqualTo(y3.PortName),   $"{m3}.{nameof(x3.PortName)}");
        });

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
                yield return new TestCaseData(Format.Json, "Sample.json", new DeviceConfig
                {
                    PortMonitor = new PortMonitorConfig
                    {
                        Name             = "CubeMon",
                        FileName         = "cubemon.dll",
                    },
                    Port = new PortConfig
                    {
                        Name             = "CubePDF:",
                        MonitorName      = "CubeMon",
                        FileName         = @"C:\Program Files\CubePDF\CubeProxy.exe",
                        Arguments        = @"/Exec ""C:\Program Files\CubePDF\cubepdf.exe""",
                        WorkingDirectory = @"C:\ProgramData\CubeSoft\CubePDF",
                        WaitForExit      = false,
                    },
                    PrinterDriver = new PrinterDriverConfig
                    {
                        Name             = "CubePDF",
                        MonitorName      = "CubeMon",
                        FileName         = "cubeps5.dll",
                        Config           = "cubeps5ui.dll",
                        Data             = "cubepdf.ppd",
                        Help             = "cubeps.hlp",
                        Dependencies     = "cubeps.ntf",
                    },
                    Printer = new PrinterConfig
                    {
                        Name             = "CubePDF",
                        ShareName        = "CubePDF",
                        DriverName       = "CubePDF",
                        PortName         = "CubePDF:",
                    }
                });

                yield return new TestCaseData(Format.Json, "SampleLite.json", new DeviceConfig
                {
                    PortMonitor = new PortMonitorConfig
                    {
                        Name             = "CubeMon",
                        FileName         = "cubemon.dll",
                    },
                    Port = new PortConfig
                    {
                        Name             = "CubePDF:",
                        MonitorName      = "CubeMon",
                        FileName         = @"C:\Program Files\CubePDF\CubeProxy.exe",
                        Arguments        = @"/Exec ""C:\Program Files\CubePDF\cubepdf.exe""",
                        WorkingDirectory = @"C:\ProgramData\CubeSoft\CubePDF",
                        WaitForExit      = false,
                    },
                    PrinterDriver = new PrinterDriverConfig
                    {
                        Name             = "CubePDF",
                        MonitorName      = "CubeMon",
                        FileName         = "cubeps5.dll",
                        Config           = "cubeps5ui.dll",
                        Data             = "cubepdf.ppd",
                        Help             = "cubeps.hlp",
                        Dependencies     = "cubeps.ntf",
                    },
                    Printer = new PrinterConfig
                    {
                        Name             = "CubePDF",
                        ShareName        = "",
                        DriverName       = "CubePDF",
                        PortName         = "CubePDF:",
                    }
                });
            }
        }

        #endregion
    }
}
