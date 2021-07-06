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
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using Cube.Mixin.Environment;
using Cube.Mixin.Observing;
using Cube.Mixin.String;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Provides binding properties and commands for the MainWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class MainViewModel : MainViewModelBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel() : this (
            new(Assembly.GetExecutingAssembly()) { AutoSave = true },
            SynchronizationContext.Current
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class
        /// with the specified settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder src, SynchronizationContext context) :
            base(new(src, context), new(), context)
        {
            var recent = Environment.SpecialFolder.Recent.GetName();
            var mon    = new DirectoryMonitor(recent, "*.pdf.lnk", GetDispatcher(false))
            {
                Enabled = src.Value.RecentVisible,
            };

            Ribbon = new(Facade, Aggregator, context);
            Recent = new(mon, Aggregator, context);
            Value.Query = new Query<string>(e => Send(new PasswordViewModel(e, context)));
            Recent.Open = GetOpenLinkCommand();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets data for binding to the MainWindow.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainBindable Value => Facade.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// Ribbon
        ///
        /// <summary>
        /// Gets the ViewModel for the Ribbon components.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RibbonViewModel Ribbon { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Recent
        ///
        /// <summary>
        /// Gets the ViewModel of the RecentCollection object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RecentViewModel Recent { get; }

        #endregion

        #region Commands

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Gets the Setup command.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Setup => Get(() => new DelegateCommand(
            () => Track(() => Facade.Setup(App.Arguments))
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop command to open a new PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand Open => Get(() => new DelegateCommand<string[]>(
            e => Track(() => Facade.Open(e.FirstPdf())),
            e => !Value.Busy && e.FirstPdf().HasValue()
        ).Associate(Value, nameof(Value.Busy)));

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOrMove
        ///
        /// <summary>
        /// Gets the Drag&amp;Drop command to insert or move items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICommand InsertOrMove => Get(() => new DelegateCommand<DragDropObject>(
            e => Track(() => Facade.InsertOrMove(e)),
            e => !Value.Busy && Value.Source != null &&
                 (!e.IsCurrentProcess || e.DropIndex - e.DragIndex != 0)
        ).Associate(Value, nameof(Value.Busy), nameof(Value.Source)));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the MainViewModel
        /// and optionally releases the managed resources.
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
            try { if (disposing) Ribbon.Dispose(); }
            finally { base.Dispose(disposing); }
        }

        #endregion
    }
}
