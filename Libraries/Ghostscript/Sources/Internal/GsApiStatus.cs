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
namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// GsApiStatus
    ///
    /// <summary>
    /// Specifies status code of the Ghostscript API.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum GsApiStatus
    {
        /// <summary>unknownerror (PostScript Level 1 error)</summary>
        UnknownError = -1,
        /// <summary>dictfull (PostScript Level 1 error)</summary>
        DictFull = -2,
        /// <summary>dictstackoverflow (PostScript Level 1 error)</summary>
        DictStackOverflow = -3,
        /// <summary>dictstackunderflow (PostScript Level 1 error)</summary>
        DictStackUnderflow = -4,
        /// <summary>execstackoverflow (PostScript Level 1 error)</summary>
        ExecStackOverflow = -5,
        /// <summary>interrupt (PostScript Level 1 error)</summary>
        Interrupt = -6,
        /// <summary>invalidaccess (PostScript Level 1 error)</summary>
        InvalidAccess = -7,
        /// <summary>invalidexit (PostScript Level 1 error)</summary>
        InvalidExit = -8,
        /// <summary>invalidfileaccess (PostScript Level 1 error)</summary>
        InvalidFileAccess = -9,
        /// <summary>invalidfont (PostScript Level 1 error)</summary>
        InvalidFont = -10,
        /// <summary>invalidrestore (PostScript Level 1 error)</summary>
        InvalidRestore = -11,
        /// <summary>ioerror (PostScript Level 1 error)</summary>
        IoError = -12,
        /// <summary>limitcheck (PostScript Level 1 error)</summary>
        LimitCheck = -13,
        /// <summary>nocurrentpoint (PostScript Level 1 error)</summary>
        NoCurrentPoint = -14,
        /// <summary>rangecheck (PostScript Level 1 error)</summary>
        RangeCheck = -15,
        /// <summary>statckoverflow (PostScript Level 1 error)</summary>
        StackOverflow = -16,
        /// <summary>stackunderflow (PostScript Level 1 error)</summary>
        StackUnderflow = -17,
        /// <summary>syntaxerror (PostScript Level 1 error)</summary>
        SyntaxError = -18,
        /// <summary>timeout (PostScript Level 1 error)</summary>
        Timeout = -19,
        /// <summary>typecheck (PostScript Level 1 error)</summary>
        TypeCheck = -20,
        /// <summary>undefined (PostScript Level 1 error)</summary>
        Undefined = -21,
        /// <summary>undefinedfilename (PostScript Level 1 error)</summary>
        UndefinedFilename = -22,
        /// <summary>undefinedresult (PostScript Level 1 error)</summary>
        UndefinedResult = -23,
        /// <summary>unmatchedmark (PostScript Level 1 error)</summary>
        UnmatchedMark = -24,
        /// <summary>vmerror (PostScript Level 1 error)</summary>
        VmError = -25,
        /// <summary>configurationerror (DPS error)</summary>
        ConfigurationError = -26,
        /// <summary>undefinedresource (DPS error)</summary>
        UndefinedResource = -27,
        /// <summary>unregistered (DPS error)</summary>
        Unregistered = -28,
        /// <summary>invalidcontext (Additional DPS error)</summary>
        InvalidContext = -29,
        /// <summary>invalidcontext (Additional DPS error)</summary>
        InvalidId = -30,
        /// <summary>Fatal (Pseudo-errors used internally)</summary>
        Fatal = -100,
        /// <summary>Quit (Pseudo-errors used internally)</summary>
        Quit = -101,
        /// <summary>InterpreterExit (Pseudo-errors used internally)</summary>
        InterpreterExit = -102,
        /// <summary>RemapColor (Pseudo-errors used internally)</summary>
        RemapColor = -103,
        /// <summary>ExecStackUnderflow (Pseudo-errors used internally)</summary>
        ExecStackUnderflow = -104,
        /// <summary>VMreclaim (Pseudo-errors used internally)</summary>
        VmReclaim = -105,
        /// <summary>NeedInput (Pseudo-errors used internally)</summary>
        NeedInput = -106,
        /// <summary>Info (Pseudo-errors used internally)</summary>
        Info = -110,
    }
}
