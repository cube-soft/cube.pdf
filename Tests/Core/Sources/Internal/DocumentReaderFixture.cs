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
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// DocumentReaderFixture
    ///
    /// <summary>
    /// Provides functionality to test IDocumentReader implemented classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class DocumentReaderFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetIds
        ///
        /// <summary>
        /// Get a list of IDs indicating the IDocumentReader implementation
        /// class to be tested.
        /// </summary>
        ///
        /// <returns>List of IDs.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static IEnumerable<string> GetIds() => GetFactory().Keys;

        /* ----------------------------------------------------------------- */
        ///
        /// GetFactory
        ///
        /// <summary>
        /// Get the list of IDocumentReader generating rules.
        /// </summary>
        ///
        /// <returns>List of generating rules.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected static Dictionary<string, Func<string, object, IDocumentReader>> GetFactory() => new()
        {
            {
                nameof(Pdf.Itext), (s, o) =>
                    o is string ?
                    new Pdf.Itext.DocumentReader(s, o as string) :
                    new Pdf.Itext.DocumentReader(s, o as IQuery<string>)
            },
            {
                nameof(Pdf.Pdfium), (s, o) =>
                    o is string ?
                    new Pdf.Pdfium.DocumentReader(s, o as string) :
                    new Pdf.Pdfium.DocumentReader(s, o as IQuery<string>)
            },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the IDocumentReader implemented class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="klass">Class ID.</param>
        /// <param name="src">Path of the source file.</param>
        /// <param name="password">Password to open.</param>
        ///
        /// <returns>IDocumentReader implemented object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected IDocumentReader Create(string klass, string src, string password)
        {
            Assert.That(GetFactory().TryGetValue(klass, out var factory), Is.True);
            return factory(src, password);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the IDocumentReader implemented class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="klass">Class ID.</param>
        /// <param name="src">Path of the source file.</param>
        /// <param name="query">Password query.</param>
        ///
        /// <returns>IDocumentReader implemented object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected IDocumentReader Create(string klass, string src, IQuery<string, string> query)
        {
            Assert.That(GetFactory().TryGetValue(klass, out var factory), Is.True);
            return factory(src, query);
        }

        #endregion
    }
}
