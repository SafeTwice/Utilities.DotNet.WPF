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
    public abstract class BooleanUnaryOperationBase : UnaryOperationBase<bool, bool>
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BooleanUnaryOperationBase() : base( false )
        {
        }
    }
}
