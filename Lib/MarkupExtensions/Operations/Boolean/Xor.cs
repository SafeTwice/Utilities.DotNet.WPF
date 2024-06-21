/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that performs the logic OR of two boolean values (A && B).
    /// </summary>
    public sealed class Xor : BooleanBinaryOperationBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Xor()
        {
        }

        /// <summary>
        /// Constructor that initializes the two boolean operands.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        public Xor( object a, object b )
        {
            A = a;
            B = b;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override bool CalculateValue( bool a, bool b )
        {
            return ( a ^ b );
        }

        /// <inheritdoc/>
        protected override (bool a, bool b)? CalculateBackValues( bool targetValue, ComponentValue a, ComponentValue b )
        {
            if( a.IsReversible && b.IsReversible )
            {
                // Both operands are reversible => reverse operation is not possible
                return null;
            }

            bool? aUsed = a.IsReversible ? null : (bool) a.Value!;
            bool? bUsed = b.IsReversible ? null : (bool) b.Value!;

            if( ( aUsed != null ) && ( bUsed != null ) )
            {
                // Neither operand is reversible.
                return null;
            }
            else if( aUsed == null )
            {
                // First operand is reversible.

                bool bValue = bUsed!.Value;
                bool aValue = ( targetValue ^ bValue );

                return (aValue, bValue);
            }
            else
            {
                // Second operand is reversible.

                bool aValue = aUsed!.Value;
                bool bValue = ( targetValue ^ aValue );

                return (aValue, bValue);
            }
        }

    }
}
