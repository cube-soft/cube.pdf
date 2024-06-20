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
using Cube.Tests;
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
    /// GhostscriptError
    ///
    /// <summary>
    /// Tests the error handling when a Ghostscript API error occurs.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private async Task GhostscriptError(MainViewModel vm)
    {
        var name = nameof(GhostscriptError);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => msg = e);

        vm.Invoke();

        Assert.That(await Wait.ForAsync(() => msg is not null), "Timeout");
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MergeError
    ///
    /// <summary>
    /// Tests the error handling when merging into an existing file fails.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private async Task MergeError(MainViewModel vm)
    {
        var name = nameof(MergeError);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => {
            if (e.Icon == DialogIcon.Warning)
            {   // Confirmation of merging into an existing file.
                e.Value  = DialogStatus.Ok;
                e.Cancel = false;
            }
            else msg = e;
        });

        Io.Copy(GetSource("SampleAes256.pdf"), vm.Settings.Destination, true);
        vm.Settings.SaveOption = SaveOption.MergeTail;
        vm.Invoke();

        Assert.That(await Wait.ForAsync(() => msg is not null), "Timeout");
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SourceEmpty
    ///
    /// <summary>
    /// Tests the error handling when source file not specified.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private async Task SourceEmpty(MainViewModel vm)
    {
        var name = nameof(MergeError);
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => msg = e);

        vm.Settings.Source = "";
        vm.Invoke();

        Assert.That(await Wait.ForAsync(() => msg is not null), "Timeout");
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DigestNotMatch
    ///
    /// <summary>
    /// Tests the error handling when the SHA-256 digest of the source file
    /// does not match.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private async Task DigestNotMatch(MainViewModel vm)
    {
        var name = nameof(DigestNotMatch);
        var src  = Io.Combine(Io.GetDirectoryName(vm.Settings.Destination), $"{name}.ps");
        var msg  = default(DialogMessage);

        using var dc = vm.Subscribe<DialogMessage>(e => msg = e);

        Io.Copy(GetSource("Sample.ps"), src, true);
        vm.Settings.Source = src;
        vm.Invoke();

        Assert.That(await Wait.ForAsync(() => msg is not null), "Timeout");
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");
    }

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

        vm.Security.Enabled       = true;
        vm.Security.OwnerPassword = name;
        vm.Invoke();

        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);

        msg = default;
        vm.Security.OwnerConfirm = "dummy";
        vm.Invoke();

        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);
        Logger.Debug($"[{name}] {msg.Text} ({vm.Settings.Language})");

        return TaskEx.FromResult(0);
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

        vm.Security.Enabled          = true;
        vm.Security.OwnerPassword    = "owner";
        vm.Security.OwnerConfirm     = "owner";
        vm.Security.OpenWithPassword = true;
        vm.Security.SharePassword    = false;
        vm.Security.UserPassword     = name;
        vm.Invoke();

        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);

        msg = default;
        vm.Security.UserConfirm = "dummy";
        vm.Invoke();

        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Icon, Is.EqualTo(DialogIcon.Error), msg.Text);
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
        yield return Make(nameof(GhostscriptError), "Sample.txt", GhostscriptError);
        yield return Make(nameof(MergeError), MergeError);
        yield return Make(nameof(SourceEmpty), SourceEmpty);
        yield return Make(nameof(DigestNotMatch), DigestNotMatch);
        yield return Make(nameof(OwnerConfirmNotMatch), OwnerConfirmNotMatch);
        yield return Make(nameof(UserConfirmNotMatch), UserConfirmNotMatch);
    }

    #endregion
}
