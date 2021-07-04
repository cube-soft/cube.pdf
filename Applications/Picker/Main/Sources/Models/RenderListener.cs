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
using System.Collections.Generic;
using System.Drawing;
using Cube.Collections;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Cube.Pdf.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// RenderListener
    ///
    /// <summary>
    /// provides functionality to extract embedded images in a PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class RenderListener : EnumerableBase<Image>, IEventListener
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        ///
        /// <returns>
        /// An IEnumerator(Image) object for this collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<Image> GetEnumerator() => _inner.GetEnumerator();

        /* ----------------------------------------------------------------- */
        ///
        /// RenderImage
        ///
        /// <summary>
        /// Occurs when the specified image is rendered.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void EventOccurred(IEventData data, EventType type)
        {
            if (type != EventType.RENDER_IMAGE || data is not ImageRenderInfo ri) return;
            var src = ri.GetImage();
            if (src is null) return;

            var bytes = src.GetImageBytes();
            var ms = new System.IO.MemoryStream(bytes);
            _inner.Add(Image.FromStream(ms));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSupportedEvents
        ///
        /// <summary>
        /// Gets the collection of EventType objects.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ICollection<EventType> GetSupportedEvents() => new[] { EventType.RENDER_IMAGE };

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Fields
        private readonly List<Image> _inner = new();
        #endregion
    }
}
