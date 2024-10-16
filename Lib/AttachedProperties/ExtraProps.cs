using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Common properties attached for WPF controls as secondary properties uses. 
    /// </summary>
	/// <remarks>
	/// Used on DefaultErrorTemplate defined in SafeSuite.App\Themes\Styles\TextBoxes.xaml for <see cref="Validation.ErrorTemplateProperty"/>
	/// </remarks>
    /// TODO: Clean and document this class and the DefaultErrorTemplate defined in SafeSuite.App\Themes\Styles\TextBoxes.xaml
    public class ExtraProps
    {
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
                "Margin", typeof( Thickness ), typeof( ExtraProps ), new FrameworkPropertyMetadata( default( Thickness ) ) );

        public static Thickness GetMargin( DependencyObject obj )
        {
            return (Thickness) obj.GetValue( MarginProperty );
        }

        public static void SetMargin( DependencyObject obj, Thickness value )
        {
            obj.SetValue( MarginProperty, value );
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
            "Height", typeof( double ), typeof( ExtraProps ), new FrameworkPropertyMetadata( default( double ) ) );

        public static double GetHeight( DependencyObject obj )
        {
            return (double) obj.GetValue( HeightProperty );
        }

        public static void SetHeight( DependencyObject obj, double value )
        {
            obj.SetValue( HeightProperty, value );
        }


        // Width
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
            "Width", typeof( double ), typeof( ExtraProps ), new FrameworkPropertyMetadata( default( double ) ) );
        public static void SetWidth( DependencyObject obj, double value )
        {
            obj.SetValue( WidthProperty, value );
        }

        public static double GetWidth( DependencyObject obj )
        {
            return (double) obj.GetValue( WidthProperty );
        }

        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached(
            "HorizontalAlignment", typeof( HorizontalAlignment ), typeof( ExtraProps ), new FrameworkPropertyMetadata( HorizontalAlignment.Stretch ) );
        public static void SetHorizontalAlignment( DependencyObject obj, HorizontalAlignment value )
        {
            obj.SetValue( HorizontalAlignmentProperty, value );
        }

        public static HorizontalAlignment GetHorizontalAlignment( DependencyObject obj )
        {
            return (HorizontalAlignment) obj.GetValue( HorizontalAlignmentProperty );
        }

        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached(
            "VerticalAlignment", typeof( VerticalAlignment ), typeof( ExtraProps ), new FrameworkPropertyMetadata( VerticalAlignment.Stretch ) );
        public static void SetVerticalAlignment( DependencyObject obj, VerticalAlignment value )
        {
            obj.SetValue( VerticalAlignmentProperty, value );
        }

        public static VerticalAlignment GetVerticalAlignment( DependencyObject obj )
        {
            return (VerticalAlignment) obj.GetValue( VerticalAlignmentProperty );
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.RegisterAttached(
            "ImageSource", typeof( ImageSource ), typeof( ExtraProps ), new FrameworkPropertyMetadata( default( ImageSource ) ) );
        public static void SetImageSource( DependencyObject obj, ImageSource value )
        {
            obj.SetValue( ImageSourceProperty, value );
        }

        public static ImageSource GetImageSource( DependencyObject obj )
        {
            return (ImageSource) obj.GetValue( ImageSourceProperty );
        }

        public static readonly DependencyProperty Brush1Property = DependencyProperty.RegisterAttached(
            "Brush1", typeof( Brush ), typeof( ExtraProps ), new FrameworkPropertyMetadata( default( Brush ), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );
        public static void SetBrush1( DependencyObject obj, Brush value )
        {
            obj.SetValue( Brush1Property, value );
        }

        public static Brush GetBrush1( DependencyObject obj )
        {

            return (Brush) obj.GetValue( Brush1Property );
        }

        public static readonly DependencyProperty Brush2Property = DependencyProperty.RegisterAttached(
            "Brush2", typeof( Brush ), typeof( ExtraProps ), new FrameworkPropertyMetadata( default( Brush ), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );
        public static void SetBrush2( DependencyObject obj, Brush value )
        {
            obj.SetValue( Brush2Property, value );
        }

        public static Brush GetBrush2( DependencyObject obj )
        {
            return (Brush) obj.GetValue( Brush2Property );
        }

        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
            "Visibility", typeof( Visibility ), typeof( ExtraProps ), new FrameworkPropertyMetadata( Visibility.Visible ) );
        public static void SetVisibility( DependencyObject obj, Visibility value )
        {
            obj.SetValue( VisibilityProperty, value );
        }

        public static Visibility GetVisibility( DependencyObject obj )
        {
            return (Visibility) obj.GetValue( VisibilityProperty );
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.RegisterAttached(
            "Thickness", typeof( Thickness ), typeof( ExtraProps ), new FrameworkPropertyMetadata( new Thickness( 1 ) ) );

        public static void SetThickness( DependencyObject obj, Thickness value )
        {
            obj.SetValue( ThicknessProperty, value );
        }

        public static Thickness GetThickness( DependencyObject obj )
        {
            return (Thickness) obj.GetValue( ThicknessProperty );
        }

        static ExtraProps()
        {

        }
    }
}
