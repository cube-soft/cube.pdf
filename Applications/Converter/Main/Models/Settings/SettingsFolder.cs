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
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.App.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsFolder
    ///
    /// <summary>
    /// 各種設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsFolder : SettingsFolder<Settings>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder() : this(
            Cube.DataContract.Format.Registry,
            @"CubeSoft\CubePDF\v2"
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">設定情報の保存方法</param>
        /// <param name="path">設定情報の保存パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Cube.DataContract.Format format, string path) :
            base(format, path, Assembly.GetExecutingAssembly())
        {
            AutoSave = false;

            var asm = new AssemblyReader(Assembly.GetExecutingAssembly());
            Version.Digit  = 3;
            Version.Suffix = $"RC{asm.Version.Revision}";

            var dir = System.IO.Path.GetDirectoryName(asm.Location);
            Startup.Command = $"\"{System.IO.Path.Combine(dir, "cubepdf-checker.exe")}\"";
            Startup.Name    = "cubepdf-checker";
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoaded
        ///
        /// <summary>
        /// 読み込み時に実行されます。
        /// </summary>
        ///
        /// <remarks>
        /// 1.0.0RC12 より Resolution を ComboBox のインデックスに対応
        /// する値から直接の値に変更しました。これに伴い、インデックスを
        /// 指していると予想される値を初期値にリセットしています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoaded(ValueChangedEventArgs<Settings> e)
        {
            e.NewValue.Format = NormalizeFormat(e.NewValue);
            e.NewValue.Resolution = NormalizeResolution(e.NewValue);
            e.NewValue.Orientation = NormalizeOrientation(e.NewValue);
            e.NewValue.Metadata.Creator = Product;
            e.NewValue.Metadata.ViewOption = ViewOption.OneColumn;
            e.NewValue.Encryption.DenyAll();
            e.NewValue.Encryption.Permission.Accessibility = PermissionMethod.Allow;

            base.OnLoaded(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeFormat
        ///
        /// <summary>
        /// Format の値を正規化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Ghostscript.Format NormalizeFormat(Settings src) =>
            ViewResource.Formats.Any(e => e.Value == src.Format) ?
            src.Format :
            Ghostscript.Format.Pdf;

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeOrientation
        ///
        /// <summary>
        /// Orientation の値を正規化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Ghostscript.Orientation NormalizeOrientation(Settings src) =>
            ViewResource.Orientations.Any(e => e.Value == src.Orientation) ?
            src.Orientation :
            Ghostscript.Orientation.Auto;

        /* ----------------------------------------------------------------- */
        ///
        /// NormalizeResolution
        ///
        /// <summary>
        /// Resolution の値を正規化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int NormalizeResolution(Settings src) =>
            src.Resolution >= 72 ?
            src.Resolution :
            600;

        #endregion
    }
}
