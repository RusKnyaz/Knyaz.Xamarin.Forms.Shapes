using Xamarin.Forms.Platform.UWP;
using Knyaz.Xamarin.Forms.Shapes;
using Knyaz.Xamaring.Shapes.UWP;

[assembly: ExportRenderer(typeof(Ellipse), typeof(EllipseRenderer))]

namespace Knyaz.Xamaring.Shapes.UWP
{
    public class EllipseRenderer : ViewRenderer<Ellipse, Windows.UI.Xaml.Shapes.Ellipse>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {

                if (e.NewElement is Ellipse control)
                {
                    var ellipse = new Windows.UI.Xaml.Shapes.Ellipse();

                    ellipse.StrokeThickness = control.StrokeThickness;
                    ellipse.Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(control.Stroke.ToWindowsColor());
                    ellipse.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
                    SetNativeControl(ellipse);
                }
            }
        }
    }
}
