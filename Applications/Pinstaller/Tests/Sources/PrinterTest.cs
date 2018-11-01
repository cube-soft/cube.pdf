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
    class PrinterTest
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
        [TestCase("Dummy Printr", ExpectedResult = false)]
        [TestCase("Fax",          ExpectedResult = true )]
        public bool Create(string name)
        {
            var src = new Printer(name);
            Assert.That(src.Name.Unify(),           Is.EqualTo(name));
            Assert.That(src.Environment.HasValue(), Is.True);
            return src.Exists;
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
        public void GetElements()
        {
            var src = Printer.GetElements();
            Assert.That(src.Count(), Is.AtLeast(1));

            foreach (var e in src)
            {
                this.LogDebug(string.Join("\t",
                    e.Name.Quote(),
                    e.ShareName.Quote(),
                    e.DriverName.Quote(),
                    e.PortName.Quote(),
                    e.Environment.Quote()
                ));

                Assert.That(e.Name.HasValue(),         Is.True, nameof(e.Name));
                Assert.That(e.DriverName.HasValue(),   Is.True, nameof(e.DriverName));
                Assert.That(e.PortName.HasValue(),     Is.True, nameof(e.PortName));
                Assert.That(e.Environment.HasValue(),  Is.True, nameof(e.Environment));
                Assert.That(e.Exists,                  Is.True, nameof(e.Exists));
            }
        }

        #endregion
    }
}
