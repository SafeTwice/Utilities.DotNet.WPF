/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.WPF.Net.MVVM
{
    /// <summary>
    /// Simple implementation of a viewmodel that is associated to an IMvvmDialog.
    /// </summary>
    public class MvvmDialogViewModel : IMvvmDialogViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

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
