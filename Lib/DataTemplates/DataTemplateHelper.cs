/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows;

namespace Utilities.DotNet.WPF.DataTemplates
{
    /// <summary>
    /// Helper class for working with data templates.
    /// </summary>
    public static class DataTemplateHelper
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Creates a data template for a view type and data type.
        /// </summary>
        /// <param name="viewType">Type of the view to be instantiated by the data template.</param>
        /// <param name="dataType">Data type for which the data template is intended.</param>
        /// <returns><see cref="DataTemplate"/> that instantiates the view type for the data type.</returns>
        public static DataTemplate CreateDataTemplate( Type viewType, Type dataType )
        {
            var dataTemplate = new DataTemplate();
            dataTemplate.DataType = dataType;

            var elementFactory = new FrameworkElementFactory( viewType );
            dataTemplate.VisualTree = elementFactory;

            return dataTemplate;
        }


        /// <summary>
        /// Creates a data template for a view type and data type, and sets the data context.
        /// </summary>
        /// <param name="viewType">Type of the view to be instantiated by the data template.</param>
        /// <param name="dataType">Data type for which the data template is intended.</param>
        /// <param name="dataContext">Data context.</param>
        /// <returns><see cref="DataTemplate"/> that instantiates the view type for the data type.</returns>
        public static DataTemplate CreateDataTemplate( Type viewType, Type dataType, object? dataContext )
        {
            var dataTemplate = CreateDataTemplate( viewType, dataType );

            dataTemplate.VisualTree.SetValue( FrameworkElement.DataContextProperty, dataContext );

            return dataTemplate;
        }
    }
}
