using ClusterNet;
using ClusterNet.Kernels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Tests.Shapes;

namespace Tests.MeanShift
{
    [TestClass]
    public class WeightedMeanShift
    {
        [TestMethod]
        public void DoubleTest1Flat()
        {
            var test = DoubleTests.DoubleTest1;
            FlatKernel kernel = new FlatKernel(test.Bandwidth);
            ClusterAlgorithms.WeightedMeanShift<double, DoubleShape, FlatKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Vector2Test1Flat()
        {
            var test = Vector2Tests.Vector2Test1;
            FlatKernel kernel = new FlatKernel(test.Bandwidth);
            ClusterAlgorithms.WeightedMeanShift<Vector2, Vector2Shape, FlatKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void DoubleTest1Gaussian()
        {
            var test = DoubleTests.DoubleTest1;
            GaussianKernel kernel = new GaussianKernel(DoubleTests.DoubleTest1.Bandwidth);
            ClusterAlgorithms.WeightedMeanShift<double, DoubleShape, GaussianKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Vector2Test1Gaussian()
        {
            var test = Vector2Tests.Vector2Test1;
            GaussianKernel kernel = new GaussianKernel(Vector2Tests.Vector2Test1.Bandwidth);
            ClusterAlgorithms.WeightedMeanShift<Vector2, Vector2Shape, GaussianKernel>(test.Input, kernel);
        }
    }
}
