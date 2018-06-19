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
using Cube.Pdf.Ghostscript;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public class MainFacade
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
            Settings.PropertyChanged += WhenPropertyChanged;
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
        public void Convert()
        {
            try
            {
                Value.IsBusy = true;

                var fs = new FileTransfer(Value.Format, Value.Destination, IO)
                {
                    AutoRename = Value.SaveOption == SaveOption.Rename,
                };

                InvokeGhostscript(fs.Value);
                InvokeDecorator(fs.Value);
                var dest = fs.Invoke();
                InvokePostProcess(dest);
            }
            finally { Value.IsBusy = false; }
        }

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

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPropertyChanged
        ///
        /// <summary>
        /// プロパティ変更時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.Value.Format)) UpdateExtension();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateExtension
        ///
        /// <summary>
        /// 拡張子を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateExtension() =>
            Value.Destination = IO.ChangeExtension(Value.Destination, Value.Format.GetExtension());

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeGhostscript
        ///
        /// <summary>
        /// Ghostscript API を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeGhostscript(string dest) =>
            GhostscriptFactory.Create(Settings).Invoke(Value.Source, dest);

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
            new FileDecorator(Settings).Invoke(dest);

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
            new ProcessLauncher(Settings).Invoke(dest);

        #endregion
    }
}
