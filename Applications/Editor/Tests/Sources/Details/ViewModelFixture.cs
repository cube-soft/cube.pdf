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
using Cube.FileSystem.TestService;
using Cube.Pdf.App.Editor;
using Cube.Xui;
using System;
using System.Windows.Media.Imaging;

namespace Cube.Pdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelFixture
    ///
    /// <summary>
    /// Provides functionality to test throw ViewModel classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewModelFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateViewModel
        ///
        /// <summary>
        /// Gets a new instance of the MainViewModel class.
        /// </summary>
        ///
        /// <returns>MainViewModel object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected MainViewModel CreateViewModel()
        {
            var dest = new MainViewModel();
            dest.Data.Images.LoadingImage = new BitmapImage(new Uri(GetExamplesWith("Loading.png")));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExecuteOpenCommand
        ///
        /// <summary>
        /// Execute the command to open the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void ExecuteOpenCommand(MainViewModel vm, string src)
        {
            vm.Messenger.Register<OpenFileMessage>(this, e =>
            {
                e.FileName = src;
                e.Result   = true;
                e.Callback.Invoke(e);
            });
            vm.Ribbon.Open.Command.Execute(null);
        }

        #endregion
    }
}
