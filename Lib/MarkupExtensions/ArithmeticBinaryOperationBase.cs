/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for arithmetic binary operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( double ) )]
    public abstract class ArithmeticBinaryOperationBase : BinaryOperationBase
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ArithmeticBinaryOperationBase() : base()
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override sealed object? CalculateValue( object? aObject, object? bObject )
        {
            if( ( aObject == null ) || ( bObject == null ) )
            {
                return null;
            }

            double aNumber;
            double bNumber;

            try
            {
                aNumber = Convert.ToDouble( aObject, CultureInfo.InvariantCulture );
            }
            catch
            {
                return null;
            }

            try
            {
                bNumber = Convert.ToDouble( bObject, CultureInfo.InvariantCulture );
            }
            catch
            {
                return null;
            }

            return CalculateValue( aNumber, bNumber );
        }

        /// <summary>
        /// Calculates the value of the operation.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="a">Left operand value.</param>
        /// <param name="b">Right operand value.</param>
        /// <returns></returns>
        protected abstract double CalculateValue( double a, double b );
    }
}
