/* ------------------------------------------------------------------------- */
///
/// FormExtensions.cs
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Cube.Pdf.ImageEx.Extensions
{
    /* --------------------------------------------------------------------- */
    ///
    /// Cube.Pdf.ImageEx.Extensions.FormExtensions
    ///
    /// <summary>
    /// フォームに対する拡張メソッド群を定義するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FormExtensions
    {
        /* ----------------------------------------------------------------- */
        ///
        /// UpdateTitle
        /// 
        /// <summary>
        /// フォームのタイトルを更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void UpdateTitle(this Form form, string str)
        {
            var asm  = Assembly.GetExecutingAssembly();
            var attr = Attribute.GetCustomAttribute(asm, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            var app  = (attr != null) ? attr.Title : string.Empty;

            var ss = new System.Text.StringBuilder();
            ss.Append(str);
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(app)) ss.Append(" - ");
            ss.Append(app);

            form.Text = ss.ToString();
        }
    }
}
