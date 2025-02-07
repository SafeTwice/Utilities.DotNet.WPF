/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Represents a view-model that is associated to an IMvvmWindow.
    /// </summary>
    /// <remarks>
    /// The IMvvmWindow interface provides basic control of the behavior of the window
    /// (which is often necessary to perform from view-models), without breaking the
    /// MVVM design pattern.
    /// </remarks>
    public interface IMvvmWindowViewModel
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets or sets the window associated to the view-model.
        /// </summary>
        IMvvmWindow MvvmWindow { get; set; }
    }
}
