/* ------------------------------------------------------------------------- */
///
/// DocumentWriterTest.cs
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
using IoEx = System.IO;

namespace Cube.Pdf.Tests.Editing
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentWriterTest
    /// 
    /// <summary>
    /// DocumentWriter のテストを行うクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class DocumentSplitterTest : DocumentResource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Save_SplitDocument
        ///
        /// <summary>
        /// PDF ファイルを分割保存するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("rotation.pdf", "", 9)]
        [TestCase("password.pdf", "password", 2)]
        public void Save(string filename, string password, int expected)
        {
            using (var writer = new Cube.Pdf.Editing.DocumentSplitter())
            {
                var reader = Create(filename, password);
                writer.Metadata = reader.Metadata;
                writer.Encryption = reader.Encryption;
                writer.UseSmartCopy = true;
                foreach (var page in reader.Pages) writer.Pages.Add(page);
                writer.Save(Results);

                Assert.That(writer.Results.Count, Is.EqualTo(expected));
            }
        }
    }
}
