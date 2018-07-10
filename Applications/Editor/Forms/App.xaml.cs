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
using Cube.Log;
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// App
    ///
    /// <summary>
    /// メインプログラムを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class App : Application, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// App
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public App()
        {
            _dispose = new OnceAction<bool>(Dispose);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnStartup
        ///
        /// <summary>
        /// 起動時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnStartup(StartupEventArgs e)
        {
            Logger.Configure();
            Logger.Info(GetType(), Assembly.GetExecutingAssembly());

            _resources.Add(Logger.ObserveTaskException());
            _resources.Add(this.ObserveUiException());

            base.OnStartup(e);
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~App
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~App() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
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
        /// リソースを開放します。
        /// </summary>
        ///
        /// <param name="disposing">
        /// マネージオブジェクトを開放するかどうか
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var obj in _resources) obj.Dispose();
            }
        }

        #endregion

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly IList<IDisposable> _resources = new List<IDisposable>();
        #endregion
    }
}
