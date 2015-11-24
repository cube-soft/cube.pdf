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
using System.Threading.Tasks;
using NUnit.Framework;
using IoEx = System.IO;

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
        /// OpenAsyncTest
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("readme.pdf", "", true)]
        public async Task TestCopy(string filename, string password, bool smart)
        {
            using (var reader = new Cube.Pdf.Editing.DocumentReader())
            {
                var src = IoEx.Path.Combine(Examples, filename);
                await reader.OpenAsync(src, password);

                var binder = new Cube.Pdf.Editing.PageBinder();
                binder.UseSmartCopy = true;
                foreach (var page in reader.Pages) binder.Pages.Add(page);

                var dest = string.Format("TestCopy-{1}", filename);
                await binder.SaveAsync(IoEx.Path.Combine(Results, dest));
            }
        }
    }
}
