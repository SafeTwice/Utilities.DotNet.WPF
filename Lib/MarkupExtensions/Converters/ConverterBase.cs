/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
            get => base.GetParameterRawValue( VALUE_INDEX );
            set => base.SetParameterRawValue( VALUE_INDEX, value );
        }

        /// <summary>
        /// Culture to use to convert the value.
        /// </summary>
        public object? Culture
        {
            get => base.GetParameterRawValue( CULTURE_INDEX );
            set => base.SetParameterRawValue( CULTURE_INDEX, value );
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
        protected ConverterBase( IEnumerable<Type>? additionalParameterTypes = null )
            : base( ( additionalParameterTypes == null ) ? PARAMETER_TYPES : PARAMETER_TYPES.Concat( additionalParameterTypes ) )
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="additionalParametersNum">Number of additional parameters.</param>
        protected ConverterBase( object? value, IEnumerable<Type>? additionalParameterTypes = null ) : this( additionalParameterTypes )
        {
            Value = value;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length >= PARAMETER_TYPES.Length );
            Debug.Assert( parameterCultures.Length >= PARAMETER_TYPES.Length );

            object? value;
            if( parameterValues.Length == PARAMETER_TYPES.Length )
            {
                value = parameterValues[ VALUE_INDEX ];
            }
            else
            {
                var values = new object?[ parameterValues.Length - 1 ];
                Array.Copy( parameterValues, VALUE_INDEX, values, 0, values.Length );
                value = values;
            }

            var usedCulture = GetUsedCulture( parameterValues[ CULTURE_INDEX ], parameterCultures[ VALUE_INDEX ] );
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
            Debug.Assert( sourceTypes.Length >= PARAMETER_TYPES.Length );
            Debug.Assert( sourceCultures.Length >= PARAMETER_TYPES.Length );

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

        /// <summary>
        /// Gets the raw value of the specified additional parameter.
        /// </summary>
        /// <remarks>
        /// <para>The parameter identifier must be in the range from 1 to the number of parameters (as specified in the constructor).</para>
        /// </remarks>
        /// <param name="additionalParameterId">Identifier of the parameter.</param>
        /// <returns>Raw value of the parameter.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter identifier is out of range.</exception>
        protected new object? GetParameterRawValue( int additionalParameterId )
        {
            if( ( additionalParameterId < 1 ) || ( additionalParameterId >= ( ComponentRawValues.Count - 1 ) ) )
            {
                throw new ArgumentOutOfRangeException( nameof( additionalParameterId ) );
            }

            return base.GetParameterRawValue( additionalParameterId + 1 );
        }

        /// <summary>
        /// Sets the raw value of the specified additional parameter.
        /// </summary>
        /// <remarks>
        /// The parameter identifier must be in the range from 1 to the number of parameters (as specified in the constructor).
        /// </remarks>
        /// <param name="additionalParameterId">Identifier of the parameter.</param>
        /// <param name="parameterValue">Raw value of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter identifier is out of range.</exception>
        protected new void SetParameterRawValue( int additionalParameterId, object? parameterValue )
        {
            if( ( additionalParameterId < 1 ) || ( additionalParameterId >= ( ComponentRawValues.Count - 1 ) ) )
            {
                throw new ArgumentOutOfRangeException( nameof( additionalParameterId ) );
            }

            base.SetParameterRawValue( additionalParameterId + 1, parameterValue );
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int CULTURE_INDEX = 0;
        private const int VALUE_INDEX = 1;

        private static readonly Type[] PARAMETER_TYPES =
        {
            typeof( object ), // Culture
            typeof( object ), // Value
        };
    }
}
