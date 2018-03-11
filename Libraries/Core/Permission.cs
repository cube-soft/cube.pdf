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

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Permission
    ///
    /// <summary>
    /// 暗号化されている PDF ファイルで許可されている権限を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Permission
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Permission()
        {
            _flags = PermissionFlags.All;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="value">許可状態を表す値</param>
        ///
        /* ----------------------------------------------------------------- */
        public Permission(long value)
        {
            var n0 = value & (long)PermissionFlags.All;
            var n1 = n0 | (long)PermissionFlags.Reserved;
            _flags = (PermissionFlags)n1;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Print
        ///
        /// <summary>
        /// 印刷操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// Print のみ Restrict と言う許可状態（低品質での印刷を許可）が
        /// 存在します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Print
        {
            get
            {
                return _flags.HasFlag(PermissionFlags.PrintHighQuality) ? PermissionMethod.Allow :
                       _flags.HasFlag(PermissionFlags.Print)            ? PermissionMethod.Restrict :
                                                                          PermissionMethod.Deny;
            }

            set
            {
                if (value == PermissionMethod.Allow) _flags |= PermissionFlags.PrintHighQuality;
                else
                {
                    _flags &= ~PermissionFlags.PrintHighQuality;
                    if (value == PermissionMethod.Restrict) _flags |= PermissionFlags.Print;
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Assemble
        ///
        /// <summary>
        /// 文書アセンブリ（ページの挿入、削除、回転、しおりとサムネイルの
        /// 作成）操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Assemble
        {
            get { return Get(PermissionFlags.Assemble); }
            set { Set(PermissionFlags.Assemble, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyContents
        ///
        /// <summary>
        /// 内容の編集操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod ModifyContents
        {
            get { return Get(PermissionFlags.ModifyContents); }
            set { Set(PermissionFlags.ModifyContents, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CopyContents
        ///
        /// <summary>
        /// 内容の選択/コピー操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod CopyContents
        {
            get { return Get(PermissionFlags.CopyContents); }
            set { Set(PermissionFlags.CopyContents, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Accessibility
        ///
        /// <summary>
        /// アクセシビリティ（視覚に障害を持つユーザに対して、読み上げ機能
        /// を提供する）のための内容抽出操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Accessibility
        {
            get { return Get(PermissionFlags.Accessibility); }
            set { Set(PermissionFlags.Accessibility, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyAnnotations
        ///
        /// <summary>
        /// 注釈の追加、編集操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod ModifyAnnotations
        {
            get { return Get(PermissionFlags.ModifyAnnotations); }
            set { Set(PermissionFlags.ModifyAnnotations, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FillInFormFields
        ///
        /// <summary>
        /// フォームフィールドへの入力操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod FillInFormFields
        {
            get { return Get(PermissionFlags.FillInFormFields); }
            set { Set(PermissionFlags.FillInFormFields, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 各種許可状態を表す値を取得します。
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
        /// 指定されたフラグの許可状態を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private PermissionMethod Get(PermissionFlags flag)
            => _flags.HasFlag(flag) ?
               PermissionMethod.Allow :
               PermissionMethod.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 指定されたフラグの許可状態を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(PermissionFlags flag, PermissionMethod method)
        {
            if (method == PermissionMethod.Allow) _flags |= flag;
            else _flags &= ~flag;
        }

        #endregion

        #region Fields
        private PermissionFlags _flags;
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PermissionMethod
    ///
    /// <summary>
    /// PDF への各種操作に対して設定されている許可状態を示す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum PermissionMethod
    {
        Deny,
        Restrict,
        Allow,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PermissionFlags
    ///
    /// <summary>
    /// 許可状態を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    internal enum PermissionFlags : uint
    {
        // 7, 8, 13-32 bit must be set
        Reserved            = 0xfffff0c0,
        All                 = 0xfffffffc,

        // Print the document (possibly not at the highest quality level,
        // depending on whether bit 12 is also set).
        Print               = 0x00000004,
        PrintHighQuality    = 0x00000800 | Print,

        // Modify the contents of the document by operations other than
        // those controlled by bits 6, 9, and 11.
        ModifyContents      = 0x00000008,

        // Copy or otherwise extract text and graphics from the document
        // by operations other than that controlled by bit 10.
        CopyContents        = 0x00000010,

        // Add or modify text annotations, fill in interactive form fields,
        // and, if bit 4 is also set, create or modify interactive
        // form fields (including signature fields).
        ModifyAnnotations   = 0x00000020,

        // Fill in existing interactive form fields (including signature
        // fields), even if bit 6 is clear.
        FillInFormFields    = 0x00000100,

        // Extract text and graphics (in support of accessibility to users
        // with disabilities or for other purposes).
        Accessibility       = 0x00000200,

        // Assemble the document (insert, rotate, or delete pages and
        // create bookmarks or thumbnail images), even if bit 4 is clear.
        Assemble            = 0x00000400,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PermissionOperations
    ///
    /// <summary>
    /// Permission クラスに関連する拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PermissionOperations
    {
        /* ----------------------------------------------------------------- */
        ///
        /// IsAllowed
        ///
        /// <summary>
        /// 許可状態かどうかを判別します。
        /// </summary>
        ///
        /// <param name="obj">PermissionMethod オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsAllowed(this PermissionMethod obj)
            => obj == PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// IsDenied
        ///
        /// <summary>
        /// 拒否状態かどうかを判別します。
        /// </summary>
        ///
        /// <param name="obj">PermissionMethod オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsDenied(this PermissionMethod obj)
            => obj == PermissionMethod.Deny;
    }
}
