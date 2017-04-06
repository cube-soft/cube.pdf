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
using System.ComponentModel;
using System.Windows.Forms;
using Cube.Forms.Bindings;
using Cube.Log;

namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ClipPresenter
    ///
    /// <summary>
    /// ClipSource と View を関連付けるためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ClipPresenter : Cube.Forms.PresenterBase<IClipView, ClipSource>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ClipPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ClipPresenter(IClipView view)
            : base(view, new ClipSource(), new EventAggregator())
        {
            // View
            View.EventAggregator = EventAggregator;
            View.DataSource = Model.Clips.ToBindingSource();

            // Model
            Model.PropertyChanged += WhenModelChanged;
            Model.Locked          += WhenLocked;

            // EventAggregator
            EventAggregator?.GetEvents()?.Open.Subscribe(WhenOpen);
            EventAggregator?.GetEvents()?.Attach.Subscribe(WhenAttach);
            EventAggregator?.GetEvents()?.Detach.Subscribe(WhenDetach);
            EventAggregator?.GetEvents()?.Reset.Subscribe(WhenReset);
            EventAggregator?.GetEvents()?.Save.Subscribe(WhenSave);
            EventAggregator?.GetEvents()?.Message.Subscribe(WhenMessage);
            EventAggregator?.GetEvents()?.Error.Subscribe(WhenError);
        }

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        /// 
        /// <param name="disposing">
        /// マネージオブジェクトを開放するかどうかを示す値
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) Model.Dispose();
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// WhenModelChanged
        /// 
        /// <summary>
        /// Model のプロパティが変化した時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenModelChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Model.Source)) return;
            Sync(() => View.Source = Model.Source?.File.FullName ?? string.Empty);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenLocked
        /// 
        /// <summary>
        /// ファイルがロックされている時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenLocked(object sender, ValueCancelEventArgs<string> e)
        {
            var result = SyncWait(() => Views.ShowMessage(
                string.Format(
                    Properties.Resources.MessageLock,
                    System.IO.Path.GetFileName(e.Value)
                ), MessageBoxButtons.RetryCancel));
            e.Cancel = (result == DialogResult.Cancel);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenOpen
        /// 
        /// <summary>
        /// Open イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenOpen(string[] files)
            => Async(() =>
        {
            foreach (var file in files)
            {
                try
                {
                    Model.Open(file);
                    break;
                }
                catch (EncryptionException err)
                {
                    this.LogError(err.Message, err);
                    EventAggregator?.GetEvents()?.Message.Publish(Properties.Resources.MessageEncryption);
                    break;
                }
                catch (Exception err) { this.LogWarn(err.Message, err); }
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// WhenAttach
        /// 
        /// <summary>
        /// Attach イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenAttach(string[] files)
            => Async(() => Model.Attach(files));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDetach
        /// 
        /// <summary>
        /// Detach イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDetach()
        {
            var indices = SyncWait(() => View.SelectedIndices);
            Async(() => Model.Detach(indices));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenReset
        /// 
        /// <summary>
        /// Reset イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenReset()
            => Async(() => Model.Reset());

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSave
        /// 
        /// <summary>
        /// Save イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async void WhenSave()
        {
            try
            {
                SyncWait(() => View.IsBusy = true);
                await Async(() => Model.Save());
                EventAggregator?.GetEvents()?.Message.Publish(Properties.Resources.MessageSuccess);
            }
            catch (Exception err)
            {
                this.LogError(err.Message, err);
                EventAggregator?.GetEvents()?.Error.Publish(err.Message);
            }
            finally { SyncWait(() => View.IsBusy = false); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenMessage
        /// 
        /// <summary>
        /// Message イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenMessage(string message)
            => Sync(() => Views.ShowMessage(message));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenError
        /// 
        /// <summary>
        /// Error イベント発生時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenError(string message)
            => Sync(() => Views.ShowError(message));

        #endregion

        #region Fields
        private bool _disposed = false;
        #endregion
    }
}
