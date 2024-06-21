/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that subtracts two numbers (A - B).
    /// </summary>
    public sealed class Subtract : ArithmeticBinaryOperationBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Subtract()
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to subtract.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public Subtract( double a, double b )
        {
            A = a;
            B = b;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override double CalculateValue( double a, double b )
        {
            return ( a - b );
        }

        /// <inheritdoc/>
        protected override double CalculateBackValueA( double targetValue, double b )
        {
            return ( targetValue + b );
        }

        /// <inheritdoc/>
        protected override double CalculateBackValueB( double targetValue, double a )
        {
            return ( a - targetValue );
        }
    }
}
