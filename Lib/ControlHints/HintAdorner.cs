// Based on https://gist.github.com/winkel/9022398

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Utilities.DotNet.WPF.ControlHints
{
    /// <summary>
    /// Adorner for the hint.
    /// </summary>
    internal class HintAdorner : Adorner
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="adornedElement"><see cref="UIElement"/> to be adorned</param>
        /// <param name="hint">The hint content</param>
        internal HintAdorner( UIElement adornedElement, object hint ) : base( adornedElement )
        {
            IsHitTestVisible = false;

            m_contentPresenter = new ContentPresenter();
            m_contentPresenter.Content = hint;
            m_contentPresenter.Opacity = 0.5;
            m_contentPresenter.Margin = new Thickness( Control.Margin.Left + Control.Padding.Left, Control.Margin.Top + Control.Padding.Top, 0, 0 );

            if( ( Control is ItemsControl ) && !( Control is ComboBox ) )
            {
                m_contentPresenter.VerticalAlignment = VerticalAlignment.Center;
                m_contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            }

            // Hide the control adorner when the adorned element is hidden
            Binding binding = new Binding( "IsVisible" );
            binding.Source = adornedElement;
            binding.Converter = new BooleanToVisibilityConverter();
            SetBinding( VisibilityProperty, binding );
        }

        //===========================================================================
        //                           PROTECTED PROPERTIES
        //===========================================================================

        protected override int VisualChildrenCount => 1;

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected override Visual GetVisualChild( int index )
        {
            return m_contentPresenter;
        }

        protected override Size MeasureOverride( Size constraint )
        {
            // Here's the secret to getting the adorner to cover the whole control
            m_contentPresenter.Measure( Control.RenderSize );
            return Control.RenderSize;
        }

        protected override Size ArrangeOverride( Size finalSize )
        {
            m_contentPresenter.Arrange( new Rect( finalSize ) );
            return finalSize;
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private Control Control
        {
            get { return (Control) AdornedElement; }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly ContentPresenter m_contentPresenter;
    }
}
