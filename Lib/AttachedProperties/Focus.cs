/// @file
/// @copyright  Copyright (c) 2020-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows;
using System.Windows.Input;
using Utilities.DotNet.WPF.Common;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Provides an attached property to set focus on a UI element.
    /// </summary>
    public static class Focus
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property to set focus on a UI element.
        /// </summary>
        public static readonly DependencyProperty SetFocusProperty =
            DependencyProperty.RegisterAttached( "SetFocus", typeof( DelegateTrigger ), typeof( Focus ),
                new FrameworkPropertyMetadata( null, OnSetFocusPropertyChanged ) );

        /// <summary>
        /// Sets the value of the SetFocus property.
        /// </summary>
        /// <param name="obj">Dependency object on which the property value is set.</param>
        /// <param name="value">Value to be set.</param>
        public static void SetSetFocus( DependencyObject obj, DelegateTrigger? value )
        {
            obj.SetValue( SetFocusProperty, value );
        }

        /// <summary>
        /// Gets the value of the SetFocus property.
        /// </summary>
        /// <param name="obj">Dependency object from which the property value is obtained.</param>
        /// <returns>Property value.</returns>
        [AttachedPropertyBrowsableForType( typeof( FrameworkElement ) )]
        public static DelegateTrigger? GetSetFocus( DependencyObject obj )
        {
            return (DelegateTrigger) obj.GetValue( SetFocusProperty );
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private static readonly DependencyPropertyKey SetFocusDelegateProperty =
            DependencyProperty.RegisterAttachedReadOnly( "_SetFocusDelegate", typeof( Action ), typeof( Focus ),
                new PropertyMetadata( null ) );

        private static void SetSetFocusDelegate( DependencyObject obj, Action? value )
        {
            obj.SetValue( SetFocusDelegateProperty, value );
        }

        private static Action? GetSetFocusDelegate( DependencyObject obj )
        {
            return (Action) obj.GetValue( SetFocusDelegateProperty.DependencyProperty );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void OnSetFocusPropertyChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
        {
            if( sender is FrameworkElement fe )
            {
                fe.Unloaded -= TargetElement_OnUnloaded;
                fe.Unloaded += TargetElement_OnUnloaded;

                if( e.OldValue is DelegateTrigger oldAction )
                {
                    var oldDelegate = GetSetFocusDelegate( fe );

                    if( oldDelegate != null )
                    {
                        oldAction.Activated -= oldDelegate;
                    }
                }

                if( e.NewValue is DelegateTrigger newAction )
                {
                    var newDelegate = () => SetFocus( fe );

                    SetSetFocusDelegate( fe, newDelegate );

                    newAction.Activated += newDelegate;
                }
                else
                {
                    SetSetFocusDelegate( fe, null );
                }
            }
        }

        private static void TargetElement_OnUnloaded( object sender, RoutedEventArgs e )
        {
            var fe = (FrameworkElement) sender;

            var currentTrigger = GetSetFocus( fe );
            var currentDelegate = GetSetFocusDelegate( fe );

            if( ( currentTrigger != null ) && ( currentDelegate != null ) )
            {
                currentTrigger.Activated -= currentDelegate;
            }
        }

        private static void TargetElement_OnLoaded( object sender, RoutedEventArgs e )
        {
            var fe = (FrameworkElement) sender;

            fe.Loaded -= TargetElement_OnLoaded;

            DoSetFocus( fe );
        }

        private static void SetFocus( FrameworkElement fe )
        {
            if( !fe.IsLoaded )
            {
                // Delay setting focus until the element is loaded.
                fe.Loaded += TargetElement_OnLoaded;
            }
            else
            {
                fe.Dispatcher?.BeginInvoke( () =>
                {
                    DoSetFocus( fe );
                } );
            }
        }

        private static void DoSetFocus( FrameworkElement fe )
        {
            Keyboard.Focus( fe );
        }
    }
}