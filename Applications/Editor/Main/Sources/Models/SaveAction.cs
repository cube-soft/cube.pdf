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
namespace Cube.Pdf.Editor;

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using Cube.FileSystem;
using Cube.Syntax.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// SaveAction
///
/// <summary>
/// Provides functionality to save the document.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class SaveAction : DisposableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAction
    ///
    /// <summary>
    /// Initializes a new instance of the SaveAction class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source reader.</param>
    /// <param name="images">Image collection.</param>
    /// <param name="options">Save options.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SaveAction(IDocumentReader src, ImageCollection images, SaveOption options)
    {
        Source  = src;
        Images  = images;
        Options = options;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the object to reader the source PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IDocumentReader Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Images
    ///
    /// <summary>
    /// Gets the collection of images.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ImageCollection Images { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Options
    ///
    /// <summary>
    /// Gets the save options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SaveOption Options { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the save action.
    /// </summary>
    ///
    /// <param name="prev">Action to be invoked before saving.</param>
    /// <param name="next">Action to be invoked after saving.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Invoke(Action<Entity> prev, Action<Entity> next)
    {
        if (Options.Format == SaveFormat.Pdf)
        {
            if (Options.Split) SplitAsDocument(prev, next);
            else SaveAsDocument(prev, next);
        }
        else SplitAsImage(prev, next);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the object
    /// and optionally releases the managed resources.
    /// </summary>
    ///
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources;
    /// false to release only unmanaged resources.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void Dispose(bool disposing)
    {
        if (disposing) Source.Dispose();
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAsDocument
    ///
    /// <summary>
    /// Saves pages as a PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SaveAsDocument(Action<Entity> prev, Action<Entity> next) => SaveAsDocument(
        Options.Destination,
        Source,
        GetIndices(Options).Select(i => Images[i].RawObject),
        prev,
        next
    );

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAsDocument
    ///
    /// <summary>
    /// Saves pages as a PDF document.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SaveAsDocument(string dest, IDocumentReader src, IEnumerable<Page> pages,
        Action<Entity> prev, Action<Entity> next
    ) {
        var exist = Io.Exists(dest);
        var dir   = Options.Temp.HasValue() ? Options.Temp : Io.GetDirectoryName(dest);
        var tmp   = Io.Combine(dir, Guid.NewGuid().ToString("n"));

        try
        {
            SaveWithItext(tmp, src, pages);
            Logger.Debug($"[Temp] {tmp.Quote()} ({Io.Exists(tmp)})");

            var e = new Entity(dest);
            prev(e);
            if (exist)
            {
                Io.SetCreationTime(tmp, e.CreationTime);
                Io.SetAttributes(tmp, e.Attributes);
            }
            Io.Copy(tmp, dest, true);
            next(e);
        }
        finally
        {
            Logger.Try(() => Io.Delete(tmp));
            Logger.Debug($"[Save] {dest.Quote()} ({exist} -> {Io.Exists(dest)})");
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveWithItext
    ///
    /// <summary>
    /// Saves pages as a PDF document using iText.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SaveWithItext(string dest, IDocumentReader src, IEnumerable<Page> pages)
    {
        using var writer = new Itext.DocumentWriter(Options.ToItext());

        if (Options.Attachments != null) writer.Add(Options.Attachments);
        if (Options.Metadata    != null) writer.Set(Options.Metadata);
        if (Options.Encryption  != null) writer.Set(Options.Encryption);
        if (src != null) writer.Add(pages, src);
        else writer.Add(pages);

        writer.Save(dest);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SplitAsDocument
    ///
    /// <summary>
    /// Saves as a PDF file per page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SplitAsDocument(Action<Entity> prev, Action<Entity> next)
    {
        var e = new Entity(Options.Destination);
        GetIndices(Options).Each(i => SaveAsDocument(
            GetPath(e, i, Images.Count),
            null,
            new[] { Images[i].RawObject },
            prev,
            next
        ));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SplitAsImage
    ///
    /// <summary>
    /// Saves as an image file per page.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SplitAsImage(Action<Entity> prev, Action<Entity> next)
    {
        var fi = new Entity(Options.Destination);
        foreach (var i in GetIndices(Options))
        {
            var ratio = Options.Resolution / Images[i].RawObject.Resolution.X;
            using var image = Images.GetImage(i, ratio);
            var dest = new Entity(GetPath(fi, i, Images.Count));
            prev(dest);
            image.Save(dest.FullName, ImageFormat.Png);
            next(dest);
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetPath
    ///
    /// <summary>
    /// Gets the output path with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetPath(Entity src, int index, int count)
    {
        var digit = $"D{Math.Max(count.ToString("D").Length, 2)}";
        var name  = $"{src.BaseName}-{(index + 1).ToString(digit)}{src.Extension}";
        return Io.Combine(src.DirectoryName, name);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetIndices
    ///
    /// <summary>
    /// Gets the target indices according to the specified options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<int> GetIndices(SaveOption e) => e.Target switch
    {
        SaveTarget.All      => Images.Count.Make(i => i),
        SaveTarget.Selected => Images.GetSelectedIndices().OrderBy(i => i),
        _ => new Range(e.Range, Images.Count).Select(i => i - 1)
    };

    #endregion
}