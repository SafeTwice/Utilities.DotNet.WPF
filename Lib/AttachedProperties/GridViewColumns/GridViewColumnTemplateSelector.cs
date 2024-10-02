using System;
using System.Windows;
using System.Windows.Controls;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Internal data template selector for column items that applies a data context selector to obtain the data context
    /// that corresponds to the column item for a row item, and then gets the corresponding data template.
    /// </summary>
    internal class GridViewColumnTemplateSelector : DataTemplateSelector
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public GridViewColumnTemplateSelector( Func<object, object?> dataContextSelector, DataTemplateSelector dataTemplateSelector )
        {
            m_dataContextSelector = dataContextSelector;
            m_dataTemplateSelector = dataTemplateSelector;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override DataTemplate? SelectTemplate( object item, DependencyObject container )
        {
            var columnDataContext = m_dataContextSelector( item );

            var dataTemplate = m_dataTemplateSelector.SelectTemplate( columnDataContext, container );

            if( dataTemplate != null )
            {
                dataTemplate.VisualTree.SetValue( FrameworkElement.DataContextProperty, columnDataContext );
            }

            return dataTemplate;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly Func<object, object?> m_dataContextSelector;
        private readonly DataTemplateSelector m_dataTemplateSelector;
    }
}
