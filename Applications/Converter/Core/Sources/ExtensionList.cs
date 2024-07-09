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

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Cube.DataContract;
using GsFormat = Ghostscript.Format;

/* ------------------------------------------------------------------------- */
///
/// ExtensionList
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
    public string Get(GsFormat src) => GetCandidates(src).First();

    /* --------------------------------------------------------------------- */
    ///
    /// GetCandidates
    ///
    /// <summary>
    /// Gets the collection of file extension candidates corresponding to
    /// the specified format.
    /// </summary>
    ///
    /// <param name="src">File format.</param>
    ///
    /// <returns>Collection of file extension candidates.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> GetCandidates(GsFormat src) => FormatGroup.Represent(src) switch
    {
        GsFormat.Pdf  => Combine(Pdf, ".pdf"),
        GsFormat.Ps   => Combine(Ps, ".ps"),
        GsFormat.Eps  => Combine(Eps, ".eps"),
        GsFormat.Png  => Combine(Png, ".png"),
        GsFormat.Jpeg => Combine(Jpeg, ".jpg", ".jpeg"),
        GsFormat.Bmp  => Combine(Bmp, ".bmp"),
        GsFormat.Tiff => Combine(Tiff, ".tiff", ".tif"),
        GsFormat.Text => [ ".txt" ],
        _ => [ $".{src.ToString().ToLowerInvariant()}" ],
    };

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Combine
    ///
    /// <summary>
    /// Combines the specified elements while removing duplicates.
    /// </summary>
    ///
    /// <param name="src">Primary file extension.</param>
    /// <param name="latter">Other file extension candidates.</param>
    ///
    /// <returns>Collection of file extension candidates.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<string> Combine(string src, params string[] latter) =>
        new[] { src }.Concat(latter.Where(e => e != src));

    #endregion
}
