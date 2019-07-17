using Xamarin.Forms.Platform.UWP;
using Knyaz.Xamarin.Forms.Shapes;
using Knyaz.Xamarin.Forms.Shapes.UWP;

[assembly: ExportRenderer(typeof(Rectangle), typeof(RectangleRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.UWP
{
	public class RectangleRenderer : ViewRenderer<Rectangle, Windows.UI.Xaml.Shapes.Rectangle>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Rectangle> e)
		{
			if (e.NewElement is Rectangle oldRectangle)
			{
				oldRectangle.PropertyChanged -= Control_PropertyChanged;
			}

			base.OnElementChanged(e);
			if (e.NewElement is Rectangle control) 
			{
				if (Control == null)
				{
					control.PropertyChanged += Control_PropertyChanged;
					SetNativeControl(new Windows.UI.Xaml.Shapes.Rectangle());
				}

				Control.StrokeThickness = control.StrokeThickness;
				Control.Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(control.Stroke.ToWindowsColor());
				Control.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
			}
		}

		private void Control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var control = (Rectangle)sender;

			if(e.PropertyName == nameof(Rectangle.Fill))
			{
				Control.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
			}
			else if (e.PropertyName == nameof(Rectangle.Stroke))
			{
				Control.Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(control.Stroke.ToWindowsColor());
			}
			else if (e.PropertyName == nameof(Rectangle.StrokeThickness))
			{
				Control.StrokeThickness = control.StrokeThickness;
			}
		}
	}
}
