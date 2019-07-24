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
using Cube.Xui;
using System;
using System.Collections.Generic;
using System.Threading;

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
    public sealed class ExtractViewModel : DialogViewModel<ExtractOption>
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
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel(Action<ExtractOption> callback,
            SynchronizationContext context
        ) : base(new ExtractOption(new ContextInvoker(context, false)),
            new Aggregator(),
            context
        ) {
            OK.Command = new DelegateCommand(
                () => Track(() => {
                    callback(Facade);
                    Send<CloseMessage>();
                }),
                () => true
            );
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
        public IEnumerable<ExtractFormat> Formats { get; } = new[]
        {
            ExtractFormat.Pdf,
            ExtractFormat.Png,
        };

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
            () => Facade.Destination,
            e  => Facade.Destination = e,
            GetInvoker(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the format menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement<ExtractFormat> Format => Get(() => new BindableElement<ExtractFormat>(
            () => Properties.Resources.MenuFormat,
            () => Facade.Format,
            e  => Facade.Format = e,
            GetInvoker(false)
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
            () => Facade.Target == ExtractTarget.Selected,
            e  => { if (e) Facade.Target = ExtractTarget.Selected; },
            GetInvoker(false)
        ));

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
            () => Facade.Target == ExtractTarget.All,
            e  => { if (e) Facade.Target = ExtractTarget.All; },
            GetInvoker(false)
        ));

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
            () => Facade.Target == ExtractTarget.Range,
            e  => { if (e) Facade.Target = ExtractTarget.Range; },
            GetInvoker(false)
        ));

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
            () => Facade.Range,
            e  => Facade.Range = e,
            GetInvoker(false)
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
            () => Facade.Split,
            e  => Facade.Split = e,
            GetInvoker(false)
        ));

        #region Texts

        /* ----------------------------------------------------------------- */
        ///
        /// Target
        ///
        /// <summary>
        /// Gets the target menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Target => Get(() => new BindableElement(
            () => Properties.Resources.MenuTarget,
            GetInvoker(false)
        ));

        /* ----------------------------------------------------------------- */
        ///
        /// Option
        ///
        /// <summary>
        /// Gets the option menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IElement Option => Get(() => new BindableElement(
            () => Properties.Resources.MenuOptions,
            GetInvoker(false)
        ));

        #endregion

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
