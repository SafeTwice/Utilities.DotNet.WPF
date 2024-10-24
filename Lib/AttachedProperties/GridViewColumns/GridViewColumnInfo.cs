
using System.ComponentModel;


using System.Runtime.CompilerServices;

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
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================
        public GridViewColumnInfo() { }
        public GridViewColumnInfo( string name, double? width = DEFAULT_WIDTH, bool isVisible = true )
        {
            Name = name;
            Width = width;
            IsVisible = isVisible;
        }

        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public double? Width { get; set; }
        /// <inheritdoc/>
        public double? ActualWidth { get; set; }

        /// <inheritdoc/>
        public bool IsVisible { get; set; } = true;

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        public event PropertyChangedEventHandler? PropertyChanged;

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = "" )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        //===========================================================================
        //                           PUBLIC CONSTANTS
        //===========================================================================

        public const double DEFAULT_WIDTH = 100.0;
    }
}
