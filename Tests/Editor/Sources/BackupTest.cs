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
namespace Cube.Pdf.Editor.Tests;

using Cube.DataContract;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// BackupTest
///
/// <summary>
/// Tests the Backup class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
internal class BackupTest : FileFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// GetRootDirectory
    ///
    /// <summary>
    /// Tests the GetRootDirectory method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(@"C:\Users\Foo\CubePdfUtility2\Backup",  ExpectedResult = @"C:\Users\Foo\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Users\Foo\CubePdfUtility2\Backup\", ExpectedResult = @"C:\Users\Foo\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Users\Foo\CubePdfUtility2",         ExpectedResult = @"C:\Users\Foo\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Users\Foo\CubePdfUtility2\",        ExpectedResult = @"C:\Users\Foo\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Users\Foo",                         ExpectedResult = @"C:\Users\Foo\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Users\Foo\",                        ExpectedResult = @"C:\Users\Foo\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Users\Foo\Backup",                  ExpectedResult = @"C:\Users\Foo\Backup\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Lower\1\cubepdfutility2\backup",    ExpectedResult = @"C:\Lower\1\cubepdfutility2\backup")]
    [TestCase(@"C:\Lower\2\Cubepdfutility2\Backup",    ExpectedResult = @"C:\Lower\2\Cubepdfutility2\Backup")]
    [TestCase(@"C:\Temp",                              ExpectedResult = @"C:\Temp\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Temp\",                             ExpectedResult = @"C:\Temp\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Backup",                            ExpectedResult = @"C:\Backup\CubePdfUtility2\Backup")]
    [TestCase(@"C:\Backup\",                           ExpectedResult = @"C:\Backup\CubePdfUtility2\Backup")]
    [TestCase(@"C:\",                                  ExpectedResult = @"C:\CubePdfUtility2\Backup")]
    [TestCase(@"C:",                                   ExpectedResult = @"C:\CubePdfUtility2\Backup")]
    [TestCase(@"Relative\To",                          ExpectedResult = @"Relative\To\CubePdfUtility2\Backup")]
    [TestCase(@"\\192.168.1.4\To",                     ExpectedResult = @"\\192.168.1.4\To\CubePdfUtility2\Backup")]
    public string GetRootDirectory(string src)
    {
        var settings = new SettingFolder(Format.Json, Get("Settings.json"));
        settings.Value.Backup = src;

        var dest = new Backup(settings);
        return dest.GetRootDirectory();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetDefaultRootDirectory
    ///
    /// <summary>
    /// Tests the GetRootDirectory and GetDefaultRootDirectory methods.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void GetDefaultRootDirectory()
    {
        var settings = new SettingFolder(Format.Json, Get("Settings.json"));
        settings.Value.Backup = string.Empty;

        var dest = new Backup(settings);
        Assert.That(dest.GetRootDirectory(), Is.EqualTo(Backup.GetDefaultRootDirectory()));
    }
}