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
namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// IInstallable
    ///
    /// <summary>
    /// Represents the interface of installable devices.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IInstallable
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the target name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Environment
        ///
        /// <summary>
        /// Gets the name of architecture (Windows NT x86 or Windows x64).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string Environment { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// Gets the value indicating whether the target has been already
        /// installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        bool Exists { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// CanInstall
        ///
        /// <summary>
        /// Determines that the target can be installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        bool CanInstall();

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs the target.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Install();

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the target.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Uninstall();
    }
}
