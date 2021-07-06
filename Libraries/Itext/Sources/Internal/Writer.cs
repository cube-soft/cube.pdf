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
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.FileSystem;
using Cube.Logging;
using Cube.Mixin.String;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Cube.Pdf.Itext
{
    /* --------------------------------------------------------------------- */
    ///
    /// Writer
    ///
    /// <summary>
    /// Provides factory and other static methods about PdfWriter.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class Writer : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Writer
        ///
        /// <summary>
        /// Initializes a new instance of the Writer class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="path">Path of the PDF document.</param>
        /// <param name="options">Save options.</param>
        /// <param name="metadata">PDF metadata.</param>
        /// <param name="crypt">PDF encryption settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public Writer(string path, SaveOption options, Metadata metadata, Encryption crypt)
        {
            var dir = options.Temp.HasValue() ?
                      options.Temp :
                      Io.Get(path).DirectoryName;

            _dest     = path;
            _tmp      = Io.Combine(dir, Guid.NewGuid().ToString("N"));
            _metadata = metadata;
            _crypt    = crypt;
            _writer   = options.Smart ?
                        new PdfSmartCopy(_document, Io.Create(_tmp)) :
                        new PdfCopy(_document, Io.Create(_tmp));

            _writer.PdfVersion = metadata.Version.Minor.ToString()[0];
            _writer.ViewerPreferences = (int)metadata.Options;
            _document.Open();
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified page to the specified writer.
        /// </summary>
        ///
        /// <param name="src">PdfReader object.</param>
        /// <param name="page">Page object.</param>
        ///
        /// <remarks>
        /// Note that the value of PdfCopy.PageNumber is automatically
        /// incremented as soon as AddPage is executed.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IDisposable src, Page page)
        {
            var obj = Reader.From(src);
            obj.Rotate(page);
            if (page.File is PdfFile)
            {
                var n = _writer.PageNumber; // see remarks
                obj.GetBookmarks(n, n - page.Number, _bookmark);
            }
            _writer.AddPage(_writer.GetImportedPage(obj, page.Number));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets attachments to the specified writer.
        /// </summary>
        ///
        /// <param name="data">Collection of attachments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(IEnumerable<Attachment> data)
        {
            var done = new List<Attachment>();

            foreach (var item in data)
            {
                var dup = done.Any(e =>
                    e.Name.ToLower() == item.Name.ToLower() &&
                    e.Length == item.Length &&
                    e.Checksum.SequenceEqual(item.Checksum)
                );

                if (dup) continue;

                var fs = item is EmbeddedAttachment ?
                         PdfFileSpecification.FileEmbedded(_writer, null, item.Name, item.Data) :
                         PdfFileSpecification.FileEmbedded(_writer, item.Source, item.Name, null);

                fs.SetUnicodeFileName(item.Name, true);
                _writer.AddFileAttachment(fs);
                done.Add(item);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the DocumentReader
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            _document.Close();
            _writer.Close();

            Stamp();
            GetType().LogWarn(() => Io.Delete(_tmp));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Stamp
        ///
        /// <summary>
        /// Adds the specified information to the source PDF file and save
        /// to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Stamp()
        {
            using var r = Reader.From(_tmp, new Password(null, ""), new());
            using var w = new PdfStamper(r, Io.Create(_dest));

            w.Writer.Outlines = _bookmark;
            w.MoreInfo = new Dictionary<string, string>
            {
                { "Author",   _metadata.Author   },
                { "Title",    _metadata.Title    },
                { "Subject",  _metadata.Subject  },
                { "Keywords", _metadata.Keywords },
                { "Creator",  _metadata.Creator  },
            };

            SetEncryption(w.Writer);
            if (_metadata.Version.Minor >= 5) w.SetFullCompression();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetEncryption
        ///
        /// <summary>
        /// Sets the encryption settings to the specified writer.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SetEncryption(PdfWriter src)
        {
            if (_crypt == null || !_crypt.Enabled || !_crypt.OwnerPassword.HasValue()) return;

            var m = GetMethod(_crypt.Method);
            var p = (int)_crypt.Permission.Value;

            var owner = _crypt.OwnerPassword;
            var user  = !_crypt.OpenWithPassword ? string.Empty :
                        _crypt.UserPassword.HasValue() ? _crypt.UserPassword :
                        owner;

            src.SetEncryption(m, user, owner, p);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethod
        ///
        /// <summary>
        /// Gets the value corresponding to the specified method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private int GetMethod(EncryptionMethod src) => src switch
        {
            EncryptionMethod.Standard40  => PdfWriter.STANDARD_ENCRYPTION_40,
            EncryptionMethod.Standard128 => PdfWriter.STANDARD_ENCRYPTION_128,
            EncryptionMethod.Aes128      => PdfWriter.ENCRYPTION_AES_128,
            EncryptionMethod.Aes256      => PdfWriter.ENCRYPTION_AES_256,
            _                            => PdfWriter.STANDARD_ENCRYPTION_40,
        };

        #endregion

        #region Fields
        private readonly string _dest;
        private readonly string _tmp;
        private readonly Document _document = new();
        private readonly PdfCopy _writer;
        private readonly Metadata _metadata;
        private readonly Encryption _crypt;
        private readonly List<Dictionary<string, object>> _bookmark = new();
        #endregion
    }
}
