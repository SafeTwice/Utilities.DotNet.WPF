/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for arithmetic binary operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( double ) )]
    public abstract class ArithmeticBinaryOperationBase : BinaryOperationBase
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ArithmeticBinaryOperationBase() : base()
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override sealed (object? value, CultureInfo culture) CalculateValue( object? a, object? b )
        {
            object? operationValue;

            if( ( a == null ) || ( b == null ) )
            {
                operationValue = null;
            }
            else
            {
                operationValue = CalculateValue( (double) a, (double) b );
            }

            return (operationValue, CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        protected override object? ConvertOperandAValue( object? value, CultureInfo culture ) => ConvertOperandValue( value, culture );

        /// <inheritdoc/>
        protected override object? ConvertOperandBValue( object? value, CultureInfo culture ) => ConvertOperandValue( value, culture );

        /// <summary>
        /// Calculates the value of the operation.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="a">Left operand value.</param>
        /// <param name="b">Right operand value.</param>
        /// <returns></returns>
        protected abstract double CalculateValue( double a, double b );

        protected override (object? a, object? b)? CalculateBackValues( object? targetValue )
        {
            return null;
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private object? ConvertOperandValue( object? value, CultureInfo culture )
        {
            if( ( value == null ) || ( value.GetType() == typeof( double ) ) )
            {
                return value;
            }
            else
            {
                try
                {
                    return Convert.ToDouble( value, culture );
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
