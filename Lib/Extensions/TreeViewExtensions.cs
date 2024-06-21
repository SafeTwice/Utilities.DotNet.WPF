/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Controls;

namespace Utilities.DotNet.WPF.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TreeView"/>.
    /// </summary>
    public static class TreeViewExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the <see cref="TreeViewItem"/> for the specified value in a <see cref="TreeView"/>.
        /// </summary>
        /// <param name="treeView">A <see cref="TreeView"/>.</param>
        /// <param name="value">Value to search.</param>
        /// <returns>The <see cref="TreeViewItem"/> for the value if found; otherwise, <c>null</c>.</returns>
        public static TreeViewItem? GetTreeViewItemForValue( this TreeView treeView, object value )
        {
            return treeView.GetContainerFromItem<TreeViewItem>( value );
        }

        /// <summary>
        /// Deselects all items in a <see cref="TreeView"/>.
        /// </summary>
        /// <param name="treeView">A <see cref="TreeView"/>.</param>
        public static void DeselectAllItems( this TreeView treeView )
        {
            treeView.ItemContainerGenerator.DeselectAllItems();
        }

        /// <summary>
        /// Deselects all items in a <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="treeView">A <see cref="TreeViewItem"/>.</param>
        public static void DeselectAllItems( this TreeViewItem treeViewItem )
        {
            treeViewItem.IsSelected = false;
            treeViewItem.ItemContainerGenerator.DeselectAllItems();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void DeselectAllItems( this ItemContainerGenerator generator )
        {
            for( int i = 0; i < generator.Items.Count; i++ )
            {
                var subContainer = generator.ContainerFromIndex( i ) as TreeViewItem;
                subContainer?.DeselectAllItems();
            }
        }
    }
}
