/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Represents the state of a RichTextBox control.
    /// </summary>
    public struct RichTextBoxState<TContent> where TContent : notnull
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Rich text content.
        /// </summary>
        public TContent Content { readonly get; set; }

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
        public int SelectionEnd { readonly get; set; }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public RichTextBoxState()
        {
            Content = default!;
            CaretIndex = 0;
            SelectionStart = 0;
            SelectionEnd = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Rich text content.</param>
        /// <param name="caretIndex">Caret index.</param>
        /// <param name="selectionStart">Selection start index.</param>
        /// <param name="selectionEnd">Selection length.</param>
        public RichTextBoxState( TContent content, int caretIndex, int selectionStart, int selectionEnd )
        {
            Content = content;
            CaretIndex = caretIndex;
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="other">Other instance to copy.</param>
        public RichTextBoxState( RichTextBoxState<TContent> other )
        {
            Content = other.Content;
            CaretIndex = other.CaretIndex;
            SelectionStart = other.SelectionStart;
            SelectionEnd = other.SelectionEnd;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public readonly override bool Equals( object? obj )
        {
            if( obj is RichTextBoxState<TContent> other )
            {
                return Content.Equals( other.Content ) &&
                       ( CaretIndex == other.CaretIndex ) &&
                       ( SelectionStart == other.SelectionStart ) &&
                       ( SelectionEnd == other.SelectionEnd );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
#if NETFRAMEWORK
            unchecked
            {
                int hash = 17;
                hash = ( hash * 23 ) + Content.GetHashCode();
                hash = ( hash * 23 ) + CaretIndex;
                hash = ( hash * 23 ) + SelectionStart;
                hash = ( hash * 23 ) + SelectionEnd;
                return hash;
            }
#else
            return HashCode.Combine( Content, CaretIndex, SelectionStart, SelectionEnd );
#endif
        }

        /// <summary>
        /// Compares two <see cref="RichTextBoxState{TContent}"/> instances for equality.
        /// </summary>
        /// <param name="left">First instance to compare.</param>
        /// <param name="right">Second instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==( RichTextBoxState<TContent> left, RichTextBoxState<TContent> right )
        {
            return left.Equals( right );
        }

        /// <summary>
        /// Compares two <see cref="RichTextBoxState{TContent}"/> instances for inequality.
        /// </summary>
        /// <param name="left">First instance to compare.</param>
        /// <param name="right">Second instance to compare.</param>
        /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=( RichTextBoxState<TContent> left, RichTextBoxState<TContent> right )
        {
            return !left.Equals( right );
        }
    }
}
