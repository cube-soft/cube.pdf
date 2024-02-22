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
namespace Cube.Pdf.Editor;

using System;
using System.Windows;
using System.Linq;

/* ------------------------------------------------------------------------- */
///
/// DragDropExtension
///
/// <summary>
/// Represents the extended methods to handle drag&amp;drop objects.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class DragDropExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// OpenOrInsert
    ///
    /// <summary>
    /// Opens or inserts the specified files according to the specified
    /// condition.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    /// <param name="obj">Drag&amp;Drop result.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void OpenOrInsert(this MainFacade src, DragEventArgs obj)
    {
        var ins = obj.KeyStates.HasFlag(DragDropKeyStates.ControlKey) && src.Value.Source != null;
        if (ins)
        {
            var files = obj.GetFiles();
            src.Insert(int.MaxValue, src.Folder.Value.AutoSort ? files.Sort() : files);
        }
        else src.Open(obj.GetFiles().FirstPdf());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InsertOrMove
    ///
    /// <summary>
    /// Inserts or moves the specified pages according to the specified
    /// condition.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    /// <param name="obj">Drag&amp;Drop result.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void InsertOrMove(this MainFacade src, DragDropObject obj)
    {
        if (!obj.IsCurrentProcess)
        {
            var index = Math.Min(obj.DropIndex + 1, src.Value.Count);
            src.Insert(index, obj.Pages);
        }
        else if (obj.DragIndex < obj.DropIndex) src.MoveNext(obj);
        else src.MovePrevious(obj);
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// MovePrevious
    ///
    /// <summary>
    /// Moves selected items according to the specified condition.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void MovePrevious(this MainFacade src, DragDropObject obj)
    {
        var drop  = Math.Max(obj.DropIndex, 0);
        var delta = drop - obj.DragIndex;
        var n = src.Value.Images.Selection.Indices
                   .Where(i => i < obj.DragIndex && i >= obj.DropIndex)
                   .Count();

        src.Move(delta + n);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MoveNext
    ///
    /// <summary>
    /// Moves selected items according to the specified condition.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void MoveNext(this MainFacade src, DragDropObject obj)
    {
        var drop  = Math.Min(obj.DropIndex, src.Value.Count - 1);
        var delta = drop - obj.DragIndex;
        var n = src.Value.Images.Selection.Indices
                   .Where(i => i > obj.DragIndex && i <= obj.DropIndex)
                   .Count();

        src.Move(delta - n);
    }

    #endregion
}
