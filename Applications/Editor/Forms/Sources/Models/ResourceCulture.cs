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

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ResourceCulture
    ///
    /// <summary>
    /// 表示言語の変更を通知するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ResourceCulture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 表示言語を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Set(string name)
        {
            var cmp = Properties.Resources.Culture.Name;
            if (name.Equals(cmp, StringComparison.InvariantCultureIgnoreCase)) return;
            lock (_lock)
            {
                foreach (var action in _subscriptions) action();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Subscribe
        ///
        /// <summary>
        /// 表示言語が変更された時に実行される動作を登録します。
        /// </summary>
        ///
        /// <param name="action">動作オブジェクト</param>
        ///
        /// <returns>購読解除用オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable Subscribe(Action action)
        {
            lock (_lock)
            {
                _subscriptions.Add(action);
                return Disposable.Create(() =>
                {
                    lock (_lock) _subscriptions.Remove(action);
                });
            }
        }

        #endregion

        #region Fields
        private static readonly object _lock = new object();
        private static readonly List<Action> _subscriptions = new List<Action>();
        #endregion
    }
}
