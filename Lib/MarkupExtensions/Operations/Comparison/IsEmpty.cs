/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that checks if a collection is empty.
    /// </summary>
    public sealed class IsEmpty : IsEmptyBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsEmpty() : base()
        {
        }

        /// <summary>
        /// Constructor that initializes the operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        public IsEmpty( object i ) : base( i )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override bool TransformResult( bool result ) => result;
    }
}
