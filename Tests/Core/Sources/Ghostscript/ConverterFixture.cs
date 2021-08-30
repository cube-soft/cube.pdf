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
using System.Reflection;
using Cube.FileSystem;
using Cube.Pdf.Ghostscript;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ConverterFixture
    ///
    /// <summary>
    /// Provides functionality to test the Converter and inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    abstract class ConverterFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        ///
        /// <summary>
        /// Invokes the specified Converter object with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="obj">Source Converter object.</param>
        /// <param name="src">Source filename.</param>
        /// <param name="dest">Filename without extension to save.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Run(Converter obj, string src, string dest) => Run(obj, src, dest, dest);

        /* ----------------------------------------------------------------- */
        ///
        /// Run
        ///
        /// <summary>
        /// Invokes the specified Converter object with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="obj">Source Converter object.</param>
        /// <param name="src">Source filename.</param>
        /// <param name="dest">Filename without extension to save.</param>
        /// <param name="log">Log filename without extension.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Run(Converter obj, string src, string dest, string log)
        {
            var asm = Assembly.GetExecutingAssembly();
            var sp  = GetSource(src);
            var dp  = Get($"{dest}{obj.Format.GetExtension()}");
            var dir = Io.Get(asm.Location).DirectoryName;

            obj.Quiet = false;
            obj.Log   = Get($"{log}.log");
            obj.Temp  = Get("Tmp");
            obj.Resources.Add(Io.Combine(dir, "lib"));
            obj.Invoke(sp, dp);

            return dp;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TestCase
        ///
        /// <summary>
        /// Creates a new test case with the specified arguments.
        /// </summary>
        ///
        /// <param name="id">Test ID.</param>
        /// <param name="obj">Source Converter object.</param>
        /// <param name="src">Source filename.</param>
        /// <param name="predicate">
        /// Object to determine the save filename.
        /// </param>
        ///
        /// <returns>TestCaseData object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData TestCase<T>(int id, Converter obj, string src, T predicate) =>
            TestCase(id, obj, src, $"{predicate.GetType().Name}_{predicate}");

        /* ----------------------------------------------------------------- */
        ///
        /// TestCase
        ///
        /// <summary>
        /// Creates a new test case with the specified arguments.
        /// </summary>
        ///
        /// <param name="id">Test ID.</param>
        /// <param name="obj">Source Converter object.</param>
        /// <param name="src">Source filename.</param>
        /// <param name="dest">Filename without extension to save.</param>
        ///
        /// <returns>TestCaseData object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData TestCase(int id, Converter obj, string src, string dest) =>
            new(id, obj, src, dest);

        #endregion
    }
}
