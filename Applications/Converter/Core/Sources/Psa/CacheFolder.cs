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
namespace Cube.Pdf.Converter.Psa;

using Cube.FileSystem;
using Windows.Storage;

/* ------------------------------------------------------------------------- */
///
/// CacheFolder
///
/// <summary>
/// Provides access to the publisher cache folder shared between the
/// virtual printer and the launcher.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class CacheFolder
{
    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Returns the publisher cache folder, or null if the folder is
    /// unavailable.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static StorageFolder Get() => ApplicationData.Current.GetPublisherCacheFolder(Metadata.DirectoryName);

    /* --------------------------------------------------------------------- */
    ///
    /// Cleanup
    ///
    /// <summary>
    /// Forcibly deletes the lock file if it exists. Called on startup
    /// with no arguments (e.g. at install time) to remove any stale lock
    /// file left by a previous abnormal termination.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static void Cleanup()
    {
        var dir = Get();
        if (dir is not null)
        {
            var lok = Io.Combine(dir.Path, Metadata.LockFileName);
            if (Io.Exists(lok))
            {
                Logger.Warn($"{Metadata.LockFileName} deleted forcibly");
                Logger.Try(() => Io.Delete(lok));
            }
        }
        else Logger.Warn($"{Metadata.DirectoryName} directory not found");
    }
}
