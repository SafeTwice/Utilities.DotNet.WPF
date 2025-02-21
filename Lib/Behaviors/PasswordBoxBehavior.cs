/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Utilities.DotNet.WPF.Behaviors
{
    /// <summary>
    /// Behavior to bind the secure password of a <see cref="PasswordBox"/> to a view-model property.
    /// </summary>
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property for the <see cref="SecurePassword"/> property.
        /// </summary>
        public static readonly DependencyProperty SecurePasswordProperty =
            DependencyProperty.Register( nameof( SecurePassword ), typeof( SecureString ), typeof( PasswordBoxBehavior ),
                                         new UIPropertyMetadata( null ) );

        /// <summary>
        /// Gets or sets the secure password of the <see cref="PasswordBox"/>.
        /// </summary>
        public SecureString? SecurePassword
        {
            get { return (SecureString) GetValue( SecurePasswordProperty ); }
            set { SetValue( SecurePasswordProperty, value ); }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if( AssociatedObject != null )
            {
                AssociatedObject.PasswordChanged -= OnPasswordChanged;
            }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnPasswordChanged( object sender, RoutedEventArgs e )
        {
            Debug.Assert( sender is PasswordBox );

            var passwordBox = (PasswordBox) sender;

            SecurePassword = passwordBox.SecurePassword;
        }
    }
}
