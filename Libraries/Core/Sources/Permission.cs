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
using Cube.Pdf.Mixin;
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
    public class Permission : ObservableProperty
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
        public Permission() : this((long)PermissionFlags.All) { }

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
            var tmp  = src & (long)PermissionFlags.All;
            var dest = tmp | (long)PermissionFlags.Reserved;
            _flags = (PermissionFlags)dest;
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
        public PermissionMethod Print
        {
            get => GetPrintPermission();
            set => SetPrintPermission(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Assemble
        ///
        /// <summary>
        /// Get or sets a permission for assembling the PDF document
        /// (insert, rotate, or delete pages and create bookmarks or
        /// thumbnail images).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Assemble
        {
            get => Get(PermissionFlags.ModifyPagesAndBookmarks);
            set => Set(PermissionFlags.ModifyPagesAndBookmarks, value);
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
        public PermissionMethod ModifyContents
        {
            get => Get(PermissionFlags.ModifyContents);
            set => Set(PermissionFlags.ModifyContents, value);
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
        public PermissionMethod CopyContents
        {
            get => Get(PermissionFlags.CopyOrExtractContents);
            set => Set(PermissionFlags.CopyOrExtractContents, value);
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
        public PermissionMethod Accessibility
        {
            get => Get(PermissionFlags.ExtractContentsForAccessibility);
            set => Set(PermissionFlags.ExtractContentsForAccessibility, value);
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
        public PermissionMethod ModifyAnnotations
        {
            get => Get(PermissionFlags.ModifyAnnotations);
            set => Set(PermissionFlags.ModifyAnnotations, value);
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
        public PermissionMethod InputForm
        {
            get => Get(PermissionFlags.InputForm);
            set => Set(PermissionFlags.InputForm, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets or sets a value for all permissions.
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
        private PermissionMethod Get(PermissionFlags src) =>
            _flags.HasFlag(src) ? PermissionMethod.Allow : PermissionMethod.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// GetPrintPermission
        ///
        /// <summary>
        /// Gets the permission for printing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionMethod GetPrintPermission() =>
            _flags.HasFlag(PermissionFlags.PrintHighQuality) ? PermissionMethod.Allow :
            _flags.HasFlag(PermissionFlags.Print)            ? PermissionMethod.Restrict :
                                                               PermissionMethod.Deny;

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
        private bool Set(ref PermissionFlags src, PermissionFlags value, string name)
        {
            var dest = SetProperty(ref src, value, name);
            if (dest) RaisePropertyChanged(nameof(Value));
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
        private bool Set(PermissionFlags src, PermissionMethod method, [CallerMemberName] string name = null)
        {
            var dest = method.IsAllowed() ? (_flags | src) : (_flags & ~src);
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
        private bool SetPrintPermission(PermissionMethod method)
        {
            var dest = method.IsAllowed() ?
                       _flags |  PermissionFlags.PrintHighQuality :
                       _flags & ~PermissionFlags.PrintHighQuality ;
            if (method == PermissionMethod.Restrict) dest |= PermissionFlags.Print;
            return Set(ref _flags, dest, nameof(Print));
        }

        #endregion

        #region Fields
        private PermissionFlags _flags;
        #endregion
    }
}
