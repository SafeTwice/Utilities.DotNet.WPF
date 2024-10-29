
using System.ComponentModel;

/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt
namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Interface for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public interface IGridViewColumnInfo : INotifyPropertyChanged
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Name of the column.
        /// </summary>
        string? Name { get; set; }

        /// <summary>
        /// Width of the column.
        /// </summary>
        double? Width { get; set; }

        /// <summary>
        /// Actual width of the column.
        /// </summary>
        double? ActualWidth { get; set; }

        /// <summary>
        /// Visibility of the column.
        /// </summary>
        bool IsVisible { get; set; }
    }
}
