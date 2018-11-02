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
using NUnit.Framework;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PortTest
    ///
    /// <summary>
    /// Represents tests of the Port class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PortTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test to create a new instance of the Port
        /// class with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Dummy Port", "Dummy PortMonitor", ExpectedResult = false)]
        public bool Create(string name, string monitor) =>
            new Port(name, monitor).Exists;

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Executes the test to get the collection of port monitors.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("CubeMon")]
        [TestCase("Dummy Monitor")]
        public void GetElements(string monitor) => Assert.DoesNotThrow(() =>
        {
            var src = Port.GetElements(monitor);

            foreach (var e in src)
            {
                this.LogDebug(string.Join("\t",
                    e.Name.Quote(),
                    e.FileName.Quote(),
                    e.Arguments,
                    e.WorkingDirectory.Quote(),
                    e.WaitForExit,
                    e.Environment.Quote()
                ));

                Assert.That(e.Name.HasValue(),        Is.True, nameof(e.Name));
                Assert.That(e.Environment.HasValue(), Is.True, nameof(e.Environment));
            }
        });

        #endregion
    }
}
