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

using System.Collections.Generic;
using Cube.Pdf.Converter;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// PsTestCase
///
/// <summary>
/// Represents test cases for PostScript to PostScript conversion.
/// These test cases are invoked through the MainTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class PsTestCase : TestCaseBase<SettingValue>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the collection of test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<TestCaseData> Get()
    {
        yield return Make("_Default", new());
    }

    #endregion

    #region Others

    /* --------------------------------------------------------------------- */
    ///
    /// OnMake
    ///
    /// <summary>
    /// Sets up additional settings for the specified settings before
    /// creating a new TestCaseData object.
    /// </summary>
    ///
    /// <param name="value">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnMake(SettingValue value)
    {
        value.Format      = Format.Ps;
        value.Resolution  = 96;
        value.PostProcess = PostProcess.None;
    }

    #endregion
}
