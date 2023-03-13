/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MVVM
{
    /// <summary>
    /// Represents a viewmodel that is associated to an IMvvmWindow.
    /// </summary>
    /// <remarks>
    /// The IMvvmWindow interface provides basic control of the behavior of the window
    /// (which is often necessary to perform from viewmodels), without breaking the
    /// MVVM design pattern.
    /// </remarks>
    public interface IMvvmWindowViewModel
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        IMvvmWindow MvvmWindow { get; set; }
    }
}
