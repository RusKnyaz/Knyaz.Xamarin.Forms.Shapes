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
			var rect = new Rect();
			GetDrawingRect(rect);

			var path = new global::Android.Graphics.Path();

			foreach (var command in PathDataParser.ToAbsolute(PathDataParser.Parse(Element.Data)))
			{
				switch (command.Type)
				{
					case PathDataParser.CommandType.MoveTo:
						path.MoveTo(
							command.Arguments[0], 
							command.Arguments[1]);
						break;
					case PathDataParser.CommandType.LineTo:
						path.LineTo(command.Arguments[0], command.Arguments[1]);
						break;
					case PathDataParser.CommandType.QBezier:
						path.QuadTo(command.Arguments[0], command.Arguments[1],
							command.Arguments[2], command.Arguments[3]);
						break;
					case PathDataParser.CommandType.Bezier:
						path.CubicTo(
							command.Arguments[0], command.Arguments[1],
							command.Arguments[2], command.Arguments[3],
							command.Arguments[4], command.Arguments[5]);
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

			try
			{
				var paint = new Paint(PaintFlags.AntiAlias);
				canvas.Scale(scale, scale);
				if (Element.Fill.A > 0)
				{
					paint.StrokeWidth = Element.StrokeThickness;
					paint.StrokeMiter = 10f;

					paint.SetStyle(Paint.Style.Fill);
					paint.Color = Element.Fill.ToAndroid();
					canvas.DrawPath(path, paint);
				}

				paint.StrokeWidth = Element.StrokeThickness;
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