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
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// FileTestCase
///
/// <summary>
/// Represents test cases to show a dialog for selecting files. These test
/// cases are invoked through the DialogTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
class FileTestCase : TestCaseBase<Func<MainViewModel, Task>>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// SelectSource
    ///
    /// <summary>
    /// Tests to select a source file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task SelectSource(MainViewModel vm)
    {
        var name = nameof(SelectSource);
        var msg  = default(OpenFileMessage);

        using var dc = vm.Subscribe<OpenFileMessage>(e => {
            e.Value  = new[] { name };
            e.Cancel = false;
            msg = e;
        });

        vm.SelectSource();
        Assert.That(msg, Is.Not.Null);
        Assert.That(vm.Settings.Source, Is.EqualTo(name));
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return TaskEx.FromResult(0);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SelectDestination
    ///
    /// <summary>
    /// Tests to select a destination file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task SelectDestination(MainViewModel vm)
    {
        var name = nameof(SelectDestination);
        var msg  = default(SaveFileMessage);

        using var dc = vm.Subscribe<SaveFileMessage>(e => {
            e.Value  = name;
            e.Cancel = false;
            msg = e;
        });

        vm.SelectDestination();
        Assert.That(msg, Is.Not.Null);
        Assert.That(vm.Settings.Destination, Is.EqualTo(name));
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return TaskEx.FromResult(0);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SelectUserProgram
    ///
    /// <summary>
    /// Tests to select a user program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task SelectUserProgram(MainViewModel vm)
    {
        var name = nameof(SelectUserProgram);
        var msg  = default(OpenFileMessage);

        using var dc = vm.Subscribe<OpenFileMessage>(e => {
            e.Value  = new[] { name };
            e.Cancel = false;
            msg = e;
        });

        vm.Settings.PostProcess = PostProcess.Others;
        Assert.That(msg, Is.Not.Null);
        Assert.That(vm.Settings.UserProgram, Is.EqualTo(name));
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
        yield return Make(nameof(SelectSource), SelectSource);
        yield return Make(nameof(SelectDestination), SelectDestination);
        yield return Make(nameof(SelectUserProgram), SelectUserProgram);
    }

    #endregion
}
