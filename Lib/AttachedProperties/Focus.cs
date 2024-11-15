﻿/// @file
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
                new FrameworkPropertyMetadata( null, OnPropertyChanged ) );

        /// <summary>
        /// Sets the value of the SetFocus property.
        /// </summary>
        /// <param name="obj">Dependency object on which the property value is set.</param>
        /// <param name="value">Value to be set.</param>
        public static void SetSetFocus( DependencyObject obj, DelegateTrigger value )
        {
            obj.SetValue( SetFocusProperty, value );
        }

        /// <summary>
        /// Gets the value of the SetFocus property.
        /// </summary>
        /// <param name="obj">Dependency object from which the property value is obtained.</param>
        /// <returns>Property value.</returns>
        public static DelegateTrigger GetSetFocus( DependencyObject obj )
        {
            return (DelegateTrigger) obj.GetValue( SetFocusProperty );
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private static readonly DependencyProperty SetFocusDelegateProperty =
            DependencyProperty.RegisterAttached( "_SetFocusDelegate", typeof( Action ), typeof( Focus ) );

        private static void SetSetFocusDelegate( DependencyObject obj, Action value )
        {
            obj.SetValue( SetFocusDelegateProperty, value );
        }

        private static Action GetSetFocusDelegate( DependencyObject obj )
        {
            return (Action) obj.GetValue( SetFocusDelegateProperty );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void OnPropertyChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
        {
            if( sender is UIElement uiElement )
            {
                if( e.OldValue is DelegateTrigger oldAction )
                {
                    var oldDelegate = GetSetFocusDelegate( uiElement );

                    if( oldDelegate != null )
                    {
                        oldAction.Activated -= oldDelegate;
                    }
                }

                if( e.NewValue is DelegateTrigger newAction )
                {
                    var newDelegate = () => SetFocus( uiElement );

                    SetSetFocusDelegate( uiElement, newDelegate );

                    newAction.Activated += newDelegate;
                }
            }
        }

        private static void SetFocus( UIElement uiElement )
        {
            uiElement.Dispatcher?.BeginInvoke( () =>
            {
                if( uiElement is IInputElement )
                {
                    Keyboard.Focus( uiElement );
                }
                else
                {
                    uiElement.Focus();
                }
            } );
        }
    }
}
