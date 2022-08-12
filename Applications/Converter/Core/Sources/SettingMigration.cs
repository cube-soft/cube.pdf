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
namespace Cube.Pdf.Converter;

using Cube.DataContract;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// SettingMigration
///
/// <summary>
/// Provides extended methods for migrating user settings from version 2
/// to version 3.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class SettingMigration
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Migrate
    ///
    /// <summary>
    /// Migrates user settings from version 2 to version 3.
    /// </summary>
    ///
    /// <param name="src">Object for version 3 user settings.</param>
    /// <param name="format">Serialized format for version 2.</param>
    /// <param name="location">Serialized path for version 2.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Migrate(this SettingFolder src, DataContract.Format format, string location)
    {
        if (!format.Exists(location)) { src.Load(); return; }

        var v3 = src.Value;
        var v2 = format.Deserialize<SettingV2>(location);

        v3.Format             = v2.Format;
        v3.SaveOption         = v2.SaveOption;
        v3.ColorMode          = v2.Grayscale ? ColorMode.Grayscale : ColorMode.SameAsSource;
        v3.Orientation        = v2.Orientation;
        v3.Encoding           = v2.ImageFilter ? Encoding.Jpeg : Encoding.Flate;
        v3.Downsampling       = v2.Downsampling;
        v3.Resolution         = v2.Resolution;
        v3.EmbedFonts         = v2.EmbedFonts;
        v3.Linearization      = v2.Linearization;
        v3.PlatformCompatible = v2.PlatformCompatible;
        v3.PostProcess        = v2.PostProcess;
        v3.UserProgram        = v2.UserProgram;
        v3.Destination        = v2.Destination;
        v3.Temp               = v2.Temp;

        // Metadata
        v3.Metadata.Version   = v2.Metadata.Version;
        v3.Metadata.Author    = v2.Metadata.Author;
        v3.Metadata.Title     = v2.Metadata.Title;
        v3.Metadata.Subject   = v2.Metadata.Subject;
        v3.Metadata.Keywords  = v2.Metadata.Keywords;
        v3.Metadata.Creator   = v2.Metadata.Creator;
        v3.Metadata.Producer  = v2.Metadata.Producer;
        v3.Metadata.Options   = v2.Metadata.Options;

        // View
        v3.View.SourceVisible     = v2.SourceVisible;
        v3.View.ExplicitDirectory = v2.ExplicitDirectory;
        v3.View.Language          = v2.Language;

        src.Save();
        format.Delete(location);
    }

    #endregion
}
