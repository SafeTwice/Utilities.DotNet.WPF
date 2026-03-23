/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that checks that a number (or any other comparable object) is equal or smaller than the other (A &lt;= B).
    /// </summary>
    public sealed class IsLessOrEqual : ComparisonOperationBase<IComparable>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsLessOrEqual() : base( false )
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to compare.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public IsLessOrEqual( object a, object b ) : base( false )
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
                return ( a!.CompareTo( b ) <= 0 );
            }
            catch( Exception ex )
            {
                throw new ArgumentException( $"Arguments A ('{a}') and B ('{b}') cannot be compared", ex );
            }
        }
    }
}
