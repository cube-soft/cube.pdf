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
using System.Threading;
using System.Windows;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Interactions
{
    /* --------------------------------------------------------------------- */
    ///
    /// DialogBehaviorTest
    ///
    /// <summary>
    /// Tests for ShowDialogBehavior(T, U) inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class DialogBehaviorTest : ViewModelFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordWindow
        ///
        /// <summary>
        /// Tests the PasswordWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void PasswordWindow()
        {
            var view = new Window();
            var src  = new ShowPasswordWindow();

            src.Attach(view);
            src.Detach();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewWindow
        ///
        /// <summary>
        /// Tests the PreviewWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void PreviewWindow()
        {
            var view = new Window();
            var src  = new ShowPreviewWindow();

            src.Attach(view);
            src.Detach();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InsertWindow
        ///
        /// <summary>
        /// Tests the InsertWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void InsertWindow()
        {
            var view = new Window();
            var src  = new ShowInsertWindow();

            src.Attach(view);
            src.Detach();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveWindow
        ///
        /// <summary>
        /// Tests the RemoveWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void RemoveWindow()
        {
            var view = new Window();
            var src  = new ShowRemoveWindow();

            src.Attach(view);
            src.Detach();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataWindow
        ///
        /// <summary>
        /// Tests the MetadataWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MetadataWindow()
        {
            var view = new Window();
            var src  = new ShowMetadataWindow();

            src.Attach(view);
            src.Detach();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionWindow
        ///
        /// <summary>
        /// Tests the EncryptionWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void EncryptionWindow()
        {
            var view = new Window();
            var src  = new ShowEncryptionWindow();

            src.Attach(view);
            src.Detach();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingWindow
        ///
        /// <summary>
        /// Tests the SettingWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SettingWindow()
        {
            var view = new Window();
            var src  = new ShowSettingWindow();

            src.Attach(view);
            src.Detach();
        }

        #endregion
    }
}
