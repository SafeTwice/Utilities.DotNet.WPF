﻿/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that converts an object to a string.
    /// </summary>
    [MarkupExtensionReturnType( typeof( string ) )]
    public sealed class ToString : ConverterBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToString()
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public ToString( object? value ) : base( value )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo culture) ConvertValue( object? value, CultureInfo culture )
        {
            return (Convert.ToString( value, culture ), culture);
        }
    }
}
