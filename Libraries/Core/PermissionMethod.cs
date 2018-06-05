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
    /// PermissionMethod
    ///
    /// <summary>
    /// PDF への各種操作に対して設定されている許可状態を示す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum PermissionMethod
    {
        /// <summary>禁止</summary>
        Deny,
        /// <summary>一部制限</summary>
        Restrict,
        /// <summary>許可</summary>
        Allow,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PermissionFlags
    ///
    /// <summary>
    /// PDF への各操作に対する許可状態を表す列挙型です。
    /// </summary>
    ///
    /// <remarks>
    /// この列挙型は Permission オブジェクトの内部処理にのみ使用します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    internal enum PermissionFlags : uint
    {
        /// <summary>All operations are allowed.</summary>
        All = 0xfffffffc,

        /// <summary>
        /// 7, 8, 13-32 bit must be set
        /// </summary>
        Reserved = 0xfffff0c0,

        /// <summary>
        /// Print the document (possibly not at the highest quality level,
        /// depending on whether bit 12 is also set).
        /// </summary>
        Print = 0x00000004,

        /// <summary>
        /// Print the document at the highest quality level.
        /// </summary>
        PrintHighQuality = 0x00000800 | Print,

        /// <summary>
        /// Modify the contents of the document by operations other than
        /// those controlled by bits 6, 9, and 11.
        /// </summary>
        ModifyContents = 0x00000008,

        /// <summary>
        /// Copy or otherwise extract text and graphics from the document
        /// by operations other than that controlled by bit 10.
        /// </summary>
        CopyContents = 0x00000010,

        /// <summary>
        /// Add or modify text annotations, fill in interactive form fields,
        /// and, if bit 4 is also set, create or modify interactive
        /// form fields (including signature fields).
        /// </summary>
        ModifyAnnotations = 0x00000020,

        /// <summary>
        /// Fill in existing interactive form fields (including signature
        /// fields), even if bit 6 is clear.
        /// </summary>
        FillInFormFields = 0x00000100,

        /// <summary>
        /// Extract text and graphics (in support of accessibility to users
        /// with disabilities or for other purposes).
        /// </summary>
        Accessibility = 0x00000200,

        /// <summary>
        // Assemble the document (insert, rotate, or delete pages and
        // create bookmarks or thumbnail images), even if bit 4 is clear.
        /// </summary>
        Assemble = 0x00000400,
    }
}
