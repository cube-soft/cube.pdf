/* ------------------------------------------------------------------------- */
///
/// Permission.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
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
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Printing
        ///
        /// <summary>
        /// 印刷操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Printing { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// Assembly
        /// 
        /// <summary>
        /// 文書アセンブリ（ページの挿入、削除、回転、しおりとサムネイルの
        /// 作成）操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Assembly { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyContents
        /// 
        /// <summary>
        /// 内容の編集操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod ModifyContents { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// CopyContents
        /// 
        /// <summary>
        /// 内容の選択/コピー操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod CopyContents { get; set; } = PermissionMethod.Allow;

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
        public PermissionMethod Accessibility { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractPage
        /// 
        /// <summary>
        /// ページ抽出操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod ExtractPage { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyAnnotations
        /// 
        /// <summary>
        /// 注釈の追加、編集操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod ModifyAnnotations { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// InputFormFields
        /// 
        /// <summary>
        /// フォームフィールドへの入力操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod InputFormFields { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// Signature
        /// 
        /// <summary>
        /// 既存の署名フィールドへの署名操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod Signature { get; set; } = PermissionMethod.Allow;

        /* ----------------------------------------------------------------- */
        ///
        /// TemplatePage
        /// 
        /// <summary>
        /// コンテンツの動的な作成等に利用するテンプレートページの
        /// 作成操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod TemplatePage { get; set; } = PermissionMethod.Allow;

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
        Deny     = 0,
        Restrict = 1,
        Allow    = 2
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
