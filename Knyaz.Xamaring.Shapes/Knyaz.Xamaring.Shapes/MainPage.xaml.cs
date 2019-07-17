using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Knyaz.Xamaring.Shapes
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

		private void Button_Clicked(object sender, EventArgs e)
		{
			_path.Fill = _rectangle.Fill = _ellipse.Fill = Color.Red;
			_path.Stroke = _rectangle.Stroke = _ellipse.Stroke = Color.DarkRed;
		}

		private void Button_Clicked_1(object sender, EventArgs e)
		{
			_path.Fill = _rectangle.Fill = _ellipse.Fill = Color.Green;
			_path.Stroke = _rectangle.Stroke = _ellipse.Stroke = Color.DarkGreen;
		}
	}
}
