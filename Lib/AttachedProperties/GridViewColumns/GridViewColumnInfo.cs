
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
        public GridViewColumnInfo( string? name, double? width = DEFAULT_WIDTH, bool isVisible = true )
        {
            m_name = name;
            m_width = width;
            m_isVisible = isVisible;

        }

        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public string? Name { get => m_name; set => SetProperty( ref m_name, value ); }
        private string? m_name;

        /// <inheritdoc/>
        public double? Width { get => m_width; set => SetProperty( ref m_width, value ); }
        private double? m_width;
        /// <inheritdoc/>
        public double? ActualWidth { get => m_actualWidth; set => SetProperty( ref m_actualWidth, value ); }
        private double? m_actualWidth;

        /// <inheritdoc/>
        public bool IsVisible { get => m_isVisible; set => SetProperty( ref m_isVisible, value ); }
        private bool m_isVisible;

        public void SetProperty<T>( ref T field, T value, [CallerMemberName] string propertyName = "" )
        {
            if( !Equals( field, value ) )
            {
                field = value;
                OnPropertyChanged( propertyName );
            }
        }

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
