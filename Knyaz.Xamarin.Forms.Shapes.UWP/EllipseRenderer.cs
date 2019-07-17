using Xamarin.Forms.Platform.UWP;
using Knyaz.Xamarin.Forms.Shapes;
using Knyaz.Xamarin.Forms.Shapes.UWP;

[assembly: ExportRenderer(typeof(Ellipse), typeof(EllipseRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.UWP
{
    public class EllipseRenderer : ViewRenderer<Ellipse, Windows.UI.Xaml.Shapes.Ellipse>
    {
		protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> e)
		{
			if(e.OldElement is Ellipse oldContorl)
			{
				oldContorl.PropertyChanged -= Control_PropertyChanged;
			}

			base.OnElementChanged(e);
			if (e.NewElement is Ellipse control)
			{
				if (Control == null)
				{
					var ellipse = new Windows.UI.Xaml.Shapes.Ellipse();
					control.PropertyChanged += Control_PropertyChanged;
					SetNativeControl(ellipse);
				}

				UpdateProperties(control, Control);
			}
		}

		private void Control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var control = (Ellipse)sender;

			if(e.PropertyName == nameof(Ellipse.Fill))
			{
				Control.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
			}
			else if (e.PropertyName == nameof(Ellipse.Stroke))
			{
				Control.Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(control.Stroke.ToWindowsColor());
			}
			else if (e.PropertyName == nameof(Ellipse.StrokeThickness))
			{
				Control.StrokeThickness = control.StrokeThickness;
			}
		}

		private static void UpdateProperties(Ellipse control, Windows.UI.Xaml.Shapes.Ellipse ellipse)
		{
			ellipse.StrokeThickness = control.StrokeThickness;
			ellipse.Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(control.Stroke.ToWindowsColor());
			ellipse.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
		}
	}
}
