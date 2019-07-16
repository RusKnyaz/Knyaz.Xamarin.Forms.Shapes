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
            base.OnElementChanged(e);
            if (Control == null)
            {

                if (e.NewElement is Path control)
                {
                    var uwpPath = new Windows.UI.Xaml.Shapes.Path();

                    uwpPath.StrokeThickness = control.StrokeThickness;
                    uwpPath.Stroke = new Windows.UI.Xaml.Media.SolidColorBrush(control.Stroke.ToWindowsColor());
					uwpPath.Data = Convert(control.Data);
					//uwpPath.Fill = new Windows.UI.Xaml.Media.SolidColorBrush(control.Fill.ToWindowsColor());
					SetNativeControl(uwpPath);
                }
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
				}
			}

			result.Figures.Add(currentFigure);
			return result;
		}
	}
}
