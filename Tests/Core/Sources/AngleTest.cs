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
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// AngleTest
    ///
    /// <summary>
    /// Tests the Angle class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class AngleTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Normalize
        ///
        /// <summary>
        /// Tests that the specified value is normalized to [0, 360) when
        /// creating a new Angle object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(   0, ExpectedResult =   0)]
        [TestCase( 359, ExpectedResult = 359)]
        [TestCase( 360, ExpectedResult =   0)]
        [TestCase(1000, ExpectedResult = 280)]
        [TestCase(  -1, ExpectedResult = 359)]
        [TestCase(-900, ExpectedResult = 180)]
        public int Normalize(int degree) => new Angle(degree).Degree;

        /* ----------------------------------------------------------------- */
        ///
        /// Plus
        ///
        /// <summary>
        /// Tests that the specified degree value is normalized to [0, 360)
        /// when the plus operator is invoked.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(   0,   90, ExpectedResult =  90)]
        [TestCase( 200,  159, ExpectedResult = 359)]
        [TestCase( 180,  180, ExpectedResult =   0)]
        [TestCase(1000, 1000, ExpectedResult = 200)]
        [TestCase(  -1,    2, ExpectedResult =   1)]
        [TestCase(-900, -700, ExpectedResult = 200)]
        public int Plus(int x, int y) => (new Angle(x) + y).Degree;
    }
}
