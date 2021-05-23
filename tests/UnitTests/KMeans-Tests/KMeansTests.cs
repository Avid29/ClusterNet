using ClusterNet;
using ClusterNet.Kernels;
using ClusterNet.Shapes;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using Tests.Shapes;
using Tests.Tests.Gradient;
using Tests.Tests.Image;

namespace Tests.KMeans
{
    [TestClass]
    public class KMeansTests
    {
        private void RunTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            try
            {
                var expected = ClusterAlgorithms.KMeans<T, TShape>(test.Input, test.K);
            }
            catch (Exception e)
            {
                throw new Exception($"Test {test.Name} failed.", e);
            }
        }

        [TestMethod]
        public void Gradients()
        {
            foreach (var test in GradientTests.All1D)
            {
                RunTest<double, DoubleShape>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                RunTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void Images()
        {
            foreach (var test in ImageTests.All)
            {
                RunTest<RGBColor, RGBShape>(test);
            }
        }
    }
}
