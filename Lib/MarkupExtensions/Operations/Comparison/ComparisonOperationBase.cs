/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
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
        protected ComparisonOperationBase()
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
