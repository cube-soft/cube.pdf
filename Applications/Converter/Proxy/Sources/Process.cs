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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Cube.Mixin.String;

namespace Cube.Pdf.Converter.Proxy
{
    /* --------------------------------------------------------------------- */
    ///
    /// Process
    ///
    /// <summary>
    /// Provides functionality to create and execute a process.
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
        /// Executes the program with active user privileges.
        /// </summary>
        ///
        /// <param name="program">Path of the program.</param>
        /// <param name="arguments">Program arguments.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /// <remarks>
        /// If there are multiple active users, the method will fail.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAsActiveUser(string program, string[] arguments) =>
            StartAsActiveUser(CreateCmdline(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAsActiveUser
        ///
        /// <summary>
        /// Executes the program with active user privileges.
        /// </summary>
        ///
        /// <param name="cmdline">Command line.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /// <remarks>
        /// If there are multiple active users, the method will fail.
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
        /// Executes the program with the specified user.
        /// </summary>
        ///
        /// <param name="user">User name.</param>
        /// <param name="program">Path of the program.</param>
        /// <param name="arguments">Program arguments.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /// <remarks>
        /// The specified user must be active.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(
            string user, string program, string[] arguments) =>
            StartAs(user, CreateCmdline(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Executes the program with the specified user.
        /// </summary>
        ///
        /// <param name="user">User name.</param>
        /// <param name="cmdline">Command line.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /// <remarks>
        /// The specified user must be active.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(string user, string cmdline) =>
            StartAs(GetActiveSessionToken(user), cmdline);

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Executes the program with the specified token privileges.
        /// </summary>
        ///
        /// <param name="token">Token of the executing user.</param>
        /// <param name="program">Path of the program.</param>
        /// <param name="arguments">Program arguments.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(IntPtr token,
            string program, string[] arguments) =>
            StartAs(token, CreateCmdline(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Executes the program with the specified token privileges.
        /// </summary>
        ///
        /// <param name="token">Token of the executing user.</param>
        /// <param name="cmdline">Command line.</param>
        ///
        /// <returns>Process object that was started.</returns>
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
                if (env != IntPtr.Zero) _ = UserEnv.NativeMethods.DestroyEnvironmentBlock(env);
                CloseHandle(token);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Executes the program with the privileges of the specified thread ID.
        /// </summary>
        ///
        /// <param name="tid">Thread ID.</param>
        /// <param name="program">Path of the program.</param>
        /// <param name="arguments">Program arguments.</param>
        ///
        /// <returns>Process object that was started.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static System.Diagnostics.Process StartAs(uint tid, string program, string[] arguments) =>
            StartAs(tid, CreateCmdline(program, arguments));

        /* ----------------------------------------------------------------- */
        ///
        /// StartAs
        ///
        /// <summary>
        /// Executes the program with the privileges of the specified thread ID.
        /// </summary>
        ///
        /// <param name="tid">Thread ID.</param>
        /// <param name="cmdline">Command line.</param>
        ///
        /// <returns>Process object that was started.</returns>
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
        /// Gets the session ID corresponding to the active user.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static uint GetActiveSessionId(string user)
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
                )) throw new Win32Exception();

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
                else return sessions.Select(x => x.SessionID).First(x => GetUserName(x) == user);
            }
            finally { if (ptr != IntPtr.Zero) WtsApi32.NativeMethods.WTSFreeMemory(ptr); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetActiveSessionToken
        ///
        /// <summary>
        /// Gets the token corresponding to the active session.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetActiveSessionToken(string user)
        {
            var id = GetActiveSessionId(user);
            var si = SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation;

            if (WtsApi32.NativeMethods.WTSQueryUserToken(id, out var token))
            {
                try { return GetPrimaryToken(token, si); }
                finally { CloseHandle(token); }
            }
            else throw new Win32Exception();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetThreadToken
        ///
        /// <summary>
        /// Get the token corresponding to the thread ID.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetThreadToken(uint tid)
        {
            var token = IntPtr.Zero;
            var ht = Kernel32.NativeMethods.OpenThread(0x40 /* QUERY_INFORMATION */, false, tid);
            if (ht == IntPtr.Zero) throw new Win32Exception();

            try
            {
                if (!AdvApi32.NativeMethods.OpenThreadToken(
                    ht,
                    0x02 /* TOKEN_DUPLICATE */,
                    true,
                    ref token
                )) throw new Win32Exception();

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
        /// Gets the primary token.
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

            _ = AdvApi32.NativeMethods.RevertToSelf();

            if (!result) throw new Win32Exception();
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUserName
        ///
        /// <summary>
        /// Gets the user name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetUserName(uint id)
        {
            var handle = IntPtr.Zero;
            var buffer = IntPtr.Zero;

            try
            {
                return WtsApi32.NativeMethods.WTSQuerySessionInformation(
                           handle,
                           (int)id,
                           WTS_INFO_CLASS.WTSUserName,
                           out buffer,
                           out _
                       ) ? Marshal.PtrToStringAnsi(buffer) : string.Empty;
            }
            finally { WtsApi32.NativeMethods.WTSFreeMemory(buffer); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnvironmentBlock
        ///
        /// <summary>
        /// Gets the environment block.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IntPtr GetEnvironmentBlock(IntPtr token)
        {
            var dest = IntPtr.Zero;
            if (UserEnv.NativeMethods.CreateEnvironmentBlock(ref dest, token, false)) return dest;
            else throw new Win32Exception();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CloseHandle
        ///
        /// <summary>
        /// Closes the specified handle.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void CloseHandle(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;
            _ = Kernel32.NativeMethods.CloseHandle(handle);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProcessAsUser
        ///
        /// <summary>
        /// Invokes the Win32 API CreateProcessAsUser.
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
                else throw new Win32Exception();
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
        /// Creates a string for the command line.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string CreateCmdline(string program, string[] arguments) =>
            arguments == null ?
            program.Quote() :
            arguments.Aggregate(program.Quote(), (x, y) => $"{x} {y.Quote()}");

        #endregion
    }
}
