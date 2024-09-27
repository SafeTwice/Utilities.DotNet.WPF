/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Utilities.DotNet.WPF.DataTemplates;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Base class for data template selectors that create data templates from the view type that correspond to
    /// different items.
    /// </summary>
    /// <remarks>
    /// Classes derived from this class will cache the data templates created for each combination of view type and item type.
    /// Therefore, data templates returned by <see cref="SelectTemplate">SelectTemplate()</see> shall not be modified. If you
    /// need to modify the data templates at runtime, use <see cref="MvvmDataTemplateCreator"/> instead.
    /// </remarks>
    public abstract class MvvmDataTemplateSelector : DataTemplateSelector
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override DataTemplate? SelectTemplate( object item, DependencyObject container )
        {
            var viewType = GetViewTypeForItem( item );

            if( viewType == null )
            {
                return base.SelectTemplate( item, container );
            }
            else
            {
                var itemType = item.GetType();

                if( !m_dataTemplates.TryGetValue( (viewType, itemType), out var dataTemplate ) )
                {
                    dataTemplate = DataTemplateHelper.CreateDataTemplate( viewType, itemType );

                    m_dataTemplates[ (viewType, itemType) ] = dataTemplate;
                }

                return dataTemplate;
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Obtains the type of the view for an item.
        /// </summary>
        /// <remarks>
        /// Must be overridden by derived classes to provide the type of the view for an item.
        /// </remarks>
        /// <param name="item">Item to get the view for.</param>
        /// <returns><see cref="Type"/> of the view to display the item, or <c>null</c> if not available.</returns>
        protected abstract Type? GetViewTypeForItem( object item );

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly Dictionary<(Type, Type), DataTemplate> m_dataTemplates = new();
    }
}
