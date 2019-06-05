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
    /// Represents the main windows of CubePDF.
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
        /// Initializes a new instance of the MainWindow class.
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
            SettingPanel.ApplyButton = ApplyButton;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether it is busy.
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
                SettingTabControl.Enabled = !value;
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
        /// Binds the specified object.
        /// </summary>
        ///
        /// <param name="src">ViewModel object.</param>
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
            SettingBindingSource.DataSource    = vm.General;
            MetadataBindingSource.DataSource   = vm.Metadata;
            EncryptionBindingSource.DataSource = vm.Encryption;

            // see remarks
            SourceLabel.DataBindings.Add("Visible", SettingBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            SourcePanel.DataBindings.Add("Visible", SettingBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            DataBindings.Add("Text", MainBindingSource, "Title", false, DataSourceUpdateMode.Never);
            DataBindings.Add("Busy", MainBindingSource, "Busy", false, DataSourceUpdateMode.OnPropertyChanged);

            SourceButton.Click      += (s, e) => vm.SelectSource();
            DestinationButton.Click += (s, e) => vm.SelectDestination();
            UserProgramButton.Click += (s, e) => vm.SelectUserProgram();
            ConvertButton.Click     += (s, e) => vm.Convert();
            SettingPanel.Apply      += (s, e) => vm.Save();

            Behaviors.Add(new CloseBehavior(src, this));
            Behaviors.Add(new DialogBehavior(src));
            Behaviors.Add(new OpenFileBehavior(src));
            Behaviors.Add(new SaveFileBehavior(src));

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
        /// Updates displayed string.
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
