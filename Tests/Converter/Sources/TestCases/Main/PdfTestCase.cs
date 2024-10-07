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
using Cube.Pdf.Converter;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// PdfTestCase
///
/// <summary>
/// Represents test cases for PostScript to PDF conversion.
/// These test cases are invoked through the MainTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class PdfTestCase : TestCaseBase<SettingValue>
{
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
        yield return Make("Color_Rgb",  new() { ColorMode = ColorMode.Rgb });
        yield return Make("Color_Gray", new() { ColorMode = ColorMode.Grayscale });
        yield return Make("Color_Mono", new() { ColorMode = ColorMode.Monochrome });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetEncodingTestCases
    ///
    /// <summary>
    /// Gets test cases for Encoding settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetEncodingTestCases()
    {
        yield return Make("Encoding_Jpeg",  new() { Encoding = Encoding.Jpeg });
        yield return Make("Encoding_Flate", new() { Encoding = Encoding.Flate });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetOrientationTestCase
    ///
    /// <summary>
    /// Gets test cases for Orientation settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetOrientationTestCases()
    {
        yield return Make("Orientation_Portrait",  new() { Orientation = Orientation.Portrait });
        yield return Make("Orientation_Landscape", new() { Orientation = Orientation.Landscape });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetSaveOptionTestCases
    ///
    /// <summary>
    /// Gets test cases for SaveOption settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetSaveOptionTestCases()
    {
        yield return Make("Save_MergeHead",   new() { SaveOption = SaveOption.MergeHead });
        yield return Make("Save_MergeTail",   new() { SaveOption = SaveOption.MergeTail });
        yield return Make("Save_MergeRename", new() { SaveOption = SaveOption.Rename });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetMetadataTestCases
    ///
    /// <summary>
    /// Gets test cases for Metadata settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetMetadataTestCases()
    {
        var en = new Metadata
        {
            Title    = "PDF Test",
            Author   = "CubeSoft, Inc.",
            Subject  = "Metadata settings",
            Keywords = "Metadata, Ascii",
            Creator  = "CubePDF Tests",
            Options  = ViewerOption.SinglePage,
        };

        var ja = new Metadata
        {
            Title    = "PDF テスト",
            Author   = "株式会社キューブ・ソフト (CubeSoft, Inc.)",
            Subject  = "Metadata の設定テスト",
            Keywords = "Metadata, CJK, 日本語",
            Creator  = "CubePDF Tests",
            Options  = ViewerOption.SinglePage,
        };

        foreach (var e in new Tuple<string, Metadata>[]
        {
            new("Metadata",     en),
            new("Metadata_Cjk", ja),
        }) yield return Make(e.Item1, new() { Metadata = e.Item2 });
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetEncryptionTestCases
    ///
    /// <summary>
    /// Gets test cases for Encryption settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<TestCaseData> GetEncryptionTestCases()
    {
        var crypt = new Encryption
        {
            Enabled          = true,
            OwnerPassword    = "Password",
            UserPassword     = "User",
            OpenWithPassword = true,
            Permission       = new()
            {
                Accessibility     = PermissionValue.Allow,
                CopyContents      = PermissionValue.Deny,
                InputForm         = PermissionValue.Allow,
                ModifyAnnotations = PermissionValue.Deny,
                ModifyContents    = PermissionValue.Deny,
                Print             = PermissionValue.Deny,
            }
        };

        foreach (var e in new Tuple<string, Encryption>[]
        {
            new("Encryption", crypt),
        }) yield return Make(e.Item1, new() { Encryption = e.Item2 });
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
        yield return Make("日本語 PDF サンプル", "Sample.ps", new() { });
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
        .Concat(GetColorModeTestCases())
        .Concat(GetEncodingTestCases())
        .Concat(GetOrientationTestCases())
        .Concat(GetSaveOptionTestCases())
        .Concat(GetMetadataTestCases())
        .Concat(GetEncryptionTestCases());

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
        value.Resolution  = 96;
        value.PostProcess = PostProcess.None;
    }

    #endregion
}
