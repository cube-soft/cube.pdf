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
namespace Cube.Pdf.Converter.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// DialogTest
///
/// <summary>
/// Tests to show dialogs through the MainViewModel class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class DialogTest : FileFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests to show dialogs.
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
    /// <param name="assert">Test action.</param>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public async Task Test(string category, string name, string src, Func<MainViewModel, Task> assert)
    {
        var ss = new SettingFolder();
        ss.Value.Temp = Get(".tmp");
        ss.Set(new MockArguments(name, GetSource(src), ss.Value.Temp));
        ss.Value.Destination = Get(category, $"{name}.pdf");
        ss.Value.Resolution  = 96;
        ss.Value.PostProcess = PostProcess.None;

        using var vm = new MainViewModel(ss);
        await assert(vm);
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
    static IEnumerable<TestCaseData> TestCases => Enumerable.Empty<TestCaseData>()
        .Concat(new ErrorTestCase())
        .Concat(new WarnTestCase())
        .Concat(new FileTestCase());

    #endregion

    #region Others

    /* ----------------------------------------------------------------- */
    ///
    /// Setup
    ///
    /// <summary>
    /// Executes in each test.
    /// </summary>
    ///
    /* ----------------------------------------------------------------- */
    [SetUp]
    protected void Setup()
    {
        SynchronizationContext.SetSynchronizationContext(new());
        Locale.Set(Language.Auto);
    }

    #endregion
}
