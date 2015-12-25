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
        /// 印刷操作が許可されているかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Printing { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Assembly
        /// 
        /// <summary>
        /// 文書アセンブリ（ページの挿入、削除、回転、しおりとサムネイルの
        /// 作成）操作が許可されているかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Assembly { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyContents
        /// 
        /// <summary>
        /// 内容の編集操作が許可されているかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ModifyContents { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// CopyContents
        /// 
        /// <summary>
        /// 内容の選択/コピー操作が許可されているかどうかを取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CopyContents { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Accessibility
        /// 
        /// <summary>
        /// アクセシビリティ（視覚に障害を持つユーザに対して、読み上げ機能
        /// を提供する）のための内容の抽出操作が許可されているかどうかを
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Accessibility { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractPage
        /// 
        /// <summary>
        /// ページの抽出操作が許可されているかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ExtractPage { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// ModifyAnnotations
        /// 
        /// <summary>
        /// 注釈の追加、編集操作が許可されているかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ModifyAnnotations { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// InputFormFields
        /// 
        /// <summary>
        /// フォームフィールドへの入力操作が許可されているかどうかを取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool InputFormFields { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// Signature
        /// 
        /// <summary>
        /// 既存の署名フィールドへの署名が許可されているかどうかを取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Signature { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// TemplatePage
        /// 
        /// <summary>
        /// コンテンツの動的な作成等に利用するテンプレートページの作成が
        /// 許可されているかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool TemplatePage { get; set; } = true;

        #endregion
    }
}
