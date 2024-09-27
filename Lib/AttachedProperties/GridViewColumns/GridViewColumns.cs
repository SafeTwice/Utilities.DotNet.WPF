/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Provides attached properties to generate columns for a <see cref="GridView"/>.
    /// </summary>
    public static class GridViewColumns
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the collection that generates the columns for a <see cref="GridView"/>.
        /// </summary>
        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.RegisterAttached( "ColumnsSource", typeof( object ), typeof( GridViewColumns ),
                new PropertyMetadata( null, ColumnsSourceChanged ) );

        /// <summary>
        /// Gets the collection that generates the columns for a <see cref="GridView"/>.
        /// </summary>
        /// <param name="obj">The dependency object to get the value from.</param>
        /// <returns>The source collection.</returns>
        [AttachedPropertyBrowsableForType( typeof( GridView ) )]
        public static object GetColumnsSource( DependencyObject obj )
        {
            return obj.GetValue( ColumnsSourceProperty );
        }

        /// <summary>
        /// Sets the collection that generates the columns for a <see cref="GridView"/>.
        /// </summary>
        /// <param name="obj">The dependency object to set the value to.</param>
        /// <param name="value">Collection of <see cref="IGridViewColumnsSourceItem"/>.</param>
        public static void SetColumnsSource( DependencyObject obj, object value )
        {
            obj.SetValue( ColumnsSourceProperty, value );
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private static readonly DependencyProperty ObserverProperty =
            DependencyProperty.RegisterAttached( "_ColumnsSourceObserver", typeof( GridViewColumnsObserver ), typeof( GridViewColumns ),
                new PropertyMetadata( null ) );

        private static GridViewColumnsObserver? GetObserver( DependencyObject obj )
        {
            return (GridViewColumnsObserver) obj.GetValue( ObserverProperty );
        }

        private static void SetObserver( DependencyObject obj, GridViewColumnsObserver? value )
        {
            obj.SetValue( ObserverProperty, value );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void ColumnsSourceChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            var gridView = obj as GridView;
            if( gridView == null )
            {
                return;
            }

            var oldObserver = GetObserver( obj );

            oldObserver?.Detach();
            oldObserver?.Dispose();

            if( e.NewValue != null )
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView( e.NewValue );
                if( collectionView != null )
                {
                    var newObserver = new GridViewColumnsObserver( gridView, collectionView );

                    SetObserver( obj, newObserver );
                }
            }
        }
    }
}