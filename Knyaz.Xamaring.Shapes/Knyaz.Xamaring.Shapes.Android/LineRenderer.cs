using Android.Content;
using Android.Graphics;
using Knyaz.Xamaring.Shapes;
using Knyaz.Xamaring.Shapes.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Line), typeof(LineRenderer))]

namespace Knyaz.Xamaring.Shapes.Droid
{
    public class LineRenderer : ViewRenderer<Line, Android.Views.View>
    {
        public LineRenderer(Context context) : base(context) => SetWillNotDraw(false);

        protected override void OnElementChanged(ElementChangedEventArgs<Line> e) => Invalidate();

        protected override void OnDraw(Canvas canvas)
        {
            var rect = new Rect();
            this.GetDrawingRect(rect);

            var paint = new Paint(PaintFlags.AntiAlias);
            paint.StrokeWidth = Element.StrokeThickness;
            paint.StrokeMiter = 10f;
            canvas.Save();
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = Element.Stroke.ToAndroid();
            canvas.DrawLine(Element.X1, Element.Y1, Element.X2, Element.Y2, paint);
            canvas.Restore();
        }
    }
}