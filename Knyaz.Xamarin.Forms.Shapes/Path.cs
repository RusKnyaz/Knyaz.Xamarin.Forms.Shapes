using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Knyaz.Xamarin.Forms.Shapes
{
	public class Path : View
	{
		public static readonly BindableProperty FillProperty =
				   BindableProperty.Create(nameof(Fill), typeof(Color), typeof(Ellipse), Color.Transparent);

		/// <summary>
		/// Fill color
		/// </summary>
		public Color Fill
		{
			get { return (Color)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(Path), Color.Black);

		/// <summary>
		/// Stroke color
		/// </summary>
		public Color Stroke
		{
			get => (Color) GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		public static readonly BindableProperty StrokeThicknessProperty =
			BindableProperty.Create(nameof(StrokeThickness), typeof(float), typeof(Path), 1.0f);

		public float StrokeThickness
		{
			get => (float) GetValue(StrokeThicknessProperty);
			set => SetValue(StrokeThicknessProperty, value);
		}

		public static readonly BindableProperty DataProperty =
			BindableProperty.Create(nameof(Data), typeof(string), typeof(Path), "");

		public string Data
		{
			get => (string) GetValue(DataProperty);
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
			Close,
			SBezier,
			RelativeSBezier,
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
				data.Split(new[] {' ', ','}, System.StringSplitOptions.RemoveEmptyEntries)
					.SelectMany(SplitIndexes)
					.ToArray();

			for (var idx = 0; idx < chops.Length; idx++)
			{
				var cmdKey = chops[idx];
                CommandType cmdType;
                int argsCount = 0;

				switch (cmdKey)
				{
					case "M":
                        cmdType = CommandType.MoveTo;
                        argsCount = 2;
						break;
					case "L":
                        cmdType = CommandType.LineTo;
                        argsCount = 2;
						break;
					case "H":
                        cmdType = CommandType.LineHor;
                        argsCount = 1;
						break;
					case "V":
                        cmdType = CommandType.LineVer;
                        argsCount = 1;
                        break;
                    case "m":
                        cmdType = CommandType.RelativeMoveTo;
                        argsCount = 2;
                        break;
                    case "l":
                        cmdType = CommandType.RelativeLineTo;
                        argsCount = 2;
                        break;
                    case "h":
                        cmdType = CommandType.RelativeLineHor;
                        argsCount = 1;
                        break;
					case "v":
                        cmdType = CommandType.RelativeLineVer;
                        argsCount = 1;
                        break;
					case "C":
                        cmdType = CommandType.Bezier;
                        argsCount = 6;
						break;
                    case "c":
                        cmdType = CommandType.RelativeBezier;
                        argsCount = 6;
                        break;
                    case "Q":
                        cmdType = CommandType.QBezier;
                        argsCount = 4;
                        break;
					case "S":
						cmdType = CommandType.SBezier;
						argsCount = 4;
						break;
					case "s":
						cmdType = CommandType.RelativeSBezier;
						argsCount = 4;
						break;
					case "Z":
					case "z":
                        cmdType = CommandType.Close;
                        argsCount = 0;
                        break;
                    default:
                        continue;
				}

                if (idx + argsCount >= chops.Length)
                    continue;

                var args = new float[argsCount];
                for(var aIdx = 0;  aIdx < argsCount; aIdx ++)
                {
                    args[aIdx] = float.Parse(chops[idx + aIdx + 1], CultureInfo.InvariantCulture);
                }

                yield return new Command { Type = cmdType, Arguments = args};
                idx += argsCount;
            }
		}

		static readonly char[] Commands = {
			'M',
			'L',
			'H',
			'V',
			'm',
			'l',
			'h',
			'v',
			'c',
			'C',
			'Q',
			'Z',
			'S',
			's'
		};

		public static string ToString(IEnumerable<Command> commands)
		{
			var result = new StringBuilder();

			foreach (var command in commands)
			{
				result.Append(Commands[(int) command.Type]);
				if (command.Arguments != null)
					foreach (var arg in command.Arguments)
					{
						result.Append(' ');
						result.Append(arg.ToString(CultureInfo.InvariantCulture));
					}

				result.Append(' ');
			}

			return result.ToString().TrimEnd();
		}

		/// <summary>
		/// Convert relative commands to absolute
		/// </summary>
		/// <param name="commands"></param>
		/// <returns></returns>
		public static IEnumerable<Command> ToAbsolute(IEnumerable<Command> commands)
		{
			float lastX = 0;
			float lastY = 0;
			float secondLastX = 0;
			float secondLastY = 0;
			bool isLastBezier = false;
			foreach(var cmd in commands)
			{
				switch (cmd.Type)
				{
					case CommandType.Bezier:
						secondLastX = cmd.Arguments[2];
						secondLastY = cmd.Arguments[3];
						lastX = cmd.Arguments[4];
						lastY = cmd.Arguments[5];
						yield return cmd;
						isLastBezier = true;
						break;
					case CommandType.LineHor:
						lastX = cmd.Arguments[0];
						yield return cmd;
						isLastBezier = false;
						break;
					case CommandType.LineTo:
						lastX = cmd.Arguments[0];
						lastY = cmd.Arguments[1];
						yield return cmd;
						isLastBezier = false;
						break;
					case CommandType.LineVer:
						yield return new Command
						{
							Type = CommandType.LineTo,
							Arguments = new float[]
							{
								lastX,
								lastY = cmd.Arguments[0]
							}
						};
						isLastBezier = false;
						break;
					case CommandType.MoveTo:
						lastX = cmd.Arguments[0];
						lastY = cmd.Arguments[1];
						yield return cmd;
						isLastBezier = false;
						break;
					case CommandType.QBezier:
						lastX = cmd.Arguments[2];
						lastY = cmd.Arguments[3];
						yield return cmd;
						isLastBezier = false;
						break;
					case CommandType.RelativeBezier:
						yield return new Command
						{
							Type = CommandType.Bezier,
							Arguments = new float[]
							{
								lastX+cmd.Arguments[0],
								lastY+cmd.Arguments[1],
								secondLastX = lastX+cmd.Arguments[2],
								secondLastY = lastY+cmd.Arguments[3],
								lastX = lastX+cmd.Arguments[4],
								lastY = lastY+cmd.Arguments[5]
							}
						};
						isLastBezier = true;
						break;
					case CommandType.RelativeLineHor:
						yield return new Command
						{
							Type = CommandType.LineTo,
							Arguments = new float[]
							{
								lastX = lastX+cmd.Arguments[0],
								lastY
							}
						};
						isLastBezier = false;
						break;
					case CommandType.RelativeLineTo:
						yield return new Command
						{
							Type = CommandType.LineTo,
							Arguments = new float[]
							{
								lastX = lastX+cmd.Arguments[0],
								lastY = lastY+cmd.Arguments[1],
							}
						};
						isLastBezier = false;
						break;
					case CommandType.RelativeLineVer:
						yield return new Command
						{
							Type = CommandType.LineTo,
							Arguments = new float[]
							{
								lastX,
								lastY = lastY + cmd.Arguments[0]
							}
						};
						isLastBezier = false;
						break;
					case CommandType.RelativeMoveTo:
						yield return new Command
						{
							Type = CommandType.MoveTo,
							Arguments = new float[]
							{
								lastX = lastX + cmd.Arguments[0],
								lastY = lastY + cmd.Arguments[1]
							}
						};
						isLastBezier = false;
						break;
					case CommandType.SBezier:
						yield return new Command
						{
							Type = CommandType.Bezier,
							Arguments = new float[]
							{
								isLastBezier ? 2f * lastX - secondLastX : lastX,
								isLastBezier ? 2f * lastY - secondLastY : lastY,
								secondLastX = cmd.Arguments[0],
								secondLastY = cmd.Arguments[1],
								lastX = cmd.Arguments[2],
								lastY = cmd.Arguments[3]
							}
						};
						isLastBezier = true;
						break;
					case CommandType.RelativeSBezier:
						yield return new Command
						{
							Type = CommandType.Bezier,
							Arguments = new float[]
							{
								isLastBezier ? 2f * lastX - secondLastX : lastX,
								isLastBezier ? 2f * lastY - secondLastY : lastY,
								secondLastX = lastX + cmd.Arguments[0],
								secondLastY = lastY + cmd.Arguments[1],
								lastX = lastX + cmd.Arguments[2],
								lastY = lastY + cmd.Arguments[3]
							}
						};
						isLastBezier = true;
						break;
					default:
						yield return cmd;
						break;
				}
			}
		}
	}
}