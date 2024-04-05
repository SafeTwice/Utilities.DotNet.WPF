/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using Utilities.WPF.Net.Extensions;

namespace Utilities.WPF.Net.Behaviors
{
    /// <summary>
    /// Behavior to bind the selected item of a TreeView to a view model property.
    /// </summary>
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        private static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register( "SelectedItem", typeof( object ), typeof( BindableSelectedItemBehavior ),
                                         new UIPropertyMetadata( null, OnSelectedItemChanged ) );

        public object? SelectedItem
        {
            get { return GetValue( SelectedItemProperty ); }
            set { SetValue( SelectedItemProperty, value ); }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if( AssociatedObject != null )
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void OnSelectedItemChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
        {
            ( sender as BindableSelectedItemBehavior )?.OnSelectedItemChanged( e.OldValue, e.NewValue );
        }

        private void OnSelectedItemChanged( object? oldValue, object? newValue )
        {
            if( newValue == oldValue )
            {
                return;
            }

            if( newValue == null )
            {
                AssociatedObject.DeselectAllItems();
            }
            else
            {
                var tvItem = AssociatedObject.GetTreeViewItemForValue( newValue );
                if( ( tvItem != null ) && ( !tvItem.IsSelected ) )
                {
                    tvItem.IsSelected = true;
                }
            }
        }

        private void OnTreeViewSelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            SelectedItem = e.NewValue;
        }
    }
}
