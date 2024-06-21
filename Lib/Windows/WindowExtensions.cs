/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

#pragma warning disable IDE1006 // Naming Styles

using System.Windows;

namespace Utilities.DotNet.WPF.Windows
{
    public class WindowExtensions : DependencyObject
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public static readonly DependencyProperty IsCloseButtonEnabledProperty =
            DependencyProperty.RegisterAttached( "IsCloseButtonEnabled", typeof( bool ), typeof( WindowExtensions ),
                                                 new PropertyMetadata( true, OnIsCloseButtonEnabledChanged ) );

        public static bool GetIsCloseButtonEnabled( DependencyObject obj )
        {
            return (bool) obj.GetValue( IsCloseButtonEnabledProperty );
        }

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
