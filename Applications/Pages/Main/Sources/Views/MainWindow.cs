/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2013 CubeSoft, Inc.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Forms.Binding;
using Cube.Mixin.Forms;
using Cube.Mixin.Syntax;

namespace Cube.Pdf.Pages
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// Represents the main window.
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
        /// SelectedIndices
        ///
        /// <summary>
        /// Gets the collection of selected indices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<int> SelectedIndices => MainGridView
            .SelectedRows
            .Cast<DataGridViewRow>()
            .Select(e => e.Index)
            .ToArray();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnBind
        ///
        /// <summary>
        /// Invokes the binding to the specified object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            base.OnBind(src);
            if (src is not MainViewModel vm) return;

            var bs = Behaviors.Hook(new BindingSource(vm, ""));
            bs.Bind(nameof(vm.Ready), MergeButton, nameof(Enabled), true);
            bs.Bind(nameof(vm.Ready), SplitButton, nameof(Enabled), true);
            bs.Bind(nameof(vm.Ready), MetadataButton, nameof(Enabled), true);

            Behaviors.Add(new ShownEventBehavior(this, vm.Setup));
            Behaviors.Add(new ClickEventBehavior(MergeButton, vm.Merge));
            Behaviors.Add(new ClickEventBehavior(SplitButton, vm.Split));
            Behaviors.Add(new ClickEventBehavior(MetadataButton, vm.Metadata));
            Behaviors.Add(new ClickEventBehavior(ExitButton, Close));
            Behaviors.Add(new ClickEventBehavior(AddButton, vm.Add));
            Behaviors.Add(new ClickEventBehavior(UpButton, () => vm.Move(SelectedIndices, -1)));
            Behaviors.Add(new ClickEventBehavior(DownButton, () => vm.Move(SelectedIndices, 1)));
            Behaviors.Add(new ClickEventBehavior(RemoveButton, () => vm.Remove(SelectedIndices)));
            Behaviors.Add(new ClickEventBehavior(ClearButton, vm.Clear));
            Behaviors.Add(new ClickEventBehavior(TitleButton, vm.Setting));
            Behaviors.Add(new EventBehavior(MainGridView, "DoubleClick", () => vm.Preview(SelectedIndices)));
            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenFileBehavior(vm));
            Behaviors.Add(new OpenDirectoryBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new FileDropBehavior(this, vm));
            Behaviors.Add(new SelectionBehavior(MainGridView));
            Behaviors.Add(new ShowDialogBehavior<MetadataWindow, MetadataViewModel>(vm));
            Behaviors.Add(new ShowDialogBehavior<PasswordWindow, PasswordViewModel>(vm));
            Behaviors.Add(new ShowDialogBehavior<SettingWindow, SettingViewModel>(vm));
            Behaviors.Add(vm.Subscribe<UpdateListMessage>(e => vm.Files.ResetBindings(false)));
            Behaviors.Add(vm.Subscribe<SelectMessage>(e => Select(e.Value)));
            Behaviors.Add(vm.Subscribe<PreviewMessage>(e => Process.Start(e.Value)));
            Behaviors.Add(Locale.Subscribe(_ => BindText(vm)));

            var ctx = new FileContextMenu(() => SelectedIndices.Count() > 0);
            Behaviors.Add(new ClickEventBehavior(ctx.PreviewMenu, () => vm.Preview(SelectedIndices)));
            Behaviors.Add(new ClickEventBehavior(ctx.UpMenu, () => vm.Move(SelectedIndices, -1)));
            Behaviors.Add(new ClickEventBehavior(ctx.DownMenu, () => vm.Move(SelectedIndices, 1)));
            Behaviors.Add(new ClickEventBehavior(ctx.RemoveMenu, () => vm.Remove(SelectedIndices)));

            MainGridView.ContextMenuStrip = ctx;
            MainGridView.DataSource = vm.Files;

            MakeShortcut(vm);
            BindText(vm);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// MakeShortcut
        ///
        /// <summary>
        /// Initializes the shortcut settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void MakeShortcut(MainViewModel vm)
        {
            ShortcutKeys.Clear();
            ShortcutKeys.Add(Keys.Delete, () => vm.Remove(SelectedIndices));
            ShortcutKeys.Add(Keys.Control | Keys.Shift | Keys.D, vm.Clear);
            ShortcutKeys.Add(Keys.Control | Keys.Q, Close);
            ShortcutKeys.Add(Keys.Control | Keys.O, vm.Add);
            ShortcutKeys.Add(Keys.Control | Keys.H, vm.Setting);
            ShortcutKeys.Add(Keys.Control | Keys.R, () => vm.Preview(SelectedIndices));
            ShortcutKeys.Add(Keys.Control | Keys.D, () => vm.Remove(SelectedIndices));
            ShortcutKeys.Add(Keys.Control | Keys.K, () => vm.Move(SelectedIndices, -1));
            ShortcutKeys.Add(Keys.Control | Keys.J, () => vm.Move(SelectedIndices, 1));
            ShortcutKeys.Add(Keys.Control | Keys.M, () => vm.Ready.Then(vm.Merge));
            ShortcutKeys.Add(Keys.Control | Keys.S, () => vm.Ready.Then(vm.Split));
            ShortcutKeys.Add(Keys.Control | Keys.E, () => vm.Ready.Then(vm.Metadata));

            // Same as Ctrl + key
            ShortcutKeys.Add(Keys.Alt | Keys.O, vm.Add);
            ShortcutKeys.Add(Keys.Alt | Keys.H, vm.Setting);
            ShortcutKeys.Add(Keys.Alt | Keys.D, () => vm.Remove(SelectedIndices));
            ShortcutKeys.Add(Keys.Alt | Keys.M, () => vm.Ready.Then(vm.Merge));
            ShortcutKeys.Add(Keys.Alt | Keys.S, () => vm.Ready.Then(vm.Split));
            ShortcutKeys.Add(Keys.Alt | Keys.E, () => vm.Ready.Then(vm.Metadata));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Select items of the specified indices.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Select(IEnumerable<int> indices)
        {
            MainGridView.ClearSelection();
            foreach (var i in indices) MainGridView.Rows[i].Selected = true;
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
            var lang = vm.Language;
            this.UpdateCulture(lang);
            Resource.UpdateCulture(lang);
            MainToolTip.SetToolTip(TitleButton, Properties.Resources.MessageSettings);
            MainGridView.Refresh();
        }

        #endregion
    }
}
