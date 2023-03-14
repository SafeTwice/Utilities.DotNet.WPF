/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using Utilities.WPF.Net.Windows;

namespace Utilities.WPF.Net.MVVM
{
    /// <summary>
    /// Basic implementation of a window associated to a viewmodel.
    /// </summary>
    public class MvvmWindow<TViewModel> : Window, IMvvmWindow where TViewModel : IMvvmWindowViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public TViewModel ViewModel => (TViewModel) DataContext;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public MvvmWindow( TViewModel viewModel )
        {
            DataContext = viewModel;

            ViewModel.MvvmWindow = this;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        void IMvvmWindow.Close()
        {
            Close();
        }

        public void NavigateTo( Window window )
        {
            WindowsUtilities.NavigateTo( this, window );
        }
    }
}
