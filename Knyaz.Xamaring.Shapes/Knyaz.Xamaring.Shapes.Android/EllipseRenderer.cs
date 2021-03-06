﻿using Android.Content;
using Android.Graphics;
using Knyaz.Xamaring.Shapes;
using Knyaz.Xamaring.Shapes.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Ellipse), typeof(EllipseRenderer))]

namespace Knyaz.Xamaring.Shapes.Droid
{
    using Path = Android.Graphics.Path;

    public class EllipseRenderer : ViewRenderer<Ellipse, Android.Views.View>
    {
        public EllipseRenderer(Context context):base(context) => SetWillNotDraw(false);

        protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> e) => Invalidate();

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            var rect = new Rect();
            this.GetDrawingRect(rect);
            Paint paint;

            var halfThickness = Element.StrokeThickness / 2f;

            var ellipseRect = new RectF(
                    rect.Left + halfThickness,
                    rect.Top + halfThickness,
                    rect.Right - halfThickness,
                    rect.Bottom - halfThickness);

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
            canvas.Save();
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = Element.Stroke.ToAndroid();
            canvas.DrawPath(circleDotStrokePath, paint);
            canvas.Restore();
        }
    }
}