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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cube.FileSystem;
using Cube.Observable.Extensions;
using Cube.Syntax.Extensions;
using Cube.Text.Extensions;
using Cube.Xui;

namespace Cube.Pdf.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractViewModel
    ///
    /// <summary>
    /// Represents the ViewModel associated with the ExtractWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractViewModel : DialogViewModel<ExtractFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the RemoveViewModel with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="callback">Callback method when applied.</param>
        /// <param name="selection">Page selection.</param>
        /// <param name="count">Number of pages.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel(Action<SaveOption> callback,
            ImageSelection selection,
            int count,
            SynchronizationContext context
        ) : base(new(selection, count, new ContextDispatcher(context, false)), new(), context)
        {
            OK.Command = new DelegateCommand(
                () => Quit(() => callback(Facade.Value), true),
                () => Facade.Value.Destination.HasValue() &&
                      !Io.IsDirectory(Facade.Value.Destination)
            ).Hook(Facade.Value, nameof(SaveOption.Destination));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// Gets the collection of extract formats.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<SaveFormat> Formats { get; } =
            Enum.GetValues(typeof(SaveFormat)).OfType<SaveFormat>();

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets the destination menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Destination => Get(() => new BindableElement<string>(
            () => Properties.Resources.MenuDestination,
            () => Facade.Value.Destination,
            e  => Facade.Value.Destination = e,
            GetDispatcher(false)
        ) {
            Command = new DelegateCommand(() => Send(
                Message.ForExtract(),
                e => Facade.Value.Destination = e,
                true
            ))
        }).Hook(Facade.Value, nameof(SaveOption.Destination));

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the format menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<SaveFormat> Format => Get(() => new BindableElement<SaveFormat>(
            () => Properties.Resources.MenuFormat,
            () => Facade.Value.Format,
            e  => Facade.Value.Format = e,
            GetDispatcher(false)
        )).Hook(Facade.Value, nameof(SaveOption.Format));

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the page count menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<int> Count => Get(() => new BindableElement<int>(
            () => Properties.Resources.MenuPageCount,
            () => Facade.Count,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Target
        ///
        /// <summary>
        /// Gets the target menu.
        /// </summary>
        ///
        /// <remarks>
        /// Value determines whether the Selected element is enabled.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Target => Get(() => new BindableElement(
            () => Properties.Resources.MenuTarget,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Selected
        ///
        /// <summary>
        /// Gets the menu to determine whether the selected pages are the
        /// target to extract.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> Selected => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuExtractSelected,
            () => Facade.Value.Target == SaveTarget.Selected,
            e  => e.Then(() => Facade.Value.Target = SaveTarget.Selected),
            GetDispatcher(false)
        ) {
            Command = new DelegateCommand(
                () => { },
                () => Facade.Selection.Count > 0
            )
        }).Hook(Facade.Value, nameof(SaveOption.Target));

        /* ----------------------------------------------------------------- */
        ///
        /// All
        ///
        /// <summary>
        /// Gets the menu to determine whether the all pages are the
        /// target to extract.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> All => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuExtractAll,
            () => Facade.Value.Target == SaveTarget.All,
            e  => e.Then(() => Facade.Value.Target = SaveTarget.All),
            GetDispatcher(false)
        )).Hook(Facade.Value, nameof(SaveOption.Target));

        /* ----------------------------------------------------------------- */
        ///
        /// Specified
        ///
        /// <summary>
        /// Gets the menu to determine whether the specified range of
        /// pages is the target to extract.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> Specified => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuExtractRange,
            () => Facade.Value.Target == SaveTarget.Range,
            e  => e.Then(() => Facade.Value.Target = SaveTarget.Range),
            GetDispatcher(false)
        )).Hook(Facade.Value, nameof(SaveOption.Target));

        /* ----------------------------------------------------------------- */
        ///
        /// Range
        ///
        /// <summary>
        /// Gets the page range menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<string> Range => Get(() => new BindableElement<string>(
            () => Properties.Resources.MessageRangeExample,
            () => Facade.Value.Range,
            e  => Facade.Value.Range = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Resolution
        ///
        /// <summary>
        /// Gets the resolution menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<int> Resolution => Get(() => new BindableElement<int>(
            () => Properties.Resources.MenuDpi,
            () => Facade.Value.Resolution,
            e  => Facade.Value.Resolution = e,
            GetDispatcher(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Gets the menu to determine whether to save as a separate file
        /// per page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<bool> Split => Get(() => new BindableElement<bool>(
            () => Properties.Resources.MenuSplit,
            () => Facade.Value.Split,
            e  => Facade.Value.Split = e,
            GetDispatcher(false)
        ) {
            Command = new DelegateCommand(
                () => { },
                () => Facade.Value.Format == SaveFormat.Pdf
            ).Hook(Facade.Value, nameof(SaveOption.Format))
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Option
        ///
        /// <summary>
        /// Gets the option menu. The property has text only.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Option => Get(() => new BindableElement(
            () => Properties.Resources.MenuOptions,
            GetDispatcher(false)
        ));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the dialog.
        /// </summary>
        ///
        /// <returns>String value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Properties.Resources.TitleExtract;

        #endregion
    }
}
