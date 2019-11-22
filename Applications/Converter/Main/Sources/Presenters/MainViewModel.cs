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
using Cube.Mixin.Logging;
using Cube.Mixin.Tasks;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
    public sealed class MainViewModel : Presentable<Facade>
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
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder settings) :
            this(settings, SynchronizationContext.Current) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder settings, SynchronizationContext context) :
            base(new Facade(settings), new Aggregator(), context)
        {
            Locale.Set(settings.Value.Language);

            General    = new SettingViewModel(settings, Aggregator, context);
            Metadata   = new MetadataViewModel(settings.Value.Metadata, Aggregator, context);
            Encryption = new EncryptionViewModel(settings.Value.Encryption, Aggregator, context);

            Facade.Settings.PropertyChanged += Observe;
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
        public string Version => Facade.Settings.Version.ToString(true);

        /* ----------------------------------------------------------------- */
        ///
        /// Uri
        ///
        /// <summary>
        /// Gets the URL of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Uri Uri => ApplicationSetting.Uri;

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether it is busy.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy => Facade.Settings.Value.Busy;

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
        /* ----------------------------------------------------------------- */
        public void Convert()
        {
            var ok = Encryption.Confirm() && General.Confirm();
            if (ok) TaskEx.Run(() =>
            {
                try { Facade.InvokeEx(); }
                catch (OperationCanceledException) { /* ignore */ }
                catch (Exception e)
                {
                    this.LogError(e);
                    Send(MessageFactory.Create(e));
                }
                finally { Post<CloseMessage>(); }
            }).Forget();
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
        public void Save() => Metadata.Save(Facade.Settings.Save);

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
            Facade.Settings.CreateForSource(),
            e => Facade.Settings.Value.Source = e.First()
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
        public void SelectDestination()
        {
            var src = Facade.Settings.CreateForDestination();
            Send(src, e => Facade.SetDestination(src));
        }

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
            Facade.Settings.CreateForUserProgram(),
            e => Facade.Settings.Value.UserProgram = e.First()
        );

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Observe
        ///
        /// <summary>
        /// Occurs when any settings are changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Observe(object s, PropertyChangedEventArgs e)
        {
            var src = Facade.Settings.Value;

            switch (e.PropertyName)
            {
                case nameof(src.Format):
                    Facade.ChangeExtension();
                    break;
                case nameof(src.PostProcess):
                    if (src.PostProcess == PostProcess.Others) SelectUserProgram();
                    break;
                case nameof(src.Language):
                    Locale.Set(src.Language);
                    break;
                case nameof(src.Busy):
                    OnPropertyChanged(e);
                    break;
            }
        }

        #endregion
    }
}
