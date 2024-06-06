/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace Utilities.WPF.Net.Behaviors
{
    /// <summary>
    /// Behavior to redirect a command received by an <see cref="UIElement"/> (normally a standard <see cref="RoutedCommand"/>
    /// like those defined in <see cref="ApplicationCommands"/>, <see cref="ComponentCommands"/> or <see cref="NavigationCommands"/> )
    /// to another command (normally bound to a command defined in a view model).
    /// </summary>
    public class RedirectCommandBinding : Behavior<UIElement>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the source command to redirect.
        /// </summary>
        public static readonly DependencyProperty SourceCommandProperty = 
            DependencyProperty.Register( "SourceCommand", typeof( ICommand ), typeof( RedirectCommandBinding ),
                new PropertyMetadata( null, OnSourceCommandChanged ) );

        /// <summary>
        /// Source command to redirect.
        /// </summary>
        public ICommand SourceCommand
        {
            get => (ICommand) GetValue( SourceCommandProperty );
            set => SetValue( SourceCommandProperty, value );
        }

        /// <summary>
        /// Dependency property for the target command to execute.
        /// </summary>
        public static readonly DependencyProperty TargetCommandProperty =
            DependencyProperty.Register( "TargetCommand", typeof( ICommand ), typeof( RedirectCommandBinding ) );

        /// <summary>
        /// Target command to execute.
        /// </summary>
        public ICommand TargetCommand
        {
            get => (ICommand) GetValue( TargetCommandProperty );
            set => SetValue( TargetCommandProperty, value );
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override void OnAttached()
        {
            base.OnAttached();
            RegisterBinding();
        }

        protected override void OnDetaching()
        {
            UnregisterBinding();
            base.OnDetaching();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void RegisterBinding()
        {
            if( ( m_commandBinding == null ) && ( AssociatedObject != null ) )
            {
                m_commandBinding = new( SourceCommand, OnExecuted, OnCanExecute );
                AssociatedObject.CommandBindings.Add( m_commandBinding );
            }
        }

        private void UnregisterBinding()
        {
            if( m_commandBinding != null )
            {
                AssociatedObject?.CommandBindings.Remove( m_commandBinding );
                m_commandBinding = null;
            }
        }

        private static void OnSourceCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var instance = (RedirectCommandBinding) d;
            instance.UnregisterBinding();
            instance.RegisterBinding();
        }

        private void OnExecuted( object sender, ExecutedRoutedEventArgs e )
        {
            TargetCommand?.Execute( e.Parameter );
        }

        private void OnCanExecute( object sender, CanExecuteRoutedEventArgs e )
        {
            e.CanExecute = TargetCommand?.CanExecute( e.Parameter ) ?? false;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private CommandBinding? m_commandBinding;
    }
}
