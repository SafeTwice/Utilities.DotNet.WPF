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
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private IMvvmDialog? m_mvvmDialog;
    }
}
