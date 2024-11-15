/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Data template selector that is configured with associations of data templates to object types and then
    /// provides the data templates associated to object types.
    /// </summary>
    [ContentProperty( nameof( Associations ) )]
    public class MvvmDataTemplateSelectorByType : DataTemplateSelector
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <value>
        /// Collection of associations of data templates to object types.
        /// </value>
        public Collection<MvvmDataTemplateForType> Associations { get; } = new();

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public override DataTemplate? SelectTemplate( object? item, DependencyObject container )
        {
            if( item == null )
            {
                return null;
            }

            var dataTemplate = Associations.Where( x => IsDerivedOfType( item.GetType(), x.Type ) ).Select( x => x.Template ).FirstOrDefault();

            return dataTemplate ?? base.SelectTemplate( item, container );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static bool IsDerivedOfType( Type itemType, Type? baseType )
        {
            return ( baseType?.IsAssignableFrom( itemType ) ?? false );
        }
    }

    /// <summary>
    /// Association of a data template to an object type.
    /// </summary>
    [ContentProperty( nameof( Template ) )]
    public class MvvmDataTemplateForType
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <value>
        /// Data template to associate to an object type.
        /// </value>
        public DataTemplate? Template { get; set; }

        /// <value>
        /// Type of objects to be associated to the data template.
        /// </value>
        public Type? Type { get; set; }
    }
}
