/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

#pragma warning disable IDE1006 // Naming Styles

using System.Windows;

namespace Utilities.DotNet.WPF.Windows
{
    /// <summary>
    /// Provides attached properties to change the behavior of <see cref="Window"/>s.
    /// </summary>
    public class WindowExtensions : DependencyObject
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Attached property to enable or disable the close button of a window.
        /// </summary>
        public static readonly DependencyProperty IsCloseButtonEnabledProperty =
            DependencyProperty.RegisterAttached( "IsCloseButtonEnabled", typeof( bool ), typeof( WindowExtensions ),
                                                 new PropertyMetadata( true, OnIsCloseButtonEnabledChanged ) );

        /// <summary>
        /// Gets the value of the IsCloseButtonEnabled attached property.
        /// </summary>
        /// <param name="obj">A dependency object</param>
        /// <returns><see langword="true"/> if the close button is enabled, <see langword="false"/> otherwise.</returns>
        public static bool GetIsCloseButtonEnabled( DependencyObject obj )
        {
            return (bool) obj.GetValue( IsCloseButtonEnabledProperty );
        }

        /// <summary>
        /// Sets the value of the IsCloseButtonEnabled attached property.
        /// </summary>
        /// <param name="obj">A dependency object</param>
        /// <param name="value"><see langword="true"/> to enable the close button, <see langword="false"/> to disable it.</param>
        public static void SetIsCloseButtonEnabled( DependencyObject obj, bool value )
        {
            obj.SetValue( IsCloseButtonEnabledProperty, value );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void OnIsCloseButtonEnabledChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if( e.NewValue != e.OldValue )
            {
                var window = d as Window;

                if( (bool) e.NewValue )
                {
                    window?.EnableCloseButton();
                }
                else
                {
                    window?.DisableCloseButton();
                }
            }
        }
    }
}
