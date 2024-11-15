/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;
using System.Windows.Input;
using Utilities.DotNet.WPF.Extensions;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    using ScrollViewerControl = System.Windows.Controls.ScrollViewer;

    /// <summary>
    /// Provides attached properties to change the behavior of <see cref="ScrollViewerControl">ScrollViewer</see>s.
    /// </summary>
    public static class ScrollViewer
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Dependency property to make scroll viewers ignore scroll wheel events.
        /// </summary>
        /// <remarks>
        /// When this property is attached to a non-<see cref="ScrollViewerControl">ScrollViewer</see> control, it will be
        /// applied to the first <see cref="ScrollViewerControl">ScrollViewer</see> visual descendant of the control.
        /// </remarks>
        public static readonly DependencyProperty IgnoreScrollWheelProperty =
            DependencyProperty.RegisterAttached( "IgnoreScrollWheel", typeof( bool ), typeof( ScrollViewer ),
                new FrameworkPropertyMetadata( false, OnIgnoreScrollWheelPropertyChanged ) );

        /// <summary>
        /// Sets the value of the IgnoreScrollWheel property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The new value for the IgnoreScrollWheel property.</param>
        public static void SetIgnoreScrollWheel( DependencyObject obj, bool value )
        {
            obj.SetValue( IgnoreScrollWheelProperty, value );
        }

        /// <summary>
        /// Gets the value of the IgnoreScrollWheel property.
        /// </summary>
        /// <param name="obj">The dependency object from which to get the property.</param>
        /// <returns>The current value of the IgnoreScrollWheel property.</returns>
        public static bool GetIgnoreScrollWheel( DependencyObject obj )
        {
            return (bool) obj.GetValue( IgnoreScrollWheelProperty );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static void OnIgnoreScrollWheelPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if( ( e.NewValue is bool newValue ) && ( e.OldValue is bool oldValue ) && ( newValue != oldValue ) )
            {
                if( ( d is FrameworkElement fe ) && !fe.IsLoaded )
                {
                    if( newValue )
                    {
                        fe.Loaded += IgnoreScrollWheel_OnLoadedEvent;
                    }
                }
                else
                {
                    var scrollViewer = d.GetFirstVisualDescendant<ScrollViewerControl>();
                    if( scrollViewer != null )
                    {
                        if( newValue )
                        {
                            scrollViewer.PreviewMouseWheel += IgnoreScrollWheel_OnPreviewMouseWheelEvent;
                        }
                        else
                        {
                            scrollViewer.PreviewMouseWheel -= IgnoreScrollWheel_OnPreviewMouseWheelEvent;
                        }
                    }
                }
            }
        }

        private static void IgnoreScrollWheel_OnLoadedEvent( object sender, RoutedEventArgs e )
        {
            if( sender is FrameworkElement fe )
            {
                fe.Loaded -= IgnoreScrollWheel_OnLoadedEvent;

                var scrollViewer = fe.GetFirstVisualDescendant<ScrollViewerControl>();
                if( scrollViewer != null )
                {
                    scrollViewer.PreviewMouseWheel += IgnoreScrollWheel_OnPreviewMouseWheelEvent;
                }
            }
        }

        private static void IgnoreScrollWheel_OnPreviewMouseWheelEvent( object sender, MouseWheelEventArgs e )
        {
            if( ( sender is ScrollViewerControl scrollViewer ) && !e.Handled )
            {
                e.Handled = true;

                var eventArg = new MouseWheelEventArgs( e.MouseDevice, e.Timestamp, e.Delta );
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;

                ( scrollViewer.Parent as UIElement )?.RaiseEvent( eventArg );
            }
        }
    }
}
