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
