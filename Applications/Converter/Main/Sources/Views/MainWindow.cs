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

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// CubePDF メイン画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : Cube.Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainWindow
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainWindow()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) => Close();

            Behaviors.Add(new PathBehavior(SourceTextBox, PathToolTip));
            Behaviors.Add(new PathBehavior(DestinationTextBox, PathToolTip));
            Behaviors.Add(new PathBehavior(UserProgramTextBox, PathToolTip));
            Behaviors.Add(new PasswordBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox));
            Behaviors.Add(new PasswordBehavior(UserPasswordTextBox, UserConfirmTextBox));

            Locale.Subscribe(e => UpdateString(e));
            SettingsPanel.ApplyButton = ApplyButton;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// 実行中かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Busy
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
        /// <param name="src">ViewModel オブジェクト</param>
        ///
        /// <remarks>
        /// MainForm.Text および各種コントロールの Visible プロパティに
        /// 対して、デザイナから Binding を設定すると意図しない動作に
        /// なる現象が確認されています。暫定的な回避策と Binding を手動
        /// 設定する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public override void Bind(IPresentable src)
        {
            base.Bind(src);
            if (!(src is MainViewModel vm)) return;

            MainBindingSource.DataSource       = vm;
            SettingsBindingSource.DataSource   = vm.General;
            MetadataBindingSource.DataSource   = vm.Metadata;
            EncryptionBindingSource.DataSource = vm.Encryption;

            // see remarks
            SourceLabel.DataBindings.Add("Visible", SettingsBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            SourcePanel.DataBindings.Add("Visible", SettingsBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            DataBindings.Add("Text", MainBindingSource, "Title", false, DataSourceUpdateMode.Never);
            DataBindings.Add("Busy", MainBindingSource, "Busy", false, DataSourceUpdateMode.OnPropertyChanged);

            SourceButton.Click      += (s, e) => vm.BrowseSource();
            DestinationButton.Click += (s, e) => vm.BrowseDestination();
            UserProgramButton.Click += (s, e) => vm.BrowseUserProgram();
            ConvertButton.Click     += (s, e) => vm.Convert();
            SettingsPanel.Apply     += (s, e) => vm.Save();

            Behaviors.Add(new CloseBehavior(src, this));
            Behaviors.Add(new DialogBehavior(src));
            Behaviors.Add(new OpenFileDialogBehavior(src));
            Behaviors.Add(new SaveFileDialogBehavior(src));

            UpdateString(vm.General.Language);
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

            FormatComboBox.Bind(ViewResources.Formats);
            PdfVersionComboBox.Bind(ViewResources.PdfVersions);
            SaveOptionComboBox.Bind(ViewResources.SaveOptions);
            ViewerPreferencesComboBox.Bind(ViewResources.ViewerOptions);
            PostProcessComboBox.Bind(ViewResources.PostProcesses);
            LanguageComboBox.Bind(ViewResources.Languages);
        }

        #endregion

        #region Fields
        private bool _busy = false;
        #endregion
    }
}
