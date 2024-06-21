/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for arithmetic binary operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( double ) )]
    public abstract class ArithmeticBinaryOperationBase : BinaryOperationBase<double, double, double>
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
        protected override (double a, double b)? CalculateBackValues( double targetValue, ComponentValue a, ComponentValue b )
        {
            if( a.IsReversible && b.IsReversible )
            {
                // Both operands are reversible => reverse operation is not possible
                return null;
            }

            double? aUsed = a.IsReversible ? null : (double) a.Value!;
            double? bUsed = b.IsReversible ? null : (double) b.Value!;

            if( ( aUsed != null ) && ( bUsed != null ) )
            {
                // Neither operand is reversible.

                if( targetValue != CalculateValue( aUsed.Value, bUsed.Value ) )
                {
                    // The target value is not the result of the operation.
                    return null;
                }
                else
                {
                    return (aUsed.Value, bUsed.Value);
                }
            }
            else if( aUsed == null )
            {
                // First operand is reversible.

                double bValue = bUsed!.Value;
                double aValue = CalculateBackValueA( targetValue, bValue );

                return (aValue, bValue);
            }
            else
            {
                // Second operand is reversible.

                double aValue = aUsed!.Value;
                double bValue = CalculateBackValueB( targetValue, aValue );

                return (aValue, bValue);
            }
        }

        /// <summary>
        /// Calculates the values to assign to the first operand when converting back the operation value and the second operand has a fixed value.
        /// </summary>
        /// <param name="targetValue">Value of the operation.</param>
        /// <param name="b">Second operand value.</param>
        /// <returns>Value of the first operand.</returns>
        protected abstract double CalculateBackValueA( double targetValue, double b );

        /// <summary>
        /// Calculates the values to assign to the second operand when converting back the operation value and the first operand has a fixed value.
        /// </summary>
        /// <param name="targetValue">Value of the operation.</param>
        /// <param name="a">First operand value.</param>
        /// <returns>Value of the second operand.</returns>
        protected abstract double CalculateBackValueB( double targetValue, double a );
    }
}
