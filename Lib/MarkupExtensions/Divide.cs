/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Markup extension that divides two numbers (A / B).
    /// </summary>
    [MarkupExtensionReturnType( typeof( double ) )]
    public class Divide : MarkupExtension
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// First number.
        /// </summary>
        [ConstructorArgument( "a" )]
        public double A { get; set; } = 0.0;

        /// <summary>
        /// Second number.
        /// </summary>
        [ConstructorArgument( "b" )]
        public double B { get; set; } = 0.0;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Divide()
        {
        }

        /// <summary>
        /// Constructor that initializes the two numbers to divide.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        public Divide( double a, double b )
        {
            A = a;
            B = b;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            return A / B;
        }
    }
}
