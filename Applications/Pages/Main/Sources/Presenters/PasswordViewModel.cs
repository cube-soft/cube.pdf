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
using System.Threading;
using Cube.FileSystem;
using Cube.Mixin.String;

namespace Cube.Pdf.Pages
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
    public sealed class PasswordViewModel : PresentableBase<QueryMessage<string, string>>
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
            base(src, new Aggregator(), context)
        {
            Facade.Cancel = true;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the password of the provided source.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password
        {
            get => Get<string>();
            set
            {
                if (!Set(value)) return;
                Facade.Value = value;
                Refresh(nameof(Ready));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Ready
        ///
        /// <summary>
        /// Gets a value indicating whether the password is ready.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Ready => Password.HasValue();

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// Gets the message to show.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Message => string.Format(Properties.Resources.MessagePassword, Io.Get(Facade.Source).Name);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// Apply the user password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Apply() => Close(() => Facade.Cancel = false, true);

        #endregion
    }
}
