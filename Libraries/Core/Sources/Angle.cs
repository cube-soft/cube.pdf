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
namespace Cube.Pdf;

using System;

/* ------------------------------------------------------------------------- */
///
/// Angle
///
/// <summary>
/// Represents an angle in degree and radian units.
/// The class normalizes the degree within the range of [0, 360).
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class Angle
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Angle
    ///
    /// <summary>
    /// Initializes a new instance of the Angle class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Angle() : this(0) { }

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    public Angle(int degree) => Degree = Normalize(degree);

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Degree
    ///
    /// <summary>
    /// Gets the angle in degree unit.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Degree { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Radian
    ///
    /// <summary>
    /// Gets the angle in radian unit.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public double Radian => Math.PI * Degree / 180.0;

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    public static Angle operator +(Angle x, Angle y) => new(x.Degree + y.Degree);

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    public static Angle operator +(Angle x, int y) => new(x.Degree + y);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Normalize
    ///
    /// <summary>
    /// Normalizes the degree within the range of [0, 360).
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private int Normalize(int src)
    {
        var degree = src;
        while (degree <    0) degree += 360;
        while (degree >= 360) degree -= 360;
        return degree;
    }

    #endregion
}
