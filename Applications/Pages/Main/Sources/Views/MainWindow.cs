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
            ExitButton.Click += (s, e) => Close();
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
        protected override void OnBind(IPresentable src)
        {
            base.OnBind(src);
            if (!(src is MainViewModel vm)) return;

            MainBindingSource.DataSource = vm;

            var ctx = new FileContextMenu(() => SelectedIndices.Count() > 0);
            ctx.PreviewMenu.Click += (s, e) => vm.Preview(SelectedIndices);
            ctx.UpMenu.Click      += (s, e) => vm.Move(SelectedIndices, -1);
            ctx.DownMenu.Click    += (s, e) => vm.Move(SelectedIndices, 1);
            ctx.RemoveMenu.Click  += (s, e) => vm.Remove(SelectedIndices);

            FileListView.ContextMenuStrip = ctx;
            FileListView.DataSource = vm.Files;

            MergeButton.Click  += (s, e) => vm.Merge();
            SplitButton.Click  += (s, e) => vm.Split();
            FileButton.Click   += (s, e) => vm.Add();
            UpButton.Click     += (s, e) => vm.Move(SelectedIndices, -1);
            DownButton.Click   += (s, e) => vm.Move(SelectedIndices, 1);
            RemoveButton.Click += (s, e) => vm.Remove(SelectedIndices);
            ClearButton.Click  += (s, e) => vm.Clear();
            TitleButton.Click  += (s, e) => vm.About();

            ShortcutKeys.Clear();
            ShortcutKeys.Add(Keys.Control | Keys.Shift | Keys.D, vm.Clear);
            ShortcutKeys.Add(Keys.Control | Keys.O, vm.Add);
            ShortcutKeys.Add(Keys.Control | Keys.H, vm.About);
            ShortcutKeys.Add(Keys.Control | Keys.M, () => vm.Invokable.Then(vm.Merge));
            ShortcutKeys.Add(Keys.Control | Keys.S, () => vm.Invokable.Then(vm.Split));

            Behaviors.Add(new CloseBehavior(vm, this));
            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenFileBehavior(vm));
            Behaviors.Add(new OpenDirectoryBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new FileDropBehavior(vm, this));
            Behaviors.Add(new ShowDialogBehavior<PasswordWindow, PasswordViewModel>(vm));
            Behaviors.Add(new ShowDialogBehavior<VersionWindow, VersionViewModel>(vm));
            Behaviors.Add(vm.Subscribe<CollectionMessage>(e => vm.Files.ResetBindings(false)));
            Behaviors.Add(vm.Subscribe<SelectMessage>(e => Select(e.Value)));
            Behaviors.Add(vm.Subscribe<PreviewMessage>(e => Process.Start(e.Value)));
        }

        #endregion

        #region Implementations

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
                InitialDelay = 200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000
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
