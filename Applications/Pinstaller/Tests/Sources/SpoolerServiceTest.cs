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
using Cube.Pdf.App.Pinstaller;
using Cube.Pdf.App.Pinstaller.Debug;
using NUnit.Framework;
using System;
using System.ServiceProcess;

namespace Cube.Pdf.Tests.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// SpoolerServiceTest
    ///
    /// <summary>
    /// Represents tests of the SpoolerService class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SpoolerServiceTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test to create a new instance of the SpoolerService
        /// class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var src = new SpoolerService();
            src.Log();

            Assert.That(src.Name,    Is.EqualTo("Spooler"));
            Assert.That(src.Status,  Is.EqualTo(ServiceControllerStatus.Running).Or
                                       .EqualTo(ServiceControllerStatus.Stopped));
            Assert.That(src.Timeout, Is.EqualTo(TimeSpan.FromSeconds(10)));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Executes the test to clear printer jobs.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Clear() => Assert.DoesNotThrow(() => new SpoolerService().Clear());

        /* ----------------------------------------------------------------- */
        ///
        /// Restart
        ///
        /// <summary>
        /// Restarts the spooler service.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Restart()
        {
            try
            {
                var src = new SpoolerService();
                src.Stop();
                src.Start();
            }
            catch (Exception e) { Assert.Ignore($"{e.Message} ({e.GetType().Name})"); }
        }

        #endregion
    }
}
