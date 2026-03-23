/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that checks that an object is equal to the other (A == B).
    /// </summary>
    public sealed class IsEqual : ComparisonOperationBase<object?>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsEqual() : base( true )
        {
        }

        /// <summary>
        /// Constructor that initializes the two objects to compare.
        /// </summary>
        /// <param name="a">First object.</param>
        /// <param name="b">Second object.</param>
        public IsEqual( object? a, object? b ) : base( true )
        {
            A = a;
            B = b;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override bool CalculateValue( object? a, object? b )
        {
            return Equals( a, b );
        }
    }
}
