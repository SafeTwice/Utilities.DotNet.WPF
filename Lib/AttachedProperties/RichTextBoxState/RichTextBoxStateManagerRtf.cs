/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Controls;
using System.Windows.Documents;
using Utilities.DotNet.WPF.Extensions;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Defines an attached property to manage and observe the state of a <see cref="RichTextBox"/>.
    /// </summary>
    public class RichTextBoxStateManagerRtf : RichTextBoxStateManagerBase<string>
    {

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RichTextBoxStateManagerRtf() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="document">Rich text document.</param>
        public RichTextBoxStateManagerRtf( FlowDocument document ) : base( document )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Rich text content.</param>
        public RichTextBoxStateManagerRtf( string content ) : base( content )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content"> content.</param>
        /// <param name="caretIndex">Insertion position index of the caret.</param>
        /// <param name="selectionStart">Character index for the beginning of the current selection.</param>
        /// <param name="SelectionEnd">Number of characters in the current selection.</param>
        public RichTextBoxStateManagerRtf( string content, int caretIndex, int selectionStart, int SelectionEnd )
            : base( content, caretIndex, selectionStart, SelectionEnd )
        {
        }

        //===========================================================================
        //                           PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc />
        protected override void LoadFromContent( FlowDocument document, string value )
        {
            document.LoadFromRtf( value );
        }

        /// <inheritdoc />
        protected override string SaveToContent( FlowDocument document )
        {
            return document.ToRtf();
        }
    }
}
