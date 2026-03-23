/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
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
    public sealed class RichTextBoxStateManagerRtf : RichTextBoxStateManagerBase<string>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the attached Manager property.
        /// </summary>
        public static readonly DependencyProperty ManagerProperty =
        DependencyProperty.RegisterAttached( "Manager", typeof( RichTextBoxStateManagerRtf ), typeof( RichTextBoxStateManagerRtf ),
                                             new PropertyMetadata( null, AttachedManagerChanged ) );

        /// <summary>
        /// Gets the state manager of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="obj">The dependency object to get the value from.</param>
        /// <returns>State of the <see cref="RichTextBox"/>.</returns>
        [Browsable( false )]
        public static RichTextBoxStateManagerRtf GetManager( RichTextBox obj )
        {
            return (RichTextBoxStateManagerRtf) obj.GetValue( ManagerProperty );
        }

        /// <summary>
        /// Sets the state manager of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="obj">The dependency object to set the value to.</param>
        /// <param name="value">State of the <see cref="RichTextBox"/>.</param>
        public static void SetManager( RichTextBox obj, RichTextBoxStateManagerRtf value )
        {
            obj.SetValue( ManagerProperty, value );
        }

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
        /// <param name="document">Flow document.</param>
        public RichTextBoxStateManagerRtf( FlowDocument document ) : base( document.ToRtf() )
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
        /// <param name="selectionEnd">Number of characters in the current selection.</param>
        public RichTextBoxStateManagerRtf( string content, int caretIndex, int selectionStart, int selectionEnd )
            : base( content, caretIndex, selectionStart, selectionEnd )
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
