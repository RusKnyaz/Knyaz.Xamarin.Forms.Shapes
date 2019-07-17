using Android.Content;
using Android.Graphics;
using Knyaz.Xamarin.Forms.Shapes;
using Knyaz.Xamarin.Forms.Shapes.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Ellipse), typeof(EllipseRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.Android
{
    using Path = global::Android.Graphics.Path;

    public class EllipseRenderer : ViewRenderer<Ellipse, global::Android.Views.View>
    {
        public EllipseRenderer(Context context) : base(context) => SetWillNotDraw(false);

		protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> e)
		{
			if (e.OldElement is Ellipse oldEllipse)
			{
				oldEllipse.PropertyChanged -= Ellipse_PropertyChanged;
			}
			
			if(e.NewElement is Ellipse ellipse)
			{
				ellipse.PropertyChanged += Ellipse_PropertyChanged;
			}
			Invalidate();
		}

		private void Ellipse_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
			Invalidate();

		protected override void OnDraw(global::Android.Graphics.Canvas canvas)
        {
            var rect = new Rect();
            GetDrawingRect(rect);
            Paint paint;

            var halfThickness = Context.DpToPixels(Element.StrokeThickness / 2f);

            var ellipseRect = new RectF(
                    rect.Left + halfThickness,
                    rect.Top + halfThickness,
                    rect.Right - halfThickness,
                    rect.Bottom - halfThickness);

			canvas.Save();

			try
			{
				var scale = Context.DpToPixels(1);
				// circleDotFill
				if (Element.Fill.A != 0)
				{
					var circleDotFillPath = new Path();
					circleDotFillPath.AddOval(ellipseRect, Path.Direction.Cw);

					paint = new Paint(PaintFlags.AntiAlias);
					paint.SetStyle(Paint.Style.Fill);
					paint.Color = Element.Fill.ToAndroid();
					canvas.DrawPath(circleDotFillPath, paint);
				}

				// circleDotStroke
				Path circleDotStrokePath = new Path();
				circleDotStrokePath.AddOval(ellipseRect, Path.Direction.Cw);

				paint = new Paint(PaintFlags.AntiAlias);
				paint.StrokeWidth = Element.StrokeThickness;
				paint.StrokeMiter = 10f;
				paint.SetStyle(Paint.Style.Stroke);
				paint.Color = Element.Stroke.ToAndroid();
				canvas.DrawPath(circleDotStrokePath, paint);
			}
			finally
			{
				canvas.Restore();
			}
            
        }
    }
}