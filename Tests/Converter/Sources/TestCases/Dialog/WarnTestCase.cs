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
namespace Cube.Pdf.Converter.Tests;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cube.Pdf.Converter;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// WarnTestCase
///
/// <summary>
/// Represents test cases to show a dialog for confirmation. These test
/// cases are invoked through the DialogTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
class WarnTestCase : TestCaseBase<Func<MainViewModel, Task>>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// MetadataHasValue
    ///
    /// <summary>
    /// Tests the confirmation when the current settings are saved.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private async Task MetadataHasValue(MainViewModel vm)
    {
        var name = nameof(MetadataHasValue);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => {
            e.Value  = DialogStatus.No;
            e.Cancel = true;
            msg = e;
        });

        vm.Metadata.Title = name;
        vm.Save();
        Assert.That(await Wait.ForAsync(() => msg is not null), "Timeout");
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Warning));
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the collection of test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override IEnumerable<TestCaseData> Get()
    {
        yield return Make(nameof(MetadataHasValue), MetadataHasValue);
    }

    #endregion
}
