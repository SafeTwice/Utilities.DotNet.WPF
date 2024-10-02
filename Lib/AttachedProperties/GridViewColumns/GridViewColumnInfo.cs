/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Base class for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public class GridViewColumnInfo : IGridViewColumnInfo
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public double? Width { get; set; }
    }
}
