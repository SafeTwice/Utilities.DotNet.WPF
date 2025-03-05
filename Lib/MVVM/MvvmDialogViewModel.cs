/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Simple implementation of a view-model that is associated to an IMvvmDialog.
    /// </summary>
    public class MvvmDialogViewModel : IMvvmDialogViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets or sets the dialog window associated to the view-model.
        /// </summary>
        public IMvvmDialog MvvmDialog
        {
            get => m_mvvmDialog ?? throw new InvalidOperationException( "MVVM dialog not initialized" );
            set => m_mvvmDialog = value;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Called when the associated dialog is loaded.
        /// </summary>
        /// <remarks>
        /// Can be overridden in derived classes to perform initialization tasks when the associated dialog is loaded.
        /// </remarks>
        public virtual void OnLoaded() { }

        /// <summary>
        /// Called when the associated dialog is closing.
        /// </summary>
        /// <remarks>
        /// Can be overridden in derived classes to decide whether the dialog can be closed or not.
        /// </remarks>
        /// <returns><see langword="true"/> if the dialog can be closed; <see langword="false"/> otherwise.</returns>
        public virtual bool OnClosing() => true;

        /// <summary>
        /// Called when the associated dialog is closed.
        /// </summary>
        /// <remarks>
        /// Can be overridden in derived classes to perform cleanup tasks when the associated dialog is closed.
        /// </remarks>
        public virtual void OnClosed() { }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private IMvvmDialog? m_mvvmDialog;
    }
}
