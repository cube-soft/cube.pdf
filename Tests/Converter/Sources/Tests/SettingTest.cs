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

using System.Linq;
using Cube.Pdf.Ghostscript;
using Cube.Tests.Forms;
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
        var ss = new SettingFolder();
        Assert.That(ss.Format,             Is.EqualTo(DataContract.Format.Registry));
        Assert.That(ss.Location,           Is.EqualTo(@"CubeSoft\CubePDF\v3"));
        Assert.That(ss.AutoSave,           Is.False, nameof(ss.AutoSave));
        Assert.That(ss.DocumentName,       Is.Not.Null, nameof(ss.DocumentName));
        Assert.That(ss.Digest,             Is.Null, nameof(ss.Digest));
        Assert.That(ss.Value.Downsampling, Is.EqualTo(Downsampling.Bicubic));
        Assert.That(ss.Value.EmbedFonts,   Is.True, nameof(ss.Value.EmbedFonts));

        using var vm = new MainViewModel(ss);
        Assert.That(vm.Busy,               Is.False, nameof(vm.Busy));
        Assert.That(vm.Results.Any(),      Is.False, nameof(vm.Results));

        var s0 = vm.Settings;
        Assert.That(s0.Title,              Does.StartWith("CubePDF 3.0.0 ("));
        Assert.That(s0.Version,            Does.StartWith("3.0.0 ("));
        Assert.That(s0.Uri.ToString(),     Does.StartWith("https://www.cube-soft.jp/cubepdf/?lang="));
        Assert.That(s0.Format,             Is.EqualTo(Format.Pdf));
        Assert.That(s0.SaveOption,         Is.EqualTo(SaveOption.Overwrite));
        Assert.That(s0.ColorMode,          Is.EqualTo(ColorMode.SameAsSource));
        Assert.That(s0.PostProcess,        Is.EqualTo(PostProcess.None));
        Assert.That(s0.Language,           Is.EqualTo(Language.Auto));
        Assert.That(s0.Source,             Is.Empty, nameof(s0.Source));
        Assert.That(s0.Destination,        Does.EndWith("Desktop"));
        Assert.That(s0.UserProgram,        Is.Empty, nameof(s0.UserProgram));
        Assert.That(s0.Resolution,         Is.EqualTo(600));
        Assert.That(s0.IsPdf,              Is.True,  nameof(s0.IsPdf));
        Assert.That(s0.IsJpegEncoding,     Is.True,  nameof(s0.IsJpegEncoding));
        Assert.That(s0.IsAutoOrientation,  Is.True,  nameof(s0.IsAutoOrientation));
        Assert.That(s0.IsPortrait,         Is.False, nameof(s0.IsPortrait));
        Assert.That(s0.IsLandscape,        Is.False, nameof(s0.IsLandscape));
        Assert.That(s0.IsUserProgram,      Is.False, nameof(s0.IsUserProgram));
        Assert.That(s0.Linearization,      Is.False, nameof(s0.Linearization));
        Assert.That(s0.SourceVisible,      Is.False, nameof(s0.SourceVisible));
        Assert.That(s0.SourceEditable,     Is.True,  nameof(s0.SourceEditable));
        Assert.That(s0.CheckUpdate,        Is.False, nameof(s0.CheckUpdate));

        var s1 = vm.Metadata;
        Assert.That(s1.Title,              Is.Empty, nameof(s1.Title));
        Assert.That(s1.Author,             Is.Empty, nameof(s1.Author));
        Assert.That(s1.Subject,            Is.Empty, nameof(s1.Subject));
        Assert.That(s1.Keywords,           Is.Empty, nameof(s1.Keywords));
        Assert.That(s1.Creator,            Is.Empty, nameof(s1.Creator));
        Assert.That(s1.Version,            Is.EqualTo(7), nameof(s1.Version));
        Assert.That(s1.Options,            Is.EqualTo(ViewerOption.OneColumn));

        var s2 = vm.Encryption;
        Assert.That(s2.Enabled,            Is.False, nameof(s2.Enabled));
        Assert.That(s2.OwnerPassword,      Is.Empty, nameof(s2.OwnerPassword));
        Assert.That(s2.OwnerConfirm,       Is.Empty, nameof(s2.OwnerConfirm));
        Assert.That(s2.OwnerCorrect,       Is.False, nameof(s2.OwnerCorrect));
        Assert.That(s2.OpenWithPassword,   Is.False, nameof(s2.OpenWithPassword));
        Assert.That(s2.SharePassword,      Is.False, nameof(s2.SharePassword));
        Assert.That(s2.UserPassword,       Is.Empty, nameof(s2.UserPassword));
        Assert.That(s2.UserConfirm,        Is.Empty, nameof(s2.UserConfirm));
        Assert.That(s2.UserCorrect,        Is.True,  nameof(s2.UserCorrect));
        Assert.That(s2.UserRequired,       Is.False, nameof(s2.UserRequired));
        Assert.That(s2.Permissible,        Is.True,  nameof(s2.Permissible));
        Assert.That(s2.AllowPrint,         Is.True,  nameof(s2.AllowPrint));
        Assert.That(s2.AllowCopy,          Is.True,  nameof(s2.AllowCopy));
        Assert.That(s2.AllowModify,        Is.True,  nameof(s2.AllowModify));
        Assert.That(s2.AllowAccessibility, Is.True,  nameof(s2.AllowAccessibility));
        Assert.That(s2.AllowForm,          Is.True,  nameof(s2.AllowForm));
        Assert.That(s2.AllowAnnotation,    Is.True,  nameof(s2.AllowAnnotation));

        var s3 = ss.Value.Appendix;
        Assert.That(s3.Language,           Is.EqualTo(Language.Auto));
        Assert.That(s3.SourceVisible,      Is.False, nameof(s3.SourceVisible));
        Assert.That(s3.ExplicitDirectory,  Is.False, nameof(s3.ExplicitDirectory));
        Assert.That(s3.Extensions.Pdf,     Is.EqualTo(".pdf"));
        Assert.That(s3.Extensions.Ps,      Is.EqualTo(".ps"));
        Assert.That(s3.Extensions.Eps,     Is.EqualTo(".eps"));
        Assert.That(s3.Extensions.Png,     Is.EqualTo(".png"));
        Assert.That(s3.Extensions.Jpeg,    Is.EqualTo(".jpg"));
        Assert.That(s3.Extensions.Bmp,     Is.EqualTo(".bmp"));
        Assert.That(s3.Extensions.Tiff,    Is.EqualTo(".tiff"));

    }

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
