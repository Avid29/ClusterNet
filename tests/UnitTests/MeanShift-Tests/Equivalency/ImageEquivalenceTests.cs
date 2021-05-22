using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Tests.Image;

namespace Tests.MeanShift.Equivalency
{
    [TestClass]
    public class ImageEquivalenceTests
    {
        [TestMethod]
        public void Weighted()
        {
            foreach (var test in ImageTests.All)
            {
                Equivalency.RunWeightedTest<RGBColor, RGBShape>(test);
            }
        }

        [TestMethod]
        public void MultiThreaded()
        {
            foreach (var test in ImageTests.All)
            {
                Equivalency.RunMultiThreadedTest<RGBColor, RGBShape>(test);
            }
        }

        [TestMethod]
        public void WeightedMultiThreaded()
        {
            foreach (var test in ImageTests.All)
            {
                Equivalency.RunWeightedMultiThreadedTest<RGBColor, RGBShape>(test);
            }
        }
    }
}
