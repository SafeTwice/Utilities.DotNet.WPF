/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Base class for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public abstract class GridViewColumnsSourceItem : IGridViewColumnsSourceItem
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public BindingBase? DisplayMemberBinding { get; protected set; }

        /// <inheritdoc/>
        public DataTemplate? CellTemplate { get; protected set; }

        /// <inheritdoc/>
        public DataTemplateSelector? CellTemplateSelector { get; protected set; }

        /// <inheritdoc/>
        public object? Header { get; protected set; }

        /// <inheritdoc/>
        public string? HeaderStringFormat { get; protected set; }

        /// <inheritdoc/>
        public Style? HeaderContainerStyle { get; protected set; }

        /// <inheritdoc/>
        public DataTemplate? HeaderTemplate { get; protected set; }

        /// <inheritdoc/>
        public DataTemplateSelector? HeaderTemplateSelector { get; protected set; }

        /// <inheritdoc/>
        public double? Width { get; protected set; }
    }
}
