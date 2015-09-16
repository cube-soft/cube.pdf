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
using System.Threading.Tasks;
using NUnit.Framework;
using IO = System.IO;

namespace Cube.Tests.Pdf.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Tests.Pdf.Editing.DocumentReaderTest
    /// 
    /// <summary>
    /// DocumentReader のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [TestFixture]
    public class DocumentReaderTest : FileResource
    {
        #region Test methods

        /* ----------------------------------------------------------------- */
        ///
        /// OpenAsyncTest
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("readme.pdf", "")]
        public async Task OpenAsyncTest(string filename, string password)
        {
            using (var doc = new Cube.Pdf.Editing.DocumentReader())
            {
                var src = IO.Path.Combine(Examples, filename);
                await doc.OpenAsync(src, password);
                Assert.That(doc.FileName, Is.EqualTo(src));
                Assert.That(doc.Metadata.Version.Major, Is.EqualTo(1));
                Assert.That(doc.Metadata.Version.Minor, Is.AtLeast(2));
                Assert.That(doc.Pages.Count, Is.AtLeast(1));
            }
        }

        #endregion
    }
}
