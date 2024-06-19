/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that multiplies two numbers.
    /// </summary>
    public sealed class Multiply : ArithmeticBinaryOperationBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Multiply()
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to multiply.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public Multiply( double a, double b )
        {
            A = a;
            B = b;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override double CalculateValue( double a, double b )
        {
            return ( a * b );
        }
    }
}
