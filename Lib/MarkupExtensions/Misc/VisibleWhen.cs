/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Markup extension that returns a <see cref="Visibility"/> value based on a condition.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="Condition"/> is <c>true</c>, the value returned is <see cref="Visibility.Visible"/>.</para>
    /// <para>Otherwise, the value returned is <see cref="Visibility.Hidden"/> if <see cref="VisibleWhenBase.HiddenWhenInvisible">HiddenWhenInvisible</see> is <c>true</c>;
    /// or <see cref="Visibility.Collapsed"/> otherwise.</para>
    /// </remarks>
    [MarkupExtensionReturnType( typeof( Visibility ) )]
    public sealed class VisibleWhen : VisibleWhenBase
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VisibleWhen() : base( false )
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
    }
}
