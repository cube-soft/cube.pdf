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

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonEntry
    ///
    /// <summary>
    /// Ribbon の項目を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RibbonEntry : ObservableProperty, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RibbonEntry
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="name">アイコン名</param>
        /// <param name="getter">表示テキスト取得用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry(string name, Func<string> getter)
        {
            _dispose     = new OnceAction<bool>(Dispose);
            _get         = getter;
            _unsubscribe = ResourceCulture.Subscribe(() => RaisePropertyChanged(nameof(Text)));
            Name         = name;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Assets
        ///
        /// <summary>
        /// アイコンが格納されている場所を示す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected static string Assets { get; } = "pack://application:,,,/Assets";

        /* ----------------------------------------------------------------- */
        ///
        /// Text
        ///
        /// <summary>
        /// 表示テキストを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text => _get();

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// アイコン名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// LargeIcon
        ///
        /// <summary>
        /// 大きいサイズのアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string LargeIcon => $"{Assets}/Large/{Name}.png";

        /* ----------------------------------------------------------------- */
        ///
        /// SmallIcon
        ///
        /// <summary>
        /// 小さいサイズのアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SmallIcon => $"{Assets}/Small/{Name}.png";

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ~RibbonEntry
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~RibbonEntry() { _dispose.Invoke(false); }

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
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _unsubscribe.Dispose();
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly Func<string> _get;
        private readonly IDisposable _unsubscribe;
        #endregion
    }
}
