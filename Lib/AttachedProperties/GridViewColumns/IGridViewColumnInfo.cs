/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Interface for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public interface IGridViewColumnInfo
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Name of the column.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Width of the column.
        /// </summary>
        double? Width { get; set; }
    }
}
