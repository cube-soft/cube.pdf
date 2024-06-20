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
namespace Cube.Pdf.Converter.Tests;

using System;
using Cube.Globalization;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// Program
///
/// <summary>
/// Represents the main program.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[SetUpFixture]
static class Program
{
    /* --------------------------------------------------------------------- */
    ///
    /// OneTimeSetup
    ///
    /// <summary>
    /// Invokes the setup method only once.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [OneTimeSetUp]
    public static void OneTimeSetup()
    {
        Logger.Configure(new Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);
        Locale.Subscribe(Surface.Localizable);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Main
    ///
    /// <summary>
    /// Represents the main method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [STAThread]
    static void Main() { }
}
