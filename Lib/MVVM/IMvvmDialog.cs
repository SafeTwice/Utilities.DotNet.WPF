/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Threading;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Represents the dialog window associated to a view-model.
    /// </summary>
    /// <remarks>
    /// This interface exposes a basic set of features to control the behavior of a dialog window from
    /// its associated view-model without breaking the MVVM design pattern.
    /// </remarks>
    public interface IMvvmDialog
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets the dispatcher of the dialog window.
        /// </summary>
        Dispatcher Dispatcher { get; }

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="result">Result of the dialog (see <see cref="System.Windows.Window.ShowDialog"/>).</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the window was not opened as a dialog.</exception>
        void Close( bool result );
    }
}
