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

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Called when the associated dialog is closing.
        /// </summary>
        /// <remarks>
        /// This method allows the view-model to decide whether the associated dialog can be closed or not.
        /// </remarks>
        /// <returns><see langword="true"/> if the dialog can be closed; <see langword="false"/> otherwise.</returns>
        bool OnClosing();

        /// <summary>
        /// Called when the associated dialog is closed.
        /// </summary>
        /// <remarks>
        /// This method allows the view-model to perform cleanup tasks when the associated dialog is closed.
        /// </remarks>
        void OnClosed();
    }
}
