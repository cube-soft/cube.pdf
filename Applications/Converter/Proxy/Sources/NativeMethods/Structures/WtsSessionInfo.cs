/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.Runtime.InteropServices;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// WTS_CONNECTSTATE_CLASS
    ///
    /// <summary>
    /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383860.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }

    /* --------------------------------------------------------------------- */
    ///
    /// WTS_INFO_CLASS
    ///
    /// <summary>
    /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383861.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum WTS_INFO_CLASS
    {
        WTSInitialProgram,
        WTSApplicationName,
        WTSWorkingDirectory,
        WTSOEMId,
        WTSSessionId,
        WTSUserName,
        WTSWinStationName,
        WTSDomainName,
        WTSConnectState,
        WTSClientBuildNumber,
        WTSClientName,
        WTSClientDirectory,
        WTSClientProductId,
        WTSClientHardwareId,
        WTSClientAddress,
        WTSClientDisplay,
        WTSClientProtocolType,
        WTSIdleTime,
        WTSLogonTime,
        WTSIncomingBytes,
        WTSOutgoingBytes,
        WTSIncomingFrames,
        WTSOutgoingFrames,
        WTSClientInfo,
        WTSSessionInfo,
        WTSConfigInfo,
        WTSValidationInfo,
        WTSSessionAddressV4,
        WTSIsRemoteSession
    }

    /* --------------------------------------------------------------------- */
    ///
    /// WTS_SESSION_INFO
    ///
    /// <summary>
    /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/aa383864.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal class WTS_SESSION_INFO
    {
        public uint SessionID;
        public string pWinStationName;
        public WTS_CONNECTSTATE_CLASS State;
    }
}
