/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Represents the state of a TextBox control.
    /// </summary>
    public struct TextBoxState
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Text contents.
        /// </summary>
        public string Text { readonly get; set; }

        /// <summary>
        /// Caret index.
        /// </summary>
        public int CaretIndex { readonly get; set; }

        /// <summary>
        /// Selection start index.
        /// </summary>
        public int SelectionStart { readonly get; set; }

        /// <summary>
        /// Selection length.
        /// </summary>
        public int SelectionLength { readonly get; set; }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextBoxState()
        {
            Text = string.Empty;
            CaretIndex = 0;
            SelectionStart = 0;
            SelectionLength = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text contents.</param>
        /// <param name="caretIndex">Caret index.</param>
        /// <param name="selectionStart">Selection start index.</param>
        /// <param name="selectionLength">Selection length.</param>
        public TextBoxState( string text, int caretIndex, int selectionStart, int selectionLength )
        {
            Text = text;
            CaretIndex = caretIndex;
            SelectionStart = selectionStart;
            SelectionLength = selectionLength;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="other">Other instance to copy.</param>
        public TextBoxState( TextBoxState other )
        {
            Text = other.Text;
            CaretIndex = other.CaretIndex;
            SelectionStart = other.SelectionStart;
            SelectionLength = other.SelectionLength;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override readonly bool Equals( object? obj )
        {
            if( obj is TextBoxState other )
            {
                return ( Text == other.Text ) &&
                       ( CaretIndex == other.CaretIndex ) &&
                       ( SelectionStart == other.SelectionStart ) &&
                       ( SelectionLength == other.SelectionLength );
            }

            return false;
        }

        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
#if NETFRAMEWORK
            unchecked
            {
                int hash = 17;
                hash = ( hash * 23 ) + Text.GetHashCode();
                hash = ( hash * 23 ) + CaretIndex;
                hash = ( hash * 23 ) + SelectionStart;
                hash = ( hash * 23 ) + SelectionLength;
                return hash;
            }
#else
            return HashCode.Combine( Text, CaretIndex, SelectionStart, SelectionLength );
#endif
        }

        /// <summary>
        /// Compares two <see cref="TextBoxState"/> instances for equality.
        /// </summary>
        /// <param name="left">First instance to compare.</param>
        /// <param name="right">Second instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==( TextBoxState left, TextBoxState right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Compares two <see cref="TextBoxState"/> instances for inequality.
        /// </summary>
        /// <param name="left">First instance to compare.</param>
        /// <param name="right">Second instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=( TextBoxState left, TextBoxState right )
        {
            return !left.Equals( right );
        }
    }
}
