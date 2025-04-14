/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Utilities.DotNet.Observables;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Contains event data for the <see cref="TextBoxStateManager.StateChanged"/> event.
    /// </summary>
    public class TextBoxStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The state of the <see cref="TextBox"/> before the change.
        /// </summary>
        public TextBoxState OldState { get; }

        /// <summary>
        /// The state of the <see cref="TextBox"/> after the change.
        /// </summary>
        public TextBoxState NewState { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="oldState">State before the change.</param>
        /// <param name="newState">State after the change.</param>
        public TextBoxStateChangedEventArgs( TextBoxState oldState, TextBoxState newState )
        {
            OldState = oldState;
            NewState = newState;
        }
    }

    /// <summary>
    /// Delegate for the <see cref="TextBoxStateManager.StateChanged"/> event.
    /// </summary>
    /// <param name="sender"><see cref="TextBoxStateManager"/> where the event handler is attached.</param>
    /// <param name="e">Event data.</param>
    public delegate void TextBoxStateChangedEventHandler( TextBoxStateManager sender, TextBoxStateChangedEventArgs e );

    /// <summary>
    /// Defines an attached property to manage and observe the state of a <see cref="TextBox"/>.
    /// </summary>
    public class TextBoxStateManager : ObservableObjectEx
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the attached Manager property.
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.RegisterAttached( "Manager", typeof( TextBoxStateManager ), typeof( TextBoxStateManager ),
                new PropertyMetadata( null, AttachedManagerChanged ) );

        /// <summary>
        /// Gets the state manager of the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The dependency object to get the value from.</param>
        /// <returns>State of the <see cref="TextBox"/>.</returns>
        public static TextBoxStateManager GetManager( DependencyObject obj )
        {
            return (TextBoxStateManager) obj.GetValue( StateProperty );
        }

        /// <summary>
        /// Sets the state manager of the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="obj">The dependency object to set the value to.</param>
        /// <param name="value">State of the <see cref="TextBox"/>.</param>
        public static void SetManager( DependencyObject obj, TextBoxStateManager value )
        {
            obj.SetValue( StateProperty, value );
        }

        /// <inheritdoc cref="TextBox.Text"/>
        public string Text
        {
            get => m_stateInfo.Text;

            set
            {
                var oldValue = m_stateInfo.Text;

                if( m_textBox != null )
                {
                    m_textBox.Text = value;
                }

                m_stateInfo.Text = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        /// <inheritdoc cref="TextBox.CaretIndex"/>
        public int CaretIndex
        {
            get => m_stateInfo.CaretIndex;

            set
            {
                var oldValue = m_stateInfo.CaretIndex;

                if( m_textBox != null )
                {
                    m_textBox.CaretIndex = value;
                }

                m_stateInfo.CaretIndex = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        /// <inheritdoc cref="TextBox.SelectionStart"/>
        public int SelectionStart
        {
            get => m_stateInfo.SelectionStart;

            set
            {
                var oldValue = m_stateInfo.SelectionStart;

                if( m_textBox != null )
                {
                    m_textBox.SelectionStart = value;
                }

                m_stateInfo.SelectionStart = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        /// <inheritdoc cref="TextBox.SelectionLength"/>
        public int SelectionLength
        {
            get => m_stateInfo.SelectionLength;

            set
            {
                var oldValue = m_stateInfo.SelectionLength;

                if( m_textBox != null )
                {
                    m_textBox.SelectionLength = value;
                }

                m_stateInfo.SelectionLength = value;

                OnPropertyChanged( oldValue, value );
            }
        }

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <summary>
        /// Event raised when the text of the <see cref="TextBox"/> changes.
        /// </summary>
        public event TextBoxStateChangedEventHandler? TextChanged;

        /// <summary>
        /// Event raised when the selection of the <see cref="TextBox"/> changes.
        /// </summary>
        public event TextBoxStateChangedEventHandler? SelectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextBoxStateManager()
        {
            m_stateInfo = new TextBoxState();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text contents.</param>
        public TextBoxStateManager( string text )
        {
            m_stateInfo = new TextBoxState( text, 0, 0, 0 );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text contents.</param>
        /// <param name="caretIndex">Insertion position index of the caret.</param>
        /// <param name="selectionStart">Character index for the beginning of the current selection.</param>
        /// <param name="selectionLength">Number of characters in the current selection.</param>
        public TextBoxStateManager( string text, int caretIndex, int selectionStart, int selectionLength )
        {
            m_stateInfo = new TextBoxState( text, caretIndex, selectionStart, selectionLength );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Sets the state of the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="stateInfo">State of the <see cref="TextBox"/>.</param>
        public void SetState( TextBoxState stateInfo )
        {
            var oldStateInfo = new TextBoxState( m_stateInfo );

            m_stateInfo = stateInfo;

            lock( m_lock )
            {
                var currentTextBox = m_textBox;

                if( currentTextBox != null )
                {
                    m_textBox = null; // Temporally disable triggering of events

                    try
                    {
                        currentTextBox.BeginChange();

                        currentTextBox.Text = m_stateInfo.Text;
                        currentTextBox.CaretIndex = m_stateInfo.CaretIndex;
                        currentTextBox.SelectionStart = m_stateInfo.SelectionStart;
                        currentTextBox.SelectionLength = m_stateInfo.SelectionLength;
                    }
                    finally
                    {
                        currentTextBox.EndChange();

                        m_textBox = currentTextBox; // Re-enabled triggering of events
                    }
                }
            }

            OnPropertyChanged( oldStateInfo.Text, stateInfo.Text, nameof( Text ) );
            OnPropertyChanged( oldStateInfo.CaretIndex, stateInfo.CaretIndex, nameof( CaretIndex ) );
            OnPropertyChanged( oldStateInfo.SelectionStart, stateInfo.SelectionStart, nameof( SelectionStart ) );
            OnPropertyChanged( oldStateInfo.SelectionLength, stateInfo.SelectionLength, nameof( SelectionLength ) );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void AttachedManagerChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            if( obj is not TextBox textBox )
            {
                return;
            }

            if( e.OldValue is TextBoxStateManager oldTextBoxState )
            {
                oldTextBoxState.UnsetTextBox();
            }

            if( e.NewValue is TextBoxStateManager newTextBoxState )
            {
                newTextBoxState.SetTextBox( textBox );
            }
        }

        private void SetTextBox( TextBox textBox )
        {
            lock( m_lock )
            {
                m_textBox = textBox;

                m_textBox.BeginChange();
                try
                {
                    m_textBox.Text = m_stateInfo.Text;
                    m_textBox.CaretIndex = m_stateInfo.CaretIndex;
                    m_textBox.SelectionStart = m_stateInfo.SelectionStart;
                    m_textBox.SelectionLength = m_stateInfo.SelectionLength;
                }
                finally
                {
                    m_textBox.EndChange();
                }

                m_textBox.TextChanged += TextBox_TextChanged;
                m_textBox.SelectionChanged += TextBox_SelectionChanged;
            }
        }

        private void UnsetTextBox()
        {
            lock( m_lock )
            {
                if( m_textBox != null )
                {
                    m_textBox.TextChanged -= TextBox_TextChanged;
                    m_textBox.SelectionChanged -= TextBox_SelectionChanged;

                    m_textBox = null;
                }
            }
        }

        private void TextBox_TextChanged( object sender, RoutedEventArgs e )
        {
            lock( m_lock )
            {
                StateChanged( sender, true );
            }
        }

        private void TextBox_SelectionChanged( object sender, RoutedEventArgs e )
        {
            lock( m_lock )
            {
                StateChanged( sender, false );
            }
        }

        private void StateChanged( object sender, bool textChange )
        {
            if( m_textBox == null )
            {
                return;
            }

            Debug.Assert( ReferenceEquals( sender, m_textBox ) );

            var oldStateInfo = new TextBoxState( m_stateInfo );

            m_stateInfo.Text = m_textBox.Text;
            m_stateInfo.CaretIndex = m_textBox.CaretIndex;
            m_stateInfo.SelectionStart = m_textBox.SelectionStart;
            m_stateInfo.SelectionLength = m_textBox.SelectionLength;

            if( !m_stateInfo.Equals( oldStateInfo ) )
            {
                var eventArgs = new TextBoxStateChangedEventArgs( oldStateInfo, m_stateInfo );

                if( textChange )
                {
                    TextChanged?.Invoke( this, eventArgs );

                    OnPropertyChanged( oldStateInfo.Text, m_stateInfo.Text, nameof( Text ) );
                }
                else
                {
                    SelectionChanged?.Invoke( this, eventArgs );

                    OnPropertyChanged( oldStateInfo.CaretIndex, m_stateInfo.CaretIndex, nameof( CaretIndex ) );
                    OnPropertyChanged( oldStateInfo.SelectionStart, m_stateInfo.SelectionStart, nameof( SelectionStart ) );
                    OnPropertyChanged( oldStateInfo.SelectionLength, m_stateInfo.SelectionLength, nameof( SelectionLength ) );
                }
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly object m_lock = new();

        private TextBox? m_textBox;

        private TextBoxState m_stateInfo;
    }
}
