/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for binary operations.
    /// </summary>
    public abstract class BinaryOperationBase : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// First operand.
        /// </summary>
        public object? A
        {
            get => GetParameterValue( A_INDEX );
            set => SetParameterValue( A_INDEX, value );
        }

        /// <summary>
        /// Second operand.
        /// </summary>
        public object? B
        {
            get => GetParameterValue( B_INDEX );
            set => SetParameterValue( B_INDEX, value );
        }

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BinaryOperationBase() : base( NUM_OPERANDS )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo[] parameterCultures, CultureInfo targetCulture )
        {
            var a = ConvertOperandAValue( parameterValues[ A_INDEX ], parameterCultures[ A_INDEX ] );
            var b = ConvertOperandBValue( parameterValues[ B_INDEX ], parameterCultures[ B_INDEX ] );

            return CalculateValue( a, b );
        }

        /// <summary>
        /// Calculates the value of the operation.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="a">First operand value.</param>
        /// <param name="b">Second operand value.</param>
        /// <returns></returns>
        protected abstract (object? value, CultureInfo culture) CalculateValue( object? a, object? b );

        /// <summary>
        /// Converts the value of the first operand to the type expected by the calculation (if necessary).
        /// </summary>
        /// <param name="value">Effective input value of the operand.</param>
        /// <param name="culture">Culture to use to convert the value.</param>
        /// <returns>Converted value.</returns>
        protected abstract object? ConvertOperandAValue( object? value, CultureInfo culture );

        /// <summary>
        /// Converts the value of the second operand to the type expected by the calculation (if necessary).
        /// </summary>
        /// <param name="value">Effective input value of the operand.</param>
        /// <param name="culture">Culture to use to convert the value.</param>
        /// <returns>Converted value.</returns>
        protected abstract object? ConvertOperandBValue( object? value, CultureInfo culture );

        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, CultureInfo[] sourceCultures )
        {
            Debug.Assert( sourceTypes.Length == NUM_OPERANDS );
            Debug.Assert( sourceCultures.Length == NUM_OPERANDS );

            var backValues = CalculateBackValues( targetValue );

            if( backValues == null )
            {
                return null;
            }

            var result = new object?[ NUM_OPERANDS ];

            try
            {
                result[ A_INDEX ] = Helper.ConvertValue( backValues.Value.a, sourceTypes[ A_INDEX ], sourceCultures[ A_INDEX ] );
            }
            catch
            {
                result[ A_INDEX ] = DependencyProperty.UnsetValue;
            }

            try
            {
                result[ B_INDEX ] = Helper.ConvertValue( backValues.Value.b, sourceTypes[ B_INDEX ], sourceCultures[ B_INDEX ] );
            }
            catch
            {
                result[ B_INDEX ] = DependencyProperty.UnsetValue;
            }

            return result;
        }

        /// <summary>
        /// Calculates the values to assign to the operands when converting back the operation value.
        /// </summary>
        /// <param name="targetValue">Value of the operation.</param>
        /// <returns>The values of the operands, or <c>null</c> if the calculation cannot be performed or is not feasible.</returns>
        protected abstract (object? a, object? b)? CalculateBackValues( object? targetValue );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 2;
        private const int A_INDEX = 0;
        private const int B_INDEX = 1;
    }
}
