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
using Cube.Pdf.Itext;
using Cube.Pdf.Mixin;
using System;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileDecorator
    ///
    /// <summary>
    /// Ghostscript API を用いて生成されたファイルに対して付随的な処理を
    /// 実行するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileDecorator
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileDecorator
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="settings">設定情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileDecorator(SettingsFolder settings)
        {
            Settings = settings;
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
        public IO IO => Settings.IO;

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Settings Value => Settings.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingsFolder Settings { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        ///
        /// <param name="src">生成されたファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(string src)
        {
            if (Value.Format != Ghostscript.Format.Pdf) return;

            InvokeItext(src);
            InvokeLinearization(src);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeItext
        ///
        /// <summary>
        /// iTextSharp による処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeItext(string src)
        {
            var tmp = IO.Combine(IO.Get(src).DirectoryName, Guid.NewGuid().ToString("D"));

            using (var writer = new DocumentWriter(IO))
            {
                var v = Value.FormatOption.GetVersion();
                Value.Metadata.Version  = v;
                Value.Encryption.Method = v.Minor >= 7 ? EncryptionMethod.Aes256 :
                                          v.Minor >= 6 ? EncryptionMethod.Aes128 :
                                          v.Minor >= 4 ? EncryptionMethod.Standard128 :
                                                         EncryptionMethod.Standard40;

                writer.Set(Value.Metadata);
                writer.Set(Value.Encryption);
                Add(writer, Value.Destination, SaveOption.MergeTail);
                writer.Add(new DocumentReader(src, string.Empty, false, IO));
                Add(writer, Value.Destination, SaveOption.MergeHead);
                writer.Save(tmp);
            }

            IO.Move(tmp, src, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeLinearization
        ///
        /// <summary>
        /// Web 表示用に最適化 (Linearization) を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeLinearization(string src)
        {
            if (!Value.Linearization || Value.Encryption.Enabled) return;

            var tmp = IO.Combine(IO.Get(src).DirectoryName, Guid.NewGuid().ToString("D"));
            var gs  = GhostscriptFactory.Create(Settings);
            gs.Options.Add(new Ghostscript.Argument('d', "FastWebView"));
            gs.Invoke(src, tmp);
            IO.Move(tmp, src, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Page オブジェクト一覧を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Add(DocumentWriter src, string path, SaveOption condition)
        {
            if (Value.SaveOption != condition || !IO.Exists(path)) return;

            var password = Value.Encryption.Enabled ?
                           Value.Encryption.OwnerPassword :
                           string.Empty;

            src.Add(new DocumentReader(path, password, true, IO));
        }

        #endregion
    }
}
