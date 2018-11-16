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
using Cube.FileSystem.Mixin;
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
    public class FileTransfer : IDisposable
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
        /// <param name="format">変換形式</param>
        /// <param name="dest">最終的な保存パス</param>
        /// <param name="work">作業ディレクトリ</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileTransfer(Format format, string dest, string work, IO io)
        {
            _dispose      = new OnceAction<bool>(Dispose);
            IO            = io;
            Format        = format;
            Information   = io.Get(dest);
            WorkDirectory = GetWorkDirectory(work);
            Value         = IO.Combine(WorkDirectory, GetName());
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
        /// AutoRename
        ///
        /// <summary>
        /// 同名のファイルが存在する場合、自動的にリネームするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AutoRename { get; set; } = false;

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
                var path = GetDestination(i + 1, src.Length);
                IO.Move(src[i], path, true);
                dest.Add(path);
            }

            return dest;
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~FileTransfer
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~FileTransfer() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを解放します。
        /// </summary>
        ///
        /// <param name="disposing">
        /// マネージリソースを解放するかどうか
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing) => IO.TryDelete(WorkDirectory);

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetWorkDirectory
        ///
        /// <summary>
        /// 作業ディレクトリのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetWorkDirectory(string src) =>
            Enumerable.Range(1, int.MaxValue)
                      .Select(e => IO.Combine(src, e.ToString()))
                      .First(e => !IO.Get(e).Exists);

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
            GhostscriptFactory.GetDocumentFormats().Contains(Format) ?
            $"tmp{Information.Extension}" :
            $"tmp-%08d{Information.Extension}";

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
            var dest = GetDestinationCore(index, count);
            return (AutoRename && IO.Exists(dest)) ? IO.GetUniqueName(dest) : dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestinationCore
        ///
        /// <summary>
        /// 保存パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDestinationCore(int index, int count)
        {
            if (count <= 1) return Information.FullName;

            var name  = Information.NameWithoutExtension;
            var ext   = Information.Extension;
            var digit = string.Format("D{0}", Math.Max(count.ToString("D").Length, 2));
            var dest  = string.Format("{0}-{1}{2}", name, index.ToString(digit), ext);

            return IO.Combine(Information.DirectoryName, dest);
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        #endregion
    }
}
