using Knyaz.Xamarin.Forms.Shapes;
using NUnit.Framework;

namespace Tests
{
    public class PathDataParserTests
    {
        [TestCase("m 100 200", ExpectedResult = "m 100 200")]
        [TestCase("M-100-200", ExpectedResult = "M -100 -200")]
        [TestCase("M-100-200V100", ExpectedResult = "M -100 -200 V 100")]
        [TestCase("L-100,-200", ExpectedResult = "L -100 -200")]
        [TestCase("c1,2,3,4,5,6z", ExpectedResult = "c 1 2 3 4 5 6 Z")]
        [TestCase("H3.3", ExpectedResult = "H 3.3")]
        public string ParseAndToString(string data) =>
            PathDataParser.ToString(PathDataParser.Parse(data));
    }
}