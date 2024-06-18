/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that converts an object to a double-precision floating-point number.
    /// </summary>
    [MarkupExtensionReturnType( typeof( double ) )]
    public sealed class ToDouble : ConverterBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToDouble()
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public ToDouble( object? value ) : base( value )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo culture) ConvertValue( object? value, CultureInfo culture )
        {
            return (Convert.ToDouble( value, culture ), CultureInfo.InvariantCulture);
        }
    }
}
