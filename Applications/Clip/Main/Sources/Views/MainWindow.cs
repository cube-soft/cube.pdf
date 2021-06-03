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
using Cube.Forms.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Cube.Pdf.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainForm
    ///
    /// <summary>
    /// Represents the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : Cube.Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        ///
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainWindow()
        {
            InitializeComponent();
            Behaviors.Add(CreateTooltip());
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
        public IEnumerable<int> SelectedIndices => ClipListView
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
        /// Initializes for the About page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            base.OnBind(src);
            if (src is not MainViewModel vm) return;

            MainBindingSource.DataSource = vm;
            ClipListView.DataSource = vm.Clips;

            OpenButton.Click   += (s, e) => vm.Open();
            AttachButton.Click += (s, e) => vm.Attach();
            DetachButton.Click += (s, e) => vm.Detach(SelectedIndices);
            SaveButton.Click   += (s, e) => vm.Save();
            ResetButton.Click  += (s, e) => vm.Reset();

            Behaviors.Add(new CloseBehavior(this, src));
            Behaviors.Add(new DialogBehavior(src));
            Behaviors.Add(new OpenFileBehavior(src));
            Behaviors.Add(new OpenDirectoryBehavior(src));
            Behaviors.Add(new SaveFileBehavior(src));
            Behaviors.Add(vm.Subscribe<UpdateListMessage>(e => vm.Clips.ResetBindings(false)));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateTooltip
        ///
        /// <summary>
        /// Creates a new instance of the ToolTip class and invokes the
        /// initialization.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDisposable CreateTooltip()
        {
            var dest = new ToolTip
            {
                InitialDelay = 200,
                AutoPopDelay = 5000,
                ReshowDelay  = 1000
            };

            dest.SetToolTip(VersionButton, Properties.Resources.TitleAbout);
            dest.SetToolTip(OpenButton, Properties.Resources.TitleOpen);
            return dest;
        }

        #endregion
    }
}
