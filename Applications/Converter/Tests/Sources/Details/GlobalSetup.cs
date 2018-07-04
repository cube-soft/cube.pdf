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
using NUnit.Framework;
using System.Reflection;

namespace Cube.Pdf.Tests.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// GlobalSetup
    ///
    /// <summary>
    /// NUnit で最初に実行する処理を記述するテストです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [SetUpFixture]
    public class GlobalSetup
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OneTimeSetup
        ///
        /// <summary>
        /// 一度だけ実行される初期化処理です。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Logger.Configure();
            Logger.ObserveTaskException();
            Logger.Info(typeof(GlobalSetup), Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}