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
    public abstract class BinaryOperationBase<TA, TB> : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// First operand.
        /// </summary>
        public object? A
        {
            get => GetParameterRawValue( A_INDEX );
            set => SetParameterRawValue( A_INDEX, value );
        }

        /// <summary>
        /// Second operand.
        /// </summary>
        public object? B
        {
            get => GetParameterRawValue( B_INDEX );
            set => SetParameterRawValue( B_INDEX, value );
        }

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BinaryOperationBase() : base( new Type[] { typeof( TA ), typeof( TB ) } )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo[] parameterCultures, CultureInfo targetCulture )
        {
            var a = (TA?) parameterValues[ A_INDEX ];
            var b = (TB?) parameterValues[ B_INDEX ];

            if( ( a == null ) || ( b == null ) )
            {
                return (DependencyProperty.UnsetValue, CultureInfo.InvariantCulture);
            }

            return CalculateOperationValue( a, b );
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
        protected abstract (object? value, CultureInfo culture) CalculateOperationValue( TA a, TB b );

        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, CultureInfo[] sourceCultures )
        {
            Debug.Assert( sourceTypes.Length == NUM_OPERANDS );
            Debug.Assert( sourceCultures.Length == NUM_OPERANDS );

            var backValues = CalculateOperationBackValues( targetValue );

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
        protected abstract (TA a, TB b)? CalculateOperationBackValues( object? targetValue );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 2;
        private const int A_INDEX = 0;
        private const int B_INDEX = 1;
    }
}
