/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that calculates the power of two numbers (A ^ B).
    /// </summary>
    public sealed class Power : ArithmeticBinaryOperationBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Power()
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to calculate the power of.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public Power( double a, double b )
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
            return Math.Pow( a, b );
        }

        /// <inheritdoc/>
        protected override double CalculateBackValueA( double targetValue, double b )
        {
            // a = targetValue ^ ( 1 / b )
            return Math.Pow( targetValue, 1.0 / b );
        }

        /// <inheritdoc/>
        protected override double CalculateBackValueB( double targetValue, double a )
        {
            // b = log_a( targetValue )
            return Math.Log( targetValue, a );
        }
    }
}
