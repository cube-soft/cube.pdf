/* ------------------------------------------------------------------------- */
///
/// DocumentReaderTest.cs
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
using System;
using System.Drawing;
using NUnit.Framework;
using IoEx = System.IO;

namespace Cube.Pdf.Tests.Drawing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReaderTest
    /// 
    /// <summary>
    /// DocumentReader のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class DocumentReaderTest : DocumentResource<Cube.Pdf.Drawing.DocumentReader>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// PDF ファイルを開くテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region Open

        [TestCase("rotation.pdf", "")]
        [TestCase("password.pdf", "password")]
        [TestCase("password-aes256.pdf", "password")]
        public void Open(string filename, string password)
        {
            Assert.That(Create(filename, password).IsOpen, Is.True);
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// GetPage
        ///
        /// <summary>
        /// 各ページの情報を取得するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region GetPage

        [TestCase("rotation.pdf", 1, 595, 842)]
        public void GetPage_Size(string filename, int number, int width, int height)
        {
            Assert.That(
                Create(filename).GetPage(number).Size,
                Is.EqualTo(new Size(width, height))
            );
        }

        [TestCase("rotation.pdf", 1, 72)]
        public void GetPage_Resolution(string filename, int number, int expected)
        {
            Assert.That(
                Create(filename).GetPage(number).Resolution,
                Is.EqualTo(new Point(expected, expected))
            );
        }

        [TestCase("rotation.pdf", 1, 0)]
        [TestCase("rotation.pdf", 2, 90)]
        [TestCase("rotation.pdf", 3, 180)]
        [TestCase("rotation.pdf", 4, 270)]
        public void GetPage_Rotation(string filename, int number, int expected)
        {
            Assert.That(
                Create(filename).GetPage(number).Rotation,
                Is.EqualTo(expected)
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImage
        ///
        /// <summary>
        /// Image オブジェクトを生成するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region CreateImage

        [TestCase("rotation.pdf", "", 1)]
        [TestCase("rotation.pdf", "", 2)]
        [TestCase("rotation.pdf", "", 3)]
        [TestCase("rotation.pdf", "", 4)]
        public void CreateImage(string filename, string password, int pagenum)
        {
            var power  = 1.0;
            var reader = Create(filename, password);
            var page   = reader.GetPage(pagenum);

            using (var image = reader.CreateImage(pagenum, power))
            {
                var dest = IoEx.Path.Combine(Results, $"CreateImage-{pagenum}.png");
                image.Save(dest);
                Assert.That(IoEx.File.Exists(dest), Is.True);
            }
        }

        #endregion
    }
}
