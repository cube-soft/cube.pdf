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
namespace Cube.Pdf.Converter.Tests.Settings;

using System.Linq;
using Cube.Pdf.Ghostscript;
using Cube.Tests.Mocks;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// SettingTest
///
/// <summary>
/// Tests to confirm and/or save user settings through the MainViewModel
/// class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class SettingTest : MockFixture
{
    #region Check

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Check()
    {
        var src = new SettingFolder();
        Check(src);
        Check(src.Value);

        using var vm = new MainViewModel(src);
        Check(vm);
        Check(vm.Settings);
        Check(vm.Metadata);
        Check(vm.Encryption);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings of the MainViewModel object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Check(MainViewModel src)
    {
        Assert.That(src.Busy,          Is.False, nameof(src.Busy));
        Assert.That(src.Results.Any(), Is.False, nameof(src.Results));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings of the SettingViewModel object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Check(SettingViewModel src)
    {
        Assert.That(src.Title,             Does.StartWith("CubePDF 3.0.2 ("));
        Assert.That(src.Version,           Does.StartWith("3.0.2 ("));
        Assert.That(src.Uri.ToString(),    Does.StartWith("https://www.cube-soft.jp/cubepdf/?lang="));
        Assert.That(src.Format,            Is.EqualTo(Format.Pdf));
        Assert.That(src.SaveOption,        Is.EqualTo(SaveOption.Overwrite));
        Assert.That(src.ColorMode,         Is.EqualTo(ColorMode.SameAsSource));
        Assert.That(src.PostProcess,       Is.EqualTo(PostProcess.None));
        Assert.That(src.Language,          Is.EqualTo(Language.Auto));
        Assert.That(src.Source,            Is.Empty, nameof(src.Source));
        Assert.That(src.Destination,       Does.EndWith("Desktop"));
        Assert.That(src.UserProgram,       Is.Empty, nameof(src.UserProgram));
        Assert.That(src.Resolution,        Is.EqualTo(600));
        Assert.That(src.IsPdf,             Is.True,  nameof(src.IsPdf));
        Assert.That(src.IsJpegEncoding,    Is.True,  nameof(src.IsJpegEncoding));
        Assert.That(src.IsAutoOrientation, Is.True,  nameof(src.IsAutoOrientation));
        Assert.That(src.IsPortrait,        Is.False, nameof(src.IsPortrait));
        Assert.That(src.IsLandscape,       Is.False, nameof(src.IsLandscape));
        Assert.That(src.IsUserProgram,     Is.False, nameof(src.IsUserProgram));
        Assert.That(src.Linearization,     Is.False, nameof(src.Linearization));
        Assert.That(src.SourceVisible,     Is.False, nameof(src.SourceVisible));
        Assert.That(src.SourceEditable,    Is.True,  nameof(src.SourceEditable));
        Assert.That(src.CheckUpdate,       Is.False, nameof(src.CheckUpdate));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings of the MetadataViewModel object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Check(MetadataViewModel src)
    {
        Assert.That(src.Title,    Is.Empty, nameof(src.Title));
        Assert.That(src.Author,   Is.Empty, nameof(src.Author));
        Assert.That(src.Subject,  Is.Empty, nameof(src.Subject));
        Assert.That(src.Keywords, Is.Empty, nameof(src.Keywords));
        Assert.That(src.Creator,  Is.Empty, nameof(src.Creator));
        Assert.That(src.Version,  Is.EqualTo(7), nameof(src.Version));
        Assert.That(src.Options,  Is.EqualTo(ViewerOption.OneColumn));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings of the EncryptionViewModel object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Check(EncryptionViewModel src)
    {
        Assert.That(src.Enabled,            Is.False, nameof(src.Enabled));
        Assert.That(src.OwnerPassword,      Is.Empty, nameof(src.OwnerPassword));
        Assert.That(src.OwnerConfirm,       Is.Empty, nameof(src.OwnerConfirm));
        Assert.That(src.OwnerCorrect,       Is.False, nameof(src.OwnerCorrect));
        Assert.That(src.OpenWithPassword,   Is.False, nameof(src.OpenWithPassword));
        Assert.That(src.SharePassword,      Is.False, nameof(src.SharePassword));
        Assert.That(src.UserPassword,       Is.Empty, nameof(src.UserPassword));
        Assert.That(src.UserConfirm,        Is.Empty, nameof(src.UserConfirm));
        Assert.That(src.UserCorrect,        Is.True,  nameof(src.UserCorrect));
        Assert.That(src.UserRequired,       Is.False, nameof(src.UserRequired));
        Assert.That(src.Permissible,        Is.True,  nameof(src.Permissible));
        Assert.That(src.AllowPrint,         Is.True,  nameof(src.AllowPrint));
        Assert.That(src.AllowCopy,          Is.True,  nameof(src.AllowCopy));
        Assert.That(src.AllowModify,        Is.True,  nameof(src.AllowModify));
        Assert.That(src.AllowAccessibility, Is.True,  nameof(src.AllowAccessibility));
        Assert.That(src.AllowForm,          Is.True,  nameof(src.AllowForm));
        Assert.That(src.AllowAnnotation,    Is.True,  nameof(src.AllowAnnotation));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings of the SettingFolder object. The method
    /// only checks values that are not accessible from the various ViewModel
    /// objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Check(SettingFolder src)
    {
        Assert.That(src.Format,       Is.EqualTo(DataContract.Format.Registry));
        Assert.That(src.Location,     Is.EqualTo(@"CubeSoft\CubePDF\v3"));
        Assert.That(src.AutoSave,     Is.False, nameof(src.AutoSave));
        Assert.That(src.DocumentName, Is.Not.Null, nameof(src.DocumentName));
        Assert.That(src.Digest,       Is.Null, nameof(src.Digest));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Check
    ///
    /// <summary>
    /// Checks the default settings of the SettingValue object. The method
    /// only checks values that are not accessible from the various ViewModel
    /// objects.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Check(SettingValue src)
    {
        Assert.That(src.Downsampling,    Is.EqualTo(Downsampling.Bicubic));
        Assert.That(src.EmbedFonts,      Is.True, nameof(src.EmbedFonts));

        Assert.That(src.Extensions.Pdf,  Is.EqualTo(".pdf"));
        Assert.That(src.Extensions.Ps,   Is.EqualTo(".ps"));
        Assert.That(src.Extensions.Eps,  Is.EqualTo(".eps"));
        Assert.That(src.Extensions.Png,  Is.EqualTo(".png"));
        Assert.That(src.Extensions.Jpeg, Is.EqualTo(".jpg"));
        Assert.That(src.Extensions.Bmp,  Is.EqualTo(".bmp"));
        Assert.That(src.Extensions.Tiff, Is.EqualTo(".tiff"));

        Assert.That(src.Appendix.Language,          Is.EqualTo(Language.Auto));
        Assert.That(src.Appendix.SourceVisible,     Is.False, nameof(src.Appendix.SourceVisible));
        Assert.That(src.Appendix.ExplicitDirectory, Is.False, nameof(src.Appendix.ExplicitDirectory));
    }

    #endregion

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Tests the Save method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Save()
    {
        var fmt  = DataContract.Format.Registry;
        var name = GetKeyName(nameof(Save));
        Assert.That(DataContract.Proxy.Exists(fmt, name), Is.False);

        var ss = new SettingFolder(fmt, name);
        using var vm = new MainViewModel(ss);
        using var dc = new MockDialogBehavior(vm);

        vm.Metadata.Title = nameof(Save);
        vm.Save();

        var dest = DataContract.Proxy.Deserialize<SettingValue>(fmt, name);
        Assert.That(dest.Metadata.Title, Is.EqualTo(nameof(Save)));
    }
}
