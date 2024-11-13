/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that formats a string.
    /// </summary>
    [MarkupExtensionReturnType( typeof( string ) )]
    public sealed class Format : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Value of parameter 0.
        /// </summary>
        [ConstructorArgument( "formatString" )]
        public object? FormatString
        {
            get => GetParameterRawValue( FORMATSTRING_INDEX );
            set => SetParameterRawValue( FORMATSTRING_INDEX, value );
        }

        /// <summary>
        /// Value of parameter 0.
        /// </summary>
        [ConstructorArgument( "p0" )]
        public object? P0
        {
            get => GetParameterRawValue( PARAM0_INDEX );
            set => SetParameterRawValue( PARAM0_INDEX, value );
        }

        /// <summary>
        /// Value of parameter 1.
        /// </summary>
        [ConstructorArgument( "p1" )]
        public object? P1
        {
            get => GetParameterRawValue( PARAM1_INDEX );
            set => SetParameterRawValue( PARAM1_INDEX, value );
        }

        /// <summary>
        /// Value of parameter 2.
        /// </summary>
        [ConstructorArgument( "p3" )]
        public object? P2
        {
            get => GetParameterRawValue( PARAM2_INDEX );
            set => SetParameterRawValue( PARAM2_INDEX, value );
        }

        /// <summary>
        /// Value of parameter 3.
        /// </summary>
        [ConstructorArgument( "p3" )]
        public object? P3
        {
            get => GetParameterRawValue( PARAM3_INDEX );
            set => SetParameterRawValue( PARAM3_INDEX, value );
        }

        /// <summary>
        /// Value of parameter 4.
        /// </summary>
        [ConstructorArgument( "p4" )]
        public object? P4
        {
            get => GetParameterRawValue( PARAM4_INDEX );
            set => SetParameterRawValue( PARAM4_INDEX, value );
        }

        /// <summary>
        /// Value of parameter 5.
        /// </summary>
        [ConstructorArgument( "p5" )]
        public object? P5
        {
            get => GetParameterRawValue( PARAM5_INDEX );
            set => SetParameterRawValue( PARAM5_INDEX, value );
        }

        /// <summary>
        /// Culture to use to convert the value.
        /// </summary>
        public object? Culture
        {
            get => GetParameterRawValue( CULTURE_INDEX );
            set => SetParameterRawValue( CULTURE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public Format() : base( PARAMETER_TYPES )
        {
        }

        /// <summary>
        /// Constructor that initializes the format string.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        public Format( object? formatString ) : this()
        {
            FormatString = formatString;
        }

        /// <summary>
        /// Constructor that initializes the format string and 1 parameter.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        /// <param name="p0">Parameter 0 value.</param>
        public Format( object? formatString, object? p0 ) : this( formatString )
        {
            P0 = p0;
        }

        /// <summary>
        /// Constructor that initializes the format string and 2 parameters.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        /// <param name="p0">Parameter 0 value.</param>
        /// <param name="p1">Parameter 1 value.</param>
        public Format( object? formatString, object? p0, object? p1 ) : this( formatString, p0 )
        {
            P1 = p1;
        }

        /// <summary>
        /// Constructor that initializes the format string and 3 parameters.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        /// <param name="p0">Parameter 0 value.</param>
        /// <param name="p1">Parameter 1 value.</param>
        /// <param name="p2">Parameter 2 value.</param>
        public Format( object? formatString, object? p0, object? p1, object? p2 ) : this( formatString, p0, p1 )
        {
            P2 = p2;
        }

        /// <summary>
        /// Constructor that initializes the format string and 4 parameters.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        /// <param name="p0">Parameter 0 value.</param>
        /// <param name="p1">Parameter 1 value.</param>
        /// <param name="p2">Parameter 2 value.</param>
        /// <param name="p3">Parameter 3 value.</param>
        public Format( object? formatString, object? p0, object? p1, object? p2, object? p3 ) : this( formatString, p0, p1, p2 )
        {
            P3 = p3;
        }

        /// <summary>
        /// Constructor that initializes the format string and 5 parameters.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        /// <param name="p0">Parameter 0 value.</param>
        /// <param name="p1">Parameter 1 value.</param>
        /// <param name="p2">Parameter 2 value.</param>
        /// <param name="p3">Parameter 3 value.</param>
        /// <param name="p4">Parameter 4 value.</param>
        public Format( object? formatString, object? p0, object? p1, object? p2, object? p3, object? p4 ) : this( formatString, p0, p1, p2, p3 )
        {
            P4 = p4;
        }

        /// <summary>
        /// Constructor that initializes the format string and 5 parameters.
        /// </summary>
        /// <param name="formatString">Format string.</param>
        /// <param name="p0">Parameter 0 value.</param>
        /// <param name="p1">Parameter 1 value.</param>
        /// <param name="p2">Parameter 2 value.</param>
        /// <param name="p3">Parameter 3 value.</param>
        /// <param name="p4">Parameter 4 value.</param>
        /// <param name="p5">Parameter 5 value.</param>
        public Format( object? formatString, object? p0, object? p1, object? p2, object? p3, object? p4, object? p5 ) : this( formatString, p0, p1, p2, p3, p4 )
        {
            P5 = p5;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo?[] componentCultures, CultureInfo targetCulture )
        {
            var formatString = ( parameterValues[ FORMATSTRING_INDEX ] as string ) ?? string.Empty;
            var usedCulture = ConverterBase.GetUserDefinedCulture( parameterValues[ CULTURE_INDEX ] ) ?? targetCulture;
            var parameters = new object?[ PARAMETER_TYPES.Length - 2 ];
            Array.Copy( parameterValues, PARAM0_INDEX, parameters, 0, parameters.Length );

            var result = String.Format( usedCulture, formatString, parameters );
            return (result, usedCulture);
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int CULTURE_INDEX = 0;
        private const int FORMATSTRING_INDEX = 1;
        private const int PARAM0_INDEX = 2;
        private const int PARAM1_INDEX = 3;
        private const int PARAM2_INDEX = 4;
        private const int PARAM3_INDEX = 5;
        private const int PARAM4_INDEX = 6;
        private const int PARAM5_INDEX = 7;

        private static readonly Type[] PARAMETER_TYPES =
        {
            typeof( object ), // Culture
            typeof( string ), // Format string
            typeof( object ), // Param 0
            typeof( object ), // Param 1
            typeof( object ), // Param 2
            typeof( object ), // Param 3
            typeof( object ), // Param 4
            typeof( object ), // Param 5
        };
    }
}
