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

namespace Tests.MeanShift
{
    [TestClass]
    public class MeanShiftTests
    {
        private void RunTest<T, TShape, TAvgProgress>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T, TAvgProgress>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            try
            {
                var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel, TAvgProgress>(test.Input, kernel);
            } catch (Exception e)
            {
                throw new Exception($"Test {test.Name} failed.", e);
            }
        }

        [TestMethod]
        public void Gradients()
        {
            foreach (var test in GradientTests.All1D)
            {
                RunTest<double, DoubleShape, (double, double)>(test);
            }

            foreach (var test in GradientTests.All2D)
            {
                RunTest<Vector2, Vector2Shape, (Vector2, double)>(test);
            }
        }

        [TestMethod]
        public void Images()
        {
            foreach(var test in ImageTests.All)
            {
                RunTest<RGBColor, RGBShape, RGBProgress>(test);
            }
        }
    }
}
