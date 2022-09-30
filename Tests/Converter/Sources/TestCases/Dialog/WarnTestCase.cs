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
using Cube.FileSystem;
using Cube.Pdf.Converter;
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
    /// FileExists
    ///
    /// <summary>
    /// Tests the confirmation when the specified file exists.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task FileExists(MainViewModel vm)
    {
        var name = nameof(FileExists);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => {
            e.Value  = DialogStatus.No;
            e.Cancel = true;
            msg = e;
        });

        Io.Copy(GetSource("Sample.pdf"), vm.Settings.Destination, true);
        vm.Invoke();

        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Warning), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return TaskEx.FromResult(0);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MetadataHasValue
    ///
    /// <summary>
    /// Tests the confirmation when the current settings are saved.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task MetadataHasValue(MainViewModel vm)
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

        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Warning), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return TaskEx.FromResult(0);
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
        yield return Make(nameof(FileExists), FileExists);
        yield return Make(nameof(MetadataHasValue), MetadataHasValue);
    }

    #endregion
}
