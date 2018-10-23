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
using Cube.Collections.Mixin;
using Cube.FileSystem;
using Cube.Generics;
using Cube.Pdf.Itext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Cube.Pdf.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainFacadeExtension
    ///
    /// <summary>
    /// Represents the extended methods to handle the MainFacade object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class MainFacadeExtension
    {
        #region Methods

        #region Open

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// Invokes some actions through the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="args">User arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Setup(this MainFacade src, IEnumerable<string> args)
        {
            foreach (var ps in src.Settings.GetSplashProcesses()) ps.Kill();
            var path = src.GetFirst(args);
            if (path.HasValue()) src.Open(path);
            src.Backup.Cleanup();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens the first item of the specified collection.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="files">File collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Open(this MainFacade src, IEnumerable<string> files)
        {
            var path = src.GetFirst(files);
            if (path.HasValue()) src.Open(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenLink
        ///
        /// <summary>
        /// Opens a PDF document with the specified link.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="link">Information for the link.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void OpenLink(this MainFacade src, Information link)
        {
            try { src.Open(Shortcut.Resolve(link?.FullName)?.Target); }
            catch (Exception e)
            {
                var cancel = e is OperationCanceledException ||
                             e is TwiceException;
                if (!cancel) src.Bindable.IO.TryDelete(link?.FullName);
                throw;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// StartProcess
        ///
        /// <summary>
        /// Starts a new process with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="args">User arguments.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void StartProcess(this MainFacade src, string args) =>
            Process.Start(new ProcessStartInfo
            {
                FileName  = Assembly.GetExecutingAssembly().Location,
                Arguments = args
            });

        /* ----------------------------------------------------------------- */
        ///
        /// GetFirst
        ///
        /// <summary>
        /// Gets the first item of the specified collection
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="files">File collection.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetFirst(this MainFacade src, IEnumerable<string> files) =>
            files.FirstOrDefault(e => e.IsPdf());

        #endregion

        #region Save

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// Overwrites the PDF document.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Overwrite(this MainFacade src)
        {
            if (src.Bindable.History.Undoable) src.Save(src.Bindable.Source.Value.FullName);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the PDF document to the specified file path.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="dest">File path.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainFacade src, string dest) => src.Save(dest, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Save the PDF document
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="dest">Saving file information.</param>
        /// <param name="close">Close action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Save(this MainFacade src, Information dest, Action close)
        {
            var data = src.Bindable;
            var tmp  = data.IO.Combine(dest.DirectoryName, Guid.NewGuid().ToString("D"));

            try
            {
                var reader = data.Source.Value.GetItexReader(data.Query, data.IO);
                data.Set(reader.Metadata, reader.Encryption);

                using (var writer = new DocumentWriter())
                {
                    writer.Add(reader.Attachments);
                    writer.Add(data.Images.Select(e => e.RawObject), reader);
                    writer.Set(data.Metadata);
                    writer.Set(data.Encryption);
                    writer.Save(tmp);
                }

                close();
                src.Backup.Invoke(dest);
                data.IO.Copy(tmp, dest.FullName, true);
            }
            finally { data.IO.TryDelete(tmp); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Restruct
        ///
        /// <summary>
        /// Restructs some properties with the specified new PDF document.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="doc">New PDF document.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Restruct(this MainFacade src, IDocumentReader doc)
        {
            var items = doc.Pages.Select((v, i) => new { Value = v, Index = i });
            foreach (var e in items) src.Bindable.Images[e.Index].RawObject = e.Value;
            src.Bindable.Source.Value = doc.File;
            src.Bindable.History.Clear();
        }

        #endregion

        #region InsertOrMove

        /* ----------------------------------------------------------------- */
        ///
        /// IsInsertable
        ///
        /// <summary>
        /// Gets the value indicating whether the specified file is
        /// insertable.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="path">File path.</param>
        ///
        /// <remarks>
        /// TODO: 現在は拡張子で判断しているが、ファイル内容の Signature を
        /// 用いて判断するように修正する。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsInsertable(this MainFacade src, string path)
        {
            var ext = src.Bindable.IO.Get(path).Extension.ToLowerInvariant();
            var cmp = new List<string> { ".pdf", ".png", ".jpg", ".jpeg", ".bmp" };
            return cmp.Contains(ext);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Insert
        ///
        /// <summary>
        /// Inserts the specified file behind the selected index.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="files">Collection of inserting files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Insert(this MainFacade src, IEnumerable<string> files) =>
            src.Insert(src.Bindable.Images.Selection.Last + 1, files);

        /* ----------------------------------------------------------------- */
        ///
        /// InsertOrMove
        ///
        /// <summary>
        /// Inserts or moves the specified pages accoding to the specified
        /// condition.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="obj">Drag&amp;Drop result.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void InsertOrMove(this MainFacade src, DragDropObject obj)
        {
            if (!obj.IsCurrentProcess)
            {
                var index = Math.Min(obj.DropIndex + 1, src.Bindable.Count.Value);
                src.Insert(index, obj.Pages);
            }
            else if (obj.DragIndex < obj.DropIndex) src.MoveNext(obj);
            else src.MovePrevious(obj);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MovePrevious
        ///
        /// <summary>
        /// Moves selected items accoding to the specified condition.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="obj">Drag&amp;Drop result.</param>
        ///
        /* ----------------------------------------------------------------- */
        private static void MovePrevious(this MainFacade src, DragDropObject obj)
        {
            var delta = obj.DropIndex - obj.DragIndex;
            var n     = src.Bindable.Images.Selection.Indices
                           .Where(i => i < obj.DragIndex && i >= obj.DropIndex).Count();
            src.Move(delta + n);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveNext
        ///
        /// <summary>
        /// Moves selected items accoding to the specified condition.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="obj">Drag&amp;Drop result.</param>
        ///
        /* ----------------------------------------------------------------- */
        private static void MoveNext(this MainFacade src, DragDropObject obj)
        {
            var delta = obj.DropIndex - obj.DragIndex;
            var n     = src.Bindable.Images.Selection.Indices
                           .Where(i => i > obj.DragIndex && i <= obj.DropIndex).Count();
            src.Move(delta - n);
        }

        #endregion

        #region Metadata

        /* ----------------------------------------------------------------- */
        ///
        /// SetMetadata
        ///
        /// <summary>
        /// Sets the Metadata object.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="value">Metadata object.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem SetMetadata(this MainFacade src, Metadata value)
        {
            var prev = src.Bindable.Metadata;
            return Invoke(
                () => src.Bindable.Metadata = value,
                () => src.Bindable.Metadata = prev
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetEncryption
        ///
        /// <summary>
        /// Sets the Encryption object.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="value">Encryption object.</param>
        ///
        /// <returns>
        /// History item to execute undo and redo actions.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static HistoryItem SetEncryption(this MainFacade src, Encryption value)
        {
            var prev = src.Bindable.Encryption;
            return Invoke(
                () => src.Bindable.Encryption = value,
                () => src.Bindable.Encryption = prev
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Sets or resets the IsSelected property of all items according
        /// to the current condition.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Select(this MainFacade src) =>
            src.Select(src.Bindable.Images.Selection.Count < src.Bindable.Images.Count);

        /* ----------------------------------------------------------------- */
        ///
        /// Zoom
        ///
        /// <summary>
        /// Executes the Zoom command by using the current settings.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Zoom(this MainFacade src)
        {
            var items = src.Bindable.Images.Preferences.ItemSizeOptions;
            var prev  = src.Bindable.Images.Preferences.ItemSizeIndex;
            var next  = items.LastIndexOf(x => x <= src.Settings.Value.ItemSize);
            src.Zoom(next - prev);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action and creates a history item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static HistoryItem Invoke(Action forward, Action reverse)
        {
            forward(); // do
            return new HistoryItem { Undo = reverse, Redo = forward };
        }

        #endregion
    }
}
