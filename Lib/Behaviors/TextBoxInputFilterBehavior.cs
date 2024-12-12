/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Utilities.DotNet.WPF.Behaviors
{
    /// <summary>
    /// Behavior to filter the allowed input characters of a <see cref="TextBox"/>.
    /// </summary>
    public class TextBoxInputFilterBehavior : Behavior<TextBox>
    {
        //===========================================================================
        //                          PUBLIC NESTED TYPES
        //===========================================================================

        /// <summary>
        /// Represents a group of characters defined as a regular expression character group pattern
        /// (i.e., the contents between square brackets).
        /// </summary>
        public sealed class RegexCharacterGroup : Regex
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="pattern">Pattern that defines a group of characters.</param>
            public RegexCharacterGroup( string pattern ) : base( $"^[{pattern}]+$" )
            {
            }
        }

        /// <summary>
        /// Provides a type converter to convert <see cref="RegexCharacterGroup"/> objects from other representations.
        /// </summary>
        public class RegexCharacterGroupTypeConverter : TypeConverter
        {
            /// <inheritdoc/>
            public override bool CanConvertFrom( ITypeDescriptorContext? context, Type sourceType )
            {
                return sourceType == typeof( string );
            }

            /// <inheritdoc/>
            public override object? ConvertFrom( ITypeDescriptorContext? context, CultureInfo? culture, object value )
            {
                return new RegexCharacterGroup( (string) value );
            }

            /// <inheritdoc/>
            public override bool CanConvertTo( ITypeDescriptorContext? context, Type? destinationType )
            {
                return false;
            }
        }

        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the <see cref="Filter"/> property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register( nameof( Filter ), typeof( RegexCharacterGroup ), typeof( TextBoxInputFilterBehavior ),
                                         new FrameworkPropertyMetadata( null ) );

        /// <summary>
        /// Gets or sets the regular expression character group pattern that defines the allowed input characters.
        /// </summary>
        [TypeConverter( typeof( RegexCharacterGroupTypeConverter ) )]
        public RegexCharacterGroup? Filter
        {
            get { return (RegexCharacterGroup) GetValue( FilterProperty ); }
            set { SetValue( FilterProperty, value ); }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewTextInput += OnPreviewTextInputEvent;
            DataObject.AddPastingHandler( AssociatedObject, OnPasteEvent );
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if( AssociatedObject != null )
            {
                AssociatedObject.PreviewTextInput -= OnPreviewTextInputEvent;
                DataObject.RemovePastingHandler( AssociatedObject, OnPasteEvent );
            }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnPreviewTextInputEvent( object sender, TextCompositionEventArgs e )
        {
            if( Filter?.IsMatch( e.Text ) == false )
            {
                e.Handled = true;
            }
        }

        private void OnPasteEvent( object sender, DataObjectPastingEventArgs e )
        {
            if( e.DataObject.GetDataPresent( DataFormats.Text ) )
            {
                var pastedText = (string) e.DataObject.GetData( DataFormats.Text );
                if( Filter?.IsMatch( pastedText ) == false )
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
