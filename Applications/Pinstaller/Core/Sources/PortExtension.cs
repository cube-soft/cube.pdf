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
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// PortExtension
    ///
    /// <summary>
    /// Represents extended methods of Port and PortConfig classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PortExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Creates a collection of ports from the specified configuration.
        /// </summary>
        ///
        /// <param name="src">Port configuration.</param>
        ///
        /// <returns>Collection of ports.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Port> Convert(this IEnumerable<PortConfig> src) =>
            src.Select(e => e.Convert());

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Creates a new instance of the Port class  from the specified
        /// configuration.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Port Convert(this PortConfig src) =>
            new Port(src.Name, src.MonitorName)
            {
                Application = src.Application,
                Arguments   = src.Arguments,
                Proxy       = src.Proxy,
                Temp        = src.Temp,
                WaitForExit = src.WaitForExit,
                RunAsUser   = src.RunAsUser,
            };

        #endregion
    }
}
