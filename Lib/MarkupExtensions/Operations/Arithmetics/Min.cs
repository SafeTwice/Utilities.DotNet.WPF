﻿/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that calculates the minimum of two numbers
    /// </summary>
    public sealed class Min : ArithmeticBinaryOperationBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Min()
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to compare.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public Min( object a, object b )
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
            return Math.Min( a, b );
        }

        /// <inheritdoc/>
        protected override double CalculateBackValueA( double targetValue, double b )
        {
            return targetValue;
        }

        /// <inheritdoc/>
        protected override double CalculateBackValueB( double targetValue, double a )
        {
            return targetValue;
        }
    }
}
