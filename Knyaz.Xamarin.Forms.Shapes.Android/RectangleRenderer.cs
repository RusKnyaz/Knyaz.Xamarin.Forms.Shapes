using Android.Content;
using Android.Graphics;
using Knyaz.Xamarin.Forms.Shapes;
using Knyaz.Xamarin.Forms.Shapes.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Rectangle = Knyaz.Xamarin.Forms.Shapes.Rectangle;

[assembly: ExportRenderer(typeof(Rectangle), typeof(RectangleRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.Android
{
	using Path = global::Android.Graphics.Path;

	public class RectangleRenderer : ViewRenderer<Rectangle, global::Android.Views.View>
	{
		public RectangleRenderer(Context context) : base(context) => SetWillNotDraw(false);


		protected override void OnElementChanged(ElementChangedEventArgs<Rectangle> e)
		{
			if (e.OldElement is Rectangle oldRectangle)
			{
				oldRectangle.PropertyChanged -= Rectangle_PropertyChanged;
			}
			if (e.NewElement is Rectangle rectangle)
			{
				rectangle.PropertyChanged += Rectangle_PropertyChanged;
			}
			Invalidate();
		}

		private void Rectangle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
			Invalidate();

		protected override void OnDraw(global::Android.Graphics.Canvas canvas)
		{
			var rect = new Rect();
			GetDrawingRect(rect);

			var halfThickness = Element.StrokeThickness / 2f;

			var RectangleRect = new RectF(
					rect.Left + halfThickness,
					rect.Top + halfThickness,
					rect.Right - halfThickness,
					rect.Bottom - halfThickness);

			var paint = new Paint(PaintFlags.AntiAlias);

			if (Element.Fill.A != 0)
			{
				var circleDotFillPath = new Path();
				circleDotFillPath.AddRect(RectangleRect, Path.Direction.Cw);

				paint.SetStyle(Paint.Style.Fill);
				paint.Color = Element.Fill.ToAndroid();
				canvas.DrawRect(RectangleRect, paint);
			}

			paint.StrokeWidth = Element.StrokeThickness;
			paint.StrokeMiter = 10f;
			canvas.Save();
			paint.SetStyle(Paint.Style.Stroke);
			paint.Color = Element.Stroke.ToAndroid();
			canvas.DrawRect(RectangleRect, paint);
			canvas.Restore();
		}
	}
}