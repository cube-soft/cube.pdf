/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Tests;
using Cube.Mixin.Logging;
using NUnit.Framework;
using System;
using System.ServiceProcess;

namespace Cube.Pdf.Pinstaller.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// DeviceFixture
    ///
    /// <summary>
    /// Provides functionality to test device installing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class DeviceFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Executes in each test.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [SetUp]
        public virtual void Setup()
        {
            var service = new SpoolerService();
            if (service.Status != ServiceControllerStatus.Running) service.Start();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /// <remarks>
        /// 実行権限がない場合のテスト結果は無視します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected void Invoke(Action action)
        {
            try { action(); }
            catch (Exception err)
            {
                if (err is InvalidOperationException ||
                    err is UnauthorizedAccessException) this.LogWarn($"{err.Message} ({err.GetType().Name})");
                else throw;
            }
        }

        #endregion
    }
}
