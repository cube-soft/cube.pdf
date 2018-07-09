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
using Cube.Xui;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
        /// <param name="text">表示テキスト取得用オブジェクト</param>
        /// <param name="name">アイコン名</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry(Func<string> text, [CallerMemberName] string name = null) :
            this(text, text, name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// RibbonEntry
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="text">表示テキスト取得用オブジェクト</param>
        /// <param name="tooltip">ツールチップ</param>
        /// <param name="name">アイコン名</param>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonEntry(Func<string> text, Func<string> tooltip, [CallerMemberName] string name = null)
        {
            _dispose     = new OnceAction<bool>(Dispose);
            _getText     = text;
            _getTooltip  = tooltip;
            _unsubscribe = ResourceCulture.Subscribe(() => RaisePropertyChanged(nameof(Text)));
            Name         = name;
            Enabled      = new BindableFunc<bool>(() => Command?.CanExecute(null) ?? true);
        }

        #endregion

        #region Properties

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
        /// Text
        ///
        /// <summary>
        /// 表示テキストを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text => _getText();

        /* ----------------------------------------------------------------- */
        ///
        /// Tooltip
        ///
        /// <summary>
        /// ツールチップ用テキストを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Tooltip => _getTooltip();

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// このオブジェクトが有効かどうかを判別するオブジェクトを取得
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public BindableFunc<bool> Enabled
        {
            get => _enabled;
            set => SetProperty(ref _enabled, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Command
        ///
        /// <summary>
        /// コマンドを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (_command == value) return;
                if (_command != null) _command.CanExecuteChanged -= WhenChanged;
                _command = value;
                if (_command != null) _command.CanExecuteChanged += WhenChanged;
                RaisePropertyChanged(nameof(Command));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LargeIcon
        ///
        /// <summary>
        /// 大きいサイズのアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string LargeIcon => $"{Assets}/Large/{GetName()}.png";

        /* ----------------------------------------------------------------- */
        ///
        /// SmallIcon
        ///
        /// <summary>
        /// 小さいサイズのアイコンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SmallIcon => $"{Assets}/Small/{GetName()}.png";

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
            if (disposing)
            {
                _unsubscribe.Dispose();
                if (_command != null) _command.CanExecuteChanged -= WhenChanged;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// アイコン名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetName() => IsEnabled() ? Name : $"{Name}Disable";

        /* ----------------------------------------------------------------- */
        ///
        /// IsEnabled
        ///
        /// <summary>
        /// 有効状態かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsEnabled()
        {
            if (!Enabled.Value) return false;
            return Command?.CanExecute(null) ?? true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenChanged
        ///
        /// <summary>
        /// ICommand.CanExecute の状態変化時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenChanged(object s, EventArgs e) => Enabled.RaiseValueChanged();

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly Func<string> _getText;
        private readonly Func<string> _getTooltip;
        private readonly IDisposable _unsubscribe;
        private ICommand _command;
        private BindableFunc<bool> _enabled;
        #endregion
    }
}
