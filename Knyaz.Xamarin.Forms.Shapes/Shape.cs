using Xamarin.Forms;

namespace Knyaz.Xamarin.Forms.Shapes
{
	using Point = System.Drawing.PointF;

	public class Shape : View
	{
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(Shape), Color.Black);

		/// <summary>
		/// Stroke color
		/// </summary>
		public Color Stroke
		{
			get => (Color)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		public static readonly BindableProperty StrokeThicknessProperty =
			BindableProperty.Create(nameof(StrokeThickness), typeof(float), typeof(Shape), 1.0f);

		public float StrokeThickness
		{
			get => (float)GetValue(StrokeThicknessProperty);
			set => SetValue(StrokeThicknessProperty, value);
		}

		public static readonly BindableProperty TransformProperty =
			BindableProperty.Create(nameof(RenderTransform), typeof(Transform), typeof(Shape), null);

		public Transform RenderTransform
		{
			get => (Transform)GetValue(TransformProperty);
			set => SetValue(TransformProperty, value);
		}
	}

	public abstract class Transform : BindableObject
	{
		public abstract Point TransformPoint(Point pt);
	}

	public sealed class TranslateTransform : Transform
	{
		public static readonly BindableProperty XProperty =
			BindableProperty.Create(nameof(X), typeof(float), typeof(TranslateTransform), 0f);

		public static readonly BindableProperty YProperty =
			BindableProperty.Create(nameof(Y), typeof(float), typeof(TranslateTransform), 0f);

		public float X
		{
			get => (float)GetValue(XProperty);
			set => SetValue(XProperty, value);
		}
		public float Y
		{
			get => (float)GetValue(YProperty);
			set => SetValue(YProperty, value);
		}

		public override Point TransformPoint(Point pt) => new Point(pt.X + X, pt.Y + X);
	}

	public sealed class ScaleTransform : Transform
	{
		public static readonly BindableProperty CenterXProperty =
			BindableProperty.Create(nameof(CenterX), typeof(float), typeof(ScaleTransform), 0f);

		public static readonly BindableProperty CenterYProperty =
			BindableProperty.Create(nameof(CenterY), typeof(float), typeof(ScaleTransform), 0f);

		public static readonly BindableProperty ScaleXProperty =
			BindableProperty.Create(nameof(ScaleX), typeof(float), typeof(ScaleTransform), 1f);

		public static readonly BindableProperty ScaleYProperty =
			BindableProperty.Create(nameof(ScaleY), typeof(float), typeof(ScaleTransform), 1f);

		public float CenterX
		{
			get => (float)GetValue(CenterXProperty);
			set => SetValue(CenterXProperty, value);
		}
		public float CenterY
		{
			get => (float)GetValue(CenterYProperty);
			set => SetValue(CenterYProperty, value);
		}
		public float ScaleX
		{
			get => (float)GetValue(ScaleXProperty);
			set => SetValue(ScaleXProperty, value);
		}
		public float ScaleY
		{
			get => (float)GetValue(ScaleYProperty);
			set => SetValue(ScaleYProperty, value);
		}

		public override Point TransformPoint(Point pt)
		{
			var zeroPt = new Point(pt.X - CenterX, pt.Y - CenterY);
			var scaledPt = new Point(zeroPt.X * ScaleX, zeroPt.Y * ScaleY);
			return new Point(scaledPt.X + CenterX, scaledPt.Y + CenterY);
		}
	}

	public class IdentityTransform : Transform
	{
		public override Point TransformPoint(Point pt) => pt;
	}
}
