/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
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

        /// <inheritdoc/>
        protected sealed override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, CultureInfo[] sourceCultures )
        {
            Debug.Assert( sourceTypes.Length == NUM_OPERANDS );
            var result = new object?[ NUM_OPERANDS ];

            result[ VALUE_INDEX ] = ConvertBackValue( targetValue, targetCulture, sourceTypes[ VALUE_INDEX ], sourceCultures[ VALUE_INDEX ] );
            result[ CULTURE_INDEX ] = Binding.DoNothing;

            return result;
        }

        /// <summary>
        /// Converts back a value.
        /// </summary>
        /// <remarks>
        /// <para>If the source value should not be modified, then <see cref="Binding.DoNothing"/> must be returned.</para>
        /// <para>If the value cannot be converted back, then <see cref="DependencyProperty.UnsetValue"/> must be returned.</para>
        /// </remarks>
        /// <param name="targetValue">Value to convert back.</param>
        /// <param name="targetCulture">Culture of the value to convert back.</param>
        /// <param name="sourceType">Type of the source to convert the value back to.</param>
        /// <param name="sourceCulture">Culture of the source.</param>
        /// <returns>Converted back value.</returns>
        protected abstract object? ConvertBackValue( object? targetValue, CultureInfo targetCulture, Type sourceType, CultureInfo sourceCulture );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 2;
        private const int VALUE_INDEX = 0;
        private const int CULTURE_INDEX = 1;
    }
}
