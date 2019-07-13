using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;
namespace Knyaz.Xamaring.Shapes
{
    public class Path : View
    {
        public static readonly BindableProperty StrokeProperty =
               BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(Path), Color.Black);

        /// <summary>
        /// Stroke color
        /// </summary>
        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(float), typeof(Path), 1.0f);

        public float StrokeThickness
        {
            get => (float)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly BindableProperty DataProperty =
            BindableProperty.Create(nameof(Data), typeof(string), typeof(Path), "");

        public string Data
        {
            get => (string)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }

    public static class PathDataParser
    {
        public class Command
        {
            public CommandType Type;
            public float[] Arguments;
        }

        public enum CommandType
        {
            MoveTo,
            LineTo,
            LineHor,
            LineVer,
            RelativeMoveTo,
            RelativeLineTo,
            RelativeLineHor,
            RelativeLineVer,
            RelativeBezier,
            Bezier,
            QBezier,
            Close
        }

        private static IEnumerable<string> SplitIndexes(string data)
        {
            var stringBuilder = new StringBuilder();
            for (var idx = 0; idx < data.Length; idx++)
            {
                var c = data[idx];
                if (char.IsLetter(c))
                {
                    var res = stringBuilder.ToString();
                    if (!string.IsNullOrEmpty(res))
                    {
                        yield return res;
                        stringBuilder.Clear();
                    }
                    yield return c.ToString();
                }
                else if (c == '-')
                {
                    var res = stringBuilder.ToString();
                    if (!string.IsNullOrEmpty(res))
                    {
                        yield return res;
                        stringBuilder.Clear();
                    }

                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            var res2 = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(res2))
            {
                yield return res2;
            }
        }


        public static IEnumerable<Command> Parse(string data)
        {
            var chops =
                data.Split(new[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(SplitIndexes)
                .ToArray(); ;

            for (var idx = 0; idx < chops.Length; idx++)
            {
                var cmdKey = chops[idx];
                switch (cmdKey)
                {
                    case "M":
                        if (idx + 2 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.MoveTo,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture) }
                        };
                        idx += 2;
                        break;
                    case "L":
                        if (idx + 2 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.LineTo,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture) }
                        };
                        idx += 2;
                        break;
                    case "H":
                        if (idx + 1 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.LineHor,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture)}
                        };
                        idx++;
                        break;
                    case "V":
                        if (idx + 1 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.LineVer,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture)}
                        };
                        idx++;
                        break;
                    case "m":
                        if (idx + 2 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.RelativeMoveTo,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture) }
                        };
                        idx += 2;
                        break;
                    case "l":
                        if (idx + 2 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.RelativeLineTo,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture) }
                        };
                        idx += 2;
                        break;
                    case "h":
                        if (idx + 1 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.RelativeLineHor,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture)}
                        };
                        idx++;
                        break;
                    case "v":
                        if (idx + 1 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.RelativeLineVer,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture)}
                        };
                        idx++;
                        break;
                    case "C":
                        if (idx + 6 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.Bezier,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 3], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 4], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 5], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 6], CultureInfo.InvariantCulture)}
                        };
                        idx += 6;
                        break;
                    case "c":
                        if (idx + 6 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.RelativeBezier,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 3], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 4], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 5], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 6], CultureInfo.InvariantCulture)}
                        };
                        idx += 6;
                        break;
                    case "Q":
                        if (idx + 4 >= chops.Length)
                            yield break;

                        yield return new Command
                        {
                            Type = CommandType.QBezier,
                            Arguments = new[] {
                                float.Parse(chops[idx + 1], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 2], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 3], CultureInfo.InvariantCulture),
                                float.Parse(chops[idx + 4], CultureInfo.InvariantCulture)}
                        };
                        idx += 4;
                        break;
                    case "Z":
                    case "z":
                        yield return new Command { Type = CommandType.Close };
                        break;

                }
            }
        }
    }
}
