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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProcessLauncher
    ///
    /// <summary>
    /// ポストプロセスを実行するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ProcessLauncher
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProcessLauncher
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="settings">設定情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public ProcessLauncher(SettingsFolder settings)
        {
            IO    = settings.IO;
            Value = settings.Value;
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
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Value { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// ポストプロセスを実行します。
        /// </summary>
        ///
        /// <param name="src">変換されたファイル一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(IEnumerable<string> src)
        {
            if (GetProcessMap().TryGetValue(Value.PostProcess, out var dest)) dest(src);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// ファイルを開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Open(IEnumerable<string> src) => InvokeCore(Create(src.First(), string.Empty));

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// ディレクトリを開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void OpenDirectory(IEnumerable<string> src) => InvokeCore(Create(
            "explorer.exe",
            IO.Get(src.First()).DirectoryName.Quote()
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeCore
        ///
        /// <summary>
        /// ユーザプログラムを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeCore(IEnumerable<string> src)
        {
            if (!Value.UserProgram.HasValue()) return;
            InvokeCore(Create(Value.UserProgram, src.First().Quote()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeCore
        ///
        /// <summary>
        /// プロセスを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeCore(ProcessStartInfo src) =>
            new Process { StartInfo = src }.Start();

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// プロセスを実行するためのオブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ProcessStartInfo Create(string exec, string args) => new ProcessStartInfo
        {
            FileName        = exec,
            Arguments       = args,
            CreateNoWindow  = false,
            UseShellExecute = true,
            LoadUserProfile = false,
            WindowStyle     = ProcessWindowStyle.Normal,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetProcessMap
        ///
        /// <summary>
        /// ポストプロセスと実行内容の対応関係一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDictionary<PostProcess, Action<IEnumerable<string>>> GetProcessMap() =>
            _processes ?? (_processes = new Dictionary<PostProcess, Action<IEnumerable<string>>>
            {
                { PostProcess.Open,          Open          },
                { PostProcess.OpenDirectory, OpenDirectory },
                { PostProcess.Others,        InvokeCore    },
            }
        );

        #endregion

        #region Fields
        private IDictionary<PostProcess, Action<IEnumerable<string>>> _processes;
        #endregion
    }
}
