/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Threading;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Represents the window associated to a view-model.
    /// </summary>
    /// <remarks>
    /// This interface exposes a basic set of features to control the behavior of a window from
    /// its associated view-model without breaking the MVVM design pattern.
    /// </remarks>
    public interface IMvvmWindow
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets the dispatcher of the window.
        /// </summary>
        Dispatcher Dispatcher { get; }

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Closes the window.
        /// </summary>
        void Close();

        /// <summary>
        /// Navigates to the specified window.
        /// </summary>
        /// <param name="window">Window to navigate to.</param>
        void NavigateTo( Window window );
    }
}
