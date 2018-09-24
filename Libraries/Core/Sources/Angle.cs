/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Angle
    ///
    /// <summary>
    /// Represents an angle in degree and radian units.
    /// The class normalizes the degree within the range of [0, 360).
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Angle
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Angle
        ///
        /// <summary>
        /// Initializes a new instance of the Angle class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Angle() : this(0) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Angle
        ///
        /// <summary>
        /// Initializes a new instance of the Angle class with the
        /// specified value in degree unit.
        /// </summary>
        ///
        /// <param name="degree">Angle in degree unit.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Angle(int degree)
        {
            Degree = Normalize(degree);
        }

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Degree
        ///
        /// <summary>
        /// Gets the agnle in degree unit.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Degree { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Ragian
        ///
        /// <summary>
        /// Gets the agnle in ragian unit.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public double Radian => Math.PI * Degree / 180.0;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// operator+
        ///
        /// <summary>
        /// Gets the result of summing the two values.
        /// </summary>
        ///
        /// <param name="x">Angle object.</param>
        /// <param name="y">Angle object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static Angle operator +(Angle x, Angle y) => new Angle(x.Degree + y.Degree);

        /* ----------------------------------------------------------------- */
        ///
        /// operator+
        ///
        /// <summary>
        /// Gets the result of summing the two values.
        /// </summary>
        ///
        /// <param name="x">Angle object.</param>
        /// <param name="y">Angle in degree unit.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static Angle operator +(Angle x, int y) => new Angle(x.Degree + y);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Normalizes the degree within the range of [0, 360).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int Normalize(int src)
        {
            var dest = src;
            while (dest <    0) dest += 360;
            while (dest >= 360) dest -= 360;
            return dest;
        }

        #endregion
    }
}
