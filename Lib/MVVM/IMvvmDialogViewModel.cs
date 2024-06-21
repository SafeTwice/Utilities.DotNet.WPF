/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Represents a viewmodel that is associated to an IMvvmDialog.
    /// </summary>
    /// <remarks>
    /// The IMvvmDialog interface provides basic control of the behavior of the dialog window
    /// (which is often necessary to perform from viewmodels), without breaking the
    /// MVVM design pattern.
    /// </remarks>
    public interface IMvvmDialogViewModel
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        IMvvmDialog MvvmDialog { get; set; }
    }
}
