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
using Cube.Forms;
using Cube.Log;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacade
    ///
    /// <summary>
    /// メイン処理を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainFacade : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainFacade
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="settings">設定情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainFacade(SettingsFolder settings)
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
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Value => Settings.Value;

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
            var format = Value.Format;
            var dest   = Value.Destination;
            var work   = Settings.WorkDirectory;

            this.LogDebug($"{nameof(Settings.WorkDirectory)}:{work}");

            using (var fs = new FileTransfer(format, dest, work, IO))
            {
                fs.AutoRename = Value.SaveOption == SaveOption.Rename;
                InvokeGhostscript(fs.Value);
                InvokeDecorator(fs.Value);
                InvokeTransfer(fs, out var paths);
                InvokePostProcess(paths);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateSource
        ///
        /// <summary>
        /// Source プロパティを更新します。
        /// </summary>
        ///
        /// <param name="e">ユーザの選択結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void UpdateSource(FileEventArgs e)
        {
            if (e.Result == DialogResult.Cancel) return;
            Value.Source = e.FileName;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateDestination
        ///
        /// <summary>
        /// Destination および Format プロパティを更新します。
        /// </summary>
        ///
        /// <param name="e">ユーザの選択結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void UpdateDestination(FileEventArgs e)
        {
            if (e.Result == DialogResult.Cancel) return;

            Debug.Assert(e.FilterIndex > 0);
            Debug.Assert(e.FilterIndex <= ViewResource.Formats.Count);

            Value.Destination = e.FileName;
            Value.Format = ViewResource.Formats[e.FilterIndex - 1].Value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateUserProgram
        ///
        /// <summary>
        /// UserProgram プロパティを更新します。
        /// </summary>
        ///
        /// <param name="e">ユーザの選択結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void UpdateUserProgram(FileEventArgs e)
        {
            if (e.Result == DialogResult.Cancel) return;
            Value.UserProgram = e.FileName;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateExtension
        ///
        /// <summary>
        /// Destination の拡張子を Format に応じて更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void UpdateExtension() => Value.Destination =
            IO.ChangeExtension(Value.Destination, Value.Format.GetExtension());

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~MainFacade
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~MainFacade() { Dispose(false); }

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
            Dispose(true);
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
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            Poll(10).Wait();
            IO.TryDelete(Settings.WorkDirectory);
            if (Value.DeleteSource) IO.TryDelete(Value.Source);
        }

        #endregion

        #endregion

        #region Implementations

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
                       .Aggregate("", (s, b) => s + $"{b:x2}");
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
                if (!Value.IsBusy) return;
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

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
                Value.IsBusy = true;
                action();
            }
            finally { Value.IsBusy = false; }
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
            if (!_disposed) action();
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
            var cmp = GetDigest(Value.Source);
            var opt = StringComparison.InvariantCultureIgnoreCase;
            if (!Settings.Digest.Equals(cmp, opt)) throw new CryptographicException();

            var gs = GhostscriptFactory.Create(Settings);
            gs.Invoke(Value.Source, dest);
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
            paths = !_disposed ? src.Invoke() : new string[0];
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

        #region Fields
        private bool _disposed = false;
        #endregion
    }
}
