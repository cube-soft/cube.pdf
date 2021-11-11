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
using System;
using Cube.DataContract;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Encryption
    ///
    /// <summary>
    /// Represents an encryption information of the PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class Encryption : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the PDF document is
        /// encrypted with password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Enabled
        {
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWithPassword
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the password is
        /// requested when opening the PDF document.
        /// </summary>
        ///
        /// <remarks>
        /// If OpenWithPassword is true, you will need to enter the
        /// OwnerPassword or UserPassword when opening the PDF file.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool OpenWithPassword
        {
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OwnerPassword
        ///
        /// <summary>
        /// Gets or sets an owner password.
        /// </summary>
        ///
        /// <remarks>
        /// The owner password is the master password that is set for the
        /// PDF file, and it enables all operations such as re-encryption
        /// and changing various permissions.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string OwnerPassword
        {
            get => Get(() => string.Empty);
            set => Set(value ?? string.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UserPassword
        ///
        /// <summary>
        /// Gets or sets a user password.
        /// </summary>
        ///
        /// <remarks>
        /// The user password represents the password required to open the
        /// PDF file.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string UserPassword
        {
            get => Get(() => string.Empty);
            set => Set(value ?? string.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets or sets an encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod Method
        {
            get => Get(() => EncryptionMethod.Unknown);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// Gets or sets permissions of various operations with the
        /// encrypted PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Permission Permission
        {
            get => Get(() => new Permission());
            set => Set(value);
        }

        #endregion
    }
}
