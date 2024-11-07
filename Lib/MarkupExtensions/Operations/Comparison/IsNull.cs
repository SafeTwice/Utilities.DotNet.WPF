/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for boolean binary operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( bool ) )]
    public class IsNull : UnaryOperationBase<object?, bool>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsNull() : base( true )
        {
        }

        /// <summary>
        /// Constructor that initializes the operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        public IsNull( object i ) : this()
        {
            I = i;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override bool CalculateValue( object? operandValue )
        {
            return operandValue is null;
        }

        protected override object? CalculateBackValue( bool targetValue, ComponentValue operandValue )
        {
            return null;
        }
    }
}
