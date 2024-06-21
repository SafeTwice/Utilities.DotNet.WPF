/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Controls;

namespace Utilities.DotNet.WPF.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ItemsControl"/>.
    /// </summary>
    public static class ItemsControlExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the container of the specified type for the specified item in an <see cref="ItemsControl"/>.
        /// </summary>
        /// <remarks>
        /// The container is searched recursively.
        /// </remarks>
        /// <typeparam name="ContainerType">Type of the container.</typeparam>
        /// <param name="itemsControl">An <see cref="ItemsControl"/>.</param>
        /// <param name="item">Item to search.</param>
        /// <returns>The container of the specified type for the item if found; otherwise, <c>null</c>.</returns>
        public static ContainerType? GetContainerFromItem<ContainerType>( this ItemsControl itemsControl, object item ) where ContainerType : DependencyObject
        {
            return itemsControl.ItemContainerGenerator.GetContainerFromItem<ContainerType>( item );
        }

        /// <summary>
        /// Gets the container of the specified type for the specified item in an <see cref="ItemContainerGenerator"/>.
        /// </summary>
        /// <remarks>
        /// The container is searched recursively.
        /// </remarks>
        /// <typeparam name="ContainerType"></typeparam>
        /// <param name="generator">An <see cref="ItemContainerGenerator"/>.</param>
        /// <param name="item">Item to search.</param>
        /// <returns>The container of the specified type for the item if found; otherwise, <c>null</c>.</returns>
        public static ContainerType? GetContainerFromItem<ContainerType>( this ItemContainerGenerator generator, object item ) where ContainerType : DependencyObject
        {
            var container = generator.ContainerFromItem( item ) as ContainerType;
            if( container != null )
            {
                return container;
            }

            for( int i = 0; i < generator.Items.Count; i++ )
            {
                var subContainer = generator.ContainerFromIndex( i ) as ItemsControl;

                container = subContainer?.GetContainerFromItem<ContainerType>( item );
                if( container != null )
                {
                    return container;
                }
            }

            return null;
        }
    }
}
