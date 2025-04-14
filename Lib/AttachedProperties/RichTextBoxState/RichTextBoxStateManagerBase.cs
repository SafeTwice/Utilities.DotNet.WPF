/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Utilities.DotNet.Observables;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Contains event data for the <see cref="RichTextBoxStateManagerBase{TContent}.StateChanged"/> event.
    /// </summary>
    public class RichTextBoxStateChangedEventArgs<TContent> : EventArgs where TContent : notnull
    {
        /// <summary>
        /// The state of the <see cref="RichTextBox"/> before the change.
        /// </summary>
        public RichTextBoxState<TContent> OldState { get; }

        /// <summary>
        /// The state of the <see cref="RichTextBox"/> after the change.
        /// </summary>
        public RichTextBoxState<TContent> NewState { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="oldState">State before the change.</param>
        /// <param name="newState">State after the change.</param>
        public RichTextBoxStateChangedEventArgs( RichTextBoxState<TContent> oldState, RichTextBoxState<TContent> newState )
        {
            OldState = oldState;
            NewState = newState;
        }
    }

    /// <summary>
    /// Delegate for the <see cref="RichTextBoxStateManagerBase{TContent}.StateChanged"/> event.
    /// </summary>
    /// <param name="sender"><see cref="RichTextBoxStateManagerBase{TContent}"/> where the event handler is attached.</param>
    /// <param name="e">Event data.</param>
    public delegate void RichTextBoxStateChangedEventHandler<T>( RichTextBoxStateManagerBase<T> sender, RichTextBoxStateChangedEventArgs<T> e )
        where T : notnull;

    /// <summary>
    /// Defines an attached property to manage and observe the state of a <see cref="RichTextBox"/>.
    /// </summary>
    public abstract class RichTextBoxStateManagerBase<TContent> : ObservableObjectEx where TContent : notnull
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the attached Manager property.
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.RegisterAttached( "Manager", typeof( RichTextBoxStateManagerBase<TContent> ), typeof( RichTextBoxStateManagerBase<TContent> ),
                new PropertyMetadata( null, AttachedManagerChanged ) );

        /// <summary>
        /// Gets the state manager of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="obj">The dependency object to get the value from.</param>
        /// <returns>State of the <see cref="RichTextBox"/>.</returns>
        public static RichTextBoxStateManagerBase<TContent> GetManager( DependencyObject obj )
        {
            return (RichTextBoxStateManagerBase<TContent>) obj.GetValue( StateProperty );
        }

        /// <summary>
        /// Sets the state manager of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="obj">The dependency object to set the value to.</param>
        /// <param name="value">State of the <see cref="RichTextBox"/>.</param>
        public static void SetManager( DependencyObject obj, RichTextBoxStateManagerBase<TContent> value )
        {
            obj.SetValue( StateProperty, value );
        }

        /// <summary>
        /// Rich text content in string format.
        /// </summary>
        public TContent Content
        {
            get => m_stateInfo.Content;
            set
            {
                var oldValue = m_stateInfo.Content;

                if( m_richTextBox != null )
                {
                    LoadFromContent( m_richTextBox.Document, value );
                }

                m_stateInfo.Content = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        /// <inheritdoc cref="RichTextBoxState{TContent}.CaretIndex"/>
        public int CaretIndex
        {
            get => m_stateInfo.CaretIndex;
            set
            {
                var oldValue = m_stateInfo.CaretIndex;

                if( m_richTextBox != null )
                {
                    m_richTextBox.CaretPosition = m_richTextBox.Document.ContentStart.GetPositionAtOffset( value );
                }

                m_stateInfo.CaretIndex = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        /// <inheritdoc cref="RichTextBoxState{TContent}.SelectionStart"/>
        public int SelectionStart
        {
            get => m_stateInfo.SelectionStart;
            set
            {
                var oldValue = m_stateInfo.SelectionStart;

                if( m_richTextBox != null )
                {
                    TextPointer start = m_richTextBox.Document.ContentStart.GetPositionAtOffset( value );
                    m_richTextBox.Selection.Select( start, m_richTextBox.Selection.End );
                }

                m_stateInfo.SelectionStart = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        /// <inheritdoc cref="RichTextBoxState{TContent}.SelectionEnd"/>
        public int SelectionEnd
        {
            get => m_stateInfo.SelectionEnd;
            set
            {
                var oldValue = m_stateInfo.SelectionEnd;

                if( m_richTextBox != null )
                {
                    TextPointer end = m_richTextBox.Document.ContentStart.GetPositionAtOffset( value );
                    m_richTextBox.Selection.Select( m_richTextBox.Selection.Start, end );
                }

                m_stateInfo.SelectionEnd = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <summary>
        /// Event raised when the text of the <see cref="RichTextBox"/> changes.
        /// </summary>
        public event RichTextBoxStateChangedEventHandler<TContent>? TextChanged;

        /// <summary>
        /// Event raised when the selection of the <see cref="RichTextBox"/> changes.
        /// </summary>
        public event RichTextBoxStateChangedEventHandler<TContent>? SelectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected RichTextBoxStateManagerBase()
        {
            m_stateInfo = new RichTextBoxState<TContent>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Rich text content.</param>
        protected RichTextBoxStateManagerBase( TContent content )
        {
            m_stateInfo = new RichTextBoxState<TContent>( content, 0, 0, 0 );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Content.</param>
        /// <param name="caretIndex">Insertion position index of the caret.</param>
        /// <param name="selectionStart">Character index for the beginning of the current selection.</param>
        /// <param name="selectionEnd">Number of characters in the current selection.</param>
        protected RichTextBoxStateManagerBase( TContent content, int caretIndex, int selectionStart, int selectionEnd )
        {
            m_stateInfo = new RichTextBoxState<TContent>( content, caretIndex, selectionStart, selectionEnd );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Sets the state of the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="stateInfo">State information.</param>
        public void SetState( RichTextBoxState<TContent> stateInfo )
        {
            var oldStateInfo = new RichTextBoxState<TContent>( m_stateInfo );

            m_stateInfo = stateInfo;

            lock( m_lock )
            {
                var currentRichTextBox = m_richTextBox;

                if( currentRichTextBox != null )
                {
                    m_richTextBox = null; // Temporarily disable triggering of events

                    currentRichTextBox.BeginChange();
                    try
                    {
                        LoadFromContent( currentRichTextBox.Document, m_stateInfo.Content );

                        TextPointer contentStart = currentRichTextBox.Document.ContentStart;
                        TextPointer caret = contentStart.GetPositionAtOffset( m_stateInfo.CaretIndex );
                        TextPointer selectionStart = contentStart.GetPositionAtOffset( m_stateInfo.SelectionStart );
                        TextPointer selectionEnd = contentStart.GetPositionAtOffset( m_stateInfo.SelectionEnd );

                        if( caret != null )
                        {
                            currentRichTextBox.CaretPosition = caret;
                        }

                        if( ( selectionStart != null ) && ( selectionEnd != null ) )
                        {
                            currentRichTextBox.Selection.Select( selectionStart, selectionEnd );
                        }
                    }
                    finally
                    {
                        currentRichTextBox.EndChange();
                    }

                    m_richTextBox = currentRichTextBox; // Re-enable triggering of events
                }
            }

            OnPropertyChanged( oldStateInfo.Content, stateInfo.Content, nameof( Content ) );
            OnPropertyChanged( oldStateInfo.CaretIndex, stateInfo.CaretIndex, nameof( CaretIndex ) );
            OnPropertyChanged( oldStateInfo.SelectionStart, stateInfo.SelectionStart, nameof( SelectionStart ) );
            OnPropertyChanged( oldStateInfo.SelectionEnd, stateInfo.SelectionEnd, nameof( SelectionEnd ) );
        }

        //===========================================================================
        //                           PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Loads the content into the specified FlowDocument.
        /// </summary>
        /// <param name="document">The FlowDocument to load the content into.</param>
        /// <param name="value">The content to load into the FlowDocument.</param>
        protected abstract void LoadFromContent( FlowDocument document, TContent value );

        /// <summary>
        /// Saves the content into the specified FlowDocument.
        /// </summary>
        /// <param name="document">The FlowDocument to save the content into.</param>
        protected abstract TContent SaveToContent( FlowDocument document );

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void AttachedManagerChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            if( obj is not RichTextBox richTextBox )
            {
                return;
            }

            if( e.OldValue is RichTextBoxStateManagerBase<TContent> oldRichTextBoxState )
            {
                oldRichTextBoxState.UnsetRichTextBox();
            }

            if( e.NewValue is RichTextBoxStateManagerBase<TContent> newRichTextBoxState )
            {
                newRichTextBoxState.SetRichTextBox( richTextBox );
            }
        }

        private void SetRichTextBox( RichTextBox richTextBox )
        {
            lock( m_lock )
            {
                m_richTextBox = richTextBox;

                m_richTextBox.BeginChange();
                try
                {
                    LoadFromContent( m_richTextBox.Document, m_stateInfo.Content );

                    TextPointer contentStart = m_richTextBox.Document.ContentStart;
                    TextPointer caret = contentStart.GetPositionAtOffset( m_stateInfo.CaretIndex );
                    TextPointer selectionStart = contentStart.GetPositionAtOffset( m_stateInfo.SelectionStart );
                    TextPointer selectionEnd = contentStart.GetPositionAtOffset( m_stateInfo.SelectionEnd );

                    if( caret != null )
                    {
                        m_richTextBox.CaretPosition = caret;
                    }

                    if( ( selectionStart != null ) && ( selectionEnd != null ) )
                    {
                        m_richTextBox.Selection.Select( selectionStart, selectionEnd );
                    }
                }
                finally
                {
                    m_richTextBox.EndChange();
                }

                m_richTextBox.TextChanged += RichTextBox_TextChanged;
                m_richTextBox.SelectionChanged += RichTextBox_SelectionChanged;
            }
        }

        private void UnsetRichTextBox()
        {
            lock( m_lock )
            {
                if( m_richTextBox != null )
                {
                    m_richTextBox.TextChanged -= RichTextBox_TextChanged;
                    m_richTextBox.SelectionChanged -= RichTextBox_SelectionChanged;

                    m_richTextBox = null;
                }
            }
        }

        private void RichTextBox_TextChanged( object sender, RoutedEventArgs e )
        {
            lock( m_lock )
            {
                StateChanged( sender, true );
            }
        }

        private void RichTextBox_SelectionChanged( object sender, RoutedEventArgs e )
        {
            lock( m_lock )
            {
                StateChanged( sender, false );
            }
        }

        private void StateChanged( object sender, bool textChange )
        {
            if( m_richTextBox == null )
            {
                return;
            }

            Debug.Assert( ReferenceEquals( sender, m_richTextBox ) );

            var oldStateInfo = new RichTextBoxState<TContent>( m_stateInfo );

            m_stateInfo.Content = SaveToContent( m_richTextBox.Document );

            TextPointer contentStart = m_richTextBox.Document.ContentStart;
            m_stateInfo.CaretIndex = contentStart.GetOffsetToPosition( m_richTextBox.CaretPosition );
            m_stateInfo.SelectionStart = contentStart.GetOffsetToPosition( m_richTextBox.Selection.Start );
            m_stateInfo.SelectionEnd = contentStart.GetOffsetToPosition( m_richTextBox.Selection.End );

            if( !m_stateInfo.Equals( oldStateInfo ) )
            {
                var eventArgs = new RichTextBoxStateChangedEventArgs<TContent>( oldStateInfo, m_stateInfo );

                if( textChange )
                {
                    TextChanged?.Invoke( this, eventArgs );
                    OnPropertyChanged( oldStateInfo.Content, m_stateInfo.Content, nameof( Content ) );
                }
                else
                {
                    SelectionChanged?.Invoke( this, eventArgs );
                    OnPropertyChanged( oldStateInfo.CaretIndex, m_stateInfo.CaretIndex, nameof( CaretIndex ) );
                    OnPropertyChanged( oldStateInfo.SelectionStart, m_stateInfo.SelectionStart, nameof( SelectionStart ) );
                    OnPropertyChanged( oldStateInfo.SelectionEnd, m_stateInfo.SelectionEnd, nameof( SelectionEnd ) );
                }
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly object m_lock = new();
        private RichTextBox? m_richTextBox;
        private RichTextBoxState<TContent> m_stateInfo;
    }
}
