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

using System.Runtime.Serialization;
using Cube.DataContract;

/* ------------------------------------------------------------------------- */
///
/// ExtensionTable
///
/// <summary>
/// Represents user settings of the default extension for each file format.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
public class ExtensionList : SerializableBase
{
    #region Properties

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

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the extension corresponding to the specified file format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Get(Ghostscript.Format src) => src switch
    {
        Ghostscript.Format.Pdf                  => Pdf,
        Ghostscript.Format.Ps                   => Ps,
        Ghostscript.Format.Eps                  => Eps,
        Ghostscript.Format.Png                  => Png,
        Ghostscript.Format.Png1bppMonochrome    => Png,
        Ghostscript.Format.Png24bppRgb          => Png,
        Ghostscript.Format.Png32bppArgb         => Png,
        Ghostscript.Format.Png4bppIndexed       => Png,
        Ghostscript.Format.Png8bppGrayscale     => Png,
        Ghostscript.Format.Png8bppIndexed       => Png,
        Ghostscript.Format.Jpeg                 => Jpeg,
        Ghostscript.Format.Jpeg24bppRgb         => Jpeg,
        Ghostscript.Format.Jpeg32bppCmyk        => Jpeg,
        Ghostscript.Format.Jpeg8bppGrayscale    => Jpeg,
        Ghostscript.Format.Bmp                  => Bmp,
        Ghostscript.Format.Bmp1bppMonochrome    => Bmp,
        Ghostscript.Format.Bmp24bppRgb          => Bmp,
        Ghostscript.Format.Bmp32bppArgb         => Bmp,
        Ghostscript.Format.Bmp4bppIndexed       => Bmp,
        Ghostscript.Format.Bmp8bppGrayscale     => Bmp,
        Ghostscript.Format.Bmp8bppIndexed       => Bmp,
        Ghostscript.Format.Tiff                 => Tiff,
        Ghostscript.Format.Tiff12bppRgb         => Tiff,
        Ghostscript.Format.Tiff1bppMonochrome   => Tiff,
        Ghostscript.Format.Tiff24bppRgb         => Tiff,
        Ghostscript.Format.Tiff32bppCmyk        => Tiff,
        Ghostscript.Format.Tiff48bppRgb         => Tiff,
        Ghostscript.Format.Tiff64bppCmyk        => Tiff,
        Ghostscript.Format.Tiff8bppGrayscale    => Tiff,
        _ => Ghostscript.FormatMethods.GetExtension(src),
    };

    #endregion
}
