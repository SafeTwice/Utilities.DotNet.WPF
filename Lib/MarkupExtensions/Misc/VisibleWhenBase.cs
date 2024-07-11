/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base for markup extensions that returns a <see cref="Visibility"/> value based on a condition.
    /// </summary>
    [MarkupExtensionReturnType( typeof( Visibility ) )]
    public abstract class VisibleWhenBase : BindingMarkupExtensionBase
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
        public VisibleWhenBase( bool invert ) : base( new Type[] { typeof( bool ), typeof( bool ) } )
        {
            m_invert = invert;
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
                returnedValue = ( visible.Value ^ m_invert ) ? Visibility.Visible :
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
                    ( ( visibility == Visibility.Visible ) ^ m_invert ),
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

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly bool m_invert;
    }
}
