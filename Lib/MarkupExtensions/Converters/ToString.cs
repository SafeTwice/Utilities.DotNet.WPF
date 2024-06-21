/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
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
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Optional format string used to convert the value.
        /// </summary>
        public object? FormatString
        {
            get => GetParameterRawValue( FORMATSTRING_INDEX );
            set => SetParameterRawValue( FORMATSTRING_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToString() : base( ADDITIONAL_PARAMETERS_TYPES )
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public ToString( object? value ) : base( value, ADDITIONAL_PARAMETERS_TYPES )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo culture) ConvertValue( object? value, CultureInfo targetCulture, CultureInfo? sourceCulture )
        {
            var values = (object?[]?) value;

            Debug.Assert( values != null );
            Debug.Assert( values!.Length == ( ADDITIONAL_PARAMETERS_TYPES.Length + 1 ) );

            var valueToConvert = values[ VALUE_INDEX ];
            var formatString = values[ FORMATSTRING_INDEX ] as string;

            if( formatString != null )
            {
                return (String.Format( targetCulture, $"{{0:{formatString}}}", valueToConvert ), targetCulture);
            }
            else
            {
                return (Convert.ToString( valueToConvert, targetCulture ), targetCulture);
            }
        }

        protected override object? ConvertBackValue( object? targetValue, CultureInfo targetCulture, Type sourceType, CultureInfo? sourceCulture )
        {
            if( targetValue == null )
            {
                return DependencyProperty.UnsetValue;
            }

            if( targetValue.GetType() != typeof( string ) )
            {
                targetValue = targetValue.ToString();
            }

            try
            {
                return Helper.ConvertValue( targetValue, sourceType, sourceCulture );
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int VALUE_INDEX = 0;
        private const int FORMATSTRING_INDEX = 1;

        private static readonly Type[] ADDITIONAL_PARAMETERS_TYPES = new Type[]
        {
            typeof( string ), // Format string
        };
    }
}
