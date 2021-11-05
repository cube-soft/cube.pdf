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
using Cube.Mixin.Observing;
using Cube.Mixin.String;
using Cube.Xui;

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
    public sealed class PasswordViewModel : DialogViewModel<QueryMessage<string, string>>
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
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordViewModel(QueryMessage<string, string> src, SynchronizationContext context) :
            base(src, new(), context)
        {
            Facade.Cancel = true;
            OK.Command = new DelegateCommand(
                () => Close(() => src.Cancel = false, true),
                () => Password.Value.HasValue()
            ).Hook(Password);
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
        public IElement<string> Password => Get(() => new BindableElement<string>(
            () => string.Format(Properties.Resources.MessagePassword, GetFileName(Facade.Source)),
            () => Facade.Value,
            e  => Facade.Value = e,
            GetDispatcher(false)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        ///
        /// <returns>String value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Properties.Resources.TitlePassword;

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets the filename of the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetFileName(string src)
        {
            var index = src.LastIndexOfAny(new[] { '/', '\\' });
            return index >= 0 && index + 1 < src.Length ? src.Substring(index + 1) : src;
        }

        #endregion
    }
}
