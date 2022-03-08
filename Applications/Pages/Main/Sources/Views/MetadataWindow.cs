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
using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Mixin.Forms;
using Cube.Mixin.Forms.Controls;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// Represents the metadata and encryption window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MetadataWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataWindow
        ///
        /// <summary>
        /// Initializes a new instance of the MetadataWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MetadataWindow() => InitializeComponent();

        #endregion

        #region Methods

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
            if (src is not MetadataViewModel vm) return;

            BindCore(vm);

            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new DialogBehavior(vm));
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

            // Text (i18n)
            VersionComboBox.Bind(Resource.PdfVersions);
            ViewOptionComboBox.Bind(Resource.ViewerOptions);
        }

        #endregion
    }
}
