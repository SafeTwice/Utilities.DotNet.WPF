/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that checks that a number (or any other comparable object) is strictly greater than the other (A > B).
    /// </summary>
    public sealed class IsGreater : ComparisonOperationBase<IComparable>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsGreater() : base( false )
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to compare.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public IsGreater( object a, object b ) : base( false )
        {
            A = a;
            B = b;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override bool CalculateValue( IComparable? a, IComparable? b )
        {
            try
            {
                return ( a!.CompareTo( b ) > 0 );
            }
            catch( Exception ex )
            {
                throw new ArgumentException( $"Arguments A ('{a}') and B ('{b}') cannot be compared", ex );
            }
        }
    }
}
