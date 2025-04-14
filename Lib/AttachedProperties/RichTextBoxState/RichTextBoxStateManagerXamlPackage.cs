/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Controls;
using System.Windows.Documents;
using Utilities.DotNet.WPF.Extensions;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Defines an attached property to manage and observe the state of a <see cref="RichTextBox"/>.
    /// </summary>
    public class RichTextBoxStateManagerXamlPackage : RichTextBoxStateManagerBase<byte[]>
    {

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RichTextBoxStateManagerXamlPackage() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="document">Flow document.</param>
        public RichTextBoxStateManagerXamlPackage( FlowDocument document ) : base( document.ToXamlPackage() )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Rich text content.</param>
        public RichTextBoxStateManagerXamlPackage( byte[] content ) : base( content )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content"> content.</param>
        /// <param name="caretIndex">Insertion position index of the caret.</param>
        /// <param name="selectionStart">Character index for the beginning of the current selection.</param>
        /// <param name="selectionEnd">Number of characters in the current selection.</param>
        public RichTextBoxStateManagerXamlPackage( byte[] content, int caretIndex, int selectionStart, int selectionEnd )
            : base( content, caretIndex, selectionStart, selectionEnd )
        {
        }

        //===========================================================================
        //                           PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc />
        protected override void LoadFromContent( FlowDocument document, byte[] value )
        {
            document.LoadFromXamlPackage( value );
        }

        /// <inheritdoc />
        protected override byte[] SaveToContent( FlowDocument document )
        {
            return document.ToXamlPackage();
        }
    }
}
