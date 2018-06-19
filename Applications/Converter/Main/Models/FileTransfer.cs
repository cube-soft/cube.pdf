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
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileTransfer
    ///
    /// <summary>
    /// ファイルの移動およびリネームを実行するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileTransfer
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileTransfer
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="dest">最終的な保存パス</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileTransfer(Format format, string dest, IO io)
        {
            IO            = io;
            Format        = format;
            Information   = io.Get(dest);
            WorkDirectory = IO.Combine(Information.DirectoryName, Guid.NewGuid().ToString("D"));
            Value         = GetName();
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
        /// Format
        ///
        /// <summary>
        /// 変換形式を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 保存用パスを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// ユーザプログラムは Value で指定されたパスに保存した後、
        /// Invoke メソッドを用いてファイルの移動を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// WorkDirectory
        ///
        /// <summary>
        /// 作業用の一時ディレクトリのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string WorkDirectory { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Information
        ///
        /// <summary>
        /// 最終的な保存パスを示すオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Information Information { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// ファイルの移動を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Invoke()
        {
            var src   = IO.GetFiles(WorkDirectory);
            var dest  = new List<string>();

            for (var i = 0; i < src.Length; ++i)
            {
                var path = GetDestination(i, src.Length);
                IO.Move(src[i], path, true);
                dest.Add(path);
            }

            return dest;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// ファイル名を表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetName() =>
            DocumentConverter.SupportedFormats.Any(e => e == Format) ?
            Information.Name :
            $"{Information.NameWithoutExtension}-%8d{Information.Extension}";

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestination
        ///
        /// <summary>
        /// 保存パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDestination(int index, int count)
        {
            if (count <= 1) return Information.FullName;

            var name  = Information.NameWithoutExtension;
            var ext   = Information.Extension;
            var digit = string.Format("D{0}", count.ToString("D").Length);
            var dest  = string.Format("{0}-{1}{2}", name, index.ToString(digit), ext);

            return IO.Combine(Information.DirectoryName, dest);
        }

        #endregion
    }
}
