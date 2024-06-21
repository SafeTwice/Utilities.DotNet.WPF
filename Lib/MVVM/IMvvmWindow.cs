/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Represents the window associated to a viewmodel.
    /// </summary>
    /// <remarks>
    /// This interface exposes a basic set of features to control the behavior of a window from
    /// its associated viewmodel without breaking the MVVM design pattern.
    /// </remarks>
    public interface IMvvmWindow
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Closes the window.
        /// </summary>
        void Close();

        void NavigateTo( Window window );
    }
}
