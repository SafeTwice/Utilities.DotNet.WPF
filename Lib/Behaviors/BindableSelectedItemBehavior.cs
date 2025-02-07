/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using Utilities.DotNet.WPF.Extensions;

namespace Utilities.DotNet.WPF.Behaviors
{
    /// <summary>
    /// Behavior to bind the selected item of a <see cref="TreeView"/> to a view-model property.
    /// </summary>
    public class BindableSelectedItemBehavior : Behavior<TreeView>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register( "SelectedItem", typeof( object ), typeof( BindableSelectedItemBehavior ),
                                         new UIPropertyMetadata( null, OnSelectedItemChanged ) );

        /// <summary>
        /// Gets or sets the selected item of the <see cref="TreeView"/>.
        /// </summary>
        public object? SelectedItem
        {
            get { return GetValue( SelectedItemProperty ); }
            set { SetValue( SelectedItemProperty, value ); }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        /// <inheritdoc/>
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
