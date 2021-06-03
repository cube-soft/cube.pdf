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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Cube.Forms;
using Cube.Forms.Behaviors;
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
        public MainWindow()
        {
            InitializeComponent();

            Behaviors.Add(SetupForAbout());
            Behaviors.Add(new SelectionBehavior(FileListView));
            Behaviors.Add(new ClickBehavior(ExitButton, Close));
        }

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
        public IEnumerable<int> SelectedIndices => FileListView
            .SelectedRows
            .Cast<DataGridViewRow>()
            .Select(e => e.Index)
            .ToArray();

        #endregion

        #region Implementations

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
            MainBindingSource.DataSource = vm;

            ShortcutKeys.Clear();
            ShortcutKeys.Add(Keys.Control | Keys.Shift | Keys.D, vm.Clear);
            ShortcutKeys.Add(Keys.Control | Keys.O, vm.Add);
            ShortcutKeys.Add(Keys.Control | Keys.H, vm.About);
            ShortcutKeys.Add(Keys.Control | Keys.K, () => vm.Move(SelectedIndices, -1));
            ShortcutKeys.Add(Keys.Control | Keys.J, () => vm.Move(SelectedIndices, 1));
            ShortcutKeys.Add(Keys.Control | Keys.M, () => vm.Invokable.Then(vm.Merge));
            ShortcutKeys.Add(Keys.Control | Keys.S, () => vm.Invokable.Then(vm.Split));

            Behaviors.Add(new ShownBehavior(this, vm.Setup));
            Behaviors.Add(new ClickBehavior(MergeButton, vm.Merge));
            Behaviors.Add(new ClickBehavior(SplitButton, vm.Split));
            Behaviors.Add(new ClickBehavior(FileButton, vm.Add));
            Behaviors.Add(new ClickBehavior(UpButton, () => vm.Move(SelectedIndices, -1)));
            Behaviors.Add(new ClickBehavior(DownButton, () => vm.Move(SelectedIndices, 1)));
            Behaviors.Add(new ClickBehavior(RemoveButton, () => vm.Remove(SelectedIndices)));
            Behaviors.Add(new ClickBehavior(ClearButton, vm.Clear));
            Behaviors.Add(new ClickBehavior(TitleButton, vm.About));
            Behaviors.Add(new EventBehavior(FileListView, "DoubleClick", () => vm.Preview(SelectedIndices)));
            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenFileBehavior(vm));
            Behaviors.Add(new OpenDirectoryBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new FileDropBehavior(this, vm));
            Behaviors.Add(new ShowDialogBehavior<PasswordWindow, PasswordViewModel>(vm));
            Behaviors.Add(new ShowDialogBehavior<VersionWindow, VersionViewModel>(vm));
            Behaviors.Add(vm.Subscribe<UpdateListMessage>(e => vm.Files.ResetBindings(false)));
            Behaviors.Add(vm.Subscribe<SelectMessage>(e => Select(e.Value)));
            Behaviors.Add(vm.Subscribe<PreviewMessage>(e => Process.Start(e.Value)));

            var ctx = new FileContextMenu(() => SelectedIndices.Count() > 0);
            Behaviors.Add(new ClickBehavior(ctx.PreviewMenu, () => vm.Preview(SelectedIndices)));
            Behaviors.Add(new ClickBehavior(ctx.UpMenu, () => vm.Move(SelectedIndices, -1)));
            Behaviors.Add(new ClickBehavior(ctx.DownMenu, () => vm.Move(SelectedIndices, 1)));
            Behaviors.Add(new ClickBehavior(ctx.RemoveMenu, () => vm.Remove(SelectedIndices)));

            FileListView.ContextMenuStrip = ctx;
            FileListView.DataSource = vm.Files;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetupForAbout
        ///
        /// <summary>
        /// Initializes for the About page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable SetupForAbout()
        {
            var dest = new ToolTip
            {
                InitialDelay =  200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000,
            };
            dest.SetToolTip(TitleButton, Properties.Resources.MessageAbout);
            return dest;
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
            FileListView.ClearSelection();
            foreach (var i in indices) FileListView.Rows[i].Selected = true;
        }

        #endregion
    }
}
