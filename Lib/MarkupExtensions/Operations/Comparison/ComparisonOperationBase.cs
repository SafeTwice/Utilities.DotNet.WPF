/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Globalization;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for comparison operations.
    /// </summary>
    [MarkupExtensionReturnType( typeof( bool ) )]
    public abstract class ComparisonOperationBase<TOperand> : BinaryOperationBase<TOperand, TOperand, bool>
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ComparisonOperationBase( bool areOperandsNullable ) : base( areOperandsNullable, areOperandsNullable )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (TOperand a, TOperand b)? CalculateBackValues( bool targetValue, ComponentValue a, ComponentValue b )
        {
            // Comparison operations are not reversible.
            return null;
        }

        /// <inheritdoc/>
        protected sealed override (TOperand? a, TOperand? b) ConvertValues( TOperand? a, TOperand? b, CultureInfo? cultureA, CultureInfo? cultureB )
        {
            var typeA = a?.GetType();
            var typeB = b?.GetType();
            if( ( typeA is null ) || ( typeB is null ) || ( typeA == typeB ) )
            {
                return (a, b);
            }
            else
            {
                if( a is string )
                {
                    var newA = (TOperand?) Helper.TryConvertValue( a, typeB, cultureA );
                    return (newA, b);
                }
                else if( b is string )
                {
                    var newB = (TOperand?) Helper.TryConvertValue( b, typeA, cultureB );
                    return (a, newB);
                }
                else
                {
                    try
                    {
                        var newA = (TOperand?) Helper.ConvertValue( a, typeB, cultureA );
                        return (newA, b);
                    }
                    catch
                    {
                        try
                        {
                            var newB = (TOperand?) Helper.ConvertValue( b, typeA, cultureB );
                            return (a, newB);
                        }
                        catch
                        {
                            return (a, b);
                        }
                    }
                }
            }
        }
    }
}