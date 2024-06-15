/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for operations that combine multiple values.
    /// </summary>
    public abstract class OperationBase : MultiBindBase
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor that initializes the number of operands.
        /// </summary>
        /// <param name="numOperands">Number of operands of the operation.</param>
        protected OperationBase( int numOperands )
        {
            for( int i = 0; i < numOperands; i++ )
            {
                Components.Add( null );
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Gets the value of the specified operand.
        /// </summary>
        /// <remarks>
        /// The operand identifier must be in the range from 0 to the number of operands (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="operandId">Identifier of the operand.</param>
        /// <returns>Value of the operand.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the operand identifier is out of range.</exception>
        protected object? GetOperandValue( int operandId )
        {
            if( ( operandId < 0 ) || ( operandId >= Components.Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( operandId ) );
            }

            return Components[ operandId ];
        }

        /// <summary>
        /// Sets the value of the specified operand.
        /// </summary>
        /// <remarks>
        /// The operand identifier must be in the range from 0 to the number of operands (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="operandId">Identifier of the operand.</param>
        /// <param name="operandValue">Value of the operand.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the operand identifier is out of range.</exception>
        protected void SetOperandValue( int operandId, object? operandValue )
        {
            if( ( operandId < 0 ) || ( operandId >= Components.Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( operandId ) );
            }

            Components[ operandId ] = operandValue;
        }

        /// <inheritdoc/>
        protected override object? CalculateValue( object?[] componentValues, Type targetType, CultureInfo culture )
        {
            return CalculateValue( componentValues );
        }

        /// <summary>
        /// Calculates the value of the operation from the values of the operands.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="operandValues">Array with the values of the operands.</param>
        /// <returns>Value of the operation.</returns>
        protected abstract object? CalculateValue( object?[] operandValues );
    }
}
