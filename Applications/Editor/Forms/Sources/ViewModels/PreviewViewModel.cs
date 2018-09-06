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
using GalaSoft.MvvmLight.Messaging;
using System.Threading;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PreviewViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for a <c>PreviewWindow</c> instance.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PreviewViewModel : DialogViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the <c>PreviewViewModel</c>
        /// with the specified argumetns.
        /// </summary>
        ///
        /// <param name="name">Name of the PDF file.</param>
        /// <param name="index">Selected index.</param>
        /// <param name="count">Page count.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PreviewViewModel(string name, int index, int count,
            SynchronizationContext context) : base(
                () => string.Format(Properties.Resources.TitlePreview, name, index + 1, count),
                new Messenger(),
                context
            ) { }

        #endregion
    }
}
