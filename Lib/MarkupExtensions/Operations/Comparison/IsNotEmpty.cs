/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that checks if a collection is not empty.
    /// </summary>
    public sealed class IsNotEmpty : IsEmptyBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IsNotEmpty() : base()
        {
        }

        /// <summary>
        /// Constructor that initializes the operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        public IsNotEmpty( object i ) : base( i )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override bool TransformResult( bool result ) => !result;
    }
}
