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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using Cube.FileSystem;
using Cube.Mixin.Collections;
using Cube.Mixin.String;
using Cube.Pdf.Converter.Mixin;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Converter.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewModelFixture
    ///
    /// <summary>
    /// Provides functionality to test ViewModel classes.
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
        /// Gets the message displayed in a common dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Message { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetArgs
        ///
        /// <summary>
        /// Gets the program arguments.
        /// </summary>
        ///
        /// <param name="docName">Document name.</param>
        ///
        /// <returns>Collection of arguments.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static IEnumerable<string> GetArgs(string docName) => new[]
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
        /// <param name="id">テスト ID</param>
        /// <param name="src">設定情報</param>
        /// <param name="args">プログラム引数</param>
        ///
        /// <returns>テストケース</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData Create(int id, SettingValue src, IEnumerable<string> args) =>
            Create(id, src, args, false);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="id">テスト ID</param>
        /// <param name="src">設定情報</param>
        /// <param name="args">プログラム引数</param>
        /// <param name="precopy">事前にコピーするかどうか</param>
        ///
        /// <returns>テストケース</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData Create(int id,
            SettingValue src,
            IEnumerable<string> args,
            bool precopy
        ) => Create(id, src, args, "SampleMix.ps", precopy);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /// <param name="id">テスト ID</param>
        /// <param name="src">設定情報</param>
        /// <param name="args">プログラム引数</param>
        /// <param name="filename">入力ファイル名</param>
        /// <param name="precopy">事前にコピーするかどうか</param>
        ///
        /// <returns>テストケース</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData Create(int id,
            SettingValue src,
            IEnumerable<string> args,
            string filename,
            bool precopy
        ) => new(id, src, args, filename, precopy);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// SettingFolder オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="args">プログラム引数一覧</param>
        ///
        /// <returns>SettingFolder</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingFolder Create(IEnumerable<string> args)
        {
            var asm  = typeof(MainWindow).Assembly;
            var fmt  = DataContract.Format.Registry;
            var path = $@"CubeSoft\CubePDF\{GetType().Name}";
            var dest = new SettingFolder(asm, fmt, path);

            dest.Load();
            dest.Normalize();
            dest.Value.Destination = Results;
            dest.Value.Temp = Get("Temp");
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
        protected IEnumerable<string> Combine(IEnumerable<string> args, string src)
        {
            var tmp = Get(Guid.NewGuid().ToString("N"));
            Io.Copy(GetSource(src), tmp, true);

            var service = new SHA256CryptoServiceProvider();
            var hash = IoEx.Load(tmp, e => service.ComputeHash(e).Join("", b => $"{b:X2}"));
            return args.Concat("/InputFile", tmp, "/Digest", hash).ToList();
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
        protected void Set(MainViewModel vm, SettingValue src)
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
        /// Occurs when the message box is displayed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetMessage(DialogMessage e)
        {
            Assert.That(e.Icon, Is.EqualTo(DialogIcon.Error).Or.EqualTo(DialogIcon.Warning));
            Message  = e.Text;
            e.Value  = DialogStatus.Yes;
            e.Cancel = false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetUiCulture
        ///
        /// <summary>
        /// Occurs when the SetCulture event is fired.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetUiCulture(Language value) =>
            Thread.CurrentThread.CurrentUICulture = value.ToCultureInfo();

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Invokes the Convert method.
        /// </summary>
        ///
        /// <param name="vm">ViewModel</param>
        ///
        /// <returns>true for success.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected bool Test(MainViewModel vm)
        {
            Message = string.Empty;

            var closed = false;
            using (vm.Subscribe<CloseMessage>(e => closed = true))
            {
                vm.Convert();
                return Wait.For(() => closed, TimeSpan.FromSeconds(10));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TestError
        ///
        /// <summary>
        /// Invokes the Convert method and waits for receiving a message.
        /// </summary>
        ///
        /// <param name="vm">ViewModel object.</param>
        ///
        /// <returns>
        /// true for receiving a message, which means that an error or
        /// warning has occurred.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        protected bool TestError(MainViewModel vm)
        {
            Message = string.Empty;
            vm.Convert();
            return Wait.For(() => Message.HasValue());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Executes in each test.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [SetUp]
        protected void Setup()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            Locale.Set(Language.Auto);
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
        private void Set(SettingViewModel vm, SettingValue src)
        {
            vm.Language          = src.View.Language;
            vm.Format            = src.Format;
            vm.SaveOption        = src.SaveOption;
            vm.Resolution        = src.Resolution;
            vm.ColorMode         = src.ColorMode;
            vm.JpegCompression   = src.Encoding == Encoding.Jpeg;
            vm.IsAutoOrientation = src.Orientation == Orientation.Auto;
            vm.IsLandscape       = src.Orientation == Orientation.Landscape;
            vm.IsPortrait        = src.Orientation == Orientation.Portrait;
            vm.Linearization     = src.Linearization;
            vm.PostProcess       = src.PostProcess;
            vm.UserProgram       = src.UserProgram;

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
            vm.Title     = src.Title;
            vm.Author    = src.Author;
            vm.Subject   = src.Subject;
            vm.Keywords  = src.Keywords;
            vm.Creator   = src.Creator;
            vm.Options   = src.Options;
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
            vm.AllowForm   = src.Permission.InputForm.IsAllowed();
            vm.AllowModify      = src.Permission.ModifyContents.IsAllowed();
        }

        #endregion
    }
}
