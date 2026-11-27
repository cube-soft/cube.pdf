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
using System.Threading;
using NUnit.Framework;

namespace Cube.Pdf.Editor.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// CacheCollectionTest
    ///
    /// <summary>
    /// Tests invalidation while a cache value is being created.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class CacheCollectionTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// ReplacePending
        ///
        /// <summary>
        /// Verifies that an old request cannot replace a newer result
        /// for the same key after removal or clearing.
        /// </summary>
        ///
        /// <param name="clear">Clear the cache instead of removing one key.</param>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(false)]
        [TestCase(true)]
        public void ReplacePending(bool clear)
        {
            using var started = new ManualResetEventSlim();
            using var release = new ManualResetEventSlim();
            using var created = new ManualResetEventSlim();
            using var discarded = new ManualResetEventSlim();
            var calls = 0;
            using var cache = new CacheCollection<int, int>(_ => {
                var value = Interlocked.Increment(ref calls);
                if (value == 1)
                {
                    started.Set();
                    if (!release.Wait(5000)) throw new System.TimeoutException();
                }
                return value;
            }, (_, value) => { if (value == 1) discarded.Set(); });
            cache.Created += (_, _) => created.Set();

            try
            {
                cache.GetOrCreate(0);
                Assert.That(started.Wait(5000), Is.True);
                cache.GetOrCreate(0);
                if (clear) cache.Clear();
                else cache.Remove(0);
                cache.GetOrCreate(0);
                Assert.That(created.Wait(5000), Is.True);
            }
            finally { release.Set(); }
            Assert.That(discarded.Wait(5000), Is.True);
            Assert.That(cache.GetOrCreate(0), Is.EqualTo(2));
            Assert.That(calls, Is.EqualTo(2));
        }

        #endregion
    }
}
