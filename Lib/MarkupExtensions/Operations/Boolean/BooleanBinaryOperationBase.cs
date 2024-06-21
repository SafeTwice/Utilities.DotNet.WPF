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
    public abstract class BooleanBinaryOperationBase : BinaryOperationBase<bool, bool, bool>
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BooleanBinaryOperationBase()
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (bool a, bool b)? CalculateBackValues( bool targetValue, ComponentValue a, ComponentValue b )
        {
            // Comparison operations are generally not reversible.
            return null;
        }
    }
}
