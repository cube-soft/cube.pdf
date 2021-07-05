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
using NUnit.Framework;
using VO = Cube.Pdf.ViewerOption;

namespace Cube.Pdf.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewerOptionTest
    ///
    /// <summary>
    /// Tests the ViewOption enum and related methods.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ViewerOptionTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// ToName
        ///
        /// <summary>
        /// Tests the ToName extended method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(VO.SinglePage,      ExpectedResult = "SinglePage")]
        [TestCase(VO.OneColumn,       ExpectedResult = "OneColumn")]
        [TestCase(VO.TwoColumnLeft,   ExpectedResult = "TwoColumnLeft")]
        [TestCase(VO.TwoColumnRight,  ExpectedResult = "TwoColumnRight")]
        [TestCase(VO.TwoPageLeft,     ExpectedResult = "TwoPageLeft")]
        [TestCase(VO.TwoPageRight,    ExpectedResult = "TwoPageRight")]
        [TestCase(VO.Outline,         ExpectedResult = "UseOutlines")]
        [TestCase(VO.Thumbnail,       ExpectedResult = "UseThumbs")]
        [TestCase(VO.FullScreen,      ExpectedResult = "FullScreen")]
        [TestCase(VO.Attachment,      ExpectedResult = "UseAttachments")]
        [TestCase(VO.OptionalContent, ExpectedResult = "UseOC")]
        [TestCase(VO.None,            ExpectedResult = "UseNone")]
        [TestCase(VO.TwoColumnLeft | VO.SinglePage,   ExpectedResult = "SinglePage")]
        [TestCase(VO.OptionalContent | VO.Attachment, ExpectedResult = "UseOC")]
        [TestCase(VO.FullScreen | VO.OneColumn,       ExpectedResult = "OneColumn")]
        public string ToName(VO src) => src.ToName();

        #endregion
    }
}
