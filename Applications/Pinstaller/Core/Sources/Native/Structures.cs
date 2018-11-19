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

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// MonitorInfo2
    ///
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/printdocs/monitor-info-2
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct MonitorInfo2
    {
        [MarshalAs(UnmanagedType.LPTStr)] public string pName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pEnvironment;
        [MarshalAs(UnmanagedType.LPTStr)] public string pDLLName;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DriverInfo3
    ///
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/printdocs/driver-info-3
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct DriverInfo3
    {
        public uint cVersion;
        [MarshalAs(UnmanagedType.LPTStr)] public string pName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pEnvironment;
        [MarshalAs(UnmanagedType.LPTStr)] public string pDriverPath;
        [MarshalAs(UnmanagedType.LPTStr)] public string pDataFile;
        [MarshalAs(UnmanagedType.LPTStr)] public string pConfigFile;
        [MarshalAs(UnmanagedType.LPTStr)] public string pHelpFile;
        [MarshalAs(UnmanagedType.LPTStr)] public string pDependentFiles;
        [MarshalAs(UnmanagedType.LPTStr)] public string pMonitorName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pDefaultDataType;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PrinterInfo2
    ///
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/printdocs/printer-info-2
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct PrinterInfo2
    {
        [MarshalAs(UnmanagedType.LPTStr)] public string pServerName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pPrinterName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pShareName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pPortName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pDriverName;
        [MarshalAs(UnmanagedType.LPTStr)] public string pComment;
        [MarshalAs(UnmanagedType.LPTStr)] public string pLocation;
        public IntPtr pDevMode;
        [MarshalAs(UnmanagedType.LPTStr)] public string pSepFile;
        [MarshalAs(UnmanagedType.LPTStr)] public string pPrintProcessor;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDatatype;
        [MarshalAs(UnmanagedType.LPTStr)] public string pParameters;
        public IntPtr pSecurityDescriptor;
        public uint Attributes;
        public uint Priority;
        public uint DefaultPriority;
        public uint StartTime;
        public uint UntilTime;
        public uint Status;
        public uint cJobs;
        public uint AveragePPM;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PrinterDefaults
    ///
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/printdocs/printer-defaults
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct PrinterDefaults
    {
        public IntPtr pDatatype;
        public IntPtr pDevMode;
        public uint DesiredAccess;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PrinterDefaults
    ///
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/secauthz/access-mask
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    internal enum AccessMask
    {
        StandardRightsRequired  = 0xf0000,
        ServerAccessAdminister  = 0x00001,
        PrinterAccessAdminister = 0x00004,
        PrinterAccessUse        = 0x00008,
        PrinterAccessAll        = StandardRightsRequired | PrinterAccessAdminister | PrinterAccessUse,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AccessMaskExtension
    ///
    /// <summary>
    /// Provides extended methods of the AccessMask enum.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class AccessMaskExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the PrinterDefaults class with the
        /// specified AccessMask object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static PrinterDefaults Create(this AccessMask src) =>
            new PrinterDefaults
            {
                pDatatype     = IntPtr.Zero,
                pDevMode      = IntPtr.Zero,
                DesiredAccess = (uint)src,
            };

        #endregion
    }
}
