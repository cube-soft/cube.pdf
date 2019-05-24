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
using Cube.Mixin.String;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// Initializes a new instance of the specified assembly.
        /// </summary>
        ///
        /// <param name="assembly">Assembly object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Facade(Assembly assembly) : this(new SettingsFolder(assembly)) { }

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
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => Settings.IO;

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
        /// Convert
        ///
        /// <summary>
        /// Invokes the conversion with the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Convert()
        {
            try
            {
                Settings.Value.Busy = true;
                var dest = new List<string>();
                using (var fs = new FileTransfer(Settings, GetTemp()))
                {
                    InvokeGhostscript(fs.Value);
                    Invoke(() => new FileDecorator(Settings).Invoke(fs.Value));
                    Invoke(() => fs.Invoke(dest));
                    Invoke(() => new ProcessLauncher(Settings).Invoke(dest));
                }
                Results = dest;
            }
            finally { Settings.Value.Busy = false; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => Settings.Save();

        /* ----------------------------------------------------------------- */
        ///
        /// ChangeExtension
        ///
        /// <summary>
        /// Changes the extension of the Destination property based on the
        /// Format property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void ChangeExtension()
        {
            var src = IO.Get(Settings.Value.Destination);
            var ext = Settings.Value.Format.GetExtension();
            if (src.Extension.FuzzyEquals(ext)) return;
            Settings.Value.Destination = IO.Combine(src.DirectoryName, $"{src.BaseName}{ext}");
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
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            Poll(10).Wait();
            IO.TryDelete(GetTemp());
            if (Settings.Value.DeleteSource) IO.TryDelete(Settings.Value.Source);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action unless the object is not Disposed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action action) { if (!Disposed) action(); }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeGhostscript
        ///
        /// <summary>
        /// Invokes the Ghostscript API.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeGhostscript(string dest) => Invoke(() =>
        {
            var src = Settings.Digest;
            var cmp = GetDigest(Settings.Value.Source);
            if (src.HasValue() && !src.FuzzyEquals(cmp)) throw new CryptographicException();

            var gs = GhostscriptFactory.Create(Settings);
            try { gs.Invoke(Settings.Value.Source, dest); }
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
        private string GetTemp() => IO.Combine(Settings.Value.Temp, Settings.Uid.ToString("D"));

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
        /// Waits until the any operations are terminated.
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

        #endregion
    }
}
