/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using Utilities.DotNet.Collections;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Observer for columns source collections.
    /// </summary>
    internal sealed class GridViewColumnsObserver : IDisposable
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public GridViewColumnsObserver( GridView gridView, ICollectionView collectionView )
        {
            m_gridView = gridView;
            m_collectionView = collectionView;

            m_collectionView.CollectionChanged += ColumnsSourceView_CollectionChanged;
            m_gridView.Columns.CollectionChanged += GridViewColumns_CollectionChanged;

            LoadColumns();
        }

        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        ~GridViewColumnsObserver()
        {
            Dispose();
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public void Dispose()
        {
            m_collectionView.CollectionChanged -= ColumnsSourceView_CollectionChanged;
            m_gridView.Columns.CollectionChanged -= GridViewColumns_CollectionChanged;
        }

        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        internal void Detach()
        {
            m_gridView.Columns.Clear();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void LoadColumns()
        {
            m_gridView.Columns.Clear();

            var cellTemplateSelector = GridViewColumns.GetCellTemplateSelector( m_gridView );

            var cellDataContextSelector = GridViewColumns.GetCellDataContextSelector( m_gridView );

            foreach( var item in m_collectionView )
            {
                var column = CreateColumn( item, cellTemplateSelector, cellDataContextSelector );
                if( column != null )
                {
                    m_gridView.Columns.Add( column );
                }
            }
        }

        private GridViewColumn? CreateColumn( object? columnSourceItem, DataTemplateSelector? cellTemplateSelector, GridViewCellDataContextSelector? cellDataContextSelector )
        {
            if( columnSourceItem is not IGridViewColumnInfo columnInfo )
            {
                return null;
            }

            GridViewColumn column = new()
            {
                Header = columnInfo
            };

            if( cellTemplateSelector != null )
            {
                Func<object, object?> columnDataContextSelector;

                if( cellDataContextSelector == null )
                {
                    columnDataContextSelector = ( item ) => item;
                }
                else
                {
                    columnDataContextSelector = ( item ) => cellDataContextSelector( item, columnInfo );
                }

                var columnTemplateSelector = new GridViewColumnTemplateSelector( columnDataContextSelector, cellTemplateSelector );

                column.CellTemplateSelector = columnTemplateSelector;
            }

            Binding widthBinding = new( nameof( columnInfo.Width ) )
            {
                Source = columnInfo,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding( column, GridViewColumn.WidthProperty, widthBinding );

            ( (INotifyPropertyChanged) column ).PropertyChanged += ( sender, e ) =>
            {
                if( e.PropertyName == nameof( column.ActualWidth ) )
                {
                    columnInfo.ActualWidth = column.ActualWidth;
                }
            };

            return column;
        }

        private void AddNewColumns( IList? newColumns, int insertionIndex )
        {
            if( newColumns == null )
            {
                return;
            }

            var cellTemplateSelector = GridViewColumns.GetCellTemplateSelector( m_gridView );

            var cellDataContextSelector = GridViewColumns.GetCellDataContextSelector( m_gridView );

            for( int i = 0; i < newColumns.Count; i++ )
            {
                var column = CreateColumn( newColumns[ i ], cellTemplateSelector, cellDataContextSelector );
                if( column != null )
                {
                    m_gridView.Columns.Insert( insertionIndex + i, column );
                }
            }
        }

        private void RemoveColumns( int startIndex, int count )
        {
            for( int i = 0; i < count; i++ )
            {
                m_gridView.Columns.RemoveAt( startIndex );
            }
        }

        private void ColumnsSourceView_CollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            if( m_ignoreColumnSourceChanges )
            {
                return;
            }

            Debug.Assert( ReferenceEquals( sender, m_collectionView ) );

            switch( e.Action )
            {
                case NotifyCollectionChangedAction.Add:
                    AddNewColumns( e.NewItems, e.NewStartingIndex );
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    RemoveColumns( e.OldStartingIndex, ( e.OldItems?.Count ?? 0 ) );
                    AddNewColumns( e.NewItems, e.NewStartingIndex );
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveColumns( e.OldStartingIndex, ( e.OldItems?.Count ?? 0 ) );
                    break;

                case NotifyCollectionChangedAction.Reset:
                    LoadColumns();
                    break;

                default:
                    break;
            }
        }

        private void GridViewColumns_CollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            if( e.Action == NotifyCollectionChangedAction.Move )
            {
                if( m_collectionView.SourceCollection is IListEx list )
                {
                    m_ignoreColumnSourceChanges = true;

                    int oldIndex = e.OldStartingIndex;
                    int newIndex = e.NewStartingIndex;

                    if( newIndex > oldIndex )
                    {
                        newIndex++;
                    }

                    list.Move( oldIndex, newIndex );

                    m_ignoreColumnSourceChanges = false;
                }
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly GridView m_gridView;
        private readonly ICollectionView m_collectionView;
        private bool m_ignoreColumnSourceChanges = false;

    }
}
