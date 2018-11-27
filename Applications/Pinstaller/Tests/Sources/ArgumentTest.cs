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
using Cube.Collections;
using Cube.DataContract;
using Cube.Pdf.App.Pinstaller;
using NUnit.Framework;
using System.Collections.Generic;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArgumentTest
    ///
    /// <summary>
    /// Represents tests of the ArgumentCollection and related classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArgumentTest : DeviceFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Parse
        ///
        /// <summary>
        /// Executes the test to parse arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Parse(IEnumerable<string> args, int id)
        {
            var src = new ArgumentCollection(args, '/', true);
            Assert.That(src.GetRetryCount(),        Is.EqualTo(10));
            Assert.That(src.GetCommand(),           Is.Not.Null.And.Not.Empty);
            Assert.That(src.GetConfiguration(),     Is.Not.EqualTo("Sample.json").And.EndWith("Sample.json"));
            Assert.That(src.GetResourceDirectory(), Is.Not.EqualTo("Printers").And.EndWith("Printers"));

            var dest = new Installer(Format.Json, GetExamplesWith("SampleSkeleton.json"));

            var s0 = dest.Config.Ports[0].Application;
            var c0 = src.ReplaceDirectory(s0);
            Assert.That(s0, Is.EqualTo("%%DIR%%\\CubeProxy.exe"));
            Assert.That(c0, Is.Not.EqualTo(s0));
            Assert.That(c0, Does.Not.StartWith("%%DIR%%"));
            Assert.That(c0, Does.EndWith("CubeProxy.exe"));

            var s1 = dest.Config.Ports[0].Arguments;
            var c1 = src.ReplaceDirectory(s1);
            Assert.That(s1, Is.EqualTo("/Exec \"%%dir%%\\CubePDF\\cubepdf.exe\""));
            Assert.That(c1, Is.Not.EqualTo(s1));
            Assert.That(c1, Does.Not.Contain("%%dir%%"));
            Assert.That(c1, Does.StartWith("/Exec"));
            Assert.That(c1, Does.EndWith("\""));

            var s2 = dest.Config.Ports[1].Application;
            var c2 = src.ReplaceDirectory(s2);
            Assert.That(s2, Is.EqualTo("C:\\Program Files\\CubePDF\\CubeProxy.exe"));
            Assert.That(c2, Is.EqualTo(s2));

            var s3 = dest.Config.Ports[1].Arguments;
            var c3 = src.ReplaceDirectory(s3);
            Assert.That(s3, Is.Empty);
            Assert.That(c3, Is.EqualTo(s3));
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
                var n = 0;

                yield return new TestCaseData(new List<string>
                {
                    "Sample.json",
                    "/Command", "Install",
                    "/Relative",
                    "/Resource", "Printers",
                    "/Retry", "10",
                }, n++);

                yield return new TestCaseData(new List<string>
                {
                    "Sample.json",
                    "/command", "install",
                    "/relative",
                    "/resource", "Printers",
                    "/retry", "10",
                }, n++);

                yield return new TestCaseData(new List<string>
                {
                    "Sample.json",
                    "/COMMAND", "INSTALL",
                    "/RELATIVE",
                    "/RESOURCE", "Printers",
                    "/RETRY", "10",
                }, n++);
            }
        }

        #endregion
    }
}
