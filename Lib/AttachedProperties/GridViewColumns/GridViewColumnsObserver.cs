/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

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

            m_collectionView.CollectionChanged += ColumnsSource_CollectionChanged;

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
            m_collectionView.CollectionChanged -= ColumnsSource_CollectionChanged;
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
                GridViewColumn column = CreateColumn( item, cellTemplateSelector, cellDataContextSelector );
                m_gridView.Columns.Add( column );
            }
        }

        private GridViewColumn CreateColumn( object? columnSourceItem, DataTemplateSelector? cellTemplateSelector, GridViewCellDataContextSelector? cellDataContextSelector )
        {
            var columnInfo = columnSourceItem as IGridViewColumnInfo;

            GridViewColumn column = new GridViewColumn();

            if( columnInfo != null )
            {
                column.Header = columnInfo.Name;

                if( cellTemplateSelector != null )
                {
                    Func<object, object?> columnDataContextSelector;
                    
                    if( cellDataContextSelector == null )
                    {
                        columnDataContextSelector = (item) => item;
                    }
                    else
                    {
                        columnDataContextSelector = ( item ) => cellDataContextSelector( item, columnInfo );
                    }

                    var columnTemplateSelector = new GridViewColumnTemplateSelector( columnDataContextSelector, cellTemplateSelector );

                    column.CellTemplateSelector = columnTemplateSelector;
                }

                if( columnInfo.Width != null )
                {
                    column.Width = columnInfo.Width.Value;
                }
            }
            else
            {
                column.Width = 0;
            }

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
                GridViewColumn column = CreateColumn( newColumns[ i ], cellTemplateSelector, cellDataContextSelector );
                m_gridView.Columns.Insert( insertionIndex + i, column );
            }
        }

        private void RemoveColumns( int startIndex, int count )
        {
            for( int i = 0; i < count; i++ )
            {
                m_gridView.Columns.RemoveAt( startIndex );
            }
        }

        private void ColumnsSource_CollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
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

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly GridView m_gridView;
        private readonly ICollectionView m_collectionView;
    }
}
