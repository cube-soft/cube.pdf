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
            Root = System.IO.Path.GetDirectoryName(reader.Location);
            _folder = GetType().FullName.Replace(string.Format("{0}.", reader.Product), "");
            if (!System.IO.Directory.Exists(Results)) System.IO.Directory.CreateDirectory(Results);
            Clean(Results);
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
            => System.IO.Path.Combine(Root, "Examples");

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
            => System.IO.Path.Combine(Root, $@"Results\{_folder}");

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Example
        /// 
        /// <summary>
        /// Examples フォルダのパスを結合した結果を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Example(string filename)
            => System.IO.Path.Combine(Examples, filename);

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        /// 
        /// <summary>
        /// Result フォルダのパスを結合した結果を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Result(string filename)
            => System.IO.Path.Combine(Results, filename);

        #endregion

        #region Implementations

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
            foreach (string file in System.IO.Directory.GetFiles(folder))
            {
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
                System.IO.File.Delete(file);
            }

            foreach (string sub in System.IO.Directory.GetDirectories(folder))
            {
                Clean(sub);
                System.IO.Directory.Delete(sub);
            }
        }

        #endregion

        #region Fields
        private string _folder = string.Empty;
        #endregion
    }
}
