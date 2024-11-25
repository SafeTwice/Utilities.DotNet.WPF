/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.Observables;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Base class for source items of <see cref="GridViewColumns.ColumnsSourceProperty"/>.
    /// </summary>
    public class GridViewColumnInfo : ObservableObjectEx, IGridViewColumnInfo
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public string Name { get => m_name; set => SetProperty( ref m_name, value ); }

        /// <inheritdoc/>
        public double? Width { get => m_width; set => SetProperty( ref m_width, value ); }

        /// <inheritdoc/>
        public double? ActualWidth { get => m_actualWidth; set => SetProperty( ref m_actualWidth, value ); }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public GridViewColumnInfo() : this( string.Empty )
        {
        }

        public GridViewColumnInfo( string name, double? width = DEFAULT_WIDTH )
        {
            m_name = name;
            m_width = width;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public override string ToString() => Name;

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const double DEFAULT_WIDTH = 100.0;

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private string m_name;
        private double? m_width;
        private double? m_actualWidth;
    }
}
