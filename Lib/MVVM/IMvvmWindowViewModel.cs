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

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Called when the associated window is loaded.
        /// </summary>
        /// <remarks>
        /// This method allows the view-model to perform initialization tasks when the associated window is loaded.
        /// </remarks>
        void OnLoaded();

        /// <summary>
        /// Called when the associated window is closing.
        /// </summary>
        /// <remarks>
        /// This method allows the view-model to decide whether the associated window can be closed or not.
        /// </remarks>
        /// <returns><see langword="true"/> if the window can be closed; <see langword="false"/> otherwise.</returns>
        bool OnClosing();

        /// <summary>
        /// Called when the associated window is closed.
        /// </summary>
        /// <remarks>
        /// This method allows the view-model to perform cleanup tasks when the associated window is closed.
        /// </remarks>
        void OnClosed();
    }
}
