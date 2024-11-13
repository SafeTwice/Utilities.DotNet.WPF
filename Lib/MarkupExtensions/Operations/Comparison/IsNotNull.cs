/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that checks if an object is not null.
    /// </summary>
    [MarkupExtensionReturnType( typeof( bool ) )]
    public class IsNotNull : UnaryOperationBase<object?, bool>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsNotNull() : base( true )
        {
        }

        /// <summary>
        /// Constructor that initializes the operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        public IsNotNull( object i ) : this()
        {
            I = i;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override bool CalculateValue( object? operandValue )
        {
            return operandValue is not null;
        }

        protected override object? CalculateBackValue( bool targetValue, ComponentValue operandValue )
        {
            return null;
        }
    }
}
