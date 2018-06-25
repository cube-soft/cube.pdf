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
    /// PageTest
    ///
    /// <summary>
    /// Page のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PageTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        ///
        /// <summary>
        /// 回転角度の正規化テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(   0, ExpectedResult =   0)]
        [TestCase( 359, ExpectedResult = 359)]
        [TestCase( 360, ExpectedResult =   0)]
        [TestCase(1000, ExpectedResult = 280)]
        [TestCase(  -1, ExpectedResult = 359)]
        [TestCase(-900, ExpectedResult = 180)]
        public int Rotation(int degree) => new Page { Rotation = degree }.Rotation;
    }
}
