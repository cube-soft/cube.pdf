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
using Cube.Generics;
using Cube.Pdf.App.Pinstaller;
using NUnit.Framework;
using System.Linq;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PortConfigTest
    ///
    /// <summary>
    /// Represents tests of the PortConfig class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PortConfigTest : DeviceFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Properties
        ///
        /// <summary>
        /// Confirms the default values of the PortConfig class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Properties()
        {
            var src = new PortConfig();
            Assert.That(src.Name,        Is.Empty);
            Assert.That(src.MonitorName, Is.Empty);
            Assert.That(src.Proxy,       Is.Empty);
            Assert.That(src.Application, Is.Empty);
            Assert.That(src.Arguments,   Is.Empty);
            Assert.That(src.Temp,        Is.Empty);
            Assert.That(src.WaitForExit, Is.False);
            Assert.That(src.RunAsUser,   Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Executes the test to convert from the PortConfig to Port object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert()
        {
            var path = GetExamplesWith("Sample.json");
            var src  = Format.Json.Deserialize<DeviceConfig>(path).Ports.ToList();
            Assert.That(src.Count,           Is.EqualTo(1));
            Assert.That(src[0].Name,         Is.EqualTo("CubePDF:"));
            Assert.That(src[0].MonitorName,  Is.EqualTo("CubeMon"));
            Assert.That(src[0].Proxy,        Is.EqualTo(@"C:\Program Files\CubePDF\CubeProxy.exe"));
            Assert.That(src[0].Application,  Is.EqualTo(@"C:\Program Files\CubePDF\cubepdf.exe"));
            Assert.That(src[0].Arguments,    Is.EqualTo("/Dummy"));
            Assert.That(src[0].Temp,         Is.EqualTo(@"CubeSoft\CubePDF"));
            Assert.That(src[0].WaitForExit,  Is.False);
            Assert.That(src[0].RunAsUser,    Is.True);

            var dest = src.Convert().ToList();
            Assert.That(dest.Count,          Is.EqualTo(1));
            Assert.That(dest[0].Name,        Is.EqualTo(src[0].Name));
            Assert.That(dest[0].MonitorName, Is.EqualTo(src[0].MonitorName));
            Assert.That(dest[0].Application, Is.EqualTo(src[0].Proxy));
            Assert.That(dest[0].Arguments,   Is.EqualTo($"/Dummy /Exec {src[0].Application.Quote()}"));
            Assert.That(dest[0].Temp,        Is.EqualTo(src[0].Temp));
            Assert.That(dest[0].WaitForExit, Is.EqualTo(src[0].WaitForExit));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_WithoutProxy
        ///
        /// <summary>
        /// Executes the test to convert from the PortConfig to Port object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_WithoutProxy()
        {
            var path = GetExamplesWith("SampleDummy.json");
            var src  = Format.Json.Deserialize<DeviceConfig>(path).Ports.ToList();
            Assert.That(src.Count,           Is.EqualTo(1));
            Assert.That(src[0].Name,         Is.EqualTo("DummyPort:"));
            Assert.That(src[0].MonitorName,  Is.EqualTo("DummyMonitor"));
            Assert.That(src[0].Proxy,        Is.Empty);
            Assert.That(src[0].Application,  Is.EqualTo("dummy.exe"));
            Assert.That(src[0].Arguments,    Is.Empty);
            Assert.That(src[0].Temp,         Is.Empty);
            Assert.That(src[0].WaitForExit,  Is.False);
            Assert.That(src[0].RunAsUser,    Is.True);

            var dest = src.Convert().ToList();
            Assert.That(dest.Count,          Is.EqualTo(1));
            Assert.That(dest[0].Name,        Is.EqualTo(src[0].Name));
            Assert.That(dest[0].MonitorName, Is.EqualTo(src[0].MonitorName));
            Assert.That(dest[0].Application, Is.EqualTo(src[0].Application));
            Assert.That(dest[0].Arguments,   Is.Empty);
            Assert.That(dest[0].Temp,        Is.EqualTo(src[0].Temp));
            Assert.That(dest[0].WaitForExit, Is.EqualTo(src[0].WaitForExit));
        }

        #endregion
    }
}
