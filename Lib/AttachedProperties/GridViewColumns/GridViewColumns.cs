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
    /// Selector for the data context of a cell in a <see cref="GridView"/>.
    /// </summary>
    /// <param name="item">Item to select the data context from.</param>
    /// <param name="columnInfo">Column for which to select the data context.</param>
    /// <returns>The data context for the cell.</returns>
    public delegate object? GridViewCellDataContextSelector( object item, IGridViewColumnInfo columnInfo );

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
                new PropertyMetadata( null, OnColumnsSourceChangedEvent ) );

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
        /// <param name="value">Collection of <see cref="IGridViewColumnInfo"/>.</param>
        public static void SetColumnsSource( DependencyObject obj, object value )
        {
            obj.SetValue( ColumnsSourceProperty, value );
        }

        /// <summary>
        /// Dependency property for the selector of the data context of a cell in a <see cref="GridView"/>.
        /// </summary>
        public static readonly DependencyProperty CellDataContextSelectorProperty =
            DependencyProperty.RegisterAttached( "CellDataContextSelector", typeof( GridViewCellDataContextSelector ), typeof( GridViewColumns ),
                new PropertyMetadata( null, OnChangeThatNeedsColumnReloadEvent ) );

        /// <summary>
        /// Gets the selector for the data context of a cell in a <see cref="GridView"/>.
        /// </summary>
        /// <param name="obj">The dependency object to get the value from.</param>
        /// <returns>The selector for the data context of a cell.</returns>
        [AttachedPropertyBrowsableForType( typeof( GridView ) )]
        public static GridViewCellDataContextSelector? GetCellDataContextSelector( DependencyObject obj )
        {
            return (GridViewCellDataContextSelector) obj.GetValue( CellDataContextSelectorProperty );
        }

        /// <summary>
        /// Sets the selector for the data context of a cell in a <see cref="GridView"/>.
        /// </summary>
        /// <param name="obj">The dependency object to set the value to.</param>
        /// <param name="value">The selector for the data context of a cell.</param>
        public static void SetCellDataContextSelector( DependencyObject obj, GridViewCellDataContextSelector? value )
        {
            obj.SetValue( CellDataContextSelectorProperty, value );
        }

        /// <summary>
        /// Dependency property for the data template selector for the cells in a <see cref="GridView"/>.
        /// </summary>
        public static readonly DependencyProperty CellTemplateSelectorProperty =
            DependencyProperty.RegisterAttached( "CellTemplateSelector", typeof( DataTemplateSelector ), typeof( GridViewColumns ),
                new PropertyMetadata( null, OnChangeThatNeedsColumnReloadEvent ) );

        /// <summary>
        /// Gets the data template selector for the cells in a <see cref="GridView"/>.
        /// </summary>
        /// <param name="obj">The dependency object to get the value from.</param>
        /// <returns>The data template selector for the cells.</returns>
        [AttachedPropertyBrowsableForType( typeof( GridView ) )]
        public static DataTemplateSelector? GetCellTemplateSelector( DependencyObject obj )
        {
            return (DataTemplateSelector) obj.GetValue( CellTemplateSelectorProperty );
        }

        /// <summary>
        /// Sets the data template selector for the cells in a <see cref="GridView"/>.
        /// </summary>
        /// <param name="obj">The dependency object to set the value to.</param>
        /// <param name="value">The data template selector for the cells.</param>
        public static void SetCellTemplateSelector( DependencyObject obj, DataTemplateSelector? value )
        {
            obj.SetValue( CellTemplateSelectorProperty, value );
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

        private static void OnColumnsSourceChangedEvent( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            var gridView = obj as GridView;
            if( gridView == null )
            {
                return;
            }

            ReloadColumns( gridView, e.NewValue );
        }
        private static void OnChangeThatNeedsColumnReloadEvent( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            var gridView = obj as GridView;
            if( gridView == null )
            {
                return;
            }

            var columnsSource = GetColumnsSource( gridView );
            ReloadColumns( gridView, columnsSource );
        }

        private static void ReloadColumns( GridView gridView, object? columnsSource )
        {
            var oldObserver = GetObserver( gridView );

            oldObserver?.Detach();
            oldObserver?.Dispose();

            if( columnsSource != null )
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView( columnsSource );
                if( collectionView != null )
                {
                    var newObserver = new GridViewColumnsObserver( gridView, collectionView );

                    SetObserver( gridView, newObserver );
                }
            }
        }
    }
}