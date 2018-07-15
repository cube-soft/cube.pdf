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
using Cube.FileSystem.TestService;
using Cube.Forms;
using Cube.Generics;
using Cube.Pdf.App.Converter;
using Cube.Pdf.Ghostscript;
using Cube.Pdf.Mixin;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Cube.Pdf.Tests.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// GlobalSetup
    ///
    /// <summary>
    /// NUnit で最初に実行する処理を記述するテストです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class ViewModelFixture : FileFixture
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        ///
        /// <summary>
        /// エラーメッセージを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Message { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateArgs
        ///
        /// <summary>
        /// プログラム引数を生成します。
        /// </summary>
        ///
        /// <param name="docName">ドキュメント名</param>
        ///
        /// <returns>プログラム引数一覧</returns>
        ///
        /// <remarks>
        /// /ThreadID, /Exec オプションは CubePDF メインプログラムでは
        /// 使用されません。テストではダミー値を設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected static string[] CreateArgs(string docName) => new[]
        {
            "/DeleteOnClose",
            "/DocumentName",
            docName,
            "/MachineName",
            Environment.MachineName,
            "/UserName",
            Environment.UserName,
            "/ThreadID",
            "15180",
            "/Exec",
            Assembly.GetExecutingAssembly().Location,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="src">設定情報</param>
        /// <param name="args">プログラム引数</param>
        ///
        /// <returns>テストケース</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData Create(Settings src, string[] args) =>
            Create(src, args, false);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="src">設定情報</param>
        /// <param name="args">プログラム引数</param>
        /// <param name="precopy">事前にコピーするかどうか</param>
        ///
        /// <returns>テストケース</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData Create(Settings src, string[] args, bool precopy) =>
            Create(src, args, "SampleMix.ps", precopy);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="src">設定情報</param>
        /// <param name="args">プログラム引数</param>
        /// <param name="filename">入力ファイル名</param>
        /// <param name="precopy">事前にコピーするかどうか</param>
        ///
        /// <returns>テストケース</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData Create(Settings src, string[] args,
            string filename, bool precopy) =>
            new TestCaseData(src, args, filename, precopy);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// SettingsFolder オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="args">プログラム引数一覧</param>
        ///
        /// <returns>SettingsFolder</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingsFolder Create(string[] args)
        {
            var path = $@"CubeSoft\CubePDF\{GetType().Name}";
            var dest = new SettingsFolder(DataContract.Format.Registry, path, IO)
            {
                WorkDirectory = GetResultsWith("Tmp"),
            };

            dest.Load();
            dest.Value.Destination = Results;
            dest.Set(args);

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Combine
        ///
        /// <summary>
        /// プログラム引数一覧に入力ファイルのパスを結合します。
        /// </summary>
        ///
        /// <param name="args">プログラム引数一覧</param>
        /// <param name="src">入力ファイル名</param>
        ///
        /// <returns>結合後のプログラム引数一覧</returns>
        ///
        /// <remarks>
        /// DeleteSource オプションのテストを考慮し、入力ファイルは
        /// 一時ファイルとしてコピーしていいます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected string[] Combine(string[] args, string src)
        {
            var tmp = GetResultsWith(Guid.NewGuid().ToString("D"));
            IO.Copy(GetExamplesWith(src), tmp);
            return args.Concat(new[] { "/InputFile", tmp }).ToArray();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// ViewModel に設定内容を反映させます。
        /// </summary>
        ///
        /// <param name="vm">ViewModel</param>
        /// <param name="src">設定内容</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Set(MainViewModel vm, Settings src)
        {
            Set(vm.Settings,   src);
            Set(vm.Metadata,   src.Metadata);
            Set(vm.Encryption, src.Encryption);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetMessage
        ///
        /// <summary>
        /// メッセージボックス表示時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetMessage(MessageEventArgs e)
        {
            Assert.That(e.Icon,
                Is.EqualTo(System.Windows.Forms.MessageBoxIcon.Error).Or
                  .EqualTo(System.Windows.Forms.MessageBoxIcon.Warning)
            );

            Message  = e.Message;
            e.Result = System.Windows.Forms.DialogResult.OK;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetUiCulture
        ///
        /// <summary>
        /// SetCulture イベント発生時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetUiCulture(string name) =>
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);

        /* ----------------------------------------------------------------- */
        ///
        /// Wait
        ///
        /// <summary>
        /// Waits for converting the document.
        /// </summary>
        ///
        /// <param name="vm">ViewModel</param>
        ///
        /// <returns>true for success.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected bool WaitConv(MainViewModel vm)
        {
            Message = string.Empty;
            vm.Convert();
            if (!Wait.For(() => vm.IsBusy == true)) return false;
            return Wait.For(() => vm.IsBusy == false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WaitMessage
        ///
        /// <summary>
        /// Waits for receiving a message.
        /// </summary>
        ///
        /// <param name="vm">ViewModel</param>
        ///
        /// <returns>true for receiving a message.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected bool WaitMessage(MainViewModel vm)
        {
            Message = string.Empty;
            vm.Convert();
            return Wait.For(() => Message.HasValue());
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// ViewModel に設定内容を反映させます。
        /// </summary>
        ///
        /// <remarks>
        /// 一部の値は無効化されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(SettingsViewModel vm, Settings src)
        {
            vm.Language          = src.Language;
            vm.Format            = src.Format;
            vm.FormatOption      = src.FormatOption;
            vm.SaveOption        = src.SaveOption;
            vm.Resolution        = src.Resolution;
            vm.Grayscale         = src.Grayscale;
            vm.ImageCompression  = src.ImageCompression;
            vm.IsAutoOrientation = src.Orientation == Orientation.Auto;
            vm.IsLandscape       = src.Orientation == Orientation.Landscape;
            vm.IsPortrait        = src.Orientation == Orientation.Portrait;
            vm.Linearization     = src.Linearization;
            vm.PostProcess       = src.PostProcess;
            vm.UserProgram       = src.UserProgram;
            vm.CheckUpdate       = src.CheckUpdate;

            // see remarks
            // vm.Source         = src.Source;
            // vm.Destination    = src.Destination;

            vm.PostProcess = src.PostProcess != PostProcess.Open &&
                             src.PostProcess != PostProcess.OpenDirectory ?
                             src.PostProcess :
                             PostProcess.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// ViewModel に設定内容を反映させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(MetadataViewModel vm, Metadata src)
        {
            vm.Title          = src.Title;
            vm.Author         = src.Author;
            vm.Subject        = src.Subject;
            vm.Keywords       = src.Keywords;
            vm.Creator        = src.Creator;
            vm.DisplayOptions = src.DisplayOptions;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// ViewModel に設定内容を反映させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(EncryptionViewModel vm, Encryption src)
        {
            vm.Enabled          = src.Enabled;
            vm.OwnerPassword    = src.OwnerPassword;
            vm.OwnerConfirm     = src.OwnerPassword;
            vm.OpenWithPassword = src.OpenWithPassword;
            vm.UserPassword     = src.UserPassword;
            vm.UserConfirm      = src.UserPassword;

            vm.AllowPrint       = src.Permission.Print.IsAllowed();
            vm.AllowCopy        = src.Permission.CopyContents.IsAllowed();
            vm.AllowInputForm   = src.Permission.InputForm.IsAllowed();
            vm.AllowModify      = src.Permission.ModifyContents.IsAllowed();
        }

        #endregion
    }
}
