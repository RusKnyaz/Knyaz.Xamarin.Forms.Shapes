using Knyaz.Xamarin.Forms.Shapes;
using Knyaz.Xamaring.Shapes.UWP;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Path), typeof(PathRenderer))]

namespace Knyaz.Xamaring.Shapes.UWP
{
    public class PathRenderer : ViewRenderer<Path, Windows.UI.Xaml.Shapes.Path>
    {
		protected override void OnElementChanged(ElementChangedEventArgs<Path> e)
		{
			if(e.OldElement is Path oldPath)
			{
				oldPath.PropertyChanged += Control_PropertyChanged;
			}

			base.OnElementChanged(e);
			if (e.NewElement is Path newPath)
			{
				if (Control == null)
				{
					var uwpPath = new Windows.UI.Xaml.Shapes.Path();
					newPath.PropertyChanged += Control_PropertyChanged;
					SetNativeControl(uwpPath);
				}

				Control.StrokeThickness = newPath.StrokeThickness;
				Control.Stroke = new SolidColorBrush(newPath.Stroke.ToWindowsColor());
				Control.Data = Convert(newPath.Data);
				Control.Fill = new SolidColorBrush(newPath.Fill.ToWindowsColor());
			}
		}

		private void Control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var path = (Path)sender;

			if(e.PropertyName == nameof(Path.Fill))
			{
				Control.Fill = new SolidColorBrush(path.Fill.ToWindowsColor());
			}
			else if(e.PropertyName == nameof(Path.Stroke))
			{
				Control.Stroke = new SolidColorBrush(path.Stroke.ToWindowsColor());
			}
			else if (e.PropertyName == nameof(Path.StrokeThickness))
			{
				Control.StrokeThickness = path.StrokeThickness;
			}
			else if(e.PropertyName == nameof(Path.Data))
			{
				Control.Data = Convert(path.Data);
			}
		}

		private static PathGeometry Convert(string data)
		{
			var result = new PathGeometry();

			var pathData = PathDataParser.ToAbsolute(PathDataParser.Parse(data));

			var currentFigure = new PathFigure();
			foreach (var cmd in pathData)
			{
				switch (cmd.Type)
				{
					case PathDataParser.CommandType.MoveTo:
						if(currentFigure != null)
							result.Figures.Add(currentFigure);

						currentFigure = new PathFigure() { StartPoint = new Point(cmd.Arguments[0], cmd.Arguments[1]) };
						break;
					case PathDataParser.CommandType.LineTo:
						var lineSegment = new LineSegment { Point = new Point(cmd.Arguments[0], cmd.Arguments[1]) };
						currentFigure.Segments.Add(lineSegment);
						break;
					case PathDataParser.CommandType.Bezier:
						{
							var bizierSegment = new BezierSegment
							{
								Point1 = new Point(cmd.Arguments[0], cmd.Arguments[1]),
								Point2 = new Point(cmd.Arguments[2], cmd.Arguments[3]),
								Point3 = new Point(cmd.Arguments[4], cmd.Arguments[5])
							};
							currentFigure.Segments.Add(bizierSegment);
						}
						break;
					case PathDataParser.CommandType.Close:
						currentFigure.IsClosed = true;
						result.Figures.Add(currentFigure);
						currentFigure = new PathFigure();
						break;
				}
			}

			result.Figures.Add(currentFigure);
			return result;
		}
	}
}
