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
using System.Threading.Tasks;
using Cube.FileSystem;
using Cube.Pdf.Converter.Extensions;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using Cube.Tests.Forms;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// MainTest
///
/// <summary>
/// Tests the main operation through the MainViewModel class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class MainTest : MockFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the main operation.
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
    public async Task Test(string category, string name, string src, SettingValue value)
    {
        var dest = Get(category, $"{name}{value.Extensions.Get(value.Format)}");
        var ss   = new SettingFolder(DataContract.Format.Registry, GetKeyName());

        ss.Load();
        ss.Normalize();
        ss.Value.Temp = Get(".tmp");
        ss.Set(new MockArguments(name, GetSource(src), ss.Value.Temp));

        using var vm = new MainViewModel(ss);
        using var dc = new DisposableContainer();

        dc.Add(new PropertyChangedCounter(vm, vm.Settings, vm.Metadata, vm.Encryption));
        dc.Add(new MockDialogBehavior(vm));
        dc.Add(new MockSaveFileBehavior(dest, vm));
        dc.Add(new MockFailBehavior<OpenFileMessage>(vm));
        dc.Add(new MockFailBehavior<ProcessMessage>(vm));

        Set(vm.Settings,   value);
        Set(vm.Metadata,   value.Metadata);
        Set(vm.Encryption, value.Encryption);

        vm.SelectDestination();

        Assert.That(Io.Exists(vm.Settings.Source), vm.Settings.Source);
        Assert.That(await Invoke(vm), "Timeout");
        foreach (var e in vm.Results) Assert.That(Io.Exists(e), e);
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
        .Concat(new PsTestCase())
        .Concat(new EpsTestCase())
        .Concat(new PdfTestCase())
        .Concat(new BmpTestCase())
        .Concat(new PngTestCase())
        .Concat(new JpegTestCase())
        .Concat(new TiffTestCase());

    #endregion

    #region Others

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets the specified settings.
    /// </summary>
    ///
    /// <remarks>
    /// Source and Destination property are not changed by the method.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private void Set(SettingViewModel src, SettingValue value)
    {
        src.Format            = value.Format;
        src.SaveOption        = value.SaveOption;
        src.ColorMode         = value.ColorMode;
        src.PostProcess       = value.PostProcess;
        src.UserProgram       = value.UserProgram;
        src.Resolution        = value.Resolution;
        src.Linearization     = value.Linearization;
        src.IsJpegEncoding    = value.Encoding == Encoding.Jpeg;
        src.IsAutoOrientation = value.Orientation == Orientation.Auto;
        src.IsPortrait        = value.Orientation == Orientation.Portrait;
        src.IsLandscape       = value.Orientation == Orientation.Landscape;
        src.Language          = value.Appendix.Language;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets the specified metadata.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Set(MetadataViewModel src, Metadata value)
    {
        src.Title    = value.Title;
        src.Author   = value.Author;
        src.Subject  = value.Subject;
        src.Keywords = value.Keywords;
        src.Version  = value.Version.Minor;
        src.Options  = value.Options;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets the specified encryption.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Set(EncryptionViewModel src, Encryption value)
    {
        src.Enabled            = value.Enabled;
        src.OwnerPassword      = value.OwnerPassword;
        src.OwnerConfirm       = value.OwnerPassword;
        src.OpenWithPassword   = value.OpenWithPassword;
        src.SharePassword      = false;
        src.UserPassword       = value.UserPassword;
        src.UserConfirm        = value.UserPassword;
        src.AllowPrint         = value.Permission.Print.IsAllowed();
        src.AllowCopy          = value.Permission.CopyContents.IsAllowed();
        src.AllowModify        = value.Permission.ModifyContents.IsAllowed();
        src.AllowAccessibility = value.Permission.Accessibility.IsAllowed();
        src.AllowForm          = value.Permission.InputForm.IsAllowed();
        src.AllowAnnotation    = value.Permission.ModifyAnnotations.IsAllowed();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the main operation and wait for completion.
    /// </summary>
    ///
    /// <param name="vm">Source ViewModel object.</param>
    ///
    /// <returns>true for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private async Task<bool> Invoke(MainViewModel vm)
    {
        // for MergeHead, MergeTail, and Rename tests.
        var copy = vm.Settings.Format == Format.Pdf &&
                   vm.Settings.SaveOption != SaveOption.Overwrite;
        if (copy) Io.Copy(GetSource("Sample.pdf"), vm.Settings.Destination, true);

        var done = false;
        using var obj = vm.Subscribe<CloseMessage>(_ => done = true);
        vm.Invoke();
        return await Wait.ForAsync(() => done);
    }

    #endregion
}
