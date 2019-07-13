using Xamarin.Forms;

namespace Knyaz.Xamaring.Shapes
{
    public class Line : View
    {
        public static readonly BindableProperty StrokeProperty =
               BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(Line), Color.Black);

        /// <summary>
        /// Stroke color
        /// </summary>
        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(float), typeof(Line), 1.0f);

        public float StrokeThickness
        {
            get => (float)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly BindableProperty X1Property =
            BindableProperty.Create(nameof(X1), typeof(float), typeof(Line), 0.0f);

        public static readonly BindableProperty X2Property =
            BindableProperty.Create(nameof(X2), typeof(float), typeof(Line), 0.0f);

        public static readonly BindableProperty Y1Property =
            BindableProperty.Create(nameof(Y1), typeof(float), typeof(Line), 0.0f);
        public static readonly BindableProperty Y2Property =
            BindableProperty.Create(nameof(Y2), typeof(float), typeof(Line), 0.0f);

        public float X1
        {
            get => (float)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public float X2
        {
            get => (float)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public float Y1
        {
            get => (float)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public float Y2
        {
            get => (float)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }
    }
}
