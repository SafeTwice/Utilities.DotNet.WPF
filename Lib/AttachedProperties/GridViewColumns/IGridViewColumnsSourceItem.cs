/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Interface for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public interface IGridViewColumnsSourceItem
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <inheritdoc cref="GridViewColumn.DisplayMemberBinding"/>
        BindingBase? DisplayMemberBinding { get; }

        /// <inheritdoc cref="GridViewColumn.CellTemplate"/>
        DataTemplate? CellTemplate { get; }

        /// <inheritdoc cref="GridViewColumn.CellTemplateSelector"/>
        DataTemplateSelector? CellTemplateSelector { get; }

        /// <inheritdoc cref="GridViewColumn.Header"/>
        object? Header { get; }

        /// <inheritdoc cref="GridViewColumn.HeaderStringFormat"/>
        string? HeaderStringFormat { get; }

        /// <inheritdoc cref="GridViewColumn.HeaderContainerStyle"/>
        Style? HeaderContainerStyle { get; }

        /// <inheritdoc cref="GridViewColumn.HeaderTemplate"/>
        DataTemplate? HeaderTemplate { get; }

        /// <inheritdoc cref="GridViewColumn.HeaderTemplateSelector"/>
        DataTemplateSelector? HeaderTemplateSelector { get; }

        /// <inheritdoc cref="GridViewColumn.Width"/>
        double? Width { get; }
    }
}
