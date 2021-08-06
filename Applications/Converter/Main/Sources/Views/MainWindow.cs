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

            Behaviors.Add(Locale.Subscribe(SetText));
            Behaviors.Add(new ClickBehavior(ExitButton, Close));
            Behaviors.Add(new PathLintBehavior(SourceTextBox, PathToolTip));
            Behaviors.Add(new PathLintBehavior(DestinationTextBox, PathToolTip));
            Behaviors.Add(new PathLintBehavior(UserProgramTextBox, PathToolTip));
            Behaviors.Add(new PasswordBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox));
            Behaviors.Add(new PasswordBehavior(UserPasswordTextBox, UserConfirmTextBox));

            SettingPanel.ApplyButton = ApplyButton;

            // Manual bindings.
            _ = DataBindings.Add("Busy", MainBindingSource, "Busy", false, DataSourceUpdateMode.OnPropertyChanged);
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
            get => ConvertProgressBar.Visible;
            set
            {
                SettingTabControl.Enabled  = !value;
                ApplyButton.Visible        = !value;
                ConvertButton.Enabled      = !value;
                ConvertProgressBar.Visible =  value;
                Cursor = value ? Cursors.WaitCursor : Cursors.Default;
            }
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
        /// OnBind
        ///
        /// <summary>
        /// Binds the specified object.
        /// </summary>
        ///
        /// <param name="src">Bindable object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            if (src is not MainViewModel vm) return;

            MainBindingSource.DataSource       = vm;
            SettingBindingSource.DataSource    = vm.General;
            MetadataBindingSource.DataSource   = vm.Metadata;
            EncryptionBindingSource.DataSource = vm.Encryption;

            Behaviors.Add(new ClickBehavior(ConvertButton, vm.Convert));
            Behaviors.Add(new ClickBehavior(SourceButton, vm.SelectSource));
            Behaviors.Add(new ClickBehavior(DestinationButton, vm.SelectDestination));
            Behaviors.Add(new ClickBehavior(UserProgramButton, vm.SelectUserProgram));
            Behaviors.Add(new EventBehavior(SettingPanel, nameof(SettingPanel.Apply), vm.Save));
            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenFileBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new UriBehavior(vm));

            ShortcutKeys.Add(Keys.F1, vm.Help);

            SourceLabel.Visible = vm.General.SourceVisible;
            SourcePanel.Visible = vm.General.SourceVisible;
            SetText(vm.General.Language);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetText
        ///
        /// <summary>
        /// Sets the displayed text with the specified language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetText(Language e)
        {
            this.UpdateCulture(e);
            Properties.Resources.Culture = e.ToCultureInfo();

            PathToolTip.ToolTipTitle = Properties.Resources.MessageInvalidChars;
            MainToolTip.SetToolTip(SharePasswordCheckBox, Properties.Resources.MessageSecurity.WordWrap(40));
            MainToolTip.SetToolTip(LinearizationCheckBox, Properties.Resources.MessageLinearization.WordWrap(40));

            FormatComboBox.Bind(ViewResource.Formats);
            PdfVersionComboBox.Bind(ViewResource.PdfVersions);
            SaveOptionComboBox.Bind(ViewResource.SaveOptions);
            ViewerPreferencesComboBox.Bind(ViewResource.ViewerOptions);
            PostProcessComboBox.Bind(ViewResource.PostProcesses);
            LanguageComboBox.Bind(ViewResource.Languages);

            MainBindingSource.ResetBindings(false);
            SettingBindingSource.ResetBindings(false);
            MetadataBindingSource.ResetBindings(false);
            EncryptionBindingSource.ResetBindings(false);
        }

        #endregion
    }
}
