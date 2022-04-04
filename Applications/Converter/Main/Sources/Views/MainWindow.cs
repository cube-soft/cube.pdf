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
using Cube.Forms;
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
    public partial class MainWindow : Window
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
        public MainWindow() => InitializeComponent();

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
            get => MainProgressBar.Visible;
            set
            {
                SettingTabControl.Enabled = !value;
                SettingButton.Visible     = !value;
                ExecButton.Enabled        = !value;
                MainProgressBar.Visible   =  value;
                Cursor = value ? Cursors.WaitCursor : Cursors.Default;
            }
        }

        #endregion

        #region Methods

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

            DestinationTextBox.SelectionStart  = DestinationTextBox.Text?.Length ?? 0;
            DestinationTextBox.SelectionLength = 0;
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

            BindCore(vm);

            Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Convert));
            Behaviors.Add(new ClickEventBehavior(SourceButton, vm.SelectSource));
            Behaviors.Add(new ClickEventBehavior(DestinationButton, vm.SelectDestination));
            Behaviors.Add(new ClickEventBehavior(UserProgramButton, vm.SelectUserProgram));
            Behaviors.Add(new ClickEventBehavior(SettingButton, vm.Save));
            Behaviors.Add(new ClickEventBehavior(ExitButton, Close));
            Behaviors.Add(new PathLintBehavior(SourceTextBox, PathLintToolTip));
            Behaviors.Add(new PathLintBehavior(DestinationTextBox, PathLintToolTip));
            Behaviors.Add(new PathLintBehavior(UserProgramTextBox, PathLintToolTip));
            Behaviors.Add(new PasswordLintBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox));
            Behaviors.Add(new PasswordLintBehavior(UserPasswordTextBox, UserConfirmTextBox));
            Behaviors.Add(new EventBehavior(DestinationTextBox, nameof(LostFocus), vm.ChangeExtension));
            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenFileBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new UriBehavior(vm));
            Behaviors.Add(Locale.Subscribe(_ => BindText(vm)));

            ShortcutKeys.Add(Keys.F1, vm.Help);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// BindCore
        ///
        /// <summary>
        /// Invokes the binding settings.
        /// </summary>
        ///
        /// <param name="vm">VM object to bind.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void BindCore(MainViewModel vm)
        {
            // General
            var s0 = vm;
            var b0 = Behaviors.Hook(new BindingSource(s0, ""));
            b0.Bind(nameof(s0.Busy),    this,         nameof(Busy), true);
            b0.Bind(nameof(s0.Version), VersionPanel, nameof(VersionPanel.Version), true);
            b0.Bind(nameof(s0.Uri),     VersionPanel, nameof(VersionPanel.Uri), true);

            // General and Settings tab
            var s1 = vm.Settings;
            var b1 = Behaviors.Hook(new BindingSource(s1, ""));
            b1.Bind(nameof(s1.Destination),         DestinationTextBox,    nameof(TextBox.Text));
            b1.Bind(nameof(s1.SaveOption),          SaveOptionComboBox,    nameof(ComboBox.SelectedValue));
            b1.Bind(nameof(s1.Format),              FormatComboBox,        nameof(ComboBox.SelectedValue));
            b1.Bind(nameof(s1.IsPdf),               PdfVersionComboBox,    nameof(Enabled), true);
            b1.Bind(nameof(s1.Resolution),          ResolutionNumeric,     nameof(NumericUpDown.Value));
            b1.Bind(nameof(s1.IsPortrait),          PortraitRadioButton,   nameof(RadioButton.Checked));
            b1.Bind(nameof(s1.IsLandscape),         LandscapeRadioButton,  nameof(RadioButton.Checked));
            b1.Bind(nameof(s1.IsAutoOrientation),   AutoRadioButton,       nameof(RadioButton.Checked));
            b1.Bind(nameof(s1.Grayscale),           GrayscaleCheckBox,     nameof(CheckBox.Checked));
            b1.Bind(nameof(s1.ImageFilter),         JpegCheckBox,          nameof(CheckBox.Checked));
            b1.Bind(nameof(s1.IsPdf),               JpegCheckBox,          nameof(Enabled), true);
            b1.Bind(nameof(s1.Linearization),       LinearizationCheckBox, nameof(CheckBox.Checked));
            b1.Bind(nameof(s1.IsPdf),               LinearizationCheckBox, nameof(Enabled), true);
            b1.Bind(nameof(s1.PostProcess),         PostProcessComboBox,   nameof(ComboBox.SelectedValue));
            b1.Bind(nameof(s1.IsUserProgram),       UserProgramPanel,      nameof(Enabled), true);
            b1.Bind(nameof(s1.UserProgram),         UserProgramTextBox,    nameof(TextBox.Text));
            b1.Bind(nameof(s1.SourceVisible),       SourceLabel,           nameof(Visible), true);
            b1.Bind(nameof(s1.SourceVisible),       SourcePanel,           nameof(Visible), true);
            b1.Bind(nameof(s1.SourceEditable),      SourcePanel,           nameof(Enabled), true);
            b1.Bind(nameof(s1.Source),              SourceTextBox,         nameof(TextBox.Text));
            b1.Bind(nameof(s1.CheckUpdate),         UpdateCheckBox,        nameof(CheckBox.Checked));
            b1.Bind(nameof(s1.Language),            LanguageComboBox,      nameof(ComboBox.SelectedValue));

            // Metadata
            var s2 = vm.Metadata;
            var b2 = Behaviors.Hook(new BindingSource(s2, ""));
            b2.Bind(nameof(s2.Version),  PdfVersionComboBox, nameof(ComboBox.SelectedValue));
            b2.Bind(nameof(s2.Title),    TitleTextBox,       nameof(TextBox.Text));
            b2.Bind(nameof(s2.Author),   AuthorTextBox,      nameof(TextBox.Text));
            b2.Bind(nameof(s2.Subject),  SubjectTextBox,     nameof(TextBox.Text));
            b2.Bind(nameof(s2.Keywords), KeywordsTextBox,    nameof(TextBox.Text));
            b2.Bind(nameof(s2.Creator),  CreatorTextBox,     nameof(TextBox.Text));
            b2.Bind(nameof(s2.Options),  ViewOptionComboBox, nameof(ComboBox.SelectedValue));

            // Encryption
            var s3 = vm.Encryption;
            var b3 = Behaviors.Hook(new BindingSource(s3, ""));
            b3.Bind(nameof(s3.Enabled),            EncryptionCheckBox,         nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.Enabled),            EncryptionPanel,            nameof(Enabled), true);
            b3.Bind(nameof(s3.OwnerPassword),      OwnerPasswordTextBox,       nameof(TextBox.Text));
            b3.Bind(nameof(s3.OwnerConfirm),       OwnerConfirmTextBox,        nameof(TextBox.Text));
            b3.Bind(nameof(s3.OpenWithPassword),   UserPasswordCheckBox,       nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.OpenWithPassword),   SharePasswordCheckBox,      nameof(Enabled), true);
            b3.Bind(nameof(s3.UserRequired),       UserPasswordPanel,          nameof(Enabled), true);
            b3.Bind(nameof(s3.UserPassword),       UserPasswordTextBox,        nameof(TextBox.Text));
            b3.Bind(nameof(s3.UserConfirm),        UserConfirmTextBox,         nameof(TextBox.Text));
            b3.Bind(nameof(s3.SharePassword),      SharePasswordCheckBox,      nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.Permissible),        PermissionPanel,            nameof(Enabled), true);
            b3.Bind(nameof(s3.AllowPrint),         AllowPrintCheckBox,         nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.AllowCopy),          AllowCopyCheckBox,          nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.AllowModify),        AllowModifyCheckBox,        nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.AllowAccessibility), AllowAccessibilityCheckBox, nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.AllowForm),          AllowFormCheckBox,          nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.AllowAnnotation),    AllowAnnotationCheckBox,    nameof(CheckBox.Checked));

            // Text (i18n)
            LanguageComboBox.Bind(Resource.Languages);
            BindText(vm);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BindText
        ///
        /// <summary>
        /// Sets the displayed text with the specified language.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void BindText(MainViewModel vm)
        {
            this.UpdateCulture(vm.Settings.Language);
            Resource.UpdateCulture(vm.Settings.Language);

            Text = vm.Title;
            PathLintToolTip.ToolTipTitle = Properties.Resources.MessageInvalidChars;

            FormatComboBox.Bind(Resource.Formats);
            PdfVersionComboBox.Bind(Resource.PdfVersions);
            SaveOptionComboBox.Bind(Resource.SaveOptions);
            ViewOptionComboBox.Bind(Resource.ViewerOptions);
            PostProcessComboBox.Bind(Resource.PostProcesses);
        }

        #endregion
    }
}
