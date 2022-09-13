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

using System;
using System.Runtime.Serialization;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// SettingValue
///
/// <summary>
/// Represents the user settings.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
public class SettingValue : DataContract.SerializableBase
{
    #region DataMember

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets or sets the format of the conversion result.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Format Format
    {
        get => Get(() => Format.Pdf);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveOption
    ///
    /// <summary>
    /// Gets or sets a value representing save options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public SaveOption SaveOption
    {
        get => Get(() => SaveOption.Overwrite);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ColorMode
    ///
    /// <summary>
    /// Gets or sets the color mode.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public ColorMode ColorMode
    {
        get => Get(() => ColorMode.SameAsSource);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Orientation
    ///
    /// <summary>
    /// Gets or sets the page orientation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Orientation Orientation
    {
        get => Get(() => Orientation.Auto);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Encoding
    ///
    /// <summary>
    /// Gets or sets the compression encoding.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Encoding Encoding
    {
        get => Get(() => Encoding.Jpeg);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Downsampling
    ///
    /// <summary>
    /// Gets or sets a value representing the method for downsampling.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Downsampling Downsampling
    {
        get => Get(() => Downsampling.Bicubic);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Resolution
    ///
    /// <summary>
    /// Gets or sets the resolution.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public int Resolution
    {
        get => Get(() => 600);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EmbedFonts
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to embed fonts.
    /// </summary>
    ///
    /// <remarks>
    /// A problem with garbled characters exists when the font is not
    /// embedded, so the property is hidden from the user.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool EmbedFonts
    {
        get => Get(() => true);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Linearization
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to apply the
    /// linearization option (a.k.a Web optimization).
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool Linearization
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcess
    ///
    /// <summary>
    /// Gets or sets a value that represents the post-process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public PostProcess PostProcess
    {
        get => Get(() => PostProcess.None);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// UserProgram
    ///
    /// <summary>
    /// Gets or sets the path of the user program that executes after
    /// converting.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string UserProgram
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Destination
    ///
    /// <summary>
    /// Gets or sets the path to save the conversion result.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Destination
    {
        get => Get(PathHelper.GetDesktopDirectoryName);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Temp
    ///
    /// <summary>
    /// Gets or sets the path of the temp directory.
    /// </summary>
    ///
    /// <remarks>
    /// Ghostscript may fail to process paths that contain multibyte
    /// characters. Therefore, move to a directory that does not contain
    /// multibyte characters and run the process.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Temp
    {
        get => Get(PathHelper.GetDeaultDirectoryName);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Metadata
    ///
    /// <summary>
    /// Gets or sets the PDF metadata.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Metadata Metadata
    {
        get => Get(() => new Metadata());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Extensions
    ///
    /// <summary>
    /// Gets or sets the user settings of the default extension for each
    /// file format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public ExtensionList Extensions
    {
        get => Get(() => new ExtensionList());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Appendix
    ///
    /// <summary>
    /// Gets or sets the user settings not directly related to the main
    /// conversion process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public SettingValueEx Appendix
    {
        get => Get(() => new SettingValueEx());
        set => Set(value);
    }

    #endregion

    #region Non-DataMember

    /* --------------------------------------------------------------------- */
    ///
    /// Encryption
    ///
    /// <summary>
    /// Gets or sets the encryption information.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Encryption Encryption
    {
        get => Get(() => new Encryption());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets or sets the path of the source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DeleteSource
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to delete the source
    /// file after converting.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool DeleteSource
    {
        get => Get(() => false);
        set => Set(value);
    }

    #endregion
}
