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
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// SettingV2
///
/// <summary>
/// Represents the user settings. This format is version 2 and is currently
/// used only for migration purposes.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
class SettingV2 : DataContract.SerializableBase
{
    #region DataMember

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets or sets the converting format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "FileType")]
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
    /// Gets or sets a value that represents save options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "ExistedFile")]
    public SaveOption SaveOption
    {
        get => Get(() => SaveOption.Overwrite);
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
    /// Downsampling
    ///
    /// <summary>
    /// Gets or sets a value that represents the method of downsampling.
    /// </summary>
    ///
    /// <remarks>
    /// Since it is not expected to have a significant effect, it is
    /// fixed at None and cannot be selected by the user.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Downsampling Downsampling
    {
        get => Get(() => Downsampling.None);
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
    /// Grayscale
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to convert in grayscale.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool Grayscale
    {
        get => Get(() => false);
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
    /// ImageFilter
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to compress embedded
    /// images as JPEG format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool ImageFilter
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
    [DataMember(Name = "WebOptimize")]
    public bool Linearization
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SourceVisible
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to display the
    /// path of the source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool SourceVisible
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExplicitDirectory
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to set a value to the
    /// InitialDirectory property explicitly when showing a dialog
    /// that selects the file or directory name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool ExplicitDirectory
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PlatformCompatible
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to ignore
    /// PlatformNotSupportedException exceptions as possible.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool PlatformCompatible
    {
        get => Get(() => true);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Language
    ///
    /// <summary>
    /// Gets or sets the displayed language.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public Language Language
    {
        get => Get(() => Language.Auto);
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
    /// Gets or sets the path to save the converted file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "LastAccess")]
    public string Destination
    {
        get => Get(PathExplorer.GetDesktopDirectoryName);
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
        get => Get(PathExplorer.GetDeaultDirectoryName);
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

    #endregion
}
