// Based on https://gist.github.com/winkel/9022398

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace Utilities.DotNet.WPF.ControlHints
{
    /// <summary>
    /// Provides the Hint attached property to display a hint on empty controls.
    /// </summary>
    public static class HintService
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached(
           "Hint", typeof( object ), typeof( HintService ),
           new FrameworkPropertyMetadata( null, new PropertyChangedCallback( OnHintChanged ) ) );

        public static object GetHint( DependencyObject d )
        {
            return d.GetValue( HintProperty );
        }

        public static void SetHint( DependencyObject d, object value )
        {
            d.SetValue( HintProperty, value );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        /// <summary>
        /// Handles changes to the Hint property.
        /// </summary>
        /// <param name="d">Dependency object that fired the event.</param>
        /// <param name="e">Event data.</param>
        private static void OnHintChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            Control control = (Control) d;
            control.Loaded += OnGenericRoutedEvent;

            if( d is TextBox tb )
            {
                control.GotKeyboardFocus += OnGenericRoutedEvent;
                control.LostKeyboardFocus += OnGenericRoutedEvent;
                tb.TextChanged += OnGenericRoutedEvent;
            }
            else if( d is PasswordBox pb )
            {
                control.GotKeyboardFocus += OnGenericRoutedEvent;
                control.LostKeyboardFocus += OnGenericRoutedEvent;
                pb.PasswordChanged += OnGenericRoutedEvent;
            }
            else if( d is ComboBox cb )
            {
                control.GotKeyboardFocus += OnGenericRoutedEvent;
                control.LostKeyboardFocus += OnGenericRoutedEvent;
                cb.SelectionChanged += OnSelectionChanged;
            }
            else if( d is ItemsControl ic )
            {
                // for Items property  
                ic.ItemContainerGenerator.ItemsChanged += OnItemsChanged;
                m_itemsControls.Add( ic.ItemContainerGenerator, ic );

                // for ItemsSource property  
                DependencyPropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty( ItemsControl.ItemsSourceProperty, ic.GetType() );
                prop.AddValueChanged( ic, OnItemsSourceChanged );
            }
        }

        #region Event Handlers

        /// <summary>
        /// Handle various events on the control
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        private static void OnGenericRoutedEvent( object sender, RoutedEventArgs e )
        {
            UpdateAdornerVisibility( (Control) sender );
        }

        /// <summary>
        /// Event handler for the selection changed event
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        private static void OnSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            UpdateAdornerVisibility( (Control) sender );
        }

        /// <summary>
        /// Event handler for the items source changed event
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        private static void OnItemsSourceChanged( object? sender, EventArgs e )
        {
            UpdateAdornerVisibility( sender as Control );
        }

        /// <summary>
        /// Event handler for the items changed event
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="ItemsChangedEventArgs"/> that contains the event data.</param>
        private static void OnItemsChanged( object sender, ItemsChangedEventArgs e )
        {
            ItemsControl? control;
            if( m_itemsControls.TryGetValue( sender, out control ) )
            {
                UpdateAdornerVisibility( control );
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Update the adorner on the specified element.
        /// </summary>
        /// <param name="control">Control to update the adorner visibility on.</param>
        private static void UpdateAdornerVisibility( Control? control )
        {
            if( control == null )
            {
                return;
            }

            if( CalculateAdornerVisibility( control ) )
            {
                AddAdorner( control );
            }
            else
            {
                RemoveAdorner( control );
            }
        }

        /// <summary>
        /// Indicates whether or not the adorner should be shown on the specified control.
        /// </summary>
        /// <param name="control"><see cref="Control"/> to test.</param>
        /// <returns><c>true</c> if the adorner should be displayed; <c>false</c> otherwise.</returns>
        private static bool CalculateAdornerVisibility( Control control )
        {
            if( control is ComboBox cb )
            {
                return cb.SelectedItem == null;
            }
            else if( control is TextBox tb )
            {
                return tb.Text == string.Empty;
            }
            else if( control is PasswordBox pb )
            {
                return pb.Password == string.Empty;
            }
            else if( control is ItemsControl ic )
            {
                return ic.Items.Count == 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add the adorner to the specified control.
        /// </summary>
        /// <param name="control">Control to add the adorner to.</param>
        private static void AddAdorner( Control control )
        {
            var adorner = GetHintAdorner( control );
            if( adorner != null )
            {
                return;
            }

            AdornerLayer layer = AdornerLayer.GetAdornerLayer( control );

            // layer could be null if control is no longer in the visual tree
            if( layer != null )
            {
                adorner = new HintAdorner( control, GetHint( control ) );

                layer.Add( adorner );

                SetHintAdorner( control, adorner );
            }
        }

        /// <summary>
        /// Remove the adorner from the specified control.
        /// </summary>
        /// <param name="control">Control to remove the adorner from.</param>
        private static void RemoveAdorner( Control control )
        {
            var adorner = GetHintAdorner( control );
            if( adorner == null )
            {
                return;
            }

            AdornerLayer layer = AdornerLayer.GetAdornerLayer( control );

            if( layer != null )
            {
                adorner.Visibility = Visibility.Hidden;

                layer.Remove( adorner );
            }

            SetHintAdorner( control, null );
        }

        #endregion

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private static readonly DependencyProperty HintAdornerProperty = DependencyProperty.RegisterAttached(
            "HintAdorner", typeof( HintAdorner ), typeof( HintService ) );

        private static HintAdorner? GetHintAdorner( DependencyObject d )
        {
            return (HintAdorner?) d.GetValue( HintAdornerProperty );
        }

        private static void SetHintAdorner( DependencyObject d, HintAdorner? value )
        {
            d.SetValue( HintAdornerProperty, value );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        /// <summary>
        /// Dictionary of ItemsControls
        /// </summary>
        private static readonly Dictionary<object, ItemsControl> m_itemsControls = new Dictionary<object, ItemsControl>();
    }
}
