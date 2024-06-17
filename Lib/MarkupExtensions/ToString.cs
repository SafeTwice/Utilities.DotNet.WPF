/// @file
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
    public sealed class ToString : BindingMarkupExtensionBase
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

        public object? Culture
        {
            get => GetParameterValue( CULTURE_INDEX );
            set => SetParameterValue( CULTURE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToString() : base( NUM_PARAMETERS )
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public ToString( object? value ) : base( NUM_PARAMETERS )
        {
            Value = value;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override object? CalculateValue( object?[] parameterValues, CultureInfo targetCulture )
        {
            var usedCulture = parameterValues[ CULTURE_INDEX ] as CultureInfo;
            if( usedCulture == null )
            {
                usedCulture = targetCulture;
            }

            return Convert.ToString( parameterValues[ VALUE_INDEX ], usedCulture );
        }

        /// <inheritdoc/>
        protected override object? ConvertParameterValue( int parameterId, object? parameterValue, CultureInfo culture )
        {
            if( ( parameterId == CULTURE_INDEX ) && ( parameterValue != null ) )
            {
                if( parameterValue is CultureInfo parameterCulture )
                {
                    return parameterCulture;
                }
                else if( parameterValue is string cultureName )
                {
                    return CultureInfo.GetCultureInfo( cultureName );
                }
                else
                {
                    throw new ArgumentException( "The Culture property is not a valid CultureInfo or language identifier." );
                }
            }
            else
            {
                return parameterValue;
            }
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_PARAMETERS = 2;
        private const int VALUE_INDEX = 0;
        private const int CULTURE_INDEX = 1;
    }
}
