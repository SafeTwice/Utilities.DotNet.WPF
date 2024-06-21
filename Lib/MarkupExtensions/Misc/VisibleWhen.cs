/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that returns a <see cref="Visibility"/> value based on a condition.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="Condition"/> is <c>true</c>, the value returned is <see cref="Visibility.Visible"/>.</para>
    /// <para>Otherwise, the value returned is <see cref="Visibility.Hidden"/> if <see cref="HiddenWhenInvisible"/> is <c>true;</c>
    /// or <see cref="Visibility.Collapsed"/> otherwise.</para>
    /// </remarks>
    [MarkupExtensionReturnType( typeof( Visibility ) )]
    public sealed class VisibleWhen : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Condition to calculate the visibility.
        /// </summary>
        [ConstructorArgument( "condition" )]
        public object? Condition
        {
            get => GetParameterRawValue( CONDITION_INDEX );
            set => SetParameterRawValue( CONDITION_INDEX, value );
        }

        /// <summary>
        /// Selects the visibility value when the condition is <c>false</c>.
        /// </summary>
        [ConstructorArgument( "hiddenWhenInvisible" )]
        public object? HiddenWhenInvisible
        {
            get => GetParameterRawValue( HIDE_WHEN_INVISIBLE_INDEX );
            set => SetParameterRawValue( HIDE_WHEN_INVISIBLE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VisibleWhen() : base( new Type[] { typeof( bool ), typeof( bool ) } )
        {
        }

        /// <summary>
        /// Constructor that initializes the condition.
        /// </summary>
        /// <param name="condition">Condition to calculate the visibility.</param>
        public VisibleWhen( object? condition ) : this()
        {
            Condition = condition;
        }

        /// <summary>
        /// Constructor that initializes the condition and selects the value to return when the condition is <c>false</c>.
        /// </summary>
        /// <param name="condition">Condition to calculate the visibility.</param>
        /// <param name="hiddenWhenInvisible">Selects the visibility value when the condition is <c>false</c>.</param>
        public VisibleWhen( object? condition, object? hiddenWhenInvisible ) : this( condition )
        {
            HiddenWhenInvisible = hiddenWhenInvisible;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length == NUM_OPERANDS );

            var visible = (bool?) parameterValues[ CONDITION_INDEX ];
            var hideWhenInvisible = ( (bool?) parameterValues[ HIDE_WHEN_INVISIBLE_INDEX ] ) ?? false;

            object? returnedValue;

            if( visible == null )
            {
                returnedValue = DependencyProperty.UnsetValue;
            }
            else
            {
                returnedValue = visible.Value ? Visibility.Visible :
                                hideWhenInvisible ? Visibility.Hidden : Visibility.Collapsed;
            }

            return (returnedValue, null);
        }

        /// <inheritdoc/>
        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
        {
            if( targetValue is Visibility visibility )
            {
                return new object?[ NUM_OPERANDS ]
                {
                    ( visibility == Visibility.Visible ),
                    Binding.DoNothing,
                };
            }
            else
            {
                return null;
            }
        }

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const int NUM_OPERANDS = 2;
        private const int CONDITION_INDEX = 0;
        private const int HIDE_WHEN_INVISIBLE_INDEX = 1;
    }
}
