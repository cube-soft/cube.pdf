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
using NUnit.Framework;
using System.Threading;
using System.Windows;

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
        /// Executes the test for creating a new instance of the
        /// PasswordWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void PasswordWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowPasswordWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewWindow
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// PreviewWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void PreviewWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowPreviewWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        /* ----------------------------------------------------------------- */
        ///
        /// InsertWindow
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// InsertWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void InsertWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowInsertWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveWindow
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// RemoveWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void RemoveWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowRemoveWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        /* ----------------------------------------------------------------- */
        ///
        /// MetadataWindow
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// MetadataWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MetadataWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowMetadataWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionWindow
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// EncryptionWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void EncryptionWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowEncryptionWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SettingWindow
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// SettingWindowBehavior class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void SettingWindow() => Create(vm =>
        {
            var view = new Window { DataContext = vm };
            var src  = new ShowSettingWindow();

            Assert.DoesNotThrow(() =>
            {
                src.Attach(view);
                src.Detach();
            });
        });

        #endregion
    }
}
