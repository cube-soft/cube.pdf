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
using Cube.Tests;
using NUnit.Framework;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageTest
    ///
    /// <summary>
    /// Tests for Page and its inherited classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class AttachmentTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Executes the test for creating a new instance of the
        /// Attachment class with the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var name = "SampleImage02.png";
            var src  = GetSource(name);
            var dest = new Attachment(src);

            Assert.That(dest.Name,            Is.EqualTo(name));
            Assert.That(dest.Source,          Is.EqualTo(src));
            Assert.That(dest.Length,          Is.EqualTo(3765));
            Assert.That(dest.Data.Length,     Is.EqualTo(3765));
            Assert.That(dest.Checksum.Length, Is.EqualTo(32));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_WithNonExistent
        ///
        /// <summary>
        /// Executes the test for confirming the result when a non-existent
        /// file is specified.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_WithNonExistent()
        {
            var name = "NotFound.txt";
            var src  = GetSource(name);
            var dest = new Attachment(src);

            Assert.That(dest.Name,     Is.EqualTo(name));
            Assert.That(dest.Source,   Is.EqualTo(src));
            Assert.That(dest.Length,   Is.EqualTo(0));
            Assert.That(dest.Data,     Is.Null, nameof(dest.Data));
            Assert.That(dest.Checksum, Is.Null, nameof(dest.Checksum));
        }

        #endregion
    }
}
