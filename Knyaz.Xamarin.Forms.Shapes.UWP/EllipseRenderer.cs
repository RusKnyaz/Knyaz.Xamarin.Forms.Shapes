using Xamarin.Forms.Platform.UWP;
using Knyaz.Xamarin.Forms.Shapes.UWP;

[assembly: ExportRenderer(typeof(Knyaz.Xamarin.Forms.Shapes.Ellipse), typeof(EllipseRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.UWP
{
    public class EllipseRenderer : ShapeRenderer<Ellipse, Windows.UI.Xaml.Shapes.Ellipse>
    {
		protected override Windows.UI.Xaml.Shapes.Ellipse CreateControl() =>
			new Windows.UI.Xaml.Shapes.Ellipse();

		protected override void Init(Ellipse element) => UpdateProperties(element, Control);

		protected override void Control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.Control_PropertyChanged(sender, e);

			var control = (Ellipse)sender;

			if(e.PropertyName == nameof(Ellipse.Fill))
			{
				Control.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
			}
		}

		private static void UpdateProperties(Ellipse control, Windows.UI.Xaml.Shapes.Ellipse ellipse)
		{
			ellipse.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
		}
	}
}
