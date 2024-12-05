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
        public double Width
        {
            get => m_width;
            set
            {
                var oldValue = m_width;

                if( double.IsNaN( value ) )
                {
#pragma warning disable CS0618
                    if( double.IsNaN( m_width ) )
                    {
                        // To force auto-sizing, the width must be set to a non-NaN value first.
                        m_width = double.IsNaN( m_actualWidth ) ? 0 : m_actualWidth;
                        OnPropertyChanged( nameof( Width ) );

                        m_width = double.NaN;
                        OnPropertyChanged( nameof( Width ) );

                        return;
                    }
#pragma warning restore CS0618

                    ActualWidth = double.NaN;
                }

                SetProperty( ref m_width, value );
            }
        }

        /// <inheritdoc/>
        public double ActualWidth { get => m_actualWidth; set => SetProperty( ref m_actualWidth, value ); }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public GridViewColumnInfo() : this( string.Empty )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the column.</param>
        /// <param name="width">Width of the column.</param>
        public GridViewColumnInfo( string name, double width = DEFAULT_WIDTH )
        {
            m_name = name;
            m_width = width;
            m_actualWidth = double.NaN;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override string ToString() => Name;

        /// <inheritdoc/>
        public void RefreshAutoSize()
        {
            if( double.IsNaN( m_width ) )
            {
                Width = double.NaN;
            }
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const double DEFAULT_WIDTH = double.NaN;

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private string m_name;
        private double m_width;
        private double m_actualWidth;
    }
}
