/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that performs the logic NOT of a boolean value (!I).
    /// </summary>
    public sealed class Not : BooleanUnaryOperationBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Not()
        {
        }

        /// <summary>
        /// Constructor that initializes the boolean operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        public Not( object i )
        {
            I = i;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override bool CalculateValue( bool operandValue )
        {
            return !operandValue;
        }

        /// <inheritdoc/>
        protected override bool CalculateBackValue( bool targetValue, ComponentValue operandValue )
        {
            return !targetValue;
        }
    }
}
