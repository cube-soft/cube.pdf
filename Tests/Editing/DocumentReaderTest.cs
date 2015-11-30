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
using IoEx = System.IO;

namespace Cube.Pdf.Tests.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.Tests.Editing.DocumentReaderTest
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
        /// MetadataTest
        ///
        /// <summary>
        /// PDF のメタデータを取得するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task MetadataTest()
        {
            var src = IoEx.Path.Combine(Examples, "readme.pdf");
            Assert.That(IoEx.File.Exists(src), Is.True);

            using (var doc = new Cube.Pdf.Editing.DocumentReader())
            {
                await doc.OpenAsync(src, string.Empty);
                var metadata = doc.Metadata;
                Assert.That(metadata, Is.Not.Null);

                // Version
                Assert.That(metadata.Version.Major, Is.EqualTo(1));
                Assert.That(metadata.Version.Minor, Is.EqualTo(7));

                // ViewLayout and ViewMode
                Assert.That(metadata.ViewLayout, Is.EqualTo(ViewLayout.TwoPageLeft));
                Assert.That(metadata.ViewMode,   Is.EqualTo(ViewMode.Thumbnail));

                // Others
                Assert.That(metadata.Title,    Is.EqualTo("README"));
                Assert.That(metadata.Author,   Is.EqualTo("株式会社キューブ・ソフト"));
                Assert.That(metadata.Subtitle, Is.EqualTo("CubePDF"));
                Assert.That(metadata.Keywords, Is.EqualTo("CubeSoft,CubePDF,更新履歴"));
            }
        }

        #endregion
    }
}
