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
using Cube.Mixin.Collections;
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// Facade
    ///
    /// <summary>
    /// Represents the facade of converting operations.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class Facade : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Facade
        ///
        /// <summary>
        /// Initializes a new instance of the specified settings.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Facade(SettingsFolder settings)
        {
            Settings = settings;
            Locale.Set(settings.Value.Language);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => Settings.IO;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 設定を保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Settings.Save();

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert() => Invoke(() =>
        {
            var format = Settings.Value.Format;
            var dest   = Settings.Value.Destination;
            var work   = Settings.WorkDirectory;

            this.LogDebug($"{nameof(Settings.WorkDirectory)}:{work}");

            using (var fs = new FileTransfer(format, dest, work, IO))
            {
                fs.AutoRename = Settings.Value.SaveOption == SaveOption.Rename;
                InvokeGhostscript(fs.Value);
                InvokeDecorator(fs.Value);
                InvokeTransfer(fs, out var paths);
                InvokePostProcess(paths);
            }
        });

        #region Set

        /* ----------------------------------------------------------------- */
        ///
        /// SetSource
        ///
        /// <summary>
        /// Source プロパティを更新します。
        /// </summary>
        ///
        /// <param name="e">ユーザの選択結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetSource(OpenFileMessage e)
        {
            if (!e.Cancel) Settings.Value.Source = e.Value.First();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Destination および Format プロパティを更新します。
        /// </summary>
        ///
        /// <param name="e">ユーザの選択結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetDestination(SaveFileMessage e)
        {
            if (e.Cancel) return;

            Debug.Assert(e.FilterIndex > 0);
            Debug.Assert(e.FilterIndex <= ViewResource.Formats.Count);

            Settings.Value.Destination = e.Value;
            Settings.Value.Format = ViewResource.Formats[e.FilterIndex - 1].Value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetUserProgram
        ///
        /// <summary>
        /// UserProgram プロパティを更新します。
        /// </summary>
        ///
        /// <param name="e">ユーザの選択結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetUserProgram(OpenFileMessage e)
        {
            if (!e.Cancel) Settings.Value.UserProgram = e.Value.First();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetExtension
        ///
        /// <summary>
        /// Destination の拡張子を Format に応じて更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SetExtension()
        {
            var fi  = IO.Get(Settings.Value.Destination);
            var ext = Settings.Value.Format.GetExtension();
            Settings.Value.Destination = IO.Combine(fi.DirectoryName, $"{fi.BaseName}{ext}");
        }

        #endregion

        #endregion

        #region Implementations

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
        protected override void Dispose(bool disposing)
        {
            Poll(10).Wait();
            IO.TryDelete(Settings.WorkDirectory);
            if (Settings.Value.DeleteSource) IO.TryDelete(Settings.Value.Source);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDigest
        ///
        /// <summary>
        /// Gets the message digest of the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDigest(string src)
        {
            using (var stream = IO.OpenRead(src))
            {
                return new SHA256CryptoServiceProvider()
                    .ComputeHash(stream)
                    .Join("", b => $"{b:x2}");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Poll
        ///
        /// <summary>
        /// 実行が終了するまで非同期で待機します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async Task Poll(int sec)
        {
            for (var i = 0; i < sec; ++i)
            {
                if (!Settings.Value.Busy) return;
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        #region Invoke

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        ///
        /// <remarks>
        /// 処理中は IsBusy プロパティが true に設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action)
        {
            try
            {
                Settings.Value.Busy = true;
                action();
            }
            finally { Settings.Value.Busy = false; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeUnlessDisposed
        ///
        /// <summary>
        /// Dispose 前に限り処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeUnlessDisposed(Action action)
        {
            if (!Disposed) action();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeGhostscript
        ///
        /// <summary>
        /// Ghostscript API を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeGhostscript(string dest) => InvokeUnlessDisposed(() =>
        {
            var cmp = GetDigest(Settings.Value.Source);
            if (!Settings.Digest.FuzzyEquals(cmp)) throw new CryptographicException();

            var gs = GhostscriptFactory.Create(Settings);
            gs.Invoke(Settings.Value.Source, dest);
            gs.LogDebug();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeDecorator
        ///
        /// <summary>
        /// Ghostscript API で生成されたファイルに対して付随的な処理を
        /// 実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeDecorator(string dest) =>
            InvokeUnlessDisposed(() => new FileDecorator(Settings).Invoke(dest));

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeTransfer
        ///
        /// <summary>
        /// 作業フォルダに生成されたファイルの移動処理を実行します。
        /// </summary>
        ///
        /// <remarks>
        /// out 引数の都合で、このメソッドのみ InvokeUnlessDisposed に
        /// 相当する処理を直接記述しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeTransfer(FileTransfer src, out IEnumerable<string> paths)
        {
            paths = !Disposed ? src.Invoke() : new string[0];
            foreach (var f in paths) this.LogDebug($"Save:{f}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokePostProcess
        ///
        /// <summary>
        /// ポストプロセスを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokePostProcess(IEnumerable<string> dest) =>
            InvokeUnlessDisposed(() => new ProcessLauncher(Settings).Invoke(dest));

        #endregion

        #endregion
    }
}
