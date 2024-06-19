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
    public abstract class ArithmeticBinaryOperationBase : BinaryOperationBase<double, double>
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ArithmeticBinaryOperationBase()
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override sealed (object? value, CultureInfo culture) CalculateOperationValue( double a, double b )
        {
            var operationValue = CalculateValue( a, b );

            return (operationValue, CultureInfo.InvariantCulture);
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
        protected abstract double CalculateValue( double a, double b );

        protected override (double a, double b)? CalculateOperationBackValues( object? targetValue )
        {
            return null;
        }
    }
}
