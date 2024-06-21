/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for binary operations.
    /// </summary>
    /// <typeparam name="TA">Type of the first operand.</typeparam>
    /// <typeparam name="TB">Type of the second operand.</typeparam>
    /// <typeparam name="TReturn">Type of the operation result.</typeparam>
    public abstract class BinaryOperationBase<TA, TB, TReturn> : BindingMarkupExtensionBase
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
        protected sealed override (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length == NUM_OPERANDS );

            var a = (TA?) parameterValues[ A_INDEX ];
            var b = (TB?) parameterValues[ B_INDEX ];

            object? operationValue;

            if( ( a == null ) || ( b == null ) )
            {
                // Binary operation cannot be performed if any of the operands is null.
                operationValue = DependencyProperty.UnsetValue;
            }
            else
            {
                operationValue = CalculateValue( a, b );

                if( operationValue == null )
                {
                    operationValue = DependencyProperty.UnsetValue;
                }
            }

            return (operationValue, null);
        }

        /// <summary>
        /// Calculates the value of the operation.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="a">First operand value.</param>
        /// <param name="b">Second operand value.</param>
        /// <returns>Effective value of the operation, or <c>null</c> of the back calculation is not possible.</returns>
        protected abstract TReturn? CalculateValue( TA a, TB b );

        protected sealed override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
        {
            Debug.Assert( sourceTypes.Length == NUM_OPERANDS );
            Debug.Assert( currentValues.Length == NUM_OPERANDS );

            if( targetValue == null )
            {
                return null;
            }

            var operationTargetValue = (TReturn) Helper.ConvertValue( targetValue, typeof( TReturn ), targetCulture )!;

            var backValues = CalculateBackValues( operationTargetValue, currentValues[ A_INDEX ], currentValues[ B_INDEX ] );

            if( backValues == null )
            {
                return null;
            }

            var result = new object?[ NUM_OPERANDS ];

            try
            {
                result[ A_INDEX ] = Helper.ConvertValue( backValues.Value.a, sourceTypes[ A_INDEX ], currentValues[ A_INDEX ].Culture );
            }
            catch
            {
                result[ A_INDEX ] = DependencyProperty.UnsetValue;
            }

            try
            {
                result[ B_INDEX ] = Helper.ConvertValue( backValues.Value.b, sourceTypes[ B_INDEX ], currentValues[ B_INDEX ].Culture );
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
        /// <param name="a">Current value of the first operand.</param>
        /// <param name="b">Current value of the second operand.</param>
        /// <returns>The values of the operands, or <c>null</c> if the calculation cannot be performed or is not feasible.</returns>
        protected abstract (TA a, TB b)? CalculateBackValues( TReturn targetValue, ComponentValue a, ComponentValue b );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 2;
        private const int A_INDEX = 0;
        private const int B_INDEX = 1;
    }
}
