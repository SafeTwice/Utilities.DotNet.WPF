/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Simple implementation of a view-model that is associated to an IMvvmWindow.
    /// </summary>
    public class MvvmWindowViewModel : IMvvmWindowViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets or sets the window associated to the view-model.
        /// </summary>
        public IMvvmWindow MvvmWindow
        {
            get => m_mvvmWindow ?? throw new InvalidOperationException( "MVVM window not initialized" );
            set => m_mvvmWindow = value;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Called when the associated window is closing.
        /// </summary>
        /// <remarks>
        /// Can be overridden in derived classes to decide whether the window can be closed or not.
        /// </remarks>
        /// <returns><see langword="true"/> if the window can be closed; <see langword="false"/> otherwise.</returns>
        public virtual bool OnClosing() => true;

        /// <summary>
        /// Called when the associated window is closed.
        /// </summary>
        /// <remarks>
        /// Can be overridden in derived classes to perform cleanup tasks when the associated window is closed.
        /// </remarks>
        public virtual void OnClosed() { }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private IMvvmWindow? m_mvvmWindow;
    }
}
