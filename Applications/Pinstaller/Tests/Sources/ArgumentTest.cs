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
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cube.Pdf.Pinstaller.Tests
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
            Assert.That(src.GetTimeout(),           Is.EqualTo(300));
            Assert.That(src.GetRetryCount(),        Is.EqualTo(10));
            Assert.That(src.GetCommand(),           Is.Not.Null.And.Not.Empty);
            Assert.That(src.GetConfiguration(),     Is.Not.EqualTo("Sample.json").And.EndWith("Sample.json"));
            Assert.That(src.GetResourceDirectory(), Is.Not.EqualTo("Printers").And.EndWith("Printers"));

        }

        /* ----------------------------------------------------------------- */
        ///
        /// Parse_Empty
        ///
        /// <summary>
        /// Executes the test to parse an empty argument.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Parse_Empty()
        {
            var src = new ArgumentCollection(new string[0], '/', true);
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Assert.That(src.GetTimeout(),           Is.EqualTo(30));
            Assert.That(src.GetRetryCount(),        Is.EqualTo(1));
            Assert.That(src.GetCommand(),           Is.Empty);
            Assert.That(src.GetConfiguration(),     Is.Null);
            Assert.That(src.GetResourceDirectory(), Is.EqualTo(dir));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Parse_Absolute
        ///
        /// <summary>
        /// Executes the test to parse arguments as absolute path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Parse_Absolute()
        {
            var src = new ArgumentCollection(new[] { "Sample.json" }, '/', true);
            Assert.That(src.GetConfiguration(), Is.EqualTo("Sample.json"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Parse_Integer
        ///
        /// <summary>
        /// Confirms the behavior that invalid integer arguments are
        /// specified.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Parse_Integer()
        {
            var src = new ArgumentCollection(new[] { "Sample.json", "/Timeout", "Dummy", "/Retry", "Dummy" }, '/', true);
            Assert.That(src.GetTimeout(),    Is.EqualTo(30));
            Assert.That(src.GetRetryCount(), Is.EqualTo(1));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Replace
        ///
        /// <summary>
        /// Executes the test to replace some strings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Replace()
        {
            var src  = new ArgumentCollection(new string[0], '/', true);
            var dest = new Installer(Format.Json, GetExamplesWith("SampleSkeleton.json"));

            var s0 = dest.Config.Ports[0].Proxy;
            var c0 = src.ReplaceDirectory(s0);
            Assert.That(s0, Is.EqualTo(@"%%dir%%\CubePDF\CubeProxy.exe"));
            Assert.That(c0, Is.Not.EqualTo(s0));
            Assert.That(c0, Does.Not.StartWith("%%dir%%"));
            Assert.That(c0, Does.EndWith("CubeProxy.exe"));

            var s1 = dest.Config.Ports[0].Application;
            var c1 = src.ReplaceDirectory(s1);
            Assert.That(s1, Is.EqualTo(@"%%DIR%%\CubePDF\cubepdf.exe"));
            Assert.That(c1, Is.Not.EqualTo(s1));
            Assert.That(c1, Does.Not.StartsWith("%%DIR%%"));
            Assert.That(c1, Does.EndWith("cubepdf.exe"));

            var s2 = dest.Config.Ports[1].Proxy;
            var c2 = src.ReplaceDirectory(s2);
            Assert.That(s2, Is.EqualTo(@"C:\Program Files\CubePDF\CubeProxy.exe"));
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
                    "/Timeout", "300",
                    "/Retry", "10",
                }, n++);

                yield return new TestCaseData(new List<string>
                {
                    "Sample.json",
                    "/command", "install",
                    "/relative",
                    "/resource", "Printers",
                    "/timeout", "300",
                    "/retry", "10",
                }, n++);

                yield return new TestCaseData(new List<string>
                {
                    "Sample.json",
                    "/COMMAND", "INSTALL",
                    "/RELATIVE",
                    "/RESOURCE", "Printers",
                    "/TIMEOUT", "300",
                    "/RETRY", "10",
                }, n++);
            }
        }

        #endregion
    }
}
