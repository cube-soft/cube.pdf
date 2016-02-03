/* ------------------------------------------------------------------------- */
///
/// FileResource.cs
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
using System.Reflection;
using IoEx = System.IO;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileResource
    /// 
    /// <summary>
    /// テストでファイルを使用するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    class FileResource
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileResource
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected FileResource()
        {
            var reader = new AssemblyReader(Assembly.GetExecutingAssembly());
            Root = IoEx.Path.GetDirectoryName(reader.Location);
            _folder = GetType().FullName.Replace(string.Format("{0}.", reader.Product), "");
            Initialize();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Root
        ///
        /// <summary>
        /// テスト用リソースの存在するルートディレクトリへのパスを
        /// 取得、または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Root { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Examples
        /// 
        /// <summary>
        /// テスト用ファイルの存在するフォルダへのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Examples
        {
            get { return IoEx.Path.Combine(Root, "Examples"); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        /// 
        /// <summary>
        /// テスト結果を格納するためのフォルダへのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Results
        {
            get
            {
                var folder = string.Format(@"Results\{0}", _folder);
                return IoEx.Path.Combine(Root, folder);
            }
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        /// 
        /// <summary>
        /// リソースファイルを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Initialize()
        {
            if (!IoEx.Directory.Exists(Results)) IoEx.Directory.CreateDirectory(Results);
            Clean(Results);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clean
        /// 
        /// <summary>
        /// 指定されたフォルダ内に存在する全てのファイルを削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Clean(string folder)
        {
            foreach (string file in IoEx.Directory.GetFiles(folder))
            {
                IoEx.File.SetAttributes(file, IoEx.FileAttributes.Normal);
                IoEx.File.Delete(file);
            }

            foreach (string sub in IoEx.Directory.GetDirectories(folder))
            {
                Clean(sub);
            }
        }

        #endregion

        #region Fields
        private string _folder = string.Empty;
        #endregion
    }
}
