﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.Globalization;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests.Presenters
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonTest
    ///
    /// <summary>
    /// Tests the RibbonViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RibbonTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetText_English
        ///
        /// <summary>
        /// Checks the display text in English.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetText_English()
        {
            using var vm = NewVM();
            vm.Value.Settings.Language = Language.English;
            var dest = vm.Ribbon;

            Assert.That(dest.File.Text,          Is.EqualTo("File"));
            Assert.That(dest.Edit.Text,          Is.EqualTo("Edit"));
            Assert.That(dest.Others.Text,        Is.EqualTo("Others"));
            Assert.That(dest.Open.Text,          Is.EqualTo("Open"));
            Assert.That(dest.Save.Text,          Is.EqualTo("Save"));
            Assert.That(dest.Close.Text,         Is.EqualTo("Close"));
            Assert.That(dest.Exit.Text,          Is.EqualTo("Exit"));
            Assert.That(dest.Undo.Text,          Is.EqualTo("Undo"));
            Assert.That(dest.Redo.Text,          Is.EqualTo("Redo"));
            Assert.That(dest.Select.Text,        Is.EqualTo("Select"));
            Assert.That(dest.SelectAll.Text,     Is.EqualTo("Select all"));
            Assert.That(dest.SelectFlip.Text,    Is.EqualTo("Invert selection"));
            Assert.That(dest.SelectClear.Text,   Is.EqualTo("Deselect all"));
            Assert.That(dest.Insert.Text,        Is.EqualTo("Insert"));
            Assert.That(dest.InsertFront.Text,   Is.EqualTo("Insert at the beginning"));
            Assert.That(dest.InsertBack.Text,    Is.EqualTo("Insert at the end"));
            Assert.That(dest.InsertOthers.Text,  Is.EqualTo("Insert at other position"));
            Assert.That(dest.Extract.Text,       Is.EqualTo("Extract"));
            Assert.That(dest.ExtractOthers.Text, Is.EqualTo("Extract with other settings"));
            Assert.That(dest.Remove.Text,        Is.EqualTo("Remove"));
            Assert.That(dest.RemoveOthers.Text,  Is.EqualTo("Remove other pages"));
            Assert.That(dest.MovePrevious.Text,  Is.EqualTo("Back"));
            Assert.That(dest.MoveNext.Text,      Is.EqualTo("Forth"));
            Assert.That(dest.RotateLeft.Text,    Is.EqualTo("Left"));
            Assert.That(dest.RotateRight.Text,   Is.EqualTo("Right"));
            Assert.That(dest.Metadata.Text,      Is.EqualTo("Metadata"));
            Assert.That(dest.Encryption.Text,    Is.EqualTo("Security"));
            Assert.That(dest.Redraw.Text,        Is.EqualTo("Refresh"));
            Assert.That(dest.ZoomIn.Text,        Is.EqualTo("ZoomIn"));
            Assert.That(dest.ZoomOut.Text,       Is.EqualTo("ZoomOut"));
            Assert.That(dest.Setting.Text,       Is.EqualTo("Settings"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTooltip_English
        ///
        /// <summary>
        /// Checks the tooltip in English.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetTooltip_English()
        {
            using var vm = NewVM();
            vm.Value.Settings.Language = Language.English;
            var dest = vm.Ribbon;

            Assert.That(dest.File.Tooltip,          Is.EqualTo("File"));
            Assert.That(dest.Edit.Tooltip,          Is.EqualTo(dest.Edit.Text));
            Assert.That(dest.Others.Tooltip,        Is.EqualTo(dest.Others.Text));
            Assert.That(dest.Open.Tooltip,          Is.EqualTo(dest.Open.Text));
            Assert.That(dest.Save.Tooltip,          Is.EqualTo("Save"));
            Assert.That(dest.Close.Tooltip,         Is.EqualTo(dest.Close.Text));
            Assert.That(dest.Exit.Tooltip,          Is.EqualTo(dest.Exit.Text));
            Assert.That(dest.Undo.Tooltip,          Is.EqualTo(dest.Undo.Text));
            Assert.That(dest.Redo.Tooltip,          Is.EqualTo(dest.Redo.Text));
            Assert.That(dest.Select.Tooltip,        Is.EqualTo(dest.Select.Text));
            Assert.That(dest.SelectAll.Tooltip,     Is.EqualTo(dest.SelectAll.Text));
            Assert.That(dest.SelectFlip.Tooltip,    Is.EqualTo(dest.SelectFlip.Text));
            Assert.That(dest.SelectClear.Tooltip,   Is.EqualTo(dest.SelectClear.Text));
            Assert.That(dest.Insert.Tooltip,        Is.EqualTo("Insert behind selected position"));
            Assert.That(dest.InsertFront.Tooltip,   Is.EqualTo(dest.InsertFront.Text));
            Assert.That(dest.InsertBack.Tooltip,    Is.EqualTo(dest.InsertBack.Text));
            Assert.That(dest.InsertOthers.Tooltip,  Is.EqualTo(dest.InsertOthers.Text));
            Assert.That(dest.Extract.Tooltip,       Is.EqualTo("Extract the selected pages"));
            Assert.That(dest.ExtractOthers.Tooltip, Is.EqualTo(dest.ExtractOthers.Text));
            Assert.That(dest.Remove.Tooltip,        Is.EqualTo("Remove the selected pages"));
            Assert.That(dest.RemoveOthers.Tooltip,  Is.EqualTo(dest.RemoveOthers.Text));
            Assert.That(dest.MovePrevious.Tooltip,  Is.EqualTo(dest.MovePrevious.Text));
            Assert.That(dest.MoveNext.Tooltip,      Is.EqualTo(dest.MoveNext.Text));
            Assert.That(dest.RotateLeft.Tooltip,    Is.EqualTo(dest.RotateLeft.Text));
            Assert.That(dest.RotateRight.Tooltip,   Is.EqualTo(dest.RotateRight.Text));
            Assert.That(dest.Metadata.Tooltip,      Is.EqualTo("PDF document metadata"));
            Assert.That(dest.Encryption.Tooltip,    Is.EqualTo(dest.Encryption.Text));
            Assert.That(dest.Redraw.Tooltip,        Is.EqualTo(dest.Redraw.Text));
            Assert.That(dest.ZoomIn.Tooltip,        Is.EqualTo(dest.ZoomIn.Text));
            Assert.That(dest.ZoomOut.Tooltip,       Is.EqualTo(dest.ZoomOut.Text));
            Assert.That(dest.Setting.Tooltip,       Is.EqualTo(dest.Setting.Text));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetText_Japanese
        ///
        /// <summary>
        /// Checks the display text in Japanese.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetText_Japanese()
        {
            using var vm = NewVM();
            vm.Value.Settings.Language = Language.Japanese;
            var dest = vm.Ribbon;

            Assert.That(dest.File.Text,          Is.EqualTo("ファイル"));
            Assert.That(dest.Edit.Text,          Is.EqualTo("編集"));
            Assert.That(dest.Others.Text,        Is.EqualTo("その他"));
            Assert.That(dest.Open.Text,          Is.EqualTo("開く"));
            Assert.That(dest.Save.Text,          Is.EqualTo("保存"));
            Assert.That(dest.Close.Text,        Is.EqualTo("閉じる"));
            Assert.That(dest.Exit.Text,          Is.EqualTo("終了"));
            Assert.That(dest.Undo.Text,          Is.EqualTo("元に戻す"));
            Assert.That(dest.Redo.Text,          Is.EqualTo("やり直し"));
            Assert.That(dest.Select.Text,        Is.EqualTo("選択"));
            Assert.That(dest.SelectAll.Text,     Is.EqualTo("すべて選択"));
            Assert.That(dest.SelectFlip.Text,    Is.EqualTo("選択の切り替え"));
            Assert.That(dest.SelectClear.Text,   Is.EqualTo("選択を解除"));
            Assert.That(dest.Insert.Text,        Is.EqualTo("挿入"));
            Assert.That(dest.InsertFront.Text,   Is.EqualTo("先頭に挿入"));
            Assert.That(dest.InsertBack.Text,    Is.EqualTo("末尾に挿入"));
            Assert.That(dest.InsertOthers.Text,  Is.EqualTo("詳細を設定して挿入"));
            Assert.That(dest.Extract.Text,       Is.EqualTo("抽出"));
            Assert.That(dest.ExtractOthers.Text, Is.EqualTo("詳細を設定して抽出"));
            Assert.That(dest.Remove.Text,        Is.EqualTo("削除"));
            Assert.That(dest.RemoveOthers.Text,  Is.EqualTo("範囲を指定して削除"));
            Assert.That(dest.MovePrevious.Text,  Is.EqualTo("前へ"));
            Assert.That(dest.MoveNext.Text,      Is.EqualTo("後へ"));
            Assert.That(dest.RotateLeft.Text,    Is.EqualTo("左90度"));
            Assert.That(dest.RotateRight.Text,   Is.EqualTo("右90度"));
            Assert.That(dest.Metadata.Text,      Is.EqualTo("プロパティ"));
            Assert.That(dest.Encryption.Text,    Is.EqualTo("セキュリティ"));
            Assert.That(dest.Redraw.Text,        Is.EqualTo("更新"));
            Assert.That(dest.ZoomIn.Text,        Is.EqualTo("拡大"));
            Assert.That(dest.ZoomOut.Text,       Is.EqualTo("縮小"));
            Assert.That(dest.Setting.Text,       Is.EqualTo("設定"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTooltip_English
        ///
        /// <summary>
        /// Checks the tooltip in Japanese.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetTooltip_Japanese()
        {
            using var vm = NewVM();
            vm.Value.Settings.Language = Language.Japanese;
            var dest = vm.Ribbon;

            Assert.That(dest.File.Tooltip,          Is.EqualTo(dest.File.Text));
            Assert.That(dest.Edit.Tooltip,          Is.EqualTo(dest.Edit.Text));
            Assert.That(dest.Others.Tooltip,        Is.EqualTo(dest.Others.Text));
            Assert.That(dest.Open.Tooltip,          Is.EqualTo(dest.Open.Text));
            Assert.That(dest.Save.Tooltip,          Is.EqualTo("上書き保存"));
            Assert.That(dest.Close.Tooltip,         Is.EqualTo(dest.Close.Text));
            Assert.That(dest.Exit.Tooltip,          Is.EqualTo(dest.Exit.Text));
            Assert.That(dest.Undo.Tooltip,          Is.EqualTo(dest.Undo.Text));
            Assert.That(dest.Redo.Tooltip,          Is.EqualTo(dest.Redo.Text));
            Assert.That(dest.Select.Tooltip,        Is.EqualTo(dest.Select.Text));
            Assert.That(dest.SelectAll.Tooltip,     Is.EqualTo(dest.SelectAll.Text));
            Assert.That(dest.SelectFlip.Tooltip,    Is.EqualTo(dest.SelectFlip.Text));
            Assert.That(dest.SelectClear.Tooltip,   Is.EqualTo(dest.SelectClear.Text));
            Assert.That(dest.Insert.Tooltip,        Is.EqualTo("選択位置の後に挿入"));
            Assert.That(dest.InsertFront.Tooltip,   Is.EqualTo(dest.InsertFront.Text));
            Assert.That(dest.InsertBack.Tooltip,    Is.EqualTo(dest.InsertBack.Text));
            Assert.That(dest.InsertOthers.Tooltip,  Is.EqualTo(dest.InsertOthers.Text));
            Assert.That(dest.Extract.Tooltip,       Is.EqualTo("選択ページを抽出"));
            Assert.That(dest.ExtractOthers.Tooltip, Is.EqualTo(dest.ExtractOthers.Text));
            Assert.That(dest.Remove.Tooltip,        Is.EqualTo("選択ページを削除"));
            Assert.That(dest.RemoveOthers.Tooltip,  Is.EqualTo(dest.RemoveOthers.Text));
            Assert.That(dest.MovePrevious.Tooltip,  Is.EqualTo(dest.MovePrevious.Text));
            Assert.That(dest.MoveNext.Tooltip,      Is.EqualTo(dest.MoveNext.Text));
            Assert.That(dest.RotateLeft.Tooltip,    Is.EqualTo(dest.RotateLeft.Text));
            Assert.That(dest.RotateRight.Tooltip,   Is.EqualTo(dest.RotateRight.Text));
            Assert.That(dest.Metadata.Tooltip,      Is.EqualTo("PDF 文書プロパティ"));
            Assert.That(dest.Encryption.Tooltip,    Is.EqualTo(dest.Encryption.Text));
            Assert.That(dest.Redraw.Tooltip,        Is.EqualTo(dest.Redraw.Text));
            Assert.That(dest.ZoomIn.Tooltip,        Is.EqualTo(dest.ZoomIn.Text));
            Assert.That(dest.ZoomOut.Tooltip,       Is.EqualTo(dest.ZoomOut.Text));
            Assert.That(dest.Setting.Tooltip,       Is.EqualTo(dest.Setting.Text));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetText_Dynamically
        ///
        /// <summary>
        /// Checks the behavior when the display language changes
        /// dynamically.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetText_Dynamically()
        {
            using var vm = NewVM();
            var dest = vm.Ribbon;

            vm.Value.Settings.Language = Language.English;
            Assert.That(dest.Open.Text,    Is.EqualTo("Open"), "en");
            Assert.That(dest.Open.Tooltip, Is.EqualTo(dest.Open.Text), "en");

            vm.Value.Settings.Language = Language.Japanese;
            Assert.That(dest.Open.Text,    Is.EqualTo("開く"), "ja");
            Assert.That(dest.Open.Tooltip, Is.EqualTo(dest.Open.Text), "ja");

            vm.Value.Settings.Language = Language.French;
            Assert.That(dest.Open.Text,    Is.EqualTo("Open"), "fr");
            Assert.That(dest.Open.Tooltip, Is.EqualTo(dest.Open.Text), "fr");

            vm.Value.Settings.Language = Language.Auto;
            Assert.That(dest.Open.Text,    Is.Not.Null.And.Not.Empty, "empty");
            Assert.That(dest.Open.Tooltip, Is.EqualTo(dest.Open.Text), "empty");
        }

        #endregion
    }
}
