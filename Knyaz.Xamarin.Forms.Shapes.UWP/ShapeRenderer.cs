using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

namespace Knyaz.Xamarin.Forms.Shapes.UWP
{
	public abstract class ShapeRenderer<TElement,TNativeElement> : ViewRenderer<TElement, TNativeElement>
		where TElement : Shape where TNativeElement : Windows.UI.Xaml.Shapes.Shape
	{
		protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
		{
			if (e.OldElement is Shape oldShape)
			{
			oldShape.PropertyChanged += Control_PropertyChanged;
			}

			base.OnElementChanged(e);
			if (e.NewElement is Shape newShape)
			{
				if (Control == null)
				{
					var uwpPath = CreateControl();
					newShape.PropertyChanged += Control_PropertyChanged;
					SetNativeControl(uwpPath);
				}

				Control.StrokeThickness = newShape.StrokeThickness;
				Control.Stroke = new SolidColorBrush(newShape.Stroke.ToWindowsColor());

				Init(e.NewElement);

				UpdateTransform(newShape);
			}
		}

		protected abstract TNativeElement CreateControl();


		virtual protected void Control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var path = (Shape)sender;
			
			if (e.PropertyName == nameof(Path.Stroke))
			{
				Control.Stroke = new SolidColorBrush(path.Stroke.ToWindowsColor());
			}
			else if (e.PropertyName == nameof(Path.StrokeThickness))
			{
				Control.StrokeThickness = path.StrokeThickness;
			}
			else if (e.PropertyName == nameof(RenderTransform))
			{
				UpdateTransform(path);
			}
		}

		protected abstract void Init(TElement element);

		private void UpdateTransform(Shape shape)
		{
			if (shape.RenderTransform != null)
			{
				switch (shape.RenderTransform)
				{
					case TranslateTransform translate:
						Control.RenderTransform = new Windows.UI.Xaml.Media.TranslateTransform()
						{
							X = translate.X,
							Y = translate.Y
						};
						break;
					case ScaleTransform scale:
						Control.RenderTransform = new Windows.UI.Xaml.Media.ScaleTransform()
						{
							ScaleX = scale.ScaleX,
							ScaleY = scale.ScaleY,
							CenterX = scale.CenterX,
							CenterY = scale.CenterY
						};
						break;
				}
			}
		}
	}
}
