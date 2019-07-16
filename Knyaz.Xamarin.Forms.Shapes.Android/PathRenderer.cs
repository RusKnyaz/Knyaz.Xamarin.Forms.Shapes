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

        protected override void OnElementChanged(ElementChangedEventArgs<Path> e) => Invalidate();

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
                        path.MoveTo(command.Arguments[0], command.Arguments[1]);
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
                        break;
                }
            }

            var paint = new Paint(PaintFlags.AntiAlias);
            paint.StrokeWidth = Element.StrokeThickness;
            paint.StrokeMiter = 10f;
            canvas.Save();
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = Element.Stroke.ToAndroid();
            canvas.DrawPath(path, paint);
            canvas.Restore();
        }
    }
}