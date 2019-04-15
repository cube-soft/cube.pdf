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
using System.Runtime.InteropServices;

namespace Cube.Pdf.Converter.Proxy
{
    /* --------------------------------------------------------------------- */
    ///
    /// PROCESS_INFORMATION
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms684873.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// STARTUPINFO
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms686331.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SECURITY_ATTRIBUTES
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa379560.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_ATTRIBUTES
    {
        public uint nLength;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SECURITY_IMPERSONATION_LEVEL
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa379572.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum SECURITY_IMPERSONATION_LEVEL
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SECURITY_ATTRIBUTES
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa379633.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum TOKEN_TYPE
    {
        TokenPrimary = 1,
        TokenImpersonation
    }

    /* --------------------------------------------------------------------- */
    ///
    /// TOKEN_INFORMATION_CLASS
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa379626.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum TOKEN_INFORMATION_CLASS
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId,
        TokenGroupsAndPrivileges,
        TokenSessionReference,
        TokenSandBoxInert,
        TokenAuditPolicy,
        TokenOrigin
    }

    /* --------------------------------------------------------------------- */
    ///
    /// TOKEN_INFORMATION_CLASS
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa379601.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum SID_NAME_USE
    {
        SidTypeUser = 1,
        SidTypeGroup,
        SidTypeDomain,
        SidTypeAlias,
        SidTypeWellKnownGroup,
        SidTypeDeletedAccount,
        SidTypeInvalid,
        SidTypeUnknown,
        SidTypeComputer
    }
}
