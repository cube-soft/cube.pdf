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
namespace Cube.Pdf.Converter.Appendix;

using System.Runtime.Serialization;
using Cube.DataContract;

/* ------------------------------------------------------------------------- */
///
/// ExtensionSettingValue
///
/// <summary>
/// Represents user settings for the default extension for each file format.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
public class ExtensionSettingValue : SerializableBase
{
    /* --------------------------------------------------------------------- */
    ///
    /// Pdf
    ///
    /// <summary>
    /// Gets or sets the default extension for the PDF format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Pdf
    {
        get => Get(() => ".pdf");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Ps
    ///
    /// <summary>
    /// Gets or sets the default extension for the PostScript format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Ps
    {
        get => Get(() => ".ps");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Eps
    ///
    /// <summary>
    /// Gets or sets the default extension for the EPS format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Eps
    {
        get => Get(() => ".eps");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Png
    ///
    /// <summary>
    /// Gets or sets the default extension for the PNG format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Png
    {
        get => Get(() => ".png");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Jpeg
    ///
    /// <summary>
    /// Gets or sets the default extension for the JPEG format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Jpeg
    {
        get => Get(() => ".jpg");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Bmp
    ///
    /// <summary>
    /// Gets or sets the default extension for the BMP format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Bmp
    {
        get => Get(() => ".bmp");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Tiff
    ///
    /// <summary>
    /// Gets or sets the default extension for the TIFF format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Tiff
    {
        get => Get(() => ".tiff");
        set => Set(value);
    }
}
