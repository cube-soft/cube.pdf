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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// Facade
    ///
    /// <summary>
    /// Represents the facade of operations in the CubePDF.
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
        /// Initializes a new instance of the specified assembly.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Facade(Assembly assembly) : this(new SettingFolder(assembly)) { }

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
        public Facade(SettingFolder settings) { Setting = settings; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Setting
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Setting { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        ///
        /// <summary>
        /// Gets the collectioin of created files.
        /// </summary>
        ///
        /// <remarks>
        /// 変換形式に PNG などを指定した場合、複数のファイルを生成する関係で
        /// 保存パスとして指定したものとは異なる名前のファイルが生成される事が
        /// あります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Results { get; private set; } = Enumerable.Empty<string>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes operations with the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke()
        {
            lock (_lock)
            {
                try
                {
                    Setting.Value.Busy = true;
                    var dest = new List<string>();
                    using (var fs = new FileTransfer(Setting, GetTemp()))
                    {
                        Run(() => new DigestChecker(Setting).Invoke());
                        RunGhostscript(fs.Value);
                        Run(() => new FileDecorator(Setting).Invoke(fs.Value));
                        Run(() => fs.Invoke(dest));
                        Run(() => new ProcessLauncher(Setting).Invoke(dest));
                    }
                    Results = dest;
                }
                finally { Setting.Value.Busy = false; }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /// <remarks>
        /// 別スレッドで変換処理中の場合、一時ファイルの削除に失敗する可能性が
        /// あるので Invoke と Dispose との間で排他制御を挿入しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            lock (_lock)
            {
                Setting.IO.TryDelete(GetTemp());
                if (!Setting.Value.DeleteSource) return;
                Setting.IO.TryDelete(Setting.Value.Source);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        ///
        /// <summary>
        /// Invokes the specified action unless the object is not Disposed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Run(Action action) { if (!Disposed) action(); }

        /* ----------------------------------------------------------------- */
        ///
        /// RunGhostscript
        ///
        /// <summary>
        /// Invokes the Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RunGhostscript(string dest) => Run(() =>
        {
            var gs = GhostscriptFactory.Create(Setting);
            try { gs.Invoke(Setting.Value.Source, dest); }
            finally { gs.LogDebug(); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetTemp
        ///
        /// <summary>
        /// Gets the temp directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTemp() => Setting.IO.Combine(Setting.Value.Temp, Setting.Uid.ToString("D"));

        #endregion

        #region Fields
        private readonly object _lock = new object();
        #endregion
    }
}
