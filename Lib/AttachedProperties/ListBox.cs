/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Controls;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    using ListBoxControl = System.Windows.Controls.ListBox;

    /// <summary>
    /// Provides attached properties to change the behavior of <see cref="ListBoxControl">ListBox</see>es.
    /// </summary>
    public static class ListBox
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property to scroll a <see cref="ListBoxControl">ListBox</see> to the selected item when the selection changes.
        /// </summary>
        public static readonly DependencyProperty ScrollToSelectedItem =
            DependencyProperty.RegisterAttached( "ScrollToSelectedItem", typeof( bool ), typeof( ListBox ),
                new PropertyMetadata( false, OnScrollToSelectedItemChanged ) );

        /// <summary>
        /// Sets the value of the ScrollToSelectedItem attached property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetScrollToSelectedItem( DependencyObject obj, bool value )
        {
            obj.SetValue( ScrollToSelectedItem, value );
        }

        /// <summary>
        /// Gets the value of the ScrollToSelectedItem attached property.
        /// </summary>
        /// <param name="obj">The dependency object from which to get the property.</param>
        /// <returns>The value of the ScrollToSelectedItem attached property.</returns>
        [AttachedPropertyBrowsableForType( typeof( ListBoxControl ) )]
        public static bool GetScrollToSelectedItem( DependencyObject obj )
        {
            return (bool) obj.GetValue( ScrollToSelectedItem );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void OnScrollToSelectedItemChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if( ( e.NewValue is bool newValue ) && ( e.OldValue is bool oldValue ) && ( newValue != oldValue ) &&
                ( d is ListBoxControl listBox ) )
            {
                if( newValue )
                {
                    listBox.SelectionChanged += ScrollToSelectedItem_OnSelectionChangedEvent;
                    listBox.IsVisibleChanged += ScrollToSelectedItem_OnIsVisibleChangedEvent;
                }
                else
                {
                    listBox.SelectionChanged -= ScrollToSelectedItem_OnSelectionChangedEvent;
                    listBox.IsVisibleChanged -= ScrollToSelectedItem_OnIsVisibleChangedEvent;

                }
            }
        }

        private static void ScrollToSelectedItem_OnSelectionChangedEvent( object sender, SelectionChangedEventArgs e )
        {
            ScrollSelectionIntoView( (ListBoxControl) sender );
        }

        private static void ScrollToSelectedItem_OnIsVisibleChangedEvent( object sender, DependencyPropertyChangedEventArgs e )
        {
            if( ( e.NewValue is bool isVisible ) && isVisible )
            {
                ScrollSelectionIntoView( (ListBoxControl) sender );
            }
        }

        private static void ScrollSelectionIntoView( ListBoxControl listBox )
        {
            if( listBox.SelectedItem == null )
            {
                return;
            }

            listBox.Dispatcher.BeginInvoke( () =>
            {
                listBox.UpdateLayout();

                var selectedItem = listBox.SelectedItem;

                if( selectedItem != null )
                {
                    listBox.ScrollIntoView( selectedItem );
                }
            } );
        }
    }
}
