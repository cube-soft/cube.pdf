/* ------------------------------------------------------------------------- */
///
/// PageBinderTest.cs
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
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.Tests.Editing.PageBinderTest
    /// 
    /// <summary>
    /// PageBinder のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [TestFixture]
    public class PageBinderTest : FileResource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SaveTheSameContents
        ///
        /// <summary>
        /// PDF の内容を変更せずに別のファイルに保存するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("readme.pdf", "",  true)]
        [TestCase("readme.pdf", "", false)]
        public async Task SaveTheSameContents(string filename, string password, bool smart)
        {
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                var src = Path.Combine(Examples, filename);
                await reader.OpenAsync(src, password);

                var binder = new Cube.Pdf.Editing.PageBinder();
                binder.UseSmartCopy = true;
                binder.Metadata     = reader.Metadata;
                binder.Encryption   = reader.Encryption;
                foreach (var page in reader.Pages) binder.Pages.Add(page);

                var sn = smart ? "Smart" : "Normal";
                var dest = Path.Combine(Results, string.Format("Bind-{0}-{1}", sn, filename));
                await binder.SaveAsync(dest);

                Assert.That(File.Exists(dest), Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BindPagesAndImage
        ///
        /// <summary>
        /// PDF ファイルと画像ファイルを結合するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task BindPagesAndImages()
        {
            var binder = new Cube.Pdf.Editing.PageBinder();
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                var src = Path.Combine(Examples, "readme.pdf");
                await reader.OpenAsync(src, string.Empty);

                binder.Pages.Add(reader.GetPage(1));
                binder.Pages.Add(new ImagePage
                {
                    FilePath = Path.Combine(Examples, "cubepdf.png"),
                    Size = new System.Drawing.Size(430, 530)
                });
                binder.Pages.Add(reader.GetPage(2));
            }

            var dest = Path.Combine(Results, "Bind-readme-cubepdf.pdf");
            await binder.SaveAsync(dest);

            Assert.That(File.Exists(dest));
        }
    }
}
