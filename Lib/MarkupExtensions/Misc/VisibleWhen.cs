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
    [MarkupExtensionReturnType( typeof( Visibility ) )]
    public class VisibleWhen : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Condition to calculate the visibility.
        /// </summary>
        public object? Condition
        {
            get => GetParameterRawValue( CONDITION_INDEX );
            set => SetParameterRawValue( CONDITION_INDEX, value );
        }

        /// <summary>
        /// Indicates whether the element should be hidden when the <see cref="Condition"> is <c>false</c>.
        /// Otherwise, the element will be collapsed.
        /// </summary>
        public object? HiddenWhenInvisible
        {
            get => GetParameterRawValue( HIDE_WHEN_INVISIBLE_INDEX );
            set => SetParameterRawValue( HIDE_WHEN_INVISIBLE_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public VisibleWhen() : base( new Type[] { typeof( bool ), typeof( bool ) } )
        {
        }

        public VisibleWhen( object? condition ) : this()
        {
            Condition = condition;
        }

        public VisibleWhen( object? condition, object? hiddenWhenInvisible ) : this( condition )
        {
            HiddenWhenInvisible = hiddenWhenInvisible;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length == NUM_OPERANDS );

            var value = (bool?) parameterValues[ CONDITION_INDEX ];
            var hideWhenInvisible = ( (bool?) parameterValues[ HIDE_WHEN_INVISIBLE_INDEX ] ) ?? false;

            object? returnedValue;

            if( value == null )
            {
                returnedValue = DependencyProperty.UnsetValue;
            }
            else
            {
                returnedValue = value.Value ? Visibility.Visible :
                                hideWhenInvisible ? Visibility.Hidden : Visibility.Collapsed;
            }

            return (returnedValue, null);
        }

        protected sealed override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
        {
            Debug.Assert( sourceTypes.Length == NUM_OPERANDS );
            Debug.Assert( currentValues.Length == NUM_OPERANDS );

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
