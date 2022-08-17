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
using System.Threading;
using Cube.FileSystem;
using Cube.Mixin.Observable;
using Cube.Pdf.Converter.Mixin;
using Cube.Pdf.Ghostscript;

/* ------------------------------------------------------------------------- */
///
/// SettingViewModel
///
/// <summary>
/// Represents the ViewModel for the general and others tabs in
/// the main window.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class SettingViewModel : PresentableBase<SettingFacade>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// SettingViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the SettingViewModel class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    /// <param name="proxy">Message aggregator.</param>
    /// <param name="ctx">Synchronization context.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SettingViewModel(SettingFolder src, Aggregator proxy, SynchronizationContext ctx) :
        base(new(src), proxy, ctx)
    {
        Assets.Add(src.Forward(this));
        Assets.Add(src.Value.View.Subscribe(new() {
            { nameof(ViewSettingValue.Language), _ => Locale.Set(src.Value.View.Language) },
        }, Refresh));
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets or sets the conversion format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Format Format
    {
        get => Facade.Settings.Value.Format;
        set => Facade.Settings.Value.Format = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveOption
    ///
    /// <summary>
    /// Gets or sets the saving option.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SaveOption SaveOption
    {
        get => Facade.Settings.Value.SaveOption;
        set => Facade.Settings.Value.SaveOption = value;
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
    public ColorMode ColorMode
    {
        get => Facade.Settings.Value.ColorMode;
        set => Facade.Settings.Value.ColorMode = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcess
    ///
    /// <summary>
    /// Gets or sets the kind of port-process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public PostProcess PostProcess
    {
        get => Facade.Settings.Value.PostProcess;
        set => Facade.Settings.Value.PostProcess = value;
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
    public Language Language
    {
        get => Facade.Settings.Value.View.Language;
        set => Facade.Settings.Value.View.Language = value;
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
        get => Facade.Settings.Value.Source;
        set => Facade.Settings.Value.Source = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Destination
    ///
    /// <summary>
    /// Gets or sets the path to save.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Destination
    {
        get => Facade.Settings.Value.Destination;
        set => Facade.Settings.Value.Destination = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// UserProgram
    ///
    /// <summary>
    /// Gets or sets the path of the user program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string UserProgram
    {
        get => Facade.Settings.Value.UserProgram;
        set => Facade.Settings.Value.UserProgram = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Title
    ///
    /// <summary>
    /// Gets the title of the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Title => Facade.Settings.GetTitle();

    /* --------------------------------------------------------------------- */
    ///
    /// Version
    ///
    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Version => Facade.Settings.Version.ToString(3, true);

    /* --------------------------------------------------------------------- */
    ///
    /// Uri
    ///
    /// <summary>
    /// Gets the URL of the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Uri Uri => Resource.ProductUri;

    /* --------------------------------------------------------------------- */
    ///
    /// Resolution
    ///
    /// <summary>
    /// Gets or sets the resolution.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Resolution
    {
        get => Facade.Settings.Value.Resolution;
        set => Facade.Settings.Value.Resolution = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsPdf
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the current format is
    /// PDF.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsPdf => Format == Format.Pdf;

    /* --------------------------------------------------------------------- */
    ///
    /// IsJpegEncoding
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to compress images
    /// embedded in the PDF.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsJpegEncoding
    {
        get => Facade.Settings.Value.Encoding == Encoding.Jpeg;
        set => Facade.Settings.Value.Encoding = value ? Encoding.Jpeg : Encoding.Flate;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsAutoOrientation
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the orientation is
    /// equal to auto.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsAutoOrientation
    {
        get => Facade.Settings.Value.Orientation == Orientation.Auto;
        set { if (value) Facade.Settings.Value.Orientation = Orientation.Auto; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsPortrait
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the orientation is
    /// equal to portrait.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsPortrait
    {
        get => Facade.Settings.Value.Orientation == Orientation.Portrait;
        set { if (value) Facade.Settings.Value.Orientation = Orientation.Portrait; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsLandscape
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the orientation is
    /// equal to landscape.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsLandscape
    {
        get => Facade.Settings.Value.Orientation == Orientation.Landscape;
        set { if (value) Facade.Settings.Value.Orientation = Orientation.Landscape; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsUserProgram
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the input form of the
    /// user program is editable.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsUserProgram => PostProcess == PostProcess.Others;

    /* --------------------------------------------------------------------- */
    ///
    /// Linearization
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to linearize the
    /// PDF (a.k.a PDF web optimization).
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Linearization
    {
        get => Facade.Settings.Value.Linearization;
        set => Facade.Settings.Value.Linearization = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CheckUpdate
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to enable the checking
    /// CubePDF updates.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CheckUpdate
    {
        get => Facade.Startup.Enabled;
        set => Facade.Startup.Enabled = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SourceVisible
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to display the input
    /// form of the source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool SourceVisible => Facade.Settings.Value.View.SourceVisible;

    /* --------------------------------------------------------------------- */
    ///
    /// SourceEditable
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the input form of
    /// the source file is editable.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool SourceEditable => !Facade.Settings.Value.DeleteSource;

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Saves the current user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Save() => Run(Facade.Save, true);

    /* --------------------------------------------------------------------- */
    ///
    /// Confirm
    ///
    /// <summary>
    /// Confirms if the current settings are acceptable.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Confirm()
    {
        if (!Io.Exists(Destination) || SaveOption == SaveOption.Rename) return true;

        var msg = Message.From(Destination, SaveOption);
        Send(msg);
        return !msg.Cancel;
    }

    #endregion
}
