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
using System;
using System.Linq;
using System.Threading;
using Cube.Mixin.Observing;
using Cube.Pdf.Converter.Mixin;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the MainWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainViewModel : PresentableBase<Facade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder src) : this(src, SynchronizationContext.Current) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder src, SynchronizationContext context) :
            base(new(src), new(12), context)
        {
            Locale.Set(src.Value.Language);

            General    = new(src, Aggregator, context);
            Metadata   = new(src.Value.Metadata, Aggregator, context);
            Encryption = new(src.Value.Encryption, Aggregator, context);

            Assets.Add(new ObservableProxy(Facade, this));
            Assets.Add(src.Subscribe(e => {
                switch (e)
                {
                    case nameof(src.Value.Format):
                        Facade.ChangeExtension();
                        break;
                    case nameof(src.Value.PostProcess):
                        if (src.Value.PostProcess == PostProcess.Others) SelectUserProgram();
                        break;
                    case nameof(src.Value.Language):
                        Locale.Set(src.Value.Language);
                        break;
                }
            }));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// General
        ///
        /// <summary>
        /// Gets the ViewModel object that represents General and Others
        /// tabs.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingViewModel General { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Metadata
        ///
        /// <summary>
        /// Gets the ViewModel object that represents a Metadata tab.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataViewModel Metadata { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encryption
        ///
        /// <summary>
        /// Gets the ViewModel object that represents an Encryption tab.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionViewModel Encryption { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title => Facade.Settings.GetTitle();

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => Facade.Settings.Version.ToString(3, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Uri
        ///
        /// <summary>
        /// Gets the URL of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri Uri => Resource.ProductUri;

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether it is busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy => Facade.Busy;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Executes the conversion.
        /// </summary>
        ///
        /// <remarks>
        /// The method will always post a CloseMessage message even if
        /// the InvokeEx method fails.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert()
        {
            if (Confirm()) Close(Facade.InvokeEx, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Metadata.Save(General.Save);

        /* ----------------------------------------------------------------- */
        ///
        /// Help
        ///
        /// <summary>
        /// Shows the help page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Help() => Send(Resource.DocumentUri);

        /* ----------------------------------------------------------------- */
        ///
        /// SelectSource
        ///
        /// <summary>
        /// Shows an OpenFileDialog dialog and set the selected path to
        /// the Source property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectSource() => Send(
            Message.ForSource(Facade.Settings),
            e => Facade.Settings.Value.Source = e.First(),
            true
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SelectDestination
        ///
        /// <summary>
        /// Shows an SaveFileDialog dialog and set the selected path to
        /// the Destination property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectDestination() => Send(
            Message.ForDestination(Facade.Settings),
            Facade.SetDestination,
            true
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SelectUserProgram
        ///
        /// <summary>
        /// Shows an OpenFileDialog dialog and set the selected path to
        /// the UserProgram property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectUserProgram() => Send(
            Message.ForUserProgram(Facade.Settings),
            e => General.UserProgram = e.First(),
            true
        );

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        protected override DialogMessage OnMessage(Exception src) =>
            src is OperationCanceledException ? null : Message.From(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Confirm
        ///
        /// <summary>
        /// Invokes the confirmation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool Confirm() => Encryption.Confirm() && General.Confirm();

        #endregion
    }
}
