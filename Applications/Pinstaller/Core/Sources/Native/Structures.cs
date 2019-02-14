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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MonitorInfo2
    {
        public string pName;
        public string pEnvironment;
        public string pDLLName;
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DriverInfo3
    {
        public uint cVersion;
        public string pName;
        public string pEnvironment;
        public string pDriverPath;
        public string pDataFile;
        public string pConfigFile;
        public string pHelpFile;
        public string pDependentFiles;
        public string pMonitorName;
        public string pDefaultDataType;
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct PrinterInfo2
    {
        public string pServerName;
        public string pPrinterName;
        public string pShareName;
        public string pPortName;
        public string pDriverName;
        public string pComment;
        public string pLocation;
        public IntPtr pDevMode;
        public string pSepFile;
        public string pPrintProcessor;
        public string pDatatype;
        public string pParameters;
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
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
