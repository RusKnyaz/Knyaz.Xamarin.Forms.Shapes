using Android.Content;
using Android.Graphics;
using Knyaz.Xamarin.Forms.Shapes.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Knyaz.Xamarin.Forms.Shapes.Path), typeof(PathRenderer))]

namespace Knyaz.Xamarin.Forms.Shapes.Android
{
    public class PathRenderer : ViewRenderer<Path, global::Android.Views.View>
    {
        public PathRenderer(Context context) : base(context) => SetWillNotDraw(false);

		protected override void OnElementChanged(ElementChangedEventArgs<Path> e)
		{
			if (e.OldElement is Path oldPath)
			{
				oldPath.PropertyChanged -= Path_PropertyChanged;
			}
			if (e.NewElement is Path path)
			{
				path.PropertyChanged += Path_PropertyChanged;
			}
			Invalidate();
		}

		private void Path_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
			Invalidate();

		protected override void OnDraw(Canvas canvas)
		{
			var trans = Element.RenderTransform ?? new IdentityTransform();

			var path = new global::Android.Graphics.Path();

			foreach (var command in PathDataParser.ToAbsolute(PathDataParser.Parse(Element.Data)))
			{
				switch (command.Type)
				{
					case PathDataParser.CommandType.MoveTo:
						{
							var pt = trans.TransformPoint(
								new System.Drawing.PointF(command.Arguments[0], command.Arguments[1]));
							path.MoveTo(pt.X, pt.Y);
						}
						break;
					case PathDataParser.CommandType.LineTo:
						{
							var pt = trans.TransformPoint(
									new System.Drawing.PointF(command.Arguments[0], command.Arguments[1]));
							path.LineTo(pt.X, pt.Y);
						}
						break;
					case PathDataParser.CommandType.QBezier:
						{
							var pt = trans.TransformPoint(
									new System.Drawing.PointF(command.Arguments[0], command.Arguments[1]));
							var pt2 = trans.TransformPoint(
									new System.Drawing.PointF(command.Arguments[2], command.Arguments[3]));

							path.QuadTo(pt.X, pt.Y, pt2.X, pt2.Y);
						}
						
						break;
					case PathDataParser.CommandType.Bezier:
						{
							var pt = trans.TransformPoint(
									new System.Drawing.PointF(command.Arguments[0], command.Arguments[1]));
							var pt2 = trans.TransformPoint(
									new System.Drawing.PointF(command.Arguments[2], command.Arguments[3]));
							var pt3 = trans.TransformPoint(
									new System.Drawing.PointF(command.Arguments[4], command.Arguments[5]));

							path.CubicTo(pt.X, pt.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
						}
						break;
					case PathDataParser.CommandType.Close:
						path.Close();
						DrawPath(canvas, path);
						path = new global::Android.Graphics.Path();
						break;
				}
			}

			if(!path.IsEmpty)
				DrawPath(canvas, path);
		}
		


		private void DrawPath(Canvas canvas, global::Android.Graphics.Path path)
		{
			var scale = Context.DpToPixels(1);

			canvas.Save();

			var strokeThickness = Element.StrokeThickness;

			if(Element.RenderTransform is ScaleTransform scaleTransform)
			{
				strokeThickness = System.Math.Min(scaleTransform.ScaleX, scaleTransform.ScaleY);
			}

			try
			{
				var paint = new Paint(PaintFlags.AntiAlias);
				canvas.Scale(scale, scale);
				if (Element.Fill.A > 0)
				{
					paint.StrokeWidth = strokeThickness;
					paint.StrokeMiter = 10f;

					paint.SetStyle(Paint.Style.Fill);
					paint.Color = Element.Fill.ToAndroid();
					canvas.DrawPath(path, paint);
				}

				paint.StrokeWidth = strokeThickness;
				paint.StrokeMiter = 10f;
				paint.SetStyle(Paint.Style.Stroke);
				paint.Color = Element.Stroke.ToAndroid();
				canvas.DrawPath(path, paint);
			}
			finally
			{
				canvas.Restore();
			}
		}
	}
}