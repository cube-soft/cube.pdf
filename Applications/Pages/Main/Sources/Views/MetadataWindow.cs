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
namespace Cube.Pdf.Pages;

using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Forms.Binding;

/* ------------------------------------------------------------------------- */
///
/// MetadataWindow
///
/// <summary>
/// Represents the metadata and encryption window.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public partial class MetadataWindow : Window
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// MetadataWindow
    ///
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public MetadataWindow() => InitializeComponent();

    #endregion

    #region Bindings

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
        if (src is not MetadataViewModel vm) return;

        BindCore(vm);
        BindTexts(vm);
        BindBehaviors(vm);
    }

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
    private void BindCore(MetadataViewModel vm)
    {
        // Metadata
        var s0 = vm;
        var b0 = Behaviors.Hook(new BindingSource(s0, ""));
        b0.Bind(nameof(s0.Version),  VersionComboBox,    nameof(ComboBox.SelectedValue));
        b0.Bind(nameof(s0.Title),    TitleTextBox,       nameof(TextBox.Text));
        b0.Bind(nameof(s0.Author),   AuthorTextBox,      nameof(TextBox.Text));
        b0.Bind(nameof(s0.Subject),  SubjectTextBox,     nameof(TextBox.Text));
        b0.Bind(nameof(s0.Keywords), KeywordTextBox,     nameof(TextBox.Text));
        b0.Bind(nameof(s0.Creator),  CreatorTextBox,     nameof(TextBox.Text));
        b0.Bind(nameof(s0.Options),  ViewOptionComboBox, nameof(ComboBox.SelectedValue));

        // Encryption
        var s1 = vm.Encryption;
        var b1 = Behaviors.Hook(new BindingSource(s1, ""));
        b1.Bind(nameof(s1.Enabled),            OwnerPasswordCheckBox,      nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.Enabled),            EncryptionSubPanel,         nameof(Enabled), true);
        b1.Bind(nameof(s1.OwnerPassword),      OwnerPasswordTextBox,       nameof(TextBox.Text));
        b1.Bind(nameof(s1.OwnerConfirm),       OwnerConfirmTextBox,        nameof(TextBox.Text));
        b1.Bind(nameof(s1.OpenWithPassword),   UserPasswordCheckBox,       nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.OpenWithPassword),   SharePasswordCheckBox,      nameof(Enabled), true);
        b1.Bind(nameof(s1.UserRequired),       UserPasswordPanel,          nameof(Enabled), true);
        b1.Bind(nameof(s1.UserPassword),       UserPasswordTextBox,        nameof(TextBox.Text));
        b1.Bind(nameof(s1.UserConfirm),        UserConfirmTextBox,         nameof(TextBox.Text));
        b1.Bind(nameof(s1.SharePassword),      SharePasswordCheckBox,      nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.Permissible),        PermissionPanel,            nameof(Enabled), true);
        b1.Bind(nameof(s1.AllowPrint),         AllowPrintCheckBox,         nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.AllowCopy),          AllowCopyCheckBox,          nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.AllowModify),        AllowModifyCheckBox,        nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.AllowAccessibility), AllowAccessibilityCheckBox, nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.AllowForm),          AllowFormCheckBox,          nameof(CheckBox.Checked));
        b1.Bind(nameof(s1.AllowAnnotation),    AllowAnnotationCheckBox,    nameof(CheckBox.Checked));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindTexts
    ///
    /// <summary>
    /// Sets the displayed text with the specified language.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void BindTexts(MetadataViewModel _)
    {
        Text = Surface.Texts.Metadata_Window;
        ExecButton.Text = Surface.Texts.Menu_Ok;
        ExitButton.Text = Surface.Texts.Menu_Cancel;

        // Labels for Metadata tab
        MetadataTabPage.Text = Surface.Texts.Metadata_Tab;
        TitleLabel.Text = Surface.Texts.Metadata_Title;
        AuthorLabel.Text = Surface.Texts.Metadata_Author;
        SubjectLabel.Text = Surface.Texts.Metadata_Subject;
        KeywordLabel.Text = Surface.Texts.Metadata_Keyword;
        CreatorLabel.Text = Surface.Texts.Metadata_Creator;
        ViewOptionLabel.Text = Surface.Texts.Metadata_Layout;
        VersionLabel.Text = Surface.Texts.Metadata_Version;

        // Menus for Metadata tab (CheckBox, RadioButton, ...)
        VersionComboBox.Bind(Surface.PdfVersions);
        ViewOptionComboBox.Bind(Surface.ViewerOptions);

        // Labels for Security tab
        EncryptionTabPage.Text = Surface.Texts.Security_Tab;
        OwnerPasswordLabel.Text = Surface.Texts.Security_OwnerPassword;
        OwnerConfirmLabel.Text = Surface.Texts.Security_ConfirmPassword;
        UserPasswordLabel.Text = Surface.Texts.Security_UserPassword;
        UserConfirmLabel.Text = Surface.Texts.Security_ConfirmPassword;
        OperationLabel.Text = Surface.Texts.Security_Operations;

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
    }

    /* --------------------------------------------------------------------- */
    ///
    /// BindBehaviors
    ///
    /// <summary>
    /// Invokes the binding settings about behaviors.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void BindBehaviors(MetadataViewModel vm)
    {
        Behaviors.Add(new ClickEventBehavior(ExecButton, vm.Apply));
        Behaviors.Add(new CloseBehavior(this, vm));
        Behaviors.Add(new DialogBehavior(vm));
        Behaviors.Add(new PasswordLintBehavior(OwnerPasswordTextBox, OwnerConfirmTextBox));
        Behaviors.Add(new PasswordLintBehavior(UserPasswordTextBox, UserConfirmTextBox));
    }

    #endregion
}