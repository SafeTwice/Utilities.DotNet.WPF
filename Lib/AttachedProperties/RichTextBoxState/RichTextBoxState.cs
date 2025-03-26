/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

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
        public TContent Content { get; set; }

        /// <summary>
        /// Caret index.
        /// </summary>
        public int CaretIndex { get; set; }

        /// <summary>
        /// Selection start index.
        /// </summary>
        public int SelectionStart { get; set; }

        /// <summary>
        /// Selection length.
        /// </summary>
        public int SelectionEnd { get; set; }

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
        public override bool Equals( object? obj )
        {
            if( obj is RichTextBoxState<TContent> other )
            {
                return Content.Equals( other.Content ) &&
                       CaretIndex == other.CaretIndex &&
                       SelectionStart == other.SelectionStart &&
                       SelectionEnd == other.SelectionEnd;
            }
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = ( hash * 23 ) + Content.GetHashCode();
                hash = ( hash * 23 ) + CaretIndex;
                hash = ( hash * 23 ) + SelectionStart;
                hash = ( hash * 23 ) + SelectionEnd;
                return hash;
            }
        }
    }
}
