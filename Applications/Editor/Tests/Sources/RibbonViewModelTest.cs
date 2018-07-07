/* ------------------------------------------------------------------------- */
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
using Cube.Pdf.App.Editor;
using NUnit.Framework;

namespace CubePdf.Tests.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// RibbonViewModel
    ///
    /// <summary>
    /// RibbonViewModel のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RibbonViewModelTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetText_English
        ///
        /// <summary>
        /// 英語の表示テキストを確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetText_English()
        {
            var dest = new RibbonViewModel();
            ResourceCulture.Set("en");

            Assert.That(dest.Open.Text,         Is.EqualTo("Open"));
            Assert.That(dest.Save.Text,         Is.EqualTo("Save"));
            Assert.That(dest.Close.Text,        Is.EqualTo("Close"));
            Assert.That(dest.Undo.Text,         Is.EqualTo("Undo"));
            Assert.That(dest.Redo.Text,         Is.EqualTo("Redo"));
            Assert.That(dest.Select.Text,       Is.EqualTo("Select"));
            Assert.That(dest.Insert.Text,       Is.EqualTo("Add"));
            Assert.That(dest.Extract.Text,      Is.EqualTo("Pick"));
            Assert.That(dest.Remove.Text,       Is.EqualTo("Remove"));
            Assert.That(dest.MovePrevious.Text, Is.EqualTo("Prev"));
            Assert.That(dest.MoveNext.Text,     Is.EqualTo("Next"));
            Assert.That(dest.RotateLeft.Text,   Is.EqualTo("Left"));
            Assert.That(dest.RotateRight.Text,  Is.EqualTo("Right"));
            Assert.That(dest.Metadata.Text,     Is.EqualTo("Metadata"));
            Assert.That(dest.Encryption.Text,   Is.EqualTo("Security"));
            Assert.That(dest.Refresh.Text,      Is.EqualTo("Refresh"));
            Assert.That(dest.ZoomIn.Text,       Is.EqualTo("ZoomIn"));
            Assert.That(dest.ZoomOut.Text,      Is.EqualTo("ZoomOut"));
            Assert.That(dest.Version.Text,      Is.EqualTo("Version"));
            Assert.That(dest.Web.Text,          Is.EqualTo("Web"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetText_Japanese
        ///
        /// <summary>
        /// 日本語の表示テキストを確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetText_Japanese()
        {
            var dest = new RibbonViewModel();
            ResourceCulture.Set("ja");

            Assert.That(dest.Open.Text,         Is.EqualTo("開く"));
            Assert.That(dest.Save.Text,         Is.EqualTo("保存"));
            Assert.That(dest.Close.Text,        Is.EqualTo("閉じる"));
            Assert.That(dest.Undo.Text,         Is.EqualTo("元に戻す"));
            Assert.That(dest.Redo.Text,         Is.EqualTo("やり直し"));
            Assert.That(dest.Select.Text,       Is.EqualTo("選択"));
            Assert.That(dest.Insert.Text,       Is.EqualTo("挿入"));
            Assert.That(dest.Extract.Text,      Is.EqualTo("抽出"));
            Assert.That(dest.Remove.Text,       Is.EqualTo("削除"));
            Assert.That(dest.MovePrevious.Text, Is.EqualTo("前へ"));
            Assert.That(dest.MoveNext.Text,     Is.EqualTo("後へ"));
            Assert.That(dest.RotateLeft.Text,   Is.EqualTo("左90度"));
            Assert.That(dest.RotateRight.Text,  Is.EqualTo("右90度"));
            Assert.That(dest.Metadata.Text,     Is.EqualTo("文書プロパティ"));
            Assert.That(dest.Encryption.Text,   Is.EqualTo("セキュリティ"));
            Assert.That(dest.Refresh.Text,      Is.EqualTo("更新"));
            Assert.That(dest.ZoomIn.Text,       Is.EqualTo("拡大"));
            Assert.That(dest.ZoomOut.Text,      Is.EqualTo("縮小"));
            Assert.That(dest.Version.Text,      Is.EqualTo("バージョン"));
            Assert.That(dest.Web.Text,          Is.EqualTo("Web"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetText_Dynamically
        ///
        /// <summary>
        /// 表示言語が動的に変更する時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetText_Dynamically()
        {
            var dest = new RibbonViewModel();

            ResourceCulture.Set("en");
            Assert.That(dest.Open.Text, Is.EqualTo("Open"), "en");

            ResourceCulture.Set("ja");
            Assert.That(dest.Open.Text, Is.EqualTo("開く"), "ja");

            ResourceCulture.Set("fr");
            Assert.That(dest.Open.Text, Is.EqualTo("Open"), "fr");

            ResourceCulture.Set("ja-jp");
            Assert.That(dest.Open.Text, Is.EqualTo("開く"), "ja");

            ResourceCulture.Set(string.Empty);
            Assert.That(dest.Open.Text, Is.Not.Null.And.Not.Empty, "empty");

            ResourceCulture.Set(null);
            Assert.That(dest.Open.Text, Is.Not.Null.And.Not.Empty, "null");
        }

        #endregion
    }
}
