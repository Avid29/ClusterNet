using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Tests.Shapes;
using Tests.Tests.Gradient;

namespace Tests.MeanShift.Equivalency
{
    [TestClass]
    public class GradientEquivalenceTests
    {
        [TestMethod]
        public void Weighted()
        {
            foreach (var test in GradientTests.All)
            {
                Equivalency.RunWeightedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void MultiThreaded()
        {
            foreach (var test in GradientTests.All)
            {
                Equivalency.RunMultiThreadedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void WeightedMultiThreaded()
        {
            foreach (var test in GradientTests.All)
            {
                Equivalency.RunWeightedMultiThreadedTest<Vector2, Vector2Shape>(test);
            }
        }
    }
}
