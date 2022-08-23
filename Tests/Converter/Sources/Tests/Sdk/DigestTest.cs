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

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// DigestTest
///
/// <summary>
/// Tests the conversion test via the Facade class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class DigestTest : FileFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the digest check.
    /// </summary>
    ///
    /// <param name="name">Test name.</param>
    /// <param name="src">Source filename.</param>
    /// <param name="digest">SHA-256 value of the specified file.</param>
    /// <param name="test">Test action.</param>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public void Test(string name, string src, string digest, Action<Facade> test)
    {
        var ss = new SettingFolder();
        ss.Value.Temp = Get(".tmp");
        ss.Set(new MockArguments(name, GetSource(src), ss.Value.Temp) { Digest = digest });
        ss.Value.Destination = Get($"{name}.pdf");
        ss.Value.Resolution  = 96;
        ss.Value.PostProcess = PostProcess.None;

        using var sdk = new Facade(ss);
        test(sdk);
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
    static IEnumerable<TestCaseData> TestCases()
    {
        Action<Facade> pass = e => Assert.DoesNotThrow(e.Invoke);
        Action<Facade> fail = e => Assert.That(e.Invoke, Throws.TypeOf<CryptographicException>());

        yield return new("Pass_UpperCase", "Sample.ps",
            "A0F7076230C1E0EC5FEBDCE483B79293FE87D15DFC4EE87731EC88321F2D30E9", pass);
        yield return new("Pass_LowerCase", "Sample.ps",
            "a0f7076230c1e0ec5febdce483b79293fe87d15dfc4ee87731ec88321f2d30e9", pass);
        yield return new("Fail", "Sample.ps", "dummy", fail);
    }
}
