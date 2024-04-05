/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Utilities.WPF.Net.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DependencyObject"/>.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the visual children of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="obj">A <see cref="DependencyObject"/>.</param>
        /// <returns>The visual children.</returns>
        public static IEnumerable<DependencyObject> GetVisualChildren( this DependencyObject obj )
        {
            for( int i = 0; i < VisualTreeHelper.GetChildrenCount( obj ); i++ )
            {
                DependencyObject child = VisualTreeHelper.GetChild( obj, i );
                yield return child;

                foreach( DependencyObject childOfChild in child.GetVisualChildren() )
                {
                    yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// Gets the visual children of the specified type of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="ChildType">Type of the searched children.</typeparam>
        /// <param name="obj">A <see cref="DependencyObject"/>.</param>
        /// <returns>The visual children of the specified type.</returns>
        public static IEnumerable<ChildType> GetVisualChildren<ChildType>( this DependencyObject obj ) where ChildType : DependencyObject
        {
            for( int i = 0; i < VisualTreeHelper.GetChildrenCount( obj ); i++ )
            {
                DependencyObject child = VisualTreeHelper.GetChild( obj, i );
                if( child is ChildType childType )
                {
                    yield return childType;
                }

                foreach( var descendant in child.GetVisualChildren<ChildType>() )
                {
                    yield return descendant;
                }
            }
        }
    }
}
