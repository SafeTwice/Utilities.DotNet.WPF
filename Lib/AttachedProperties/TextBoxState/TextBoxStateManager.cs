/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    public class TextBoxStateManager : INotifyPropertyChanged
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
                if( m_textBox != null )
                {
                    m_textBox.Text = value;
                }

                m_stateInfo.Text = value;

                OnPropertyChanged();
            }
        }

        /// <inheritdoc cref="TextBox.CaretIndex"/>
        public int CaretIndex
        {
            get => m_stateInfo.CaretIndex;

            set
            {
                if( m_textBox != null )
                {
                    m_textBox.CaretIndex = value;
                }

                m_stateInfo.CaretIndex = value;

                OnPropertyChanged();
            }
        }

        /// <inheritdoc cref="TextBox.SelectionStart"/>
        public int SelectionStart
        {
            get => m_stateInfo.SelectionStart;

            set
            {
                if( m_textBox != null )
                {
                    m_textBox.SelectionStart = value;
                }

                m_stateInfo.SelectionStart = value;

                OnPropertyChanged();
            }
        }

        /// <inheritdoc cref="TextBox.SelectionLength"/>
        public int SelectionLength
        {
            get => m_stateInfo.SelectionLength;

            set
            {
                if( m_textBox != null )
                {
                    m_textBox.SelectionLength = value;
                }

                m_stateInfo.SelectionLength = value;

                OnPropertyChanged();
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

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

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

        public void SetState( TextBoxState stateInfo )
        {
            m_stateInfo = stateInfo;

            var textBox = m_textBox;

            if( textBox != null )
            {
                m_textBox = null; // Temporally disable triggering of events

                textBox.Text = m_stateInfo.Text;
                textBox.CaretIndex = m_stateInfo.CaretIndex;
                textBox.SelectionStart = m_stateInfo.SelectionStart;
                textBox.SelectionLength = m_stateInfo.SelectionLength;

                m_textBox = textBox; // Re-enabled triggering of events
            }

            OnPropertyChanged( nameof( Text ) );
            OnPropertyChanged( nameof( CaretIndex ) );
            OnPropertyChanged( nameof( SelectionStart ) );
            OnPropertyChanged( nameof( SelectionLength ) );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void AttachedManagerChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            var textBox = obj as TextBox;
            if( textBox == null )
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
            m_textBox = textBox;

            m_textBox.Text = m_stateInfo.Text;
            m_textBox.CaretIndex = m_stateInfo.CaretIndex;
            m_textBox.SelectionStart = m_stateInfo.SelectionStart;
            m_textBox.SelectionLength = m_stateInfo.SelectionLength;

            m_textBox.TextChanged += TextBox_TextChanged;
            m_textBox.SelectionChanged += TextBox_SelectionChanged;
        }

        private void UnsetTextBox()
        {
            if( m_textBox != null )
            {
                m_textBox.TextChanged -= TextBox_TextChanged;
                m_textBox.SelectionChanged -= TextBox_SelectionChanged;

                m_textBox = null;
            }
        }

        private void TextBox_TextChanged( object sender, RoutedEventArgs e )
        {
            StateChanged( sender, e, true );
        }

        private void TextBox_SelectionChanged( object sender, RoutedEventArgs e )
        {
            StateChanged( sender, e, false );
        }

        private void StateChanged( object sender, RoutedEventArgs e, bool textChange )
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

                    OnPropertyChanged( nameof( Text ) );
                }
                else
                {
                    SelectionChanged?.Invoke( this, eventArgs );

                    OnPropertyChanged( nameof( CaretIndex ) );
                    OnPropertyChanged( nameof( SelectionStart ) );
                    OnPropertyChanged( nameof( SelectionLength ) );
                }
            }
        }

        private void OnPropertyChanged( [CallerMemberName] string propertyName = "" )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private TextBox? m_textBox;

        private TextBoxState m_stateInfo;
    }
}
