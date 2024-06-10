/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for binary operations.
    /// </summary>
    public abstract class BinaryOperationBase : OperationBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Left operand.
        /// </summary>
        public object? A
        {
            get => GetOperandValue( A_INDEX );
            set => SetOperandValue( A_INDEX, value );
        }

        /// <summary>
        /// Right operand.
        /// </summary>
        public object? B
        {
            get => GetOperandValue( B_INDEX );
            set => SetOperandValue( B_INDEX, value );
        }

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BinaryOperationBase() : base( ITEM_COUNT )
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected sealed override object? CalculateValue( object?[] itemValues )
        {
            var a = itemValues[ A_INDEX ];
            var b = itemValues[ B_INDEX ];

            return CalculateValue( a, b );
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
        protected abstract object? CalculateValue( object? a, object? b );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int ITEM_COUNT = 2;
        private const int A_INDEX = 0;
        private const int B_INDEX = 1;
    }
}
