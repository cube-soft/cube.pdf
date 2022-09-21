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
using System.Linq;
using Cube.Pdf.Converter;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// JpegTestCase
///
/// <summary>
/// Represents test cases for PostScript to JPEG conversion.
/// These test cases are invoked through the MainTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class JpegTestCase : TestCaseBase<SettingValue>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// JpegTestCase
    ///
    /// <summary>
    /// Initializes a new instance of the JpegTestCase class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public JpegTestCase() : base("Sample1pPhoto.ps") { }

    #endregion

    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// GetColorModeTestCases
    ///
    /// <summary>
    /// Gets test cases for ColorMode settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetColorModeTestCases()
    {
        foreach (var n in new[] { 100, 150 })
        {
            yield return Make($"R{n}_Rgb",  new() { Resolution = n, ColorMode = ColorMode.Rgb });
            yield return Make($"R{n}_Gray", new() { Resolution = n, ColorMode = ColorMode.Grayscale });
            yield return Make($"R{n}_Mono", new() { Resolution = n, ColorMode = ColorMode.Monochrome });
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetOtherTestCases
    ///
    /// <summary>
    /// Gets test cases for other settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetOtherTestCases()
    {
        yield return Make("Multipage", "Sample3pMix.ps", new());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the collection of test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<TestCaseData> Get() => GetOtherTestCases()
        .Concat(GetColorModeTestCases());

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
        value.Format      = Format.Jpeg;
        value.PostProcess = PostProcess.None;
        if (value.Resolution > 300) value.Resolution = 96;
    }

    #endregion
}
