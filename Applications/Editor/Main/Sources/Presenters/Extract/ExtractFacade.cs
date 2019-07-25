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
using Cube.Mixin.String;
using System.Collections.Generic;
using System.Linq;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractFacade
    ///
    /// <summary>
    /// Provides functionality to communicate with the ExtractViewModel
    /// and other model classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFacade
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="selection">Page selection.</param>
        /// <param name="count">Number of pages.</param>
        /// <param name="io">I/O handler.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(ImageSelection selection, int count, IO io, Invoker invoker)
        {
            _io       = io;
            Count     = count;
            Selection = selection;
            Value     = Create(selection, invoker);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Selection
        ///
        /// <summary>
        /// Gets the page selection.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageSelection Selection { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the extract options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractOption Value { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// Gets the supported formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ExtractFormat> Formats { get; } = new[]
        {
            ExtractFormat.Pdf,
            ExtractFormat.Png,
        };

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ExtractOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ExtractOption Create(ImageSelection src, Invoker invoker)
        {
            var dest = new ExtractOption(invoker) {
                Target = src.Count > 0 ? ExtractTarget.Selected : ExtractTarget.All,
            };

            dest.PropertyChanged += (s, e) => {
                switch (e.PropertyName)
                {
                    case nameof(ExtractOption.Format):
                        SetDestination(Value.Format);
                        break;
                    case nameof(ExtractOption.Destination):
                        SetFormat(Value.Destination);
                        break;
                }
            };
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetFormat
        ///
        /// <summary>
        /// Sets the Format property according to the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetFormat(string src)
        {
            var fi = GetEntity(src);
            if (fi == null || !fi.Extension.HasValue()) return;

            try { Value.Format = Formats.First(e => fi.Extension.FuzzyEquals($".{e}")); }
            catch { /* Not found */ }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Sets the Destination property according to the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetDestination(ExtractFormat src)
        {
            var fi = GetEntity(Value.Destination);
            if (fi == null || fi.Extension.FuzzyEquals($".{src}")) return;

            var name = $"{fi.BaseName}.{src.ToString().ToLowerInvariant()}";
            Value.Destination = _io.Combine(fi.DirectoryName, name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEntity
        ///
        /// <summary>
        /// Creates a new instance of the Entity class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Entity GetEntity(string src) => src.HasValue() ? _io.Get(src) : null;

        #endregion

        #region Fields
        private readonly IO _io;
        #endregion
    }
}
