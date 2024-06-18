/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for conversions.
    /// </summary>
    public abstract class ConverterBase : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Value to convert.
        /// </summary>
        [ConstructorArgument( "value" )]
        public object? Value
        {
            get => GetParameterValue( VALUE_INDEX );
            set => SetParameterValue( VALUE_INDEX, value );
        }

        /// <summary>
        /// Culture to use to convert the value.
        /// </summary>
        public object? Culture
        {
            get => GetParameterValue( CULTURE_INDEX );
            set => SetParameterValue( CULTURE_INDEX, value );
        }

        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        internal static CultureInfo GetUsedCulture( object? cultureParameter, CultureInfo fallbackCulture )
        {
            if( cultureParameter == null )
            {
                return fallbackCulture;
            }
            else if( cultureParameter is CultureInfo cultureParameterCulture )
            {
                return cultureParameterCulture;
            }
            else if( cultureParameter is string cultureName )
            {
                return CultureInfo.GetCultureInfo( cultureName );
            }
            else
            {
                throw new ArgumentException( "The Culture property is not a valid CultureInfo or language identifier." );
            }
        }

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ConverterBase() : base( NUM_OPERANDS )
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        protected ConverterBase( object? value ) : base( NUM_OPERANDS )
        {
            Value = value;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo[] componentCultures, CultureInfo targetCulture )
        {
            var value = parameterValues[ VALUE_INDEX ];
            var usedCulture = GetUsedCulture( parameterValues[ CULTURE_INDEX ], componentCultures[ VALUE_INDEX ] );
            return ConvertValue( value, usedCulture );
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual conversion.
        /// </remarks>
        /// <param name="value">Value to convert.</param>
        /// <param name="culture">Culture to use to convert the value.</param>
        /// <returns>Converted value and its associated culture.</returns>
        protected abstract (object? value, CultureInfo culture) ConvertValue( object? value, CultureInfo culture );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 2;
        private const int VALUE_INDEX = 0;
        private const int CULTURE_INDEX = 1;
    }
}
