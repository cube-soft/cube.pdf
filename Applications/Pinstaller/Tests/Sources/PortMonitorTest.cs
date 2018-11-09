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
using Cube.Log;
using Cube.Pdf.App.Pinstaller;
using Cube.Pdf.App.Pinstaller.Log;
using NUnit.Framework;
using System.Linq;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PortMonitorTest
    ///
    /// <summary>
    /// Represents tests of the PortMonitor class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PortMonitorTest : DeviceFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test to create a new instance of the PortMonitor
        /// class with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Dummy Port", "",             ExpectedResult = false)]
        [TestCase("Local Port", "localspl.dll", ExpectedResult = true )]
        public bool Create(string name, string filename) => Invoke(() =>
        {
            var src = new PortMonitor(name);
            Assert.That(src.Name.Unify(),             Is.EqualTo(name));
            Assert.That(src.FileName.Unify(),         Is.EqualTo(filename));
            Assert.That(src.Config.HasValue(),        Is.False, nameof(src.Config));
            Assert.That(src.Environment.HasValue(),   Is.True,  nameof(src.Environment));
            Assert.That(src.DirectoryName.HasValue(), Is.True,  nameof(src.DirectoryName));
            return src.Exists;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// CreateForce
        ///
        /// <summary>
        /// Executes the test to create a new instance of the PortMonitor
        /// class with a force option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CreateForce()
        {
            var name = "Dummy Port";
            var src  = new PortMonitor(name, true);
            Assert.That(src.Name,                     Is.EqualTo(name));
            Assert.That(src.Exists,                   Is.False, nameof(src.Exists));
            Assert.That(src.FileName.HasValue(),      Is.False, nameof(src.FileName));
            Assert.That(src.Config.HasValue(),        Is.False, nameof(src.Config));
            Assert.That(src.Environment.HasValue(),   Is.True,  nameof(src.Environment));
            Assert.That(src.DirectoryName.HasValue(), Is.True,  nameof(src.DirectoryName));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall_Ignore
        ///
        /// <summary>
        /// Confirms the behavior to uninstall the inexistent port monitor.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Uninstall_Ignore()
        {
            var src = new PortMonitor("Dummy Monitor", true);
            Assert.That(src.Exists, Is.False);

            src.FileName = "DummyMon.dll";
            src.Config   = "DummyMonUi.dll";
            Assert.DoesNotThrow(() => src.Uninstall());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Executes the test to get the collection of port monitors.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetElements() => Invoke(() =>
        {
            var src = PortMonitor.GetElements();
            Assert.That(src.Count(), Is.AtLeast(2));

            foreach (var e in src)
            {
                e.Debug();
                Assert.That(e.Name.HasValue(),        Is.True, nameof(e.Name));
                Assert.That(e.FileName.HasValue(),    Is.True, nameof(e.FileName));
                Assert.That(e.Environment.HasValue(), Is.True, nameof(e.Environment));
            }
        });

        #endregion
    }
}
