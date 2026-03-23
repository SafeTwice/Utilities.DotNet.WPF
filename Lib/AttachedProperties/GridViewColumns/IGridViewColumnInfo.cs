/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.ComponentModel;
using Utilities.DotNet.Observables;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Interface for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public interface IGridViewColumnInfo : INotifyPropertyChangedEx, INotifyPropertyChanged
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
        /// <remarks>
        /// Set to <see cref="double.NaN"/> for auto-sizing.
        /// </remarks>
        double Width { get; set; }

        /// <summary>
        /// Actual width of the column.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is automatically updated when the associated column width is set or updated. Setting this property has no effect.
        /// </para>
        /// <para>
        /// When this property is set to <see cref="double.NaN"/>, the actual column width is unknown.
        /// </para>
        /// </remarks>
        double ActualWidth { get; set; }

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// If the column is set to auto-size, forces the column to recalculate its width.
        /// </summary>
        void RefreshAutoSize();
    }
}
