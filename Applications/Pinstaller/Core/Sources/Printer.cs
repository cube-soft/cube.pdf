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

namespace Cube.Pdf.App.Pinstaller
{
    /* --------------------------------------------------------------------- */
    ///
    /// Printer
    ///
    /// <summary>
    /// Provides functionality to install or uninstall a printer.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Printer : IInstallable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Printer
        ///
        /// <summary>
        /// Initializes a new instance of the Printer class with the
        /// specified name.
        /// </summary>
        ///
        /// <param name="name">Printer name.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Printer(string name) : this(CreateCore())
        {
            var opt = StringComparison.InvariantCultureIgnoreCase;
            var obj = GetElements().FirstOrDefault(e => e.Name.Equals(name, opt));

            Exists = (obj != null);
            if (Exists) _core = obj._core;
            else
            {
                Name      = name;
                ShareName = name;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Printer
        ///
        /// <summary>
        /// Initializes a new instance of the Printer class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Printer(PrinterInfo2 core)
        {
            Environment = this.GetEnvironment();
            _core = core;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Gets the name of the printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name
        {
            get => _core.pPrinterName;
            private set => _core.pPrinterName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShareName
        ///
        /// <summary>
        /// Gets or sets the name that identifies the share point for the
        /// printer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string ShareName
        {
            get => _core.pShareName;
            set => _core.pShareName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DriverName
        ///
        /// <summary>
        /// Gets the name of the printer driver.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DriverName
        {
            get => _core.pDriverName;
            set => _core.pDriverName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PortName
        ///
        /// <summary>
        /// Gets the port name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string PortName
        {
            get => _core.pPortName;
            set => _core.pPortName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Environment
        ///
        /// <summary>
        /// Gets the name of architecture (Windows NT x86 or Windows x64).
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Environment { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// Gets the value indicating whether the port has been already
        /// installed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetElements
        ///
        /// <summary>
        /// Gets the collection of currently installed printers.
        /// </summary>
        ///
        /// <returns>Collection of printers.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<Printer> GetElements()
        {
            var bytes = 0u;
            var count = 0u;

            bool f(IntPtr p, uint n) => NativeMethods.EnumPrinters(2, null, 2, p, n, ref bytes, ref count);
            if (f(IntPtr.Zero, 0)) return new Printer[0];
            if (Marshal.GetLastWin32Error() != 122) throw new Win32Exception();

            var buffer = Marshal.AllocHGlobal((int)bytes);
            try
            {
                if (f(buffer, bytes)) return Convert(buffer, count);
                else throw new Win32Exception();
            }
            finally { Marshal.FreeHGlobal(buffer); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Install
        ///
        /// <summary>
        /// Installs the port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Install()
        {
            var dest = NativeMethods.AddPrinter(Name, 2, ref _core);
            if (dest == IntPtr.Zero) throw new Win32Exception();
            NativeMethods.ClosePrinter(dest);
            Exists = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Uninstall
        ///
        /// <summary>
        /// Uninstalls the port.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Uninstall()
        {
            if (!NativeMethods.OpenPrinter(Name, out var src, IntPtr.Zero)) throw new Win32Exception();
            try
            {
                if (!NativeMethods.DeletePrinter(src)) throw new Win32Exception();
                Exists = false;
            }
            finally { NativeMethods.ClosePrinter(src); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateCore
        ///
        /// <summary>
        /// Initializes a new instance of the PrinterInfo2 class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static PrinterInfo2 CreateCore() => new PrinterInfo2
        {
            pPrintProcessor = "winprint",
            pDatatype       = "RAW",
            Priority        = 1,
            DefaultPriority = 1,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts unmanaged resources to the Printer collection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<Printer> Convert(IntPtr src, uint n)
        {
            var dest = new List<Printer>();
            var ptr  = src;

            for (var i = 0; i < n; ++i)
            {
                var e = (PrinterInfo2)Marshal.PtrToStructure(ptr, typeof(PrinterInfo2));
                dest.Add(new Printer(e) { Exists = true });
                ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(PrinterInfo2)));
            }

            return dest;
        }

        #endregion

        #region Fields
        private PrinterInfo2 _core;
        #endregion
    }
}
