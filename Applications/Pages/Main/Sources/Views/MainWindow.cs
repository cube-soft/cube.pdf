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
using Cube.Forms;
using Cube.Forms.Behaviors;
using System;
using System.Windows.Forms;

namespace Cube.Pdf.Pages
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
    public partial class MainWindow : Window
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
            Behaviors.Add(SetupForAbout());
            ExitButton.Click += (s, e) => Close();
        }

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
        protected override void OnBind(IPresentable src)
        {
            base.OnBind(src);
            if (!(src is MainViewModel vm)) return;

            FileListView.DataSource = vm.Files;

            FileButton.Click  += (s, e) => vm.Add();
            MergeButton.Click += (s, e) => vm.Merge();
            SplitButton.Click += (s, e) => vm.Split();

            Behaviors.Add(new CloseBehavior(src, this));
            Behaviors.Add(new DialogBehavior(src));
            Behaviors.Add(new OpenFileBehavior(src));
            Behaviors.Add(new OpenDirectoryBehavior(src));
            Behaviors.Add(new SaveFileBehavior(src));
            Behaviors.Add(vm.Subscribe<CollectionMessage>(e => vm.Files.ResetBindings(false)));
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

        #endregion
    }
}
