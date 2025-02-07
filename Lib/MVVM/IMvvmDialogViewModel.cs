/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Represents a view-model that is associated to an IMvvmDialog.
    /// </summary>
    /// <remarks>
    /// The IMvvmDialog interface provides basic control of the behavior of the dialog window
    /// (which is often necessary to perform from view-models), without breaking the
    /// MVVM design pattern.
    /// </remarks>
    public interface IMvvmDialogViewModel
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets or sets the dialog window associated to the view-model.
        /// </summary>
        IMvvmDialog MvvmDialog { get; set; }
    }
}
