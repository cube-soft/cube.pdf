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
namespace Cube.Pdf;

using System.Security.Cryptography;
using Cube.FileSystem;

/* ------------------------------------------------------------------------- */
///
/// Attachment
///
/// <summary>
/// Represents an attachment file in the PDF document.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class Attachment
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Attachment
    ///
    /// <summary>
    /// Initializes a new instance of the Attachment class with the
    /// specified parameters.
    /// </summary>
    ///
    /// <param name="path">Path of the attached file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Attachment(string path) : this(Io.GetFileName(path), path) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Attachment
    ///
    /// <summary>
    /// Initializes a new instance of the Attachment class with the
    /// specified parameters.
    /// </summary>
    ///
    /// <param name="name">Display name.</param>
    /// <param name="path">Path of the attached file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Attachment(string name, string path)
    {
        Name   = name;
        Source = path;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Name
    ///
    /// <summary>
    /// Gets the displayed name of the attached file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Name { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the path of the attached file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Length
    ///
    /// <summary>
    /// Gets the data length of the attached file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long Length => GetLength();

    /* --------------------------------------------------------------------- */
    ///
    /// Data
    ///
    /// <summary>
    /// Gets the data of the attached file in byte unit.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public byte[] Data => _data ??= GetData();

    /* --------------------------------------------------------------------- */
    ///
    /// Checksum
    ///
    /// <summary>
    /// Gets the checksum of attached file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public byte[] Checksum => _checksum ??= GetChecksum();

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetLength
    ///
    /// <summary>
    /// Gets the data length of the attached file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual long GetLength() => Io.Get(Source)?.Length ?? 0;

    /* --------------------------------------------------------------------- */
    ///
    /// GetData
    ///
    /// <summary>
    /// Gets the data of the attached file in byte unit.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual byte[] GetData()
    {
        if (!Io.Exists(Source)) return null;

        using var src  = Io.Open(Source);
        using var dest = new System.IO.MemoryStream();

        src.CopyTo(dest);
        return dest.ToArray();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetChecksum
    ///
    /// <summary>
    /// Gets the checksum of attached file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual byte[] GetChecksum() =>
        Io.Exists(Source) ?
        IoEx.Load(Source, e => new SHA256CryptoServiceProvider().ComputeHash(e)) :
        default;

    #endregion

    #region Fields
    private byte[] _data;
    private byte[] _checksum;
    #endregion
}
