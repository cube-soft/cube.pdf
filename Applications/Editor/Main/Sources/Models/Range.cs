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
using Cube.Collections;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RangeException
    ///
    /// <summary>
    /// Represents that the specified string is invalid.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class RangeException : Exception
    {
        /* ----------------------------------------------------------------- */
        ///
        /// RangeException
        ///
        /// <summary>
        /// Initializes a new instance of the RangeException class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RangeException() : base(Properties.Resources.MessageRangeError) { }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Range
    ///
    /// <summary>
    /// Collection that is the result of parsing the specified string.
    /// </summary>
    ///
    /// <remarks>
    /// range  = value , [ { "," value } ]
    /// value  = number | number , "-" , number
    /// number = digit , [ { digit } ]
    /// digit  = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Range : EnumerableBase<int>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Range
        ///
        /// <summary>
        /// Initializes a new instance of the Range class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">String that represents the range.</param>
        /// <param name="n">Maximum value.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Range(string src, int n)
        {
            _inner = Parse(src, n);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(int) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<int> GetEnumerator() => _inner.GetEnumerator();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the
        /// ImageCollection and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Parse
        ///
        /// <summary>
        /// Parses the specified string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<int> Parse(string src, int n)
        {
            try
            {
                if (!src.HasValue()) return new int[0];

                var dest = new List<int>();
                foreach (var token in src.Split(','))
                {
                    if (token.IndexOf('-') != -1)
                    {
                        var pair = token.Split('-');
                        if (pair.Length != 2) throw new RangeException();
                        for (var i = int.Parse(pair[0]); i <= int.Parse(pair[1]); ++i) dest.Add(i);
                    }
                    else dest.Add(int.Parse(token));
                }
                return dest.Where(i => i > 0 && i <= n).Distinct().OrderBy(i => i);
            }
            catch { throw new RangeException(); }
        }

        #endregion

        #region Fields
        private readonly IEnumerable<int> _inner;
        #endregion
    }
}
