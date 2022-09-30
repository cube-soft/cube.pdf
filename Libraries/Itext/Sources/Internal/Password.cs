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
namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Represents the password string or query.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class Password : QueryMessage<IQuery<string>, string>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Initializes a new instance of the Password class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="query">Password query.</param>
        /// <param name="value">Initial password value.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Password(IQuery<string> query, string value) : base(query) => Value  = value;
    }
}
