using Xamarin.Forms.Platform.UWP;
using Knyaz.Xamarin.Forms.Shapes.UWP;

[assembly: ExportRenderer(typeof(Knyaz.Xamarin.Forms.Shapes.Rectangle), typeof(RectangleRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.UWP
{
	public class RectangleRenderer : ShapeRenderer<Rectangle, Windows.UI.Xaml.Shapes.Rectangle>
	{
		protected override Windows.UI.Xaml.Shapes.Rectangle CreateControl() => new Windows.UI.Xaml.Shapes.Rectangle();

		protected override void Init(Rectangle element)
		{
			Control.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(element.Fill.ToWindowsColor());
		}

		protected void Control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.Control_PropertyChanged(sender, e);
			var control = (Rectangle)sender;

			if(e.PropertyName == nameof(Rectangle.Fill))
			{
				Control.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
			}
		}
	}
}
