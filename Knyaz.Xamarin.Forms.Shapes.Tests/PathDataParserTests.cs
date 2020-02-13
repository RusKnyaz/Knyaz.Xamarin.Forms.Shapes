using Knyaz.Xamarin.Forms.Shapes;
using NUnit.Framework;

namespace Tests
{
    public class PathDataParserTests
    {
		
		[TestCase("M0,0 L100 0 100 100 -5,-5", ExpectedResult = "M 0 0 L 100 0 L 100 100 L -5 -5")]
		[TestCase("m 0 0 C 1,2 3,4 5,6 7,8 9,10, 11,12", ExpectedResult = "m 0 0 C 1 2 3 4 5 6 C 7 8 9 10 11 12")]
		[TestCase("m 100 200", ExpectedResult = "m 100 200")]
        [TestCase("M-100-200", ExpectedResult = "M -100 -200")]
        [TestCase("M-100-200V100", ExpectedResult = "M -100 -200 V 100")]
        [TestCase("L-100,-200", ExpectedResult = "L -100 -200")]
        [TestCase("c1,2,3,4,5,6z", ExpectedResult = "c 1 2 3 4 5 6 Z")]
        [TestCase("H3.3", ExpectedResult = "H 3.3")]
        public string ParseAndToString(string data) =>
            PathDataParser.ToString(PathDataParser.Parse(data));

		[TestCase("M10,90 H40", ExpectedResult = "M 10 90 L 40 90")]
		[TestCase("M 10 10 l 100 50", ExpectedResult ="M 10 10 L 110 60")]
		[TestCase("M 0 0 c 1 1 2 2 3 3 1 1 2 2 3 3", ExpectedResult ="M 0 0 C 1 1 2 2 3 3 C 4 4 5 5 6 6")]
		public string ParseToAbsolute(string data) =>
			PathDataParser.ToString(PathDataParser.ToAbsolute(PathDataParser.Parse(data)));
		//148.78 47.65
    }
}