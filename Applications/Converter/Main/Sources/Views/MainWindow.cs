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
using System.ComponentModel;
using System.Windows.Forms;
using Cube.Forms.Behaviors;
using Cube.Mixin.Forms;
using Cube.Mixin.Forms.Controls;

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

            Behaviors.Add(new PathLintBehavior(SourceTextBox, PathToolTip));
            Behaviors.Add(new PathLintBehavior(DestinationTextBox, PathToolTip));
            Behaviors.Add(new PathLintBehavior(UserProgramTextBox, PathToolTip));
            Behaviors.Add(new PasswordBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox));
            Behaviors.Add(new PasswordBehavior(UserPasswordTextBox, UserConfirmTextBox));
            Behaviors.Add(Locale.Subscribe(_ => UpdateString()));

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
        protected override void OnBind(IBindable src)
        {
            if (src is not MainViewModel vm) return;

            MainBindingSource.DataSource       = vm;
            SettingBindingSource.DataSource    = vm.General;
            MetadataBindingSource.DataSource   = vm.Metadata;
            EncryptionBindingSource.DataSource = vm.Encryption;

            // see remarks
            _ = SourceLabel.DataBindings.Add("Visible", SettingBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            _ = SourcePanel.DataBindings.Add("Visible", SettingBindingSource, "SourceVisible", false, DataSourceUpdateMode.Never);
            _ = DataBindings.Add("Text", MainBindingSource, "Title", false, DataSourceUpdateMode.Never);
            _ = DataBindings.Add("Busy", MainBindingSource, "Busy", false, DataSourceUpdateMode.OnPropertyChanged);

            SourceButton.Click      += (s, e) => vm.SelectSource();
            DestinationButton.Click += (s, e) => vm.SelectDestination();
            UserProgramButton.Click += (s, e) => vm.SelectUserProgram();
            ConvertButton.Click     += (s, e) => vm.Convert();
            SettingPanel.Apply      += (s, e) => vm.Save();

            ShortcutKeys.Add(Keys.F1, vm.Help);

            Behaviors.Add(new CloseBehavior(this, src));
            Behaviors.Add(new DialogBehavior(src));
            Behaviors.Add(new OpenFileBehavior(src));
            Behaviors.Add(new SaveFileBehavior(src));

            this.UpdateCulture(vm.General.Language);
            UpdateString();
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
        private void UpdateString()
        {
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
