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
    /// Base class for unary operations.
    /// </summary>
    public abstract class UnaryOperationBase<TValue, TReturn> : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Operand.
        /// </summary>
        public object? I
        {
            get => GetParameterRawValue( VALUE_INDEX );
            set => SetParameterRawValue( VALUE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected UnaryOperationBase() : base( new Type[] { typeof( TValue ) } )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length == NUM_OPERANDS );

            var value = (TValue?) parameterValues[ VALUE_INDEX ];

            object? operationValue;

            if( value == null )
            {
                // Unary operation cannot be performed if the operand is null.
                operationValue = DependencyProperty.UnsetValue;
            }
            else
            {
                operationValue = CalculateValue( value );

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
        /// <param name="operandValue">Value of the operand.</param>
        /// <returns></returns>
        protected abstract TReturn? CalculateValue( TValue operandValue );

        protected sealed override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
        {
            Debug.Assert( sourceTypes.Length == NUM_OPERANDS );
            Debug.Assert( currentValues.Length == NUM_OPERANDS );

            if( targetValue == null )
            {
                return null;
            }

            var operationTargetValue = (TReturn) Helper.ConvertValue( targetValue, typeof( TReturn ), targetCulture )!;

            var backValue = CalculateBackValue( operationTargetValue, currentValues[ VALUE_INDEX ] );

            if( backValue == null )
            {
                return null;
            }

            var result = new object?[ NUM_OPERANDS ];

            try
            {
                result[ VALUE_INDEX ] = Helper.ConvertValue( backValue, sourceTypes[ VALUE_INDEX ], currentValues[ VALUE_INDEX ].Culture );
            }
            catch
            {
                result[ VALUE_INDEX ] = DependencyProperty.UnsetValue;
            }

            return result;
        }

        /// <summary>
        /// Calculates the value to assign to the operand when converting back the operation value.
        /// </summary>
        /// <param name="targetValue">Value of the operation.</param>
        /// <param name="operandValue">Current value of the operand.</param>
        /// <returns>The value of the operand, or <c>null</c> if the calculation cannot be performed or is not feasible.</returns>
        protected abstract TValue? CalculateBackValue( TReturn targetValue, ComponentValue operandValue );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 1;
        private const int VALUE_INDEX = 0;
    }
}
