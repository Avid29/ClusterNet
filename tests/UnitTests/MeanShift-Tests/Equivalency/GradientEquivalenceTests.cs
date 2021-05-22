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
            foreach (var test in GradientTests.All1D)
            {
                Equivalency.RunWeightedTest<double, DoubleShape>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                Equivalency.RunWeightedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void MultiThreaded()
        {
            foreach (var test in GradientTests.All1D)
            {
                Equivalency.RunMultiThreadedTest<double, DoubleShape>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                Equivalency.RunMultiThreadedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void WeightedMultiThreaded()
        {
            foreach (var test in GradientTests.All1D)
            {
                Equivalency.RunWeightedMultiThreadedTest<double, DoubleShape>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                Equivalency.RunWeightedMultiThreadedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void FixedThreaded()
        {
            foreach (var test in GradientTests.All1D)
            {
                Equivalency.RunFixedThreadedTest<double, DoubleShape>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                Equivalency.RunFixedThreadedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void WeightedFixedThreaded()
        {
            foreach (var test in GradientTests.All1D)
            {
                Equivalency.RunWeightedFixedThreadedTest<double, DoubleShape>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                Equivalency.RunWeightedFixedThreadedTest<Vector2, Vector2Shape>(test);
            }
        }
    }
}
