/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MVVM
{
    /// <summary>
    /// Represents the dialog window associated to a viewmodel.
    /// </summary>
    /// <remarks>
    /// This interface exposes a basic set of features to control the behavior of a dialog window from
    /// its associated viewmodel without breaking the MVVM design pattern.
    /// </remarks>
    public interface IMvvmDialog
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="result">Result of the dialog (see <see cref="System.Windows.Window.ShowDialog"/>)</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the window was not opened as a dialog</exception>
        void Close( bool result );
    }
}
