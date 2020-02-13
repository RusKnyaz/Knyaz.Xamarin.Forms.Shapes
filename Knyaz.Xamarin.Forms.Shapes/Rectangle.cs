using Xamarin.Forms;

namespace Knyaz.Xamarin.Forms.Shapes
{
	public class Rectangle : Shape
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
	}
}