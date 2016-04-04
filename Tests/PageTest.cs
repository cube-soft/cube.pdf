/* ------------------------------------------------------------------------- */
///
/// PageTest.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageTest
    /// 
    /// <summary>
    /// Page のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class PageTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Rotation
        ///
        /// <summary>
        /// 回転角度の正規化テストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(   0,   0)]
        [TestCase( 359, 359)]
        [TestCase( 360,   0)]
        [TestCase(  -1, 359)]
        [TestCase(1000, 280)]
        public void Rotation(int input, int expected)
        {
            var page = new Page();
            page.Rotation = input;
            Assert.That(page.Rotation, Is.EqualTo(expected));
        }
    }
}
