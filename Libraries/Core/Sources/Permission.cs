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
using System.Runtime.CompilerServices;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Permission
    ///
    /// <summary>
    /// Represents permissions of various operations with the encrypted
    /// PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class Permission : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// Initializes a new instance of the Permission class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Permission() : this((long)PermissionFlag.All) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// Initializes a new instance of the Permission class with the
        /// specified value.
        /// </summary>
        ///
        /// <param name="src">Value for permissions.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Permission(long src)
        {
            var tmp  = src & (long)PermissionFlag.All;
            var dest = tmp | (long)PermissionFlag.Reserved;
            _flags = (PermissionFlag)dest;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Print
        ///
        /// <summary>
        /// Gets or sets a permission for printing.
        /// </summary>
        ///
        /// <remarks>
        /// The PermissionMethod.Restrict value may be used only in
        /// the Print property.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue Print
        {
            get => GetPrintPermission();
            set => SetPrintPermission(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyContents
        ///
        /// <summary>
        /// Gets or sets a permission for modifing the contents of the PDF
        /// document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue ModifyContents
        {
            get => GetModifyContentsPermission();
            set => SetModifyContentsPermission(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CopyContents
        ///
        /// <summary>
        /// Gets or sets a permission for coping or otherwise extracting
        /// text and graphics from the PDF document.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue CopyContents
        {
            get => Get(PermissionFlag.CopyOrExtractContents);
            set => Set(PermissionFlag.CopyOrExtractContents, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Accessibility
        ///
        /// <summary>
        /// Gets or sets a permission for extracting text and graphics
        /// in support of accessibility to users with disabilities or
        /// for other purposes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue Accessibility
        {
            get => Get(PermissionFlag.ExtractContentsForAccessibility);
            set => Set(PermissionFlag.ExtractContentsForAccessibility, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyAnnotations
        ///
        /// <summary>
        /// Gets or sets a permission for adding or modifing text
        /// annotations, fill in interactive form fields.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue ModifyAnnotations
        {
            get => Get(PermissionFlag.ModifyAnnotations);
            set => Set(PermissionFlag.ModifyAnnotations, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InputForm
        ///
        /// <summary>
        /// Gets or sets a permission for filling in existing interactive
        /// form fields (including signature fields).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionValue InputForm
        {
            get => Get(PermissionFlag.InputForm);
            set => Set(PermissionFlag.InputForm, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets or sets the raw value that represents permissions of
        /// all operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Value => (long)_flags;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the permission for the specified operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue Get(PermissionFlag src) =>
            _flags.HasFlag(src) ? PermissionValue.Allow : PermissionValue.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the permission for the specified operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue Get(PermissionFlag primary, PermissionFlag secondary) =>
            _flags.HasFlag(primary)   ? PermissionValue.Allow    :
            _flags.HasFlag(secondary) ? PermissionValue.Restrict : PermissionValue.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// GetPrintPermission
        ///
        /// <summary>
        /// Gets the permission for printing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue GetPrintPermission() =>
            Get(PermissionFlag.PrintHighQuality, PermissionFlag.Print);

        /* ----------------------------------------------------------------- */
        ///
        /// GetModifyContentsPermission
        ///
        /// <summary>
        /// Gets the permission for modifying contents.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionValue GetModifyContentsPermission() =>
            Get(PermissionFlag.ModifyContents, PermissionFlag.Assemble);

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the value of the specfied permission.
        /// </summary>
        ///
        /// <remarks>
        /// 各種プロパティの変更時に Value プロパティに対しても
        /// PropertyChanged イベントが発生します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private bool Set(ref PermissionFlag src, PermissionFlag value, string name)
        {
            var dest = SetProperty(ref src, value, name);
            if (dest) Refresh(nameof(Value));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the value of the specfied permission.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool Set(PermissionFlag src, PermissionValue value, [CallerMemberName] string name = null)
        {
            var dest = value.IsAllowed() ? (_flags | src) : (_flags & ~src);
            return Set(ref _flags, dest, name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetPrintPermission
        ///
        /// <summary>
        /// Sets the value for printing permission.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool SetPrintPermission(PermissionValue value)
        {
            var both = PermissionFlag.Print | PermissionFlag.PrintHighQuality;
            var dest = value.IsAllowed() ? (_flags | both) : (_flags & ~both);
            if (value == PermissionValue.Restrict) dest |= PermissionFlag.Print;
            return Set(ref _flags, dest, nameof(Print));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetModifyContentsPermission
        ///
        /// <summary>
        /// Sets the value for modifying contents permission.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool SetModifyContentsPermission(PermissionValue value)
        {
            var both = PermissionFlag.ModifyContents | PermissionFlag.Assemble;
            var dest = value.IsAllowed() ? (_flags | both) : (_flags & ~both);
            if (value == PermissionValue.Restrict) dest |= PermissionFlag.Assemble;
            return Set(ref _flags, dest, nameof(ModifyContents));
        }

        #endregion

        #region Fields
        private PermissionFlag _flags;
        #endregion
    }
}
