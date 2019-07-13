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


            var lastPoint = new PointF();

            foreach (var command in PathDataParser.Parse(Element.Data))
            {
                switch (command.Type)
                {
                    case PathDataParser.CommandType.MoveTo:
                        lastPoint = new PointF(command.Arguments[0], command.Arguments[1]);
                        path.MoveTo(lastPoint.X, lastPoint.Y);
                        break;
                    case PathDataParser.CommandType.LineTo:
                        lastPoint = new PointF(command.Arguments[0], command.Arguments[1]);
                        path.LineTo(lastPoint.X, lastPoint.Y);
                        break;
                    case PathDataParser.CommandType.LineHor:
                        lastPoint = new PointF(command.Arguments[0], lastPoint.Y);
                        path.LineTo(lastPoint.X, lastPoint.Y);
                        break;
                    case PathDataParser.CommandType.LineVer:
                        lastPoint = new PointF(lastPoint.X, command.Arguments[0]);
                        path.LineTo(lastPoint.X, lastPoint.Y);
                        break;

                    case PathDataParser.CommandType.RelativeLineHor:
                        lastPoint = new PointF(command.Arguments[0] + lastPoint.X, lastPoint.Y);
                        path.LineTo(lastPoint.X, lastPoint.Y);
                        break;
                    case PathDataParser.CommandType.RelativeLineVer:
                        lastPoint = new PointF(lastPoint.X, lastPoint.Y + command.Arguments[0]);
                        path.LineTo(lastPoint.X, lastPoint.Y);
                        break;

                    case PathDataParser.CommandType.RelativeLineTo:
                        lastPoint = new PointF(lastPoint.X + command.Arguments[0], lastPoint.Y + command.Arguments[1]);
                        path.LineTo(lastPoint.X, lastPoint.Y);
                        break;

                    case PathDataParser.CommandType.RelativeMoveTo:
                        lastPoint = new PointF(lastPoint.X + command.Arguments[0], lastPoint.Y + command.Arguments[1]);
                        path.MoveTo(lastPoint.X, lastPoint.Y);
                        break;

                    case PathDataParser.CommandType.QBezier:
                        path.QuadTo(command.Arguments[0], command.Arguments[1],
                            command.Arguments[2], command.Arguments[3]);
                        lastPoint = new PointF(command.Arguments[2], command.Arguments[3]);
                        break;

                    case PathDataParser.CommandType.Bezier:
                        path.CubicTo(
                            command.Arguments[0], command.Arguments[1],
                            command.Arguments[2], command.Arguments[3],
                            command.Arguments[4], command.Arguments[5]);
                        lastPoint = new PointF(command.Arguments[4], command.Arguments[5]);
                        break;

                    case PathDataParser.CommandType.RelativeBezier:
                        var p1 = new PointF(lastPoint.X + command.Arguments[0], lastPoint.Y + command.Arguments[1]);
                        var p2 = new PointF(p1.X + command.Arguments[2], p1.Y + command.Arguments[3]);
                        lastPoint = new PointF(p2.X + command.Arguments[4], p2.Y + command.Arguments[5]);
                        path.CubicTo(p1.X, p1.Y, p2.X, p2.Y, lastPoint.X, lastPoint.Y);
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