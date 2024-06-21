/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that selects a value based on a condition.
    /// </summary>
    [MarkupExtensionReturnType( typeof( object ) )]
    public sealed class Choice : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Condition to calculate the value to choose.
        /// </summary>
        [ConstructorArgument( "condition" )]
        public object? Condition
        {
            get => GetParameterRawValue( CONDITION_INDEX );
            set => SetParameterRawValue( CONDITION_INDEX, value );
        }

        /// <summary>
        /// Value returned when <see cref="Condition"/> is <c>true</c>.
        /// </summary>
        [ConstructorArgument( "trueValue" )]
        public object? True
        {
            get => GetParameterRawValue( TRUE_VALUE_INDEX );
            set => SetParameterRawValue( TRUE_VALUE_INDEX, value );
        }

        /// <summary>
        /// Value returned when <see cref="Condition"/> is <c>false</c>.
        /// </summary>
        [ConstructorArgument( "falseValue" )]
        public object? False
        {
            get => GetParameterRawValue( FALSE_VALUE_INDEX );
            set => SetParameterRawValue( FALSE_VALUE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Choice() : base( new Type[] { typeof( bool ), typeof( object ), typeof( object ) } )
        {
        }

        /// <summary>
        /// Constructor that initializes the condition and the values to choose.
        /// </summary>
        /// <param name="condition">Condition to calculate the value to choose.</param>
        /// <param name="trueValue">Value returned when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="falseValue">Value returned when <paramref name="condition"/> is <c>false</c>.</param>
        public Choice( object? condition, object? trueValue, object? falseValue ) : this()
        {
            Condition = condition;
            True = trueValue;
            False = falseValue;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length == NUM_OPERANDS );
            Debug.Assert( parameterCultures.Length == NUM_OPERANDS );

            var condition = (bool?) parameterValues[ CONDITION_INDEX ];

            object? returnedValue;
            CultureInfo? returnedCulture;

            if( condition == null )
            {
                returnedValue = DependencyProperty.UnsetValue;
                returnedCulture = null;
            }
            else
            {
                returnedValue = condition.Value ? parameterValues[ TRUE_VALUE_INDEX ] : parameterValues[ FALSE_VALUE_INDEX ];
                returnedCulture = condition.Value ? parameterCultures[ TRUE_VALUE_INDEX ] : parameterCultures[ FALSE_VALUE_INDEX ];
            }

            return (returnedValue, returnedCulture);
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 3;
        private const int CONDITION_INDEX = 0;
        private const int TRUE_VALUE_INDEX = 1;
        private const int FALSE_VALUE_INDEX = 2;
    }
}
