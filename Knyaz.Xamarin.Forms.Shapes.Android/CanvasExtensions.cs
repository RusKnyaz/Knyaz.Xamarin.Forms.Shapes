using Android.Content;
using Android.Util;

namespace Knyaz.Xamarin.Forms.Shapes.Android
{
	static class CanvasExtensions
	{
		public static float DpToPixels(this Context context, float valueInDp)
		{
			var metrics = context.Resources.DisplayMetrics;
			return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
		}
	}
}