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
/// BmpTestCase
///
/// <summary>
/// Represents test cases for PostScript to BMP conversion.
/// These test cases are invoked through the MainTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class BmpTestCase : TestCaseBase<SettingValue>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// BmpTestCase
    ///
    /// <summary>
    /// Initializes a new instance of the BmpTestCase class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public BmpTestCase() : base("Sample1pPhoto.ps") { }

    #endregion

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
        yield return Make("R96_Rgb",  new() { ColorMode = ColorMode.Rgb });
        yield return Make("R96_Gray", new() { ColorMode = ColorMode.Grayscale });
        yield return Make("R96_Mono", new() { ColorMode = ColorMode.Monochrome });
        yield return Make("Multipage", "Sample3pMix.ps", new());
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
        value.Format      = Format.Bmp;
        value.Resolution  = 96;
        value.PostProcess = PostProcess.None;
    }

    #endregion
}
