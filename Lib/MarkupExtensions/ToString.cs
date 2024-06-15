/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that converts an object to a string.
    /// </summary>
    [MarkupExtensionReturnType( typeof( string ) )]
    public sealed class ToString : OperationBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Value to convert.
        /// </summary>
        [ConstructorArgument( "value" )]
        public object? Value
        {
            get => GetOperandValue( VALUE_INDEX );
            set => SetOperandValue( VALUE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToString() : base( ITEM_COUNT )
        {
        }

        /// <summary>
        /// Constructor that initializes the value to convert.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public ToString( object value ) : base( ITEM_COUNT )
        {
            Value = value;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override object CalculateValue( object?[] operandValues )
        {
            return Convert.ToString( operandValues[ VALUE_INDEX ] )!;
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int ITEM_COUNT = 1;
        private const int VALUE_INDEX = 0;
    }
}
