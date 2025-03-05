/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.ComponentModel;
using System.Windows;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Basic implementation of a window associated to a view-model.
    /// </summary>
    public class MvvmDialog<TViewModel> : Window, IMvvmDialog where TViewModel : IMvvmDialogViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets the view-model associated to the dialog.
        /// </summary>
        public TViewModel ViewModel => (TViewModel) DataContext;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="viewModel">View-model associated to the dialog.</param>
        public MvvmDialog( TViewModel viewModel )
        {
            DataContext = viewModel;

            viewModel.MvvmDialog = this;

            Loaded += OnLoaded;
            Closing += OnClosing;
            Closed += OnClosed;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        void IMvvmDialog.Close( bool result )
        {
            DialogResult = result;
            Close();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            ViewModel.OnLoaded();
        }

        private void OnClosing( object? sender, CancelEventArgs e )
        {
            e.Cancel = !ViewModel.OnClosing();
        }

        private void OnClosed( object? sender, EventArgs e )
        {
            ViewModel.OnClosed();

            Loaded -= OnLoaded;
            Closing -= OnClosing;
            Closed -= OnClosed;

            DataContext = null;
        }
    }
}
