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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cube.Mixin.Observable;
using Cube.Pdf.Converter.Mixin;

/* ------------------------------------------------------------------------- */
///
/// MainViewModel
///
/// <summary>
/// Represents the ViewModel for the MainWindow.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class MainViewModel : PresentableBase<Facade>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the MainViewModel class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public MainViewModel(SettingFolder src) : this(src, SynchronizationContext.Current) { }

    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the MainViewModel class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    /// <param name="ctx">Synchronization context.</param>
    ///
    /* --------------------------------------------------------------------- */
    public MainViewModel(SettingFolder src, SynchronizationContext ctx) :
        base(new(src), new(12), ctx)
    {
        Locale.Set(src.Value.Appendix.Language);

        Settings   = new(src, Aggregator, ctx);
        Metadata   = new(src.Value.Metadata, Aggregator, ctx);
        Encryption = new(src.Value.Encryption, Aggregator, ctx);

        void select_if() { if (src.Value.PostProcess == PostProcess.Others) SelectUserProgram(); }

        Assets.Add(Facade.Forward(this));
        Assets.Add(src.Subscribe(new() {
            { nameof(SettingValue.Format),      _ => ChangeExtension() },
            { nameof(SettingValue.PostProcess), _ => select_if() },
        }, default));
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Settings
    ///
    /// <summary>
    /// Gets the ViewModel object that represents General and Others
    /// tabs.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SettingViewModel Settings { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Metadata
    ///
    /// <summary>
    /// Gets the ViewModel object that represents a Metadata tab.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public MetadataViewModel Metadata { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Encryption
    ///
    /// <summary>
    /// Gets the ViewModel object that represents an Encryption tab.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public EncryptionViewModel Encryption { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Results
    ///
    /// <summary>
    /// Gets the collection of created files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> Results => Facade.Results;

    /* --------------------------------------------------------------------- */
    ///
    /// Busy
    ///
    /// <summary>
    /// Gets a value indicating whether it is busy.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Busy => Facade.Busy;

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the main operation.
    /// </summary>
    ///
    /// <remarks>
    /// The method will always post a CloseMessage message even if
    /// the InvokeEx method fails.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke()
    {
        var ready = Encryption.Confirm() && Settings.Confirm();
        if (ready) Quit(Facade.InvokeEx, false);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Saves the current user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Save() { if (Metadata.Confirm()) Settings.Save(); }

    /* --------------------------------------------------------------------- */
    ///
    /// Help
    ///
    /// <summary>
    /// Shows the help page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Help() => Send(new ProcessMessage(Resource.DocumentUri.ToString()));

    /* --------------------------------------------------------------------- */
    ///
    /// SelectSource
    ///
    /// <summary>
    /// Shows an OpenFileDialog dialog and set the selected path to
    /// the Source property.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void SelectSource() => Send(
        Message.ForSource(Facade.Settings),
        e => Facade.Settings.Value.Source = e.First(),
        true
    );

    /* --------------------------------------------------------------------- */
    ///
    /// SelectDestination
    ///
    /// <summary>
    /// Shows an SaveFileDialog dialog and set the selected path to
    /// the Destination property.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void SelectDestination() => Send(
        Message.ForDestination(Facade.Settings),
        Facade.SetDestination,
        true
    );

    /* --------------------------------------------------------------------- */
    ///
    /// SelectUserProgram
    ///
    /// <summary>
    /// Shows an OpenFileDialog dialog and set the selected path to
    /// the UserProgram property.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void SelectUserProgram() => Send(
        Message.ForUserProgram(Facade.Settings),
        e => Settings.UserProgram = e.First(),
        true
    );

    /* --------------------------------------------------------------------- */
    ///
    /// ChangeExtension
    ///
    /// <summary>
    /// Changes the extension of the destination path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void ChangeExtension() => Facade.ChangeExtension();

    /* --------------------------------------------------------------------- */
    ///
    /// OnMessage
    ///
    /// <summary>
    /// Converts the specified exception to a new instance of the
    /// DialogMessage class.
    /// </summary>
    ///
    /// <param name="src">Source exception.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /// <remarks>
    /// The Method is called from the Track methods.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    protected override DialogMessage OnMessage(Exception src) =>
        src is OperationCanceledException ? null : Message.From(src);

    #endregion
}
