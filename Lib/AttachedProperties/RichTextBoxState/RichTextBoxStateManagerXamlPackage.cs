/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Utilities.DotNet.WPF.Extensions;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Defines an attached property to manage and observe the state of a <see cref="RichTextBox"/>.
    /// </summary>
    public sealed class RichTextBoxStateManagerXamlPackage : RichTextBoxStateManagerBase<XamlPackageContainer>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the attached Manager property.
        /// </summary>
        public static readonly DependencyProperty ManagerProperty =
        DependencyProperty.RegisterAttached( "Manager", typeof( RichTextBoxStateManagerXamlPackage ), typeof( RichTextBoxStateManagerXamlPackage ),
                                             new PropertyMetadata( null, AttachedManagerChanged ) );

        /// <summary>
        /// Gets the state manager of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="obj">The rich text box to get the value from.</param>
        /// <returns>State manager of the rich text box.</returns>
        [Browsable( false )]
        public static RichTextBoxStateManagerXamlPackage GetManager( RichTextBox obj )
        {
            return (RichTextBoxStateManagerXamlPackage) obj.GetValue( ManagerProperty );
        }

        /// <summary>
        /// Sets the state manager of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="richTextBox">The rich text box to set the value to.</param>
        /// <param name="value">State manager for the rich text box.</param>
        public static void SetManager( RichTextBox richTextBox, RichTextBoxStateManagerXamlPackage value )
        {
            richTextBox.SetValue( ManagerProperty, value );
        }

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
        public RichTextBoxStateManagerXamlPackage( FlowDocument document ) : base( new( document.ToXamlPackage() ) )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Rich text content.</param>
        public RichTextBoxStateManagerXamlPackage( XamlPackageContainer content ) : base( content )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Rich text content.</param>
        public RichTextBoxStateManagerXamlPackage( byte[] content ) : base( new( content ) )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content"> content.</param>
        /// <param name="caretIndex">Insertion position index of the caret.</param>
        /// <param name="selectionStart">Character index for the beginning of the current selection.</param>
        /// <param name="selectionEnd">Number of characters in the current selection.</param>
        public RichTextBoxStateManagerXamlPackage( XamlPackageContainer content, int caretIndex, int selectionStart, int selectionEnd )
            : base( content, caretIndex, selectionStart, selectionEnd )
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
            : base( new( content ), caretIndex, selectionStart, selectionEnd )
        {
        }

        //===========================================================================
        //                           PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc />
        protected override void LoadFromContent( FlowDocument document, XamlPackageContainer value )
        {
            document.LoadFromXamlPackage( value.EncodedData );
        }

        /// <inheritdoc />
        protected override XamlPackageContainer SaveToContent( FlowDocument document )
        {
            return new( document.ToXamlPackage() );
        }
    }
}
