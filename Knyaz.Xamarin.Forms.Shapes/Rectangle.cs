using Xamarin.Forms;

namespace Knyaz.Xamarin.Forms.Shapes
{
	public class Rectangle : View
	{
		public static readonly BindableProperty FillProperty =
				   BindableProperty.Create(nameof(Fill), typeof(Color), typeof(Ellipse), Color.Transparent);

		/// <summary>
		/// Fill color
		/// </summary>
		public Color Fill
		{
			get { return (Color)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		public static readonly BindableProperty StrokeProperty =
			   BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(Ellipse), Color.Black);

		/// <summary>
		/// Stroke color
		/// </summary>
		public Color Stroke
		{
			get { return (Color)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		public static readonly BindableProperty StrokeThicknessProperty =
			BindableProperty.Create(nameof(StrokeThickness), typeof(float), typeof(Ellipse), 1.0f);

		public float StrokeThickness
		{
			get => (float)GetValue(StrokeThicknessProperty);
			set => SetValue(StrokeThicknessProperty, value);
		}
	}
}