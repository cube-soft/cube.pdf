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
using Cube.Mixin.Assembly;
using Cube.Mixin.String;
using System;
using System.ComponentModel;
using System.Threading;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// SettingFolder とメイン画面を関連付ける ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainViewModel : CommonViewModel
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
        /// <param name="setting">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder setting) :
            this(setting, SynchronizationContext.Current) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="setting">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder setting, SynchronizationContext context) :
            base(new Aggregator(), context)
        {
            Locale.Set(setting.Value.Language);

            General    = new SettingViewModel(setting, Aggregator, context);
            Metadata   = new MetadataViewModel(setting.Value.Metadata, Aggregator, context);
            Encryption = new EncryptionViewModel(setting.Value.Encryption, Aggregator, context);

            _model = new Facade(setting);
            _model.Setting.PropertyChanged += Observe;
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
        public string Title =>
            _model.Setting.DocumentName.Source.HasValue() ?
            $"{_model.Setting.DocumentName.Source} - {Product} {Version}" :
            $"{Product} {Version}";

        /* ----------------------------------------------------------------- */
        ///
        /// Product
        ///
        /// <summary>
        /// Gets the product name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Product => _model.Setting.Assembly.GetProduct();

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => _model.Setting.Version.ToString(true);

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
        public bool Busy => _model.Setting.Value.Busy;

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
            if (Encryption.Confirm() && General.Confirm()) TrackClose(() => _model.InvokeEx());
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
        public void Save()
        {
            if (Metadata.ConfirmWhenSave()) _model.Setting.Save();
        }

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
        public void SelectSource() =>
            Send(_model.Setting.CreateForSource(), e => _model.SetSource(e));

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
        public void SelectDestination() =>
            Send(_model.Setting.CreateForDestination(), e => _model.SetDestination(e));

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
        public void SelectUserProgram() =>
            Send(_model.Setting.CreateForUserProgram(), e => _model.SetUserProgram(e));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            try { if (disposing) _model.Dispose(); }
            finally { base.Dispose(disposing); }
        }

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
            var value = _model.Setting.Value;

            switch (e.PropertyName)
            {
                case nameof(value.Format):
                    _model.ChangeExtension();
                    break;
                case nameof(value.PostProcess):
                    if (value.PostProcess == PostProcess.Others) SelectUserProgram();
                    break;
                case nameof(value.Language):
                    Locale.Set(value.Language);
                    break;
                case nameof(value.Busy):
                    OnPropertyChanged(e);
                    break;
            }
        }

        #endregion

        #region Fields
        private readonly Facade _model;
        #endregion
    }
}
