/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that binds a routed event to a command.
    /// </summary>
    public class RoutedEventToCommand : MarkupExtension
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Binding to the command to execute when the event is raised.
        /// </summary>
        public Bind? Command { get; set; }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RoutedEventToCommand()
        {
        }

        /// <summary>
        /// Constructor with command parameter.
        /// </summary>
        /// <param name="command">Binding to the command to execute when the event is raised.</param>
        public RoutedEventToCommand( Bind command )
        {
            Command = command;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            if( serviceProvider == null )
            {
                return this;
            }

            var provideValueTarget = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;

            if( ( provideValueTarget != null ) && ( provideValueTarget.TargetObject is FrameworkElement targetObject ) )
            {
                if( Command != null )
                {
                    m_bindTarget = new BindTarget( targetObject );
                }

                return new RoutedEventHandler( OnEvent );
            }
            else
            {
                return this;
            }
        }

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private sealed class BindTarget : FrameworkElement, IProvideValueTarget, IServiceProvider
        {
            public static readonly DependencyProperty CommandProperty =
                DependencyProperty.Register( nameof( Command ), typeof( ICommand ), typeof( BindTarget ) );

            public ICommand? Command
            {
                get => (ICommand?) GetValue( CommandProperty );
                set => SetValue( CommandProperty, value );
            }

            public BindTarget( FrameworkElement referenceObject )
            {
                var dataContextBinding = new Binding
                {
                    Path = new PropertyPath( nameof( DataContext ) ),
                    Source = referenceObject,
                    Mode = BindingMode.OneWay
                };

                BindingOperations.SetBinding( this, DataContextProperty, dataContextBinding );
            }

            object IProvideValueTarget.TargetObject => this;
            object IProvideValueTarget.TargetProperty => CommandProperty;

            object? IServiceProvider.GetService( Type serviceType )
            {
                return ( serviceType == typeof( IProvideValueTarget ) ) ? this : null;
            }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnEvent( object sender, RoutedEventArgs e )
        {
            ICommand? command = null;

            if( ( Command != null ) && ( m_bindTarget != null ) )
            {
                // Binding is lazy to avoid binding errors if the reference object data context is not bound yet.
                if( m_bindingExpression == null )
                {
                    m_bindingExpression = BindingOperations.SetBinding( m_bindTarget, BindTarget.CommandProperty, Command.InternalBinding );
                }
                else
                {
                    m_bindingExpression.UpdateTarget();
                }

                command = m_bindTarget.Command;
            }

            if( command?.CanExecute( e ) == true )
            {
                command.Execute( e );
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private BindTarget? m_bindTarget;
        private BindingExpressionBase? m_bindingExpression;
    }
}
