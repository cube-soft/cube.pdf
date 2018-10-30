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
    /// PrinterDriverTest
    ///
    /// <summary>
    /// Represents tests of the PrinterDriver class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PrinterDriverTest
    {
        #region Tests

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
        public void GetElements()
        {
            var src = PrinterDriver.GetElements();
            foreach (var e in src)
            {
                this.LogDebug(string.Format("({0},{1}):{2} ({3})",
                    e.Name.Quote(),
                    e.MonitorName.Quote(),
                    e.FileName.Quote(),
                    e.Environment
                ));
            }
            Assert.That(src.Count(), Is.AtLeast(1));
        }

        #endregion
    }
}
