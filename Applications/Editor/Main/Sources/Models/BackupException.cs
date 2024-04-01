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

/* ------------------------------------------------------------------------- */
///
/// BackupException
///
/// <summary>
/// Represents an exception that occurred during the backup process.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class BackupException : Exception
{
    /* --------------------------------------------------------------------- */
    ///
    /// PdfiumException
    ///
    /// <summary>
    /// Initializes a new instance of the class with the specified arguments.
    /// </summary>
    ///
    /// <param name="inner">Inner exception.</param>
    ///
    /* --------------------------------------------------------------------- */
    public BackupException(Exception inner) : base("Backup failed", inner) { }
}
