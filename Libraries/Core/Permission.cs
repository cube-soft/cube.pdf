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
using Cube.Enumerations;
using System.Runtime.CompilerServices;

namespace Cube.Pdf
{
    /* --------------------------------------------------------------------- */
    ///
    /// Permission
    ///
    /// <summary>
    /// 暗号化されている PDF ファイルで許可されている操作を表すクラスです。
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
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Permission() : this((long)PermissionFlags.All) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Permission
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">許可状態を表す値</param>
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
            get => GetPrintPermission();
            set => SetPrintPermission(value);
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
            get => Get(PermissionFlags.Assemble);
            set => Set(PermissionFlags.Assemble, value);
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
            get => Get(PermissionFlags.ModifyContents);
            set => Set(PermissionFlags.ModifyContents, value);
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
            get => Get(PermissionFlags.CopyContents);
            set => Set(PermissionFlags.CopyContents, value);
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
            get => Get(PermissionFlags.Accessibility);
            set => Set(PermissionFlags.Accessibility, value);
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
            get => Get(PermissionFlags.ModifyAnnotations);
            set => Set(PermissionFlags.ModifyAnnotations, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InputForms
        ///
        /// <summary>
        /// フォームフィールドへの入力操作の許可設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PermissionMethod InputForms
        {
            get => Get(PermissionFlags.InputForms);
            set => Set(PermissionFlags.InputForms, value);
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
        private PermissionMethod Get(PermissionFlags src) =>
            _flags.HasFlag(src) ? PermissionMethod.Allow : PermissionMethod.Deny;

        /* ----------------------------------------------------------------- */
        ///
        /// GetPrintPermission
        ///
        /// <summary>
        /// 印刷に関する許可状態を取得します。
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
        /// プロパティに値を設定します。
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
        /// 指定されたフラグの許可状態を設定します。
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
        /// 印刷に関する許可状態を設定します。
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
