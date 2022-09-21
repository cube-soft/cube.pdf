/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Converter.Tests.Sdk;

using System.Collections.Generic;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// SdkTest
///
/// <summary>
/// Tests the conversion through the Facade class. Note that the class
/// tests only special situations, and other cases are invoked in the
/// MainTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class SdkTest : FileFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the main conversion.
    /// </summary>
    ///
    /// <param name="category">
    /// Category name. Test results are saved to a directory with the
    /// specified name.
    /// </param>
    ///
    /// <param name="name">
    /// Test name, which is used for a part of the destination path.
    /// </param>
    ///
    /// <param name="src">Source filename.</param>
    /// <param name="value">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public void Test(string category, string name, string src, SettingValue value)
    {
        var ss = new SettingFolder();
        ss.Value.Temp = Get(".tmp");
        ss.Set(new MockArguments(name, GetSource(src), ss.Value.Temp));
        ss.Value.Destination = Get(category, $"{name}{value.Extensions.Get(value.Format)}");
        ss.Value.Format      = value.Format;
        ss.Value.ColorMode   = value.ColorMode;
        ss.Value.Encoding    = value.Encoding;
        ss.Value.Resolution  = 96;
        ss.Value.PostProcess = PostProcess.None;

        using var sdk = new Facade(ss);
        sdk.Invoke();
        foreach (var e in sdk.Results) Assert.That(Io.Exists(e), e);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// TestCases
    ///
    /// <summary>
    /// Gets test cases of the Test method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static IEnumerable<TestCaseData> TestCases => new SdkTestCase();
}
