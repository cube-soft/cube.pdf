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
namespace Cube.Pdf.Ghostscript;

using System.Text;
using Cube.Mixin.String;

/* ------------------------------------------------------------------------- */
///
/// Argument
///
/// <summary>
/// Represents an element of Ghostscript command arguments.
/// </summary>
///
/// <see href="https://www.ghostscript.com/doc/current/Use.htm" />
///
/* ------------------------------------------------------------------------- */
public class Argument
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified name and value.
    /// </summary>
    ///
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /// <remarks>
    /// Only this constructor considers the value to be a literal object.
    /// If you need to consider a value as a literal object otherwise,
    /// execute the Argument(string, string, string, bool) constructor.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(string name, string value) : this('d', name, value, true) { } // see remarks.

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type) : this(type, string.Empty, string.Empty) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type and value.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /// <remarks>
    /// This is mainly used with the non-d options. (e.g, -r72)
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type, int value) : this(type, string.Empty, value) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type and name.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    /// <param name="name">Name of the argument.</param>
    ///
    /// <remarks>
    /// This is mainly used with the d options. (e.g, -dBATCH)
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type, string name) : this(type, name, string.Empty) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified name and value.
    /// </summary>
    ///
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(string name, bool value) : this('d', name, value) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified name and value.
    /// </summary>
    ///
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(string name, int value) : this('d', name, value) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type, name, and value.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type, string name, bool value) :
        this(type, name, value.ToString().ToLowerInvariant()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type, name, and value.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type, string name, int value) :
        this(type, name, value.ToString()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type, name, and value.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type, string name, string value) :
        this(type, name, value, false) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified type, name, and value.
    /// </summary>
    ///
    /// <param name="type">Type of the argument.</param>
    /// <param name="name">Name of the argument.</param>
    /// <param name="value">Value of the argument.</param>
    /// <param name="literal">Value is literal or not.</param>
    ///
    /// <remarks>
    /// For literal values, the value is prefixed with "/".
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public Argument(char type, string name, string value, bool literal)
    {
        Type      = type;
        Name      = name;
        Value     = value;
        IsLiteral = literal;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Argument
    ///
    /// <summary>
    /// Initializes a new instance of the Argument class with the
    /// specified description.
    /// </summary>
    ///
    /// <param name="description">Code description.</param>
    ///
    /// <remarks>
    /// This is mainly used to hold PostScript code.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    protected Argument(string description) :
        this(default, string.Empty, description, false) { }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Dummy
    ///
    /// <summary>
    /// Gets the dummy instance of the Argument class.
    /// </summary>
    ///
    /// <remarks>
    /// It is used mainly as the first argument because Ghostscript
    /// ignores the first argument.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static Argument Dummy { get; } = new Argument("gs");

    /* --------------------------------------------------------------------- */
    ///
    /// Type
    ///
    /// <summary>
    /// Gets the type of the argument.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public char Type { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Name
    ///
    /// <summary>
    /// Gets the name of the argument.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Name { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Value
    ///
    /// <summary>
    /// Gets the value of the argument.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Value { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// IsLiteral
    ///
    /// <summary>
    /// Gets the value indicating whether the value is literal.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool IsLiteral { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// ToString
    ///
    /// <summary>
    /// Returns a string that represents the argument.
    /// </summary>
    ///
    /// <returns>String that represents the argument.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (Type != default(char)) _ = sb.Append($"-{Type}");
        if (Name.HasValue()) _ = sb.Append(Name);
        if (Value.HasValue())
        {
            if (Name.HasValue()) _ = sb.Append('=');
            if (IsLiteral) _ = sb.Append('/');
            _ = sb.Append(Value);
        }

        return sb.ToString();
    }

    #endregion
}

/* ------------------------------------------------------------------------- */
///
/// Code
///
/// <summary>
/// Represents a PostScript code that is executed in the Ghostscript.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class Code : Argument
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Code
    ///
    /// <summary>
    /// Initializes a new instance of the Code class with the
    /// specified description.
    /// </summary>
    ///
    /// <param name="description">Code description.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Code(string description) : base(description) { }

    #endregion
}
