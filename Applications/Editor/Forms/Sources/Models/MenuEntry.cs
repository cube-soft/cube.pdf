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
using Cube.Xui.Mixin;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MenuEntry
    ///
    /// <summary>
    /// Represents the components of a menu that has text, tooltip, and
    /// command.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MenuEntry : ObservableProperty, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MenuEntry
        ///
        /// <summary>
        /// Initializes a new instance of the MenuEntry class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="text">Delegation for getting text.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry(Func<string> text) : this(text, text) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MenuEntry
        ///
        /// <summary>
        /// Initializes a new instance of the MenuEntry class with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="text">Delegation for getting text.</param>
        /// <param name="tooltip">Delegation for getting tooltip.</param>
        ///
        /* ----------------------------------------------------------------- */
        public MenuEntry(Func<string> text, Func<string> tooltip)
        {
            _dispose    = new OnceAction<bool>(Dispose);
            _getText    = text;
            _getTooltip = tooltip;
            _registry   = ResourceCulture.Subscribe(() => RaisePropertyChanged(nameof(Text)));
            Enabled     = new BindableFunc<bool>(() => Command?.CanExecute() ?? true);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Text
        ///
        /// <summary>
        /// Gets the display text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text => _getText();

        /* ----------------------------------------------------------------- */
        ///
        /// Tooltip
        ///
        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Tooltip => _getTooltip();

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// Gets the object indicating whether the related view component
        /// is enabled.
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
        /// Gets the command to be executed when clicking the related
        /// view component.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (_command == value) return;
                if (_command != null) _command.CanExecuteChanged -= WhenCanExecuteChanged;
                _command = value;
                if (_command != null) _command.CanExecuteChanged += WhenCanExecuteChanged;
                RaisePropertyChanged(nameof(Command));
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ~MenuEntry
        ///
        /// <summary>
        /// Finalizes the MenuEntry.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~MenuEntry() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the class.
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
        /// Releases the unmanaged resources used by the class and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _registry.Dispose();
                if (_command != null) _command.CanExecuteChanged -= WhenCanExecuteChanged;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCanExecuteChanged
        ///
        /// <summary>
        /// Occurs when the CanExecuteChanged event of the Command
        /// property is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnCanExecuteChanged(EventArgs e) =>
            Enabled.RaiseValueChanged();

        /* ----------------------------------------------------------------- */
        ///
        /// IsEnabled
        ///
        /// <summary>
        /// Returns whether the related view component is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected bool IsEnabled()
        {
            if (!Enabled.Value) return false;
            return Command?.CanExecute() ?? true;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WhenCanExecuteChanged
        ///
        /// <summary>
        /// Occurs when the CanExecuteChanged event of the Command
        /// property is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenCanExecuteChanged(object s, EventArgs e) => OnCanExecuteChanged(e);

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly Func<string> _getText;
        private readonly Func<string> _getTooltip;
        private readonly IDisposable _registry;
        private ICommand _command;
        private BindableFunc<bool> _enabled;
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ImageMenuEntry
    ///
    /// <summary>
    /// Represents the components of a menu that has text, tooltip,
    /// command, and two sizes of icons.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageMenuEntry : MenuEntry
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageMenuEntry
        ///
        /// <summary>
        /// Initializes a new instance of the ImageMenuEntry class
        /// with the specified parameters.
        /// </summary>
        ///
        /// <param name="text">Delegation for getting text.</param>
        /// <param name="name">Name for icons.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageMenuEntry(Func<string> text, [CallerMemberName] string name = null) :
            this(text, text, name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageMenuEntry
        ///
        /// <summary>
        /// Initializes a new instance of the ImageMenuEntry class
        /// with the specified parameters.
        /// </summary>
        ///
        /// <param name="text">Delegation for getting text.</param>
        /// <param name="tooltip">Delegation for getting tooltip.</param>
        /// <param name="name">Name for icons.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageMenuEntry(Func<string> text, Func<string> tooltip,
            [CallerMemberName] string name = null) : base(text, tooltip)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the name for icons.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// LargeIcon
        ///
        /// <summary>
        /// Gets the file path of large size icon with the WPF URI format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string LargeIcon => $"{Assets}/Large/{GetName()}.png";

        /* ----------------------------------------------------------------- */
        ///
        /// SmallIcon
        ///
        /// <summary>
        /// Gets the file path of small size icon with the WPF URI format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SmallIcon => $"{Assets}/Small/{GetName()}.png";

        /* ----------------------------------------------------------------- */
        ///
        /// Assets
        ///
        /// <summary>
        /// Gets the root path of icon files with the WPF URI format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected static string Assets { get; } = "pack://application:,,,/Assets";

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnCanExecuteChanged
        ///
        /// <summary>
        /// Occurs when the CanExecuteChanged event of the Command
        /// property is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCanExecuteChanged(EventArgs e)
        {
            RaisePropertyChanged(nameof(SmallIcon));
            RaisePropertyChanged(nameof(LargeIcon));
            base.OnCanExecuteChanged(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// Gets a name of icons.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetName() => IsEnabled() ? Name : $"{Name}Disable";

        #endregion
    }
}
