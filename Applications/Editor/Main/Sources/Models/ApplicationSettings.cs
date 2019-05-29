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
namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ApplicationSettings
    ///
    /// <summary>
    /// Represents the global settings of the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ApplicationSettings
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ApplicationSettings
        ///
        /// <summary>
        /// Initializes a new instance of the ApplicationSettings class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ApplicationSettings()
        {
            Locale.Subscribe(e => Properties.Resources.Culture = e.ToCultureInfo());
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// Configures global settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure() => _core.Invoke();

        #endregion

        #region Fields
        private static readonly OnceAction _core = new OnceAction(() => new ApplicationSettings());
        #endregion
    }
}
