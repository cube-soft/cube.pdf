/* ------------------------------------------------------------------------- */
///
/// Bootstrap.cs
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Lifetime;
using log4net;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Bootstrap
    ///
    /// <summary>
    /// プロセス間通信 (IPC: Inter-Process Communication) によって
    /// プロセスの起動およびアクティブ化を行うためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// 二重起動を抑止したい時に、二重起動する代わりに既に起動している
    /// 同名プロセスをアクティブ化します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Bootstrap : IDisposable
    {
        #region Constructors and the destructor

        /* ----------------------------------------------------------------- */
        ///
        /// Bootstrap
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Bootstrap(string name)
        {
            Name = name;
            Logger = LogManager.GetLogger(GetType());
            _mutex = new System.Threading.Mutex(false, Name);
            _core.Received += (s, e) => OnActivated(e);

            LifetimeServices.LeaseTime = TimeSpan.Zero;
            LifetimeServices.RenewOnCallTime = TimeSpan.Zero;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IpcBootstrap
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~Bootstrap()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        /// 
        /// <summary>
        /// プロセス間通信の際の識別子となる名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Logger
        /// 
        /// <summary>
        /// ログ出力用オブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ILog Logger { get; set; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Activated
        /// 
        /// <summary>
        /// 他のプロセスからアクティブ化された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler<DataEventArgs<object>> Activated;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        /// 
        /// <summary>
        /// 同じ名前を持つプロセスが存在するかどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists()
        {
            return !_mutex.WaitOne(0, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Activate
        /// 
        /// <summary>
        /// 既に起動しているプロセスをアクティブ化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Activate(object args = null)
        {
            try
            {
                var client = new IpcClientChannel();
                ChannelServices.RegisterChannel(client, true);
                var channel = String.Format("ipc://{0}/{1}", Name, _ActivateCommand);
                var proxy = Activator.GetObject(typeof(IpcProxy), channel) as IpcProxy;
                if (proxy != null) proxy.Send(args);
            }
            catch (Exception err) { Logger.Error(err); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        /// 
        /// <summary>
        /// 他のプロセスからメッセージを受け取るための登録を行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Register()
        {
            try
            {
                var server = new IpcServerChannel(Name);
                ChannelServices.RegisterChannel(server, true);
                RemotingServices.Marshal(_core, _ActivateCommand, typeof(IpcProxy));
            }
            catch (Exception err) { Logger.Error(err); }
        }

        #endregion

        #region Methods for IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                _mutex.Close();
            }
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnActivated
        /// 
        /// <summary>
        /// 他のプロセスからアクティブ化された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnActivated(DataEventArgs<object> e)
        {
            if (Activated != null) Activated(this, e);
        }

        #endregion

        #region Internal class

        public class IpcProxy : MarshalByRefObject
        {
            public event EventHandler<DataEventArgs<object>> Received;

            public void Send(object args)
            {
                if (Received != null) Received(this, new DataEventArgs<object>(args));
            }
        }

        #endregion

        #region Fields
        private bool _disposed = false;
        private System.Threading.Mutex _mutex = null;
        private IpcProxy _core = new IpcProxy();
        #endregion

        #region Constant fields
        private static readonly string _ActivateCommand = "activate";
        #endregion
    }
}
