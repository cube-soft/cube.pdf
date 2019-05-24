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
using Cube.FileSystem;
using Cube.Mixin.IO;
using Cube.Pdf.Ghostscript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Converter
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileTransfer
    ///
    /// <summary>
    /// Provides functionality to move or rename files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class FileTransfer : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileTransfer
        ///
        /// <summary>
        /// Initializes a new instance of the FileTransfer class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="format">Target format.</param>
        /// <param name="dest">Path to save.</param>
        /// <param name="temp">Working directory.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileTransfer(Format format, string dest, string temp, IO io)
        {
            IO          = io;
            Format      = format;
            Information = io.Get(dest);
            Temp        = GetTempDirectory(temp);
            Value       = IO.Combine(Temp, GetName());
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O hander.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the target format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// AutoRename
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to rename files
        /// automatically when the specified file exists.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AutoRename { get; set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the path to save the file.
        /// </summary>
        ///
        /// <remarks>
        /// ユーザプログラムは Value で指定されたパスに保存した後、
        /// Invoke メソッドを用いてファイルの移動を実行します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// WorkDirectory
        ///
        /// <summary>
        /// Gets the path of the working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Temp { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Information
        ///
        /// <summary>
        /// Gets a value that represents the path to save the file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Information Information { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes operations to move or rename files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Invoke()
        {
            var src   = IO.GetFiles(Temp);
            var dest  = new List<string>();

            for (var i = 0; i < src.Length; ++i)
            {
                var path = GetDestination(i + 1, src.Length);
                IO.Move(src[i], path, true);
                dest.Add(path);
            }

            return dest;
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the
        /// FileTransfer and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) => IO.TryDelete(Temp);

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTempDirectory
        ///
        /// <summary>
        /// Gets the path of the working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTempDirectory(string src) =>
            Enumerable.Range(1, int.MaxValue)
                      .Select(e => IO.Combine(src, e.ToString()))
                      .First(e => !IO.Get(e).Exists);

        /* ----------------------------------------------------------------- */
        ///
        /// GetName
        ///
        /// <summary>
        /// Gets the value that represents the filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetName() =>
            DocumentConverter.SupportedFormats.Contains(Format) ?
            $"tmp{Information.Extension}" :
            $"tmp-%08d{Information.Extension}";

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestination
        ///
        /// <summary>
        /// Gets the path to save the file from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDestination(int index, int count)
        {
            var dest = GetDestinationCore(index, count);
            return (AutoRename && IO.Exists(dest)) ? IO.GetUniqueName(dest) : dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestinationCore
        ///
        /// <summary>
        /// Gets the path to save the file from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDestinationCore(int index, int count)
        {
            if (count <= 1) return Information.FullName;

            var name  = Information.BaseName;
            var ext   = Information.Extension;
            var digit = string.Format("D{0}", Math.Max(count.ToString("D").Length, 2));
            var dest  = string.Format("{0}-{1}{2}", name, index.ToString(digit), ext);

            return IO.Combine(Information.DirectoryName, dest);
        }

        #endregion
    }
}
