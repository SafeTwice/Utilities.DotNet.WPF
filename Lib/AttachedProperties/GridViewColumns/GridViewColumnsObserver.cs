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

            CreateColumns();
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
        //                            PRIVATE METHODS
        //===========================================================================

        private void CreateColumns()
        {
            m_gridView.Columns.Clear();

            foreach( var item in m_collectionView )
            {
                GridViewColumn column = CreateColumn( item );
                m_gridView.Columns.Add( column );
            }
        }

        private static GridViewColumn CreateColumn( object? columnSourceItem )
        {
            var sourceItem = columnSourceItem as IGridViewColumnsSourceItem;

            GridViewColumn column = new GridViewColumn();

            if( sourceItem != null )
            {
                if( sourceItem.DisplayMemberBinding != null )
                {
                    column.DisplayMemberBinding = sourceItem.DisplayMemberBinding;
                }

                if( sourceItem.CellTemplate != null )
                {
                    column.CellTemplate = sourceItem.CellTemplate;
                }

                if( sourceItem.CellTemplateSelector != null )
                {
                    column.CellTemplateSelector = sourceItem.CellTemplateSelector;
                }

                if( sourceItem.Header != null )
                {
                    column.Header = sourceItem.Header;
                }

                if( !string.IsNullOrEmpty( sourceItem.HeaderStringFormat ) )
                {
                    column.HeaderStringFormat = sourceItem.HeaderStringFormat;
                }

                if( sourceItem.HeaderContainerStyle != null )
                {
                    column.HeaderContainerStyle = sourceItem.HeaderContainerStyle;
                }

                if( sourceItem.HeaderTemplate != null )
                {
                    column.HeaderTemplate = sourceItem.HeaderTemplate;
                }

                if( sourceItem.HeaderTemplateSelector != null )
                {
                    column.HeaderTemplateSelector = sourceItem.HeaderTemplateSelector;
                }

                if( sourceItem.Width != null )
                {
                    column.Width = sourceItem.Width.Value;
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

            for( int i = 0; i < newColumns.Count; i++ )
            {
                GridViewColumn column = CreateColumn( newColumns[ i ] );
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
                    CreateColumns();
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