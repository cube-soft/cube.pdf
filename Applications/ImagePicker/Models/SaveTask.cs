/* ------------------------------------------------------------------------- */
///
/// SaveTask.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using TaskEx = System.Threading.Tasks.Task;

namespace Cube.Pdf.ImageEx
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.SaveTask
    ///
    /// <summary>
    /// 抽出画像を保存する処理を非同期で実行するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SaveTask
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SaveTask
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public SaveTask() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Folder
        /// 
        /// <summary>
        /// 画像を保存するフォルダへのパスを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Folder { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        /// 
        /// <summary>
        /// 抽出画像の一覧を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IList<Image> Images { get; set; } = null;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// RunAsync
        /// 
        /// <summary>
        /// 保存処理を非同期で実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public async Task RunAsync(string basename, IEnumerable<int> indices)
        {
            if (Folder == null || Images == null || indices == null) return;

            foreach (var index in indices)
            {
                if (index < 0 || index >= Images.Count) continue;
                await SaveAsync(Images[index], basename, index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RunAsync
        /// 
        /// <summary>
        /// 保存処理を非同期で実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public async Task RunAsync(string basename)
        {
            if (Folder == null || Images == null) return;
            for (var i = 0; i < Images.Count; ++i) await SaveAsync(Images[i], basename, i);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AskFolder
        /// 
        /// <summary>
        /// 保存場所をユーザに尋ねて設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// ダイアログを表示してユーザに保存場所を尋ねます。ユーザが保存場所を
        /// 指定した場合は Folder にその値を設定します。ユーザが操作を
        /// キャンセルした場合は string.Empty が返ります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public string AskFolder(string initpath)
        {
            var dir = System.IO.Directory.Exists(initpath) ? initpath : System.IO.Path.GetDirectoryName(initpath);
            var dialog = new FolderBrowserDialog();
            dialog.Description = Properties.Resources.SaveFolder;
            dialog.SelectedPath = dir;
            if (dialog.ShowDialog() == DialogResult.Cancel) return string.Empty;

            Folder = dialog.SelectedPath;
            return Folder;
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAsync
        /// 
        /// <summary>
        /// 保存処理を非同期で実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private Task SaveAsync(Image src, string basename, int index)
        {
            return TaskEx.Run(() =>
            {
                var path = Unique(basename, index);
                src.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unique
        /// 
        /// <summary>
        /// 一意のパス名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string Unique(string basename, int index)
        {
            var digit = string.Format("D{0}", Images.Count.ToString("D").Length);
            for (var i = 1; i < 1000; ++i)
            {
                var filename = (i == 1) ?
                               string.Format("{0}-{1}.png", basename, index.ToString(digit)) :
                               string.Format("{0}-{1} ({2}).png", basename, index.ToString(digit), i);
                var dest = System.IO.Path.Combine(Folder, filename);
                if (!System.IO.File.Exists(dest)) return dest;
            }

            return System.IO.Path.Combine(Folder, System.IO.Path.GetRandomFileName());
        }

        #endregion
    }
}
