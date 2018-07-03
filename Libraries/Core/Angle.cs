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
    /// 角度を表すクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// 入力値に対して [0, 360) の範囲で正規化します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Angle
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Angle
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Angle() : this(0) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Angle
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="degree">度単位の値</param>
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
        /// 度単位の角度を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Degree { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Ragian
        ///
        /// <summary>
        /// ラジアン単位の角度を取得します。
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
        /// 角度の足し算を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Angle operator +(Angle x, Angle y) => new Angle(x.Degree + y.Degree);

        /* ----------------------------------------------------------------- */
        ///
        /// operator+
        ///
        /// <summary>
        /// 角度の足し算を実行します。
        /// </summary>
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
        /// 角度を正規化します。
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
