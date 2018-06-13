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
using Cube.FileSystem.Tests;
using Cube.Pdf.Ghostscript;
using NUnit.Framework;

namespace Cube.Pdf.Tests.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// ConverterFixture
    ///
    /// <summary>
    /// Converter およびそのサブクラスのテストを補助するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class ConverterFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// TestCase
        ///
        /// <summary>
        /// テストケースを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected static TestCaseData TestCase<T>(string src, Converter conv, T name)
        {
            var cvt = $"{name.GetType().Name}_{name.ToString()}";
            return new TestCaseData(src, conv, cvt);
        }

        #endregion
    }
}
