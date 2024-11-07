/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for comparison operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( double ) )]
    public abstract class ComparisonOperationBase : BinaryOperationBase<double, double, bool>
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ComparisonOperationBase() : base( false, false )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (double a, double b)? CalculateBackValues( bool targetValue, ComponentValue a, ComponentValue b )
        {
            // Comparison operations are not reversible.
            return null;
        }
    }
}
