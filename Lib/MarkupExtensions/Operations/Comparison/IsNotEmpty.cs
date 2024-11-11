/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for boolean binary operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( bool ) )]
    public class IsNotEmpty : UnaryOperationBase<IEnumerable?, bool>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsNotEmpty() : base( true )
        {
        }

        /// <summary>
        /// Constructor that initializes the operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        public IsNotEmpty( object i ) : this()
        {
            I = i;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override bool CalculateValue( IEnumerable? operandValue )
        {
            if( operandValue == null )
            {
                return false;
            }
            else
            {
                return operandValue.GetEnumerator().MoveNext();
            }
        }

        protected override IEnumerable? CalculateBackValue( bool targetValue, ComponentValue operandValue )
        {
            return null;
        }
    }
}
