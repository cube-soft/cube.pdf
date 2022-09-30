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

using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// MigrationTest
///
/// <summary>
/// Tests the migration of settings from V2 to V3.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class MigrationTest : RegistryFixture
{
    /* ----------------------------------------------------------------- */
    ///
    /// Test
    ///
    /// <summary>
    /// Tests the migration of settings.
    /// </summary>
    ///
    /* ----------------------------------------------------------------- */
    [Test]
    public void Test()
    {
        var fmt = DataContract.Format.Registry;
        var v2  = GetKeyName(nameof(Test), "V2");
        var v3  = GetKeyName(nameof(Test), "V3");

        DataContract.Proxy.Serialize(fmt, v2, new SettingV2
        {
            EmbedFonts        = false,
            ExplicitDirectory = true,
            Downsampling      = Downsampling.None,
            ImageFilter       = false,
            Grayscale         = true,
        });

        var dest = new SettingFolder(fmt, v3);
        dest.Migrate(v2);
        Assert.That(dest.Value.ColorMode,    Is.EqualTo(ColorMode.Grayscale));
        Assert.That(dest.Value.Encoding,     Is.EqualTo(Encoding.Flate));
        Assert.That(dest.Value.Downsampling, Is.EqualTo(Downsampling.None));
        Assert.That(dest.Value.EmbedFonts,   Is.False, nameof(dest.Value.EmbedFonts));
        Assert.That(dest.Value.Appendix.ExplicitDirectory, Is.True);
        Assert.That(DataContract.Proxy.Exists(fmt, v2), Is.False, "V2");
        Assert.That(DataContract.Proxy.Exists(fmt, v3), Is.True,  "V3");
    }
}
