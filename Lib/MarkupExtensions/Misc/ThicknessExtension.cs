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
    /// Markup extension to make <see cref="Thickness"/> bindable.
    /// </summary>
    [MarkupExtensionReturnType( typeof( Thickness ) )]
    public sealed class ThicknessExtension : BindingMarkupExtensionBase
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Left side thickness.
        /// </summary>
        [ConstructorArgument( "left" )]
        public object? Left
        {
            get => GetParameterRawValue( LEFT_INDEX );
            set => SetParameterRawValue( LEFT_INDEX, value );
        }

        /// <summary>
        /// Top side thickness.
        /// </summary>
        [ConstructorArgument( "top" )]
        public object? Top
        {
            get => GetParameterRawValue( TOP_INDEX );
            set => SetParameterRawValue( TOP_INDEX, value );
        }

        /// <summary>
        /// Right side thickness.
        /// </summary>
        [ConstructorArgument( "right" )]
        public object? Right
        {
            get => GetParameterRawValue( RIGHT_INDEX );
            set => SetParameterRawValue( RIGHT_INDEX, value );
        }

        /// <summary>
        /// Right side thickness.
        /// </summary>
        [ConstructorArgument( "bottom" )]
        public object? Bottom
        {
            get => GetParameterRawValue( BOTTOM_INDEX );
            set => SetParameterRawValue( BOTTOM_INDEX, value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ThicknessExtension() : base( new Type[] { typeof( double ), typeof( double ), typeof( double ), typeof( double ) } )
        {
        }

        /// <summary>
        /// Constructor that initializes the thickness with the same value on all sides.
        /// </summary>
        /// <param name="uniformThickness">Thickness for all sides.</param>
        public ThicknessExtension( object? uniformThickness ) : this()
        {
            Left = uniformThickness;
            Top = uniformThickness;
            Right = uniformThickness;
            Bottom = uniformThickness;
        }

        /// <summary>
        /// Constructor that initializes the thickness with the same value on opposite sides.
        /// </summary>
        /// <param name="leftRight">Left and right sides thickness.</param>
        /// <param name="topBottom">Top and bottom sides thickness.</param>
        public ThicknessExtension( object? leftRight, object? topBottom ) : this()
        {
            Left = leftRight;
            Top = topBottom;
            Right = leftRight;
            Bottom = topBottom;
        }

        /// <summary>
        /// Constructor that initializes the thickness with different values on each side.
        /// </summary>
        /// <param name="left">Left side thickness.</param>
        /// <param name="top">Top side thickness.</param>
        /// <param name="right">Right side thickness.</param>
        /// <param name="bottom">Bottom side thickness.</param>
        public ThicknessExtension( object? left, object? top, object? right, object? bottom ) : this()
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture )
        {
            Debug.Assert( parameterValues.Length == NUM_OPERANDS );
            Debug.Assert( parameterCultures.Length == NUM_OPERANDS );

            var left = ( (double?) parameterValues[ LEFT_INDEX ] ) ?? 0.0;
            var top = ( (double?) parameterValues[ TOP_INDEX ] ) ?? 0.0;
            var right = ( (double?) parameterValues[ RIGHT_INDEX ] ) ?? 0.0;
            var bottom = ( (double?) parameterValues[ BOTTOM_INDEX ] ) ?? 0.0;

            object? returnedValue = new Thickness
            {
                Left = left,
                Top = top,
                Right = right,
                Bottom = bottom
            };

            return (returnedValue, null);
        }

        /// <inheritdoc/>
        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
        {
            if( targetValue is Thickness thickness )
            {
                return new object?[ NUM_OPERANDS ]
                {
                    thickness.Left,
                    thickness.Top,
                    thickness.Right,
                    thickness.Bottom
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

        private const int NUM_OPERANDS = 4;
        private const int LEFT_INDEX = 0;
        private const int TOP_INDEX = 1;
        private const int RIGHT_INDEX = 2;
        private const int BOTTOM_INDEX = 3;
    }
}
