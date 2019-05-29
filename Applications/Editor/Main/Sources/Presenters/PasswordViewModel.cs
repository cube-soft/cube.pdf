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
using Cube.FileSystem;
using Cube.Mixin.String;
using Cube.Xui;
using System.Threading;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordViewModel
    ///
    /// <summary>
    /// Provides binding properties and commands for the PasswordWindow
    /// class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PasswordViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordViewModel class.
        /// </summary>
        ///
        /// <param name="src">Query for password.</param>
        /// <param name="io">I/O handler</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordViewModel(QueryMessage<string, string> src, IO io, SynchronizationContext context) :
            base(() => Properties.Resources.TitlePassword, new Aggregator(), context)
        {
            var fi = io.Get(src.Query);

            Password = new BindableElement<string>(
                () => string.Format(Properties.Resources.MessagePassword, fi.Name),
                () => src.Value,
                e  => src.Value = e,
                GetDispatcher(false)
            );

            OK.Command = new BindableCommand(
                () => { src.Cancel = false; Send<CloseMessage>(); },
                () => Password.Value.HasValue(),
                Password
            );

            src.Cancel = true;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets the password menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableElement<string> Password { get; }

        #endregion
    }
}
