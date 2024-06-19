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
            get => GetParameterValue( CultureIndex );
            set => SetParameterValue( CultureIndex, value );
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
        /// <param name="additionalParametersNum">Number of additional parameters.</param>
        protected ConverterBase( int additionalParametersNum = 0 ) : base( BASE_NUM_PARAMETERS + additionalParametersNum )
        {
            CultureIndex = BASE_NUM_PARAMETERS + additionalParametersNum - 1;
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="additionalParametersNum">Number of additional parameters.</param>
        protected ConverterBase( object? value, int additionalParametersNum = 0 ) : this( additionalParametersNum )
        {
            Value = value;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length >= BASE_NUM_PARAMETERS );
            Debug.Assert( parameterCultures.Length >= BASE_NUM_PARAMETERS );

            object? value;
            if( parameterValues.Length == BASE_NUM_PARAMETERS )
            {
                value = parameterValues[ VALUE_INDEX ];
            }
            else
            {
                var values = new object?[ parameterValues.Length - 1 ];
                Array.Copy( parameterValues, 0, values, 0, values.Length );
                value = values;
            }

            var usedCulture = GetUsedCulture( parameterValues[ CultureIndex ], parameterCultures[ VALUE_INDEX ] );
            return ConvertValue( value, usedCulture );
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <remarks>
        /// <para>Must be implemented by derived classes to perform the actual conversion.</para>
        /// <para>If the derived class does not specify additional parameters, then <paramref name="value"/> is the value to convert.</para>
        /// <para>If the derived class specifies additional parameters, then <paramref name="value"/> is an array with the value to convert
        ///       in the first position followed with the values of the additional parameters.</para>
        /// </remarks>
        /// <param name="value">Value to convert.</param>
        /// <param name="culture">Culture to use to convert the value.</param>
        /// <returns>Converted value and its associated culture.</returns>
        protected abstract (object? value, CultureInfo culture) ConvertValue( object? value, CultureInfo culture );

        /// <inheritdoc/>
        protected sealed override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, CultureInfo[] sourceCultures )
        {
            Debug.Assert( sourceTypes.Length >= BASE_NUM_PARAMETERS );
            Debug.Assert( sourceCultures.Length >= BASE_NUM_PARAMETERS );

            var result = new object?[ sourceTypes.Length ];
            for( var i = 0; i < result.Length; i++ )
            {
                result[ i ] = Binding.DoNothing;
            }

            result[ VALUE_INDEX ] = ConvertBackValue( targetValue, targetCulture, sourceTypes[ VALUE_INDEX ], sourceCultures[ VALUE_INDEX ] );

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

        private const int VALUE_INDEX = 0;
        private const int BASE_NUM_PARAMETERS = 2;

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly int CultureIndex;
    }
}
