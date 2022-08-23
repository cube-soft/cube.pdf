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
/// ErrorTestCase
///
/// <summary>
/// Represents test cases to show a dialog when some entered settings
/// are wrong. These test cases are invoked through the DialogTest class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
sealed class ErrorTestCase : TestCaseBase<Func<MainViewModel, Task>>
{
    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// OwnerConfirmNotMatch
    ///
    /// <summary>
    /// Tests the error handling when the entered confirmation password
    /// does not match.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task OwnerConfirmNotMatch(MainViewModel vm)
    {
        var name = nameof(OwnerConfirmNotMatch);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => msg = e);

        vm.Encryption.Enabled       = true;
        vm.Encryption.OwnerPassword = name;

        vm.Invoke();
        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error));

        msg = default;
        vm.Encryption.OwnerConfirm = "dummy";
        vm.Invoke();
        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error));
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return Task.FromResult(0);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// UserConfirmNotMatch
    ///
    /// <summary>
    /// Tests the error handling when the entered confirmation password
    /// does not match.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Task UserConfirmNotMatch(MainViewModel vm)
    {
        var name = nameof(UserConfirmNotMatch);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => msg = e);

        vm.Encryption.Enabled          = true;
        vm.Encryption.OwnerPassword    = "owner";
        vm.Encryption.OwnerConfirm     = "owner";
        vm.Encryption.OpenWithPassword = true;
        vm.Encryption.SharePassword    = false;
        vm.Encryption.UserPassword     = name;

        vm.Invoke();
        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error));

        msg = default;
        vm.Encryption.UserConfirm = "dummy";
        vm.Invoke();
        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error));
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return Task.FromResult(0);
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
        yield return Make(nameof(OwnerConfirmNotMatch), OwnerConfirmNotMatch);
        yield return Make(nameof(UserConfirmNotMatch), UserConfirmNotMatch);
    }

    #endregion
}
