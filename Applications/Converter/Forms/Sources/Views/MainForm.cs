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
using Cube.Forms.Behaviors;
using Cube.Forms.Controls;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainForm
    ///
    /// <summary>
    /// CubePDF メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.StandardForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) => Close();

            new PathBehavior(SourceTextBox, PathToolTip);
            new PathBehavior(DestinationTextBox, PathToolTip);
            new PathBehavior(UserProgramTextBox, PathToolTip);
            new PasswordBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox);
            new PasswordBehavior(UserPasswordTextBox, UserConfirmTextBox);

            Locale.Subscribe(e => UpdateString(e));
            SettingsPanel.ApplyButton = ApplyButton;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IsBusy
        ///
        /// <summary>
        /// 実行中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBusy
        {
            get => _busy;
            set
            {
                _busy = value;
                ConvertButton.Enabled = !value;
                SettingsTabControl.Enabled = !value;
                ApplyButton.Visible = !value;
                ConvertProgressBar.Visible = value;
                Cursor = value ? Cursors.WaitCursor : Cursors.Default;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// オブジェクトを関連付けます。
        /// </summary>
        ///
        /// <param name="vm">ViewModel オブジェクト</param>
        ///
        /// <remarks>
        /// MainForm.Text および各種コントロールの Visible プロパティに
        /// 対して、デザイナから Binding を設定すると意図しない動作に
        /// なる現象が確認されています。暫定的な回避策と Binding を手動
        /// 設定する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Bind(MainViewModel vm)
        {
            if (vm == null) return;

            MainBindingSource.DataSource       = vm;
            SettingsBindingSource.DataSource   = vm.Settings;
            MetadataBindingSource.DataSource   = vm.Metadata;
            EncryptionBindingSource.DataSource = vm.Encryption;

            // see remarks
            SourceLabel.DataBindings.Add("Visible", SettingsBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            SourcePanel.DataBindings.Add("Visible", SettingsBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            DataBindings.Add("Text",   MainBindingSource, "Title",  false, DataSourceUpdateMode.Never);
            DataBindings.Add("IsBusy", MainBindingSource, "IsBusy", false, DataSourceUpdateMode.OnPropertyChanged);

            SourceButton.Click      += (s, e) => vm.BrowseSource();
            DestinationButton.Click += (s, e) => vm.BrowseDestination();
            UserProgramButton.Click += (s, e) => vm.BrowseUserProgram();
            ConvertButton.Click     += (s, e) => vm.Convert();
            SettingsPanel.Apply     += (s, e) => vm.Save();

            vm.Messenger.Close.Subscribe(() => Close());
            vm.Messenger.MessageBox.Subscribe(e => new MessageBoxBehavior().Invoke(e));
            vm.Messenger.OpenFileDialog.Subscribe(e => new OpenFileBehavior().Invoke(e));
            vm.Messenger.SaveFileDialog.Subscribe(e => new SaveFileBehavior().Invoke(e));

            UpdateString(vm.Settings.Language);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// Occurs when the Shown event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Activate();
            TopMost = true;
            TopMost = false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateString
        ///
        /// <summary>
        /// 表示言語に関わる文字列を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateString(Language value)
        {
            this.UpdateCulture(value);

            PathToolTip.ToolTipTitle = Properties.Resources.MessageInvalidChars;
            MainToolTip.SetToolTip(SharePasswordCheckBox, Properties.Resources.MessageSecurity.WordWrap(40));
            MainToolTip.SetToolTip(LinearizationCheckBox, Properties.Resources.MessageLinearization.WordWrap(40));

            FormatComboBox.Bind(ViewResource.Formats);
            FormatOptionComboBox.Bind(ViewResource.FormatOptions);
            SaveOptionComboBox.Bind(ViewResource.SaveOptions);
            ViewerPreferencesComboBox.Bind(ViewResource.ViewerOptions);
            PostProcessComboBox.Bind(ViewResource.PostProcesses);
            LanguageComboBox.Bind(ViewResource.Languages);
        }

        #endregion

        #region Fields
        private bool _busy = false;
        #endregion
    }
}
