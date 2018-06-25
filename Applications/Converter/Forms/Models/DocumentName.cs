/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem;
using Cube.Generics;
using System.Diagnostics;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentName
    ///
    /// <summary>
    /// ドキュメント名を管理するクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// ドキュメント名をファイル名の初期値として利用する時に利用不可能な
    /// 文字の除去等の処理を担います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class DocumentName
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// DocumentName
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">ドキュメント名を示す文字列</param>
        /// <param name="alternate">代替名</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public DocumentName(string src, string alternate, IO io)
        {
            IO          = io;
            DefaultName = alternate;
            Filter      = new PathFilter(src)
            {
                AllowCurrentDirectory = false,
                AllowDriveLetter      = false,
                AllowInactivation     = false,
                AllowParentDirectory  = false,
                AllowUnc              = false,
            };
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// オリジナルのドキュメント名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Value => Filter.RawPath;

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// ファイル名として利用可能な名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name => GetName();

        /* ----------------------------------------------------------------- */
        ///
        /// DefaultName
        ///
        /// <summary>
        /// ドキュメント名がファイル名として利用できない場合に代替する
        /// 名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DefaultName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// ドキュメント名のフィルタ用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PathFilter Filter { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// ファイル名として利用する文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetName()
        {
            if (!Value.HasValue()) return DefaultName;

            var dest = IO.Get(Filter.EscapedPath).Name;
            var key  = " - ";
            var pos  = dest.LastIndexOf(key);
            if (pos == -1) return dest;

            var head = dest.Substring(0, pos);
            var tail = dest.Substring(pos);

            if (System.IO.Path.HasExtension(head)) return head;
            if (System.IO.Path.HasExtension(tail))
            {
                Debug.Assert(tail.StartsWith(key));
                return tail.Substring(key.Length);
            }
            return dest;
        }

        #endregion
    }
}
