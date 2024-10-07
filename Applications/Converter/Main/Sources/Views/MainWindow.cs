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
using System.ComponentModel;
using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Forms.Binding;
using Cube.Globalization;

/* ------------------------------------------------------------------------- */
///
/// MainWindow
///
/// <summary>
/// Represents the main windows of CubePDF.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public partial class MainWindow : Window
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public MainWindow() => InitializeComponent();

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Busy
    ///
    /// <summary>
    /// Gets a value indicating whether it is busy.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Busy
    {
        get => MainProgressBar.Visible;
        set
        {
            SettingTabControl.Enabled = !value;
            SettingButton.Visible     = !value;
            ConvertButton.Enabled     = !value;
            MainProgressBar.Visible   =  value;
            Cursor = value ? Cursors.WaitCursor : Cursors.Default;
        }
    }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// OnShown
    ///
    /// <summary>
    /// Occurs when the Shown event is fired.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Activate();

        TopMost = true;
        TopMost = false;

        static void scroll(TextBox s) {
            s.SelectionStart  = s.Text?.Length ?? 0;
            s.SelectionLength = 0;
        }

        scroll(DestinationTextBox);
        scroll(SourceTextBox);
        scroll(UserProgramTextBox);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OnBind
    ///
    /// <summary>
    /// Binds the specified object.
    /// </summary>
    ///
    /// <param name="src">Bindable object.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnBind(IBindable src)
    {
        if (src is not MainViewModel vm) return;

        BindCore(vm);
        BindTexts(vm); // i18n
        BindBehaviors(vm);

        ShortcutKeys.Add(Keys.F1, vm.Help);
    }

    #endregion

    #region Bindings

    /* --------------------------------------------------------------------- */
    ///
    /// BindCore
    ///
    /// <summary>
    /// Invokes the binding settings.
    /// </summary>
    ///
    /// <param name="vm">VM object to bind.</param>
    ///
    /* --------------------------------------------------------------------- */
    private void BindCore(MainViewModel vm)
    {
        var s0 = vm;
        var b0 = Behaviors.Hook(new BindingSource(s0, ""));
        b0.Bind(nameof(s0.Busy), this, nameof(Busy), true);

        // General and Misc tab
        var s1 = vm.Settings;
        var b1 = Behaviors.Hook(new BindingSource(s1, ""));
        b1.Bind(nameof(s1.Destination),         DestinationTextBox,     nameof(DestinationTextBox.Text));
        b1.Bind(nameof(s1.SaveOption),          SaveOptionComboBox,     nameof(SaveOptionComboBox.SelectedValue));
        b1.Bind(nameof(s1.Format),              FormatComboBox,         nameof(FormatComboBox.SelectedValue));
        b1.Bind(nameof(s1.IsPdf),               PdfVersionComboBox,     nameof(PdfVersionComboBox.Enabled), true);
        b1.Bind(nameof(s1.ColorMode),           ColorComboBox,          nameof(ColorComboBox.SelectedValue));
        b1.Bind(nameof(s1.Resolution),          ResolutionNumeric,      nameof(ResolutionNumeric.Value));
        b1.Bind(nameof(s1.IsPortrait),          PortraitRadioButton,    nameof(PortraitRadioButton.Checked));
        b1.Bind(nameof(s1.IsLandscape),         LandscapeRadioButton,   nameof(LandscapeRadioButton.Checked));
        b1.Bind(nameof(s1.IsAutoOrientation),   AutoRadioButton,        nameof(AutoRadioButton.Checked));
        b1.Bind(nameof(s1.IsJpegEncoding),      JpegCheckBox,           nameof(JpegCheckBox.Checked));
        b1.Bind(nameof(s1.IsPdf),               JpegCheckBox,           nameof(JpegCheckBox.Enabled), true);
        b1.Bind(nameof(s1.PostProcess),         PostProcessComboBox,    nameof(PostProcessComboBox.SelectedValue));
        b1.Bind(nameof(s1.IsUserProgram),       UserProgramPanel,       nameof(UserProgramPanel.Enabled), true);
        b1.Bind(nameof(s1.UserProgram),         UserProgramTextBox,     nameof(UserProgramTextBox.Text));
        b1.Bind(nameof(s1.SourceVisible),       SourceLabel,            nameof(SourceLabel.Visible), true);
        b1.Bind(nameof(s1.SourceVisible),       SourcePanel,            nameof(SourcePanel.Visible), true);
        b1.Bind(nameof(s1.SourceEditable),      SourcePanel,            nameof(SourcePanel.Enabled), true);
        b1.Bind(nameof(s1.Source),              SourceTextBox,          nameof(SourceTextBox.Text));
        b1.Bind(nameof(s1.CheckUpdate),         UpdateCheckBox,         nameof(UpdateCheckBox.Checked));
        b1.Bind(nameof(s1.Language),            LanguageComboBox,       nameof(LanguageComboBox.SelectedValue));
        b1.Bind(nameof(s1.Version),             VersionPanel,           nameof(VersionPanel.Version), true);
        b1.Bind(nameof(s1.Uri),                 VersionPanel,           nameof(VersionPanel.Uri), true);

        // Metadata
        var s2 = vm.Metadata;
        var b2 = Behaviors.Hook(new BindingSource(s2, ""));
        b2.Bind(nameof(s2.Version),  PdfVersionComboBox, nameof(PdfVersionComboBox.SelectedValue));
        b2.Bind(nameof(s2.Title),    TitleTextBox,       nameof(TitleTextBox.Text));
        b2.Bind(nameof(s2.Author),   AuthorTextBox,      nameof(AuthorTextBox.Text));
        b2.Bind(nameof(s2.Subject),  SubjectTextBox,     nameof(SubjectTextBox.Text));
        b2.Bind(nameof(s2.Keywords), KeywordsTextBox,    nameof(KeywordsTextBox.Text));
        b2.Bind(nameof(s2.Creator),  CreatorTextBox,     nameof(CreatorTextBox.Text));
        b2.Bind(nameof(s2.Options),  ViewOptionComboBox, nameof(ViewOptionComboBox.SelectedValue));

        // Security
        var s3 = vm.Security;
        var b3 = Behaviors.Hook(new BindingSource(s3, ""));
        b3.Bind(nameof(s3.Enabled),            SecurityInnerPanel,         nameof(SecurityInnerPanel.Enabled), true);
        b3.Bind(nameof(s3.Enabled),            OwnerPasswordCheckBox,      nameof(OwnerPasswordCheckBox.Checked));
        b3.Bind(nameof(s3.OwnerPassword),      OwnerPasswordTextBox,       nameof(OwnerPasswordTextBox.Text));
        b3.Bind(nameof(s3.OwnerConfirm),       OwnerConfirmTextBox,        nameof(OwnerConfirmTextBox.Text));
        b3.Bind(nameof(s3.OpenWithPassword),   UserPasswordCheckBox,       nameof(UserPasswordCheckBox.Checked));
        b3.Bind(nameof(s3.OpenWithPassword),   SharePasswordCheckBox,      nameof(SharePasswordCheckBox.Enabled), true);
        b3.Bind(nameof(s3.UserRequired),       UserPasswordPanel,          nameof(UserPasswordPanel.Enabled), true);
        b3.Bind(nameof(s3.UserPassword),       UserPasswordTextBox,        nameof(UserPasswordTextBox.Text));
        b3.Bind(nameof(s3.UserConfirm),        UserConfirmTextBox,         nameof(UserConfirmTextBox.Text));
        b3.Bind(nameof(s3.SharePassword),      SharePasswordCheckBox,      nameof(SharePasswordCheckBox.Checked));
        b3.Bind(nameof(s3.Permissible),        PermissionPanel,            nameof(PermissionPanel.Enabled), true);
        b3.Bind(nameof(s3.AllowPrint),         AllowPrintCheckBox,         nameof(AllowPrintCheckBox.Checked));
        b3.Bind(nameof(s3.AllowCopy),          AllowCopyCheckBox,          nameof(AllowCopyCheckBox.Checked));
        b3.Bind(nameof(s3.AllowModify),        AllowModifyCheckBox,        nameof(AllowModifyCheckBox.Checked));
        b3.Bind(nameof(s3.AllowAccessibility), AllowAccessibilityCheckBox, nameof(AllowAccessibilityCheckBox.Checked));
        b3.Bind(nameof(s3.AllowForm),          AllowFormCheckBox,          nameof(AllowFormCheckBox.Checked));
        b3.Bind(nameof(s3.AllowAnnotation),    AllowAnnotationCheckBox,    nameof(AllowAnnotationCheckBox.Checked));

        FormatComboBox.Bind(Surface.Formats);
        PdfVersionComboBox.Bind(Surface.PdfVersions);
        LanguageComboBox.Bind(Surface.Languages);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindTexts
    ///
    /// <summary>
    /// Sets the displayed texts and related settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void BindTexts(MainViewModel vm)
    {
        Text = vm.Settings.Title;
        PathLintToolTip.ToolTipTitle = Surface.Texts.Error_InvalidChars;
        PortraitRadioButton.Text = Surface.Texts.General_Portrait;

        // Buttons
        ConvertButton.Text = Surface.Texts.Menu_Convert;
        ExitButton.Text = Surface.Texts.Menu_Cancel;
        SettingButton.Text = Surface.Texts.Menu_Save;

        // Tabs
        GeneralTabPage.Text = Surface.Texts.General_Tab;
        MetadataTabPage.Text = Surface.Texts.Metadata_Tab;
        SecurityTabPage.Text = Surface.Texts.Security_Tab;
        MiscTabPage.Text = Surface.Texts.Misc_Tab;

        // Labels for General tab
        SourceLabel.Text = Surface.Texts.General_Source;
        DestinationLabel.Text = Surface.Texts.General_Destination;
        FormatLabel.Text = Surface.Texts.General_Format;
        ColorLabel.Text = Surface.Texts.General_Color;
        ResolutionLabel.Text = Surface.Texts.General_Resolution;
        OrientationLabel.Text = Surface.Texts.General_Orientation;
        OptionLabel.Text = Surface.Texts.General_Options;
        PostProcessLabel.Text = Surface.Texts.General_PostProcess;

        // Labels for Metadata tab
        TitleLabel.Text = Surface.Texts.Metadata_Title;
        AuthorLabel.Text = Surface.Texts.Metadata_Author;
        SubjectLabel.Text = Surface.Texts.Metadata_Subject;
        KeywordLabel.Text = Surface.Texts.Metadata_Keyword;
        CreatorLabel.Text = Surface.Texts.Metadata_Creator;
        ViewOptionLabel.Text = Surface.Texts.Metadata_Layout;

        // Labels for Security tab
        OwnerPasswordLabel.Text = Surface.Texts.Security_OwnerPassword;
        OwnerConfirmLabel.Text = Surface.Texts.Security_ConfirmPassword;
        UserPasswordLabel.Text = Surface.Texts.Security_UserPassword;
        UserConfirmLabel.Text = Surface.Texts.Security_ConfirmPassword;
        OperationLabel.Text = Surface.Texts.Security_Operations;

        // Labels for Misc tab
        AboutLabel.Text = Surface.Texts.Misc_About;
        LanguageLabel.Text = Surface.Texts.Misc_Language;

        // Menus for General tab (CheckBox, RadioButton, ...)
        PortraitRadioButton.Text = Surface.Texts.General_Portrait;
        LandscapeRadioButton.Text = Surface.Texts.General_Landscape;
        AutoRadioButton.Text = Surface.Texts.General_Auto;
        JpegCheckBox.Text = Surface.Texts.General_Jpeg;

        // Menus for Security tab (CheckBox, RadioButton, ...)
        OwnerPasswordCheckBox.Text = Surface.Texts.Security_Enable;
        UserPasswordCheckBox.Text = Surface.Texts.Security_OpenWithPassword;
        SharePasswordCheckBox.Text = Surface.Texts.Security_SharePassword;
        AllowPrintCheckBox.Text = Surface.Texts.Security_AllowPrint;
        AllowCopyCheckBox.Text = Surface.Texts.Security_AllowCopy;
        AllowModifyCheckBox.Text = Surface.Texts.Security_AllowModify;
        AllowAccessibilityCheckBox.Text = Surface.Texts.Security_AllowAccessibility;
        AllowFormCheckBox.Text = Surface.Texts.Security_AllowForm;
        AllowAnnotationCheckBox.Text = Surface.Texts.Security_AllowAnnotation;

        // Menus for Misc tab (CheckBox, RadioButton, ...)
        UpdateCheckBox.Text = Surface.Texts.Misc_CheckUpdate;

        // ComboBox items
        SaveOptionComboBox.Bind(Surface.SaveOptions);
        ViewOptionComboBox.Bind(Surface.ViewerOptions);
        PostProcessComboBox.Bind(Surface.PostProcesses);
        ColorComboBox.Bind(Surface.ColorModes);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindCore
    ///
    /// <summary>
    /// Invokes the binding settings about behaviors.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void BindBehaviors(MainViewModel vm)
    {
        Behaviors.Add(new ClickEventBehavior(ConvertButton, vm.Invoke));
        Behaviors.Add(new ClickEventBehavior(SourceButton, vm.SelectSource));
        Behaviors.Add(new ClickEventBehavior(DestinationButton, vm.SelectDestination));
        Behaviors.Add(new ClickEventBehavior(UserProgramButton, vm.SelectUserProgram));
        Behaviors.Add(new ClickEventBehavior(SettingButton, vm.Save));
        Behaviors.Add(new ClickEventBehavior(ExitButton, Close));
        Behaviors.Add(new EventBehavior(DestinationTextBox, nameof(LostFocus), vm.ChangeExtension));
        Behaviors.Add(new PathLintBehavior(SourceTextBox, PathLintToolTip));
        Behaviors.Add(new PathLintBehavior(DestinationTextBox, PathLintToolTip));
        Behaviors.Add(new PathLintBehavior(UserProgramTextBox, PathLintToolTip));
        Behaviors.Add(new PasswordLintBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox));
        Behaviors.Add(new PasswordLintBehavior(UserPasswordTextBox, UserConfirmTextBox));
        Behaviors.Add(new CloseBehavior(this, vm));
        Behaviors.Add(new DialogBehavior(vm));
        Behaviors.Add(new OpenFileBehavior(vm));
        Behaviors.Add(new SaveFileBehavior(vm));
        Behaviors.Add(new ProcessBehavior(vm));
        Behaviors.Add(Locale.Subscribe(_ => BindTexts(vm)));
    }

    #endregion
}
