/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;

namespace Utilities.WPF.Net.MVVM
{
    /// <summary>
    /// Basic implementation of a window associated to a viewmodel.
    /// </summary>
    public class MvvmDialog<TViewModel> : Window, IMvvmDialog where TViewModel : IMvvmDialogViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public TViewModel ViewModel => (TViewModel) DataContext;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public MvvmDialog( TViewModel viewModel )
        {
            DataContext = viewModel;

            ViewModel.MvvmDialog = this;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        void IMvvmDialog.Close( bool result )
        {
            DialogResult = result;
            Close();
        }
    }
}
