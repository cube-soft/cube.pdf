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
            Behaviors.Add(Locale.Subscribe(_ => BindText(vm)));
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
        private void BindText(MetadataViewModel vm)
        {
            VersionComboBox.Bind(Resource.PdfVersions);
            LayoutComboBox.Bind(Resource.ViewerOptions);
        }

        #endregion
    }
}
