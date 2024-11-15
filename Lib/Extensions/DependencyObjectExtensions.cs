/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Utilities.DotNet.WPF.Extensions
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
            if( obj is FrameworkElement fe )
            {
                fe.ApplyTemplate();
            }

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
        /// <typeparam name="TChild">Type of the searched visual children.</typeparam>
        /// <param name="obj">A <see cref="DependencyObject"/>.</param>
        /// <returns>The visual children of the specified type.</returns>
        public static IEnumerable<TChild> GetVisualChildren<TChild>( this DependencyObject obj ) where TChild : DependencyObject
        {
            if( obj is FrameworkElement fe )
            {
                fe.ApplyTemplate();
            }

            for( int i = 0; i < VisualTreeHelper.GetChildrenCount( obj ); i++ )
            {
                DependencyObject child = VisualTreeHelper.GetChild( obj, i );
                if( child is TChild childType )
                {
                    yield return childType;
                }

                foreach( var descendant in child.GetVisualChildren<TChild>() )
                {
                    yield return descendant;
                }
            }
        }

        /// <summary>
        /// Gets the first visual descendant of the specified type from a <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="TDescendant">Type of the searched visual descendant.</typeparam>
        /// <param name="obj">A <see cref="DependencyObject"/>.</param>
        /// <returns>The first visual descendant of the specified type, or <c>null</c> if no such descendant is found.</returns>
        public static TDescendant? GetFirstVisualDescendant<TDescendant>( this DependencyObject obj ) where TDescendant : DependencyObject
        {
            if( obj is TDescendant descendant )
            {
                return descendant;
            }
            else
            {
                return obj.GetVisualChildren<TDescendant>().FirstOrDefault();
            }
        }
    }
}
