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
using System.Reflection;
using System.Windows.Forms;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// Represents the splash window of the CubePDF Utility.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : Form
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

            var count = 0;

            VersionLabel.Text = GetVersion();
            RefreshTimer.Tick += (s, e) =>
            {
                if (count++ >= 60) Close();
                else MessageLabel.Text += ".";
            };

            RefreshTimer.Start();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CreateParams
        ///
        /// <summary>
        /// Gets the value of initialzing information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ClassStyle |= 0x00020000;
                return cp;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Error
        ///
        /// <summary>
        /// Shows the error message and close the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Error(Exception src)
        {
            MessageBox.Show($"{src.Message} ({src.GetType().Name})",
                "CubePDF Utility", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetVersion
        ///
        /// <summary>
        /// Get the version string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetVersion()
        {
            var app  = Assembly.GetExecutingAssembly().GetName().Version;
            var fw   = Environment.Version;
            var arch = (IntPtr.Size == 4) ? "x86" : "x64";
            return $"Version {app} ({arch}) Microsoft {fw}";
        }

        #endregion
    }
}
