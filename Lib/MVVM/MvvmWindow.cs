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
        public TViewModel ViewModel => m_viewModel!;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        static MvvmWindow()
        {
            DataContextProperty.OverrideMetadata(
                typeof( MvvmWindow<TViewModel> ),
                new FrameworkPropertyMetadata( OnDataContextChangedEvent ) );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="viewModel">View-model associated to the window</param>
        public MvvmWindow( TViewModel viewModel )
        {
            m_viewModel = viewModel;
            DataContext = viewModel;

            m_viewModel.MvvmWindow = this;

            Loaded += OnLoaded;
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

        private static void OnDataContextChangedEvent( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var window = (MvvmWindow<TViewModel>) d;
            if( !ReferenceEquals( e.NewValue, window.ViewModel ) )
            {
                throw new InvalidOperationException( $"The DataContext of {typeof( MvvmWindow<TViewModel> ).Name} windows cannot be modified." );
            }
        }

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
            Loaded -= OnLoaded;
            Closing -= OnClosing;
            Closed -= OnClosed;

            ViewModel.OnClosed();

            m_viewModel = default;
            DataContext = m_viewModel;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private TViewModel? m_viewModel;
    }
}
