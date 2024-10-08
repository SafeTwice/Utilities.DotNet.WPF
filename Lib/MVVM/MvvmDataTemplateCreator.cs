﻿/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
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
    /// Classes derived from this class will create a new data template each time that <see cref="SelectTemplate">SelectTemplate()</see> is called.
    /// This is only necessary if the data templates will be modified at runtime, otherwise it is recommended to use <see cref="MvvmDataTemplateSelector"/>.
    /// </remarks>
    public abstract class MvvmDataTemplateCreator : DataTemplateSelector
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
                return DataTemplateHelper.CreateDataTemplate( viewType, item.GetType() );
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
    }
}
