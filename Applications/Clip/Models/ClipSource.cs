/* ------------------------------------------------------------------------- */
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cube.Log;

namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipSource
    /// 
    /// <summary>
    /// PDF ファイルおよび添付ファイル一覧を管理するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ClipSource : ObservableProperty, IDisposable
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        /// 
        /// <summary>
        /// 添付元の PDF ファイルを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDocumentReader Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clips
        /// 
        /// <summary>
        /// 添付ファイル一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ObservableCollection<ClipItem> Clips { get; }
            = new ObservableCollection<ClipItem>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// PDF ファイルを読み込みます。
        /// </summary>
        /// 
        /// <param name="src">PDF ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Open(string src)
        {
            Close();
            Source = new Cube.Pdf.Editing.DocumentReader(src);
            Reset();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// ファイルの添付状況を PDF ファイルを読み込んだ直後の状態に
        /// リセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            Clips.Clear();
            var msg = Properties.Resources.ConditionEmbedded;
            foreach (var item in Source.Attachments)
            {
                Clips.Add(new ClipItem(item) { Condition = msg });
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// PDF ファイルを上書き保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save()
        {
            if (Source == null || !Source.IsOpen) return;

            var dest  = Source.File.FullName;
            var tmp   = System.IO.Path.GetTempFileName();
            var items = Clips.Select(x => x.RawObject)
                             .Where(x => System.IO.File.Exists(x.File.FullName));

            using (var writer = new Cube.Pdf.Editing.DocumentWriter())
            {
                writer.Metadata = Source.Metadata;
                writer.Encryption = Source.Encryption;
                writer.UseSmartCopy = true;
                writer.Add(Source.Pages);
                writer.Attach(items);

                System.IO.File.Delete(tmp);
                writer.Save(tmp);
            }

            Close();
            System.IO.File.Copy(tmp, dest, true);
            System.IO.File.Delete(tmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// 新しいファイルを添付します。
        /// </summary>
        /// 
        /// <param name="files">添付ファイル一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(IEnumerable<string> files)
        {
            foreach (var file in files) Attach(file);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// 新しいファイルを添付します。
        /// </summary>
        /// 
        /// <param name="file">添付ファイル</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Attach(string file)
        {
            try { if (!System.IO.File.Exists(file)) return; }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                return;
            }

            if (Clips.Any(x => x.RawObject.File.FullName == file)) return;

            var item = new Attachment
            {
                Name = System.IO.Path.GetFileName(file),
                File = new File(file)
            };

            Clips.Insert(0, new ClipItem(item)
            {
                Condition = Properties.Resources.ConditionNew
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// 添付ファイルを削除します。
        /// </summary>
        /// 
        /// <param name="indices">
        /// 削除する添付ファイルのインデックス一覧
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach(IEnumerable<int> indices)
        {
            foreach (var index in indices.OrderByDescending(x => x))
            {
                Detach(index);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// 添付ファイルを削除します。
        /// </summary>
        /// 
        /// <param name="index">削除する添付ファイルのインデックス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach(int index)
        {
            if (index < 0 || index >= Clips.Count) return;
            Clips.RemoveAt(index);
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ClipSource
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        /// 
        /// <remarks>
        /// Dispose(bool) にアンマネージリソースを解放するコードが含まれる
        /// 場合にのみ、ファイナライザーをオーバーライドします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        //~ClipSource()
        //{
        //    Dispose(false);
        //}

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        /// 
        /// <remarks>
        /// クリーンアップコードを Dispose(bool) に記述します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);

            // TODO: ファイナライザーがオーバーライドされる場合は、
            // 次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        /// 
        /// <param name="disposing">
        /// マネージオブジェクトを開放するかどうかを表す値
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) Close();
                _disposed = true;
            }
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// PDF ファイルを閉じます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Close()
        {
            Clips.Clear();
            Source?.Dispose();
            Source = null;
        }

        #region Fields
        private bool _disposed = false;
        private IDocumentReader _source = null;
        #endregion

        #endregion
    }
}
