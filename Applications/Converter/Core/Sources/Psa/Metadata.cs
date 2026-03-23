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
namespace Cube.Pdf.Converter.Psa;

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

/* ------------------------------------------------------------------------- */
///
/// Metadata
///
/// <summary>
/// Represents the information shared across processes for a print job.
/// Carries the data needed for the virtual printer and the launcher
/// to coordinate without direct communication.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class Metadata
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// JobTitle
    ///
    /// <summary>
    /// Gets or sets the title for this print job.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string JobTitle { get; set; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// SessionId
    ///
    /// <summary>
    /// Gets or sets the session ID for this print job.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string SessionId { get; set; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// AppName
    ///
    /// <summary>
    /// Gets or sets the display name of the external application that
    /// initiated the print job.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string AppName { get; set; } = string.Empty;

    #endregion

    #region Constants

    /* --------------------------------------------------------------------- */
    ///
    /// DirectoryName
    ///
    /// <summary>
    /// The name of the publisher cache subfolder used to exchange data
    /// between the virtual printer and the launcher.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public const string DirectoryName = "printing";

    /* --------------------------------------------------------------------- */
    ///
    /// FileName
    ///
    /// <summary>
    /// The name of the metadata file written by the virtual printer and
    /// read by the launcher.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public const string FileName = "metadata.json";

    /* --------------------------------------------------------------------- */
    ///
    /// SourceFileName
    ///
    /// <summary>
    /// The name of the file that contains the raw print data written by
    /// the virtual printer and read by the launcher.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public const string SourceFileName = "source.dat";

    /* --------------------------------------------------------------------- */
    ///
    /// LockFileName
    ///
    /// <summary>
    /// The name of the lock file used to coordinate exclusive access
    /// between the virtual printer and the launcher.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public const string LockFileName = "job.lock";

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// LoadAsync
    ///
    /// <summary>
    /// Loads a Metadata instance from the specified JSON file. Returns
    /// null if the file does not exist or cannot be deserialized.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static async Task<Metadata?> LoadAsync(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            using var stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<Metadata>(stream, _options);
        }
        catch { return null; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAsync
    ///
    /// <summary>
    /// Saves this instance to the specified path as a JSON file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public async Task SaveAsync(string path)
    {
        using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, this, _options);
    }

    #endregion

    #region Fields
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented        = true,
    };
    #endregion
}
