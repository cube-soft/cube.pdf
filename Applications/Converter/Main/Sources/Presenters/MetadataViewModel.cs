﻿/* ------------------------------------------------------------------------- */
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
namespace Cube.Pdf.Converter;

using System.Linq;
using System.Threading;
using Cube.Observable.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// MetadataViewModel
///
/// <summary>
/// Represents the ViewModel for the metadata tab in the main window.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class MetadataViewModel : PresentableBase<Metadata>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// MetadataViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the MetadataViewModel class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">PDF metadata.</param>
    /// <param name="proxy">Message aggregator.</param>
    /// <param name="ctx">Synchronization context.</param>
    ///
    /* --------------------------------------------------------------------- */
    public MetadataViewModel(Metadata src, Aggregator proxy, SynchronizationContext ctx) :
        base(src, proxy, ctx) => Assets.Add(src.Forward(this));

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Title
    ///
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Title
    {
        get => Facade.Title;
        set => Facade.Title = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Author
    ///
    /// <summary>
    /// Gets or sets the author.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Author
    {
        get => Facade.Author;
        set => Facade.Author = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Subject
    ///
    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Subject
    {
        get => Facade.Subject;
        set => Facade.Subject = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Keywords
    ///
    /// <summary>
    /// Gets or sets the keywords.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Keywords
    {
        get => Facade.Keywords;
        set => Facade.Keywords = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Creator
    ///
    /// <summary>
    /// Gets or sets the name of creator program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Creator
    {
        get => Facade.Creator;
        set => Facade.Creator = value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Version
    ///
    /// <summary>
    /// Gets or sets the PDF version.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Version
    {
        get => Facade.Version.Minor;
        set => Facade.Version = new PdfVersion(1, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Options
    ///
    /// <summary>
    /// Gets or sets the view options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ViewerOption Options
    {
        get => Facade.Options;
        set => Facade.Options = value;
    }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Confirm
    ///
    /// <summary>
    /// Confirms if it is OK to save the current settings.
    /// </summary>
    ///
    /// <returns>true for OK.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public bool Confirm()
    {
        var src = new[] { Title, Author, Subject, Keywords };
        if (src.All(e => !e.HasValue())) return true;

        var msg = Message.Warn(Surface.Texts.Warn_Metadata);
        Send(msg);
        return !msg.Cancel;
    }

    #endregion
}
