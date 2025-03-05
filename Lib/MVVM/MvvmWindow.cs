/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.ComponentModel;
using System.Windows;
using Utilities.DotNet.WPF.Windows;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Basic implementation of a window associated to a view-model.
    /// </summary>
    public class MvvmWindow<TViewModel> : Window, IMvvmWindow where TViewModel : IMvvmWindowViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets the view-model associated to the window.
        /// </summary>
        public TViewModel ViewModel => (TViewModel) DataContext;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="viewModel">View-model associated to the window</param>
        public MvvmWindow( TViewModel viewModel )
        {
            DataContext = viewModel;

            viewModel.MvvmWindow = this;

            Closing += OnClosing;

            Closed += OnClosed;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        void IMvvmWindow.Close()
        {
            Close();
        }

        /// <summary>
        /// Navigates to the specified window.
        /// </summary>
        /// <param name="window">Window to navigate to.</param>
        public void NavigateTo( Window window )
        {
            WindowsUtilities.NavigateTo( this, window );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnClosing( object? sender, CancelEventArgs e )
        {
            e.Cancel = !ViewModel.OnClosing();
        }

        private void OnClosed( object? sender, EventArgs e )
        {
            ViewModel.OnClosed();

            Closing -= OnClosing;
            Closed -= OnClosed;

            DataContext = null;
        }
    }
}
