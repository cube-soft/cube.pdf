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
using Cube.Generics;
using Cube.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Cube.Processes
{
    /* --------------------------------------------------------------------- */
    ///
    /// Process
    ///
    /// <summary>
    /// プロセスを扱うクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Process
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// StartAsActiveUser
        ///
        /// <summary>
        /// アクティブユーザ権限でプログラムを実行します。
        /// </summary>
        ///
        /// <param name="program">実行プログラムのパス</param>
        /// <param name="arguments">プログラムの引数</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /// <remarks>
        /// アクティブユーザが複数存在する場合、操作は失敗します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAsActiveUser(string program, string[] arguments) =>
            StartAsActiveUser(CreateCmdLine(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAsActiveUser
        ///
        /// <summary>
        /// アクティブユーザ権限でプログラムを実行します。
        /// </summary>
        ///
        /// <param name="cmdline">実行するコマンドライン</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /// <remarks>
        /// アクティブユーザが複数存在する場合、操作は失敗します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAsActiveUser(string cmdline) =>
            StartAs(string.Empty, cmdline);

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// 指定されたユーザでプログラムを実行します。
        /// </summary>
        ///
        /// <param name="username">ユーザ名</param>
        /// <param name="program">実行プログラムのパス</param>
        /// <param name="arguments">プログラムの引数</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /// <remarks>
        /// 指定されたユーザがアクティブである必要があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(
            string username, string program, string[] arguments) =>
            StartAs(username, CreateCmdLine(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// 指定されたユーザでプログラムを実行します。
        /// </summary>
        ///
        /// <param name="username">ユーザ名</param>
        /// <param name="cmdline">実行するコマンドライン</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /// <remarks>
        /// 指定されたユーザがアクティブである必要があります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(string username, string cmdline) =>
            StartAs(GetActiveSessionToken(username), cmdline);

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// 指定されたトークンの権限でプログラムを実行します。
        /// </summary>
        ///
        /// <param name="token">実行ユーザのトークン</param>
        /// <param name="program">実行プログラムのパス</param>
        /// <param name="arguments">プログラムの引数</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(IntPtr token,
            string program, string[] arguments) =>
            StartAs(token, CreateCmdLine(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// 指定されたトークンの権限でプログラムを実行します。
        /// </summary>
        ///
        /// <param name="token">実行ユーザのトークン</param>
        /// <param name="cmdline">実行するコマンドライン</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(IntPtr token, string cmdline)
        {
            var env = IntPtr.Zero;

            try
            {
                if (token == IntPtr.Zero) throw new ArgumentException("PrimaryToken");

                env = GetEnvironmentBlock(token);
                if (env == IntPtr.Zero) throw new ArgumentException("EnvironmentBlock");

                return CreateProcessAsUser(cmdline, token, env);
            }
            finally
            {
                if (env != IntPtr.Zero) UserEnv.NativeMethods.DestroyEnvironmentBlock(env);
                CloseHandle(token);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// 指定されたスレッド ID の権限でプログラムを実行します。
        /// </summary>
        ///
        /// <param name="tid">スレッド ID</param>
        /// <param name="program">実行プログラムのパス</param>
        /// <param name="arguments">プログラムの引数</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(uint tid, string program, string[] arguments) =>
            StartAs(tid, CreateCmdLine(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// 指定されたスレッド ID の権限でプログラムを実行します。
        /// </summary>
        ///
        /// <param name="tid">スレッド ID</param>
        /// <param name="cmdline">実行するコマンドライン</param>
        ///
        /// <returns>実行に成功した Process オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(uint tid, string cmdline) =>
            StartAs(GetThreadToken(tid), cmdline);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetActiveSessionId
        ///
        /// <summary>
        /// 現在ログオン中のユーザに対応するセッション ID を取得します。
        /// </summary>
        ///
        /// <param name="username">ユーザ名</param>
        ///
        /* ----------------------------------------------------------------- */
        private static uint GetActiveSessionId(string username)
        {
            var ptr = IntPtr.Zero;
            var count = 0u;

            try
            {
                if (!WtsApi32.NativeMethods.WTSEnumerateSessions(
                    (IntPtr)0, // WTS_CURRENT_SERVER_HANDLE
                    0,
                    1,
                    ref ptr,
                    ref count
                )) throw Win32Error(nameof(WtsApi32.NativeMethods.WTSEnumerateSessions));

                var sessions = new List<WTS_SESSION_INFO>();

                for (var i = 0; i < count; ++i)
                {
                    var info = (WTS_SESSION_INFO)Marshal.PtrToStructure(
                        new IntPtr(ptr.ToInt64() + i * Marshal.SizeOf(typeof(WTS_SESSION_INFO))),
                        typeof(WTS_SESSION_INFO)
                    );

                    if (info.State == WTS_CONNECTSTATE_CLASS.WTSActive) sessions.Add(info);

                    Logger.Debug(typeof(Process),
                        string.Format("SessionID:{0}\tState:{1}\tName:{2}",
                        info.SessionID, info.State, info.pWinStationName
                    ));
                }

                if (sessions.Count <= 0) throw new ArgumentException("Session not found");
                else if (sessions.Count == 1) return sessions[0].SessionID;
                else return sessions.Select(x => x.SessionID).First(x => GetUserName(x) == username);
            }
            finally { if (ptr != IntPtr.Zero) WtsApi32.NativeMethods.WTSFreeMemory(ptr); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetActiveSessionToken
        ///
        /// <summary>
        /// アクティブなセッションに対応するトークンを取得します。
        /// </summary>
        ///
        /// <param name="username">ユーザ名</param>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetActiveSessionToken(string username)
        {
            var id = GetActiveSessionId(username);
            var si = SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation;

            if (WtsApi32.NativeMethods.WTSQueryUserToken(id, out var token))
            {
                try { return GetPrimaryToken(token, si); }
                finally { CloseHandle(token); }
            }
            else throw Win32Error(nameof(WtsApi32.NativeMethods.WTSQueryUserToken));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetThreadToken
        ///
        /// <summary>
        /// スレッド ID に対応するトークンを取得します。
        /// </summary>
        ///
        /// <param name="tid">スレッド ID</param>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetThreadToken(uint tid)
        {
            var token = IntPtr.Zero;
            var ht = Kernel32.NativeMethods.OpenThread(0x40 /* QUERY_INFORMATION */, false, tid);
            if (ht == IntPtr.Zero) throw Win32Error(nameof(Kernel32.NativeMethods.OpenThread));

            try
            {
                if (!AdvApi32.NativeMethods.OpenThreadToken(
                    ht,
                    0x02 /* TOKEN_DUPLICATE */,
                    true,
                    ref token
                )) throw Win32Error(nameof(AdvApi32.NativeMethods.OpenThreadToken));

                return GetPrimaryToken(token, SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation);
            }
            finally
            {
                CloseHandle(ht);
                CloseHandle(token);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPrimaryToken
        ///
        /// <summary>
        /// プライマリトークンを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetPrimaryToken(IntPtr token, SECURITY_IMPERSONATION_LEVEL level)
        {
            var dest = IntPtr.Zero;
            var attr = new SECURITY_ATTRIBUTES();
            attr.nLength = (uint)Marshal.SizeOf(attr);
            var result = AdvApi32.NativeMethods.DuplicateTokenEx(
                token,
                0x0001 | // TOKEN_ASSIGN_PRIMARY
                0x0002 | // TOKEN_DUPLICATE
                0x0004 | // TOKEN_IMPERSONATE
                0x0008,  // TOKEN_QUERY
                ref attr,
                (int)level,
                (int)TOKEN_TYPE.TokenPrimary,
                ref dest
            );

            AdvApi32.NativeMethods.RevertToSelf();

            if (!result) throw Win32Error(nameof(AdvApi32.NativeMethods.DuplicateTokenEx));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserName
        ///
        /// <summary>
        /// ユーザ名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetUserName(uint id)
        {
            var handle = IntPtr.Zero;
            var buffer = IntPtr.Zero;
            var result = 0u;

            try
            {
                return WtsApi32.NativeMethods.WTSQuerySessionInformation(
                           handle,
                           (int)id,
                           WTS_INFO_CLASS.WTSUserName,
                           out buffer,
                           out result
                       ) ? Marshal.PtrToStringAnsi(buffer) : string.Empty;
            }
            finally { WtsApi32.NativeMethods.WTSFreeMemory(buffer); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnvironmentBlock
        ///
        /// <summary>
        /// 環境ブロックを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetEnvironmentBlock(IntPtr token)
        {
            var dest = IntPtr.Zero;
            if (UserEnv.NativeMethods.CreateEnvironmentBlock(ref dest, token, false)) return dest;
            else throw Win32Error(nameof(UserEnv.NativeMethods.CreateEnvironmentBlock));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CloseHandle
        ///
        /// <summary>
        /// ハンドルを閉じます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void CloseHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;
            Kernel32.NativeMethods.CloseHandle(handle);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProcessAsUser
        ///
        /// <summary>
        /// Win32 API の CreateProcessAsUser を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static System.Diagnostics.Process CreateProcessAsUser(string cmdline, IntPtr token, IntPtr env)
        {
            var si = new STARTUPINFO
            {
                lpDesktop   = @"WinSta0\Default",
                wShowWindow = 0x05,  // SW_SHOW
                dwFlags     = 0x01 | // STARTF_USESHOWWINDOW
                              0x40,  // STARTF_FORCEONFEEDBACK
            };
            si.cb = (uint)Marshal.SizeOf(si);

            var sa = new SECURITY_ATTRIBUTES();
            sa.nLength = (uint)Marshal.SizeOf(sa);

            var thread = new SECURITY_ATTRIBUTES();
            thread.nLength = (uint)Marshal.SizeOf(thread);

            var pi = new PROCESS_INFORMATION();
            try
            {
                if (AdvApi32.NativeMethods.CreateProcessAsUser(
                    token,
                    null,
                    cmdline,
                    ref sa,
                    ref thread,
                    false,
                    0x0400, // CREATE_UNICODE_ENVIRONMENT
                    env,
                    null,
                    ref si,
                    out pi
                )) return System.Diagnostics.Process.GetProcessById((int)pi.dwProcessId);
                else throw Win32Error(nameof(AdvApi32.NativeMethods.CreateProcessAsUser));
            }
            finally
            {
                CloseHandle(pi.hProcess);
                CloseHandle(pi.hThread);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateCmdLine
        ///
        /// <summary>
        /// コマンドライン用の文字列を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string CreateCmdLine(string program, string[] arguments) =>
            arguments == null ?
            program.Quote() :
            arguments.Aggregate(program.Quote(), (x, y) => $"{x} {y.Quote()}");

        /* ----------------------------------------------------------------- */
        ///
        /// Win32Error
        ///
        /// <summary>
        /// Win32Exception オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Win32Exception Win32Error(string message) =>
            new Win32Exception(Marshal.GetLastWin32Error(), message);

        #endregion
    }
}
