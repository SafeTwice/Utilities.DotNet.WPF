/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows;
using System.Windows.Controls;

namespace Utilities.WPF.Net.MVVM
{
    /// <summary>
    /// Base class for data template selectors that create data templates from the view type that correspond to
    /// different items.
    /// </summary>
    public abstract class MvvmDataTemplateCreator : DataTemplateSelector
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public override DataTemplate? SelectTemplate( object item, DependencyObject container )
        {
            var viewType = GetViewTypeForItem( item );

            if( viewType == null )
            {
                return base.SelectTemplate( item, container );
            }
            else
            {
                var dataTemplate = new DataTemplate();
                dataTemplate.DataType = item.GetType();

                var elementFactory = new FrameworkElementFactory( viewType );
                dataTemplate.VisualTree = elementFactory;

                return dataTemplate;
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Obtains the type of the view for an item.
        /// </summary>
        /// <param name="item">Item to get the view for</param>
        /// <returns><see cref="Type"/> of the view to display the item, or <c>null</c> if not available</returns>
        protected abstract Type? GetViewTypeForItem( object item );
    }
}
