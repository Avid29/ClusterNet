using ClusterNet;
using ClusterNet.Kernels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Tests.Shapes;
using Tests_Shared.Tests;

namespace UnitTests
{
    [TestClass]
    public class MeanShift
    {
        [TestMethod]
        public void DoubleTest1Flat()
        {
            FlatKernel kernel = new FlatKernel(DoubleTests.DoubleTest1.Bandwidth);
            ClusterAlgorithms.MeanShift<double, DoubleShape, FlatKernel>(DoubleTests.DoubleTest1.Input, kernel);
        }

        [TestMethod]
        public void Vector2Test1Flat()
        {
            FlatKernel kernel = new FlatKernel(Vector2Tests.Vector2Test1.Bandwidth);
            ClusterAlgorithms.MeanShift<Vector2, Vector2Shape, FlatKernel>(Vector2Tests.Vector2Test1.Input, kernel);
        }

        [TestMethod]
        public void Vector2Test1Gaussian()
        {
            GaussianKernel kernel = new GaussianKernel(Vector2Tests.Vector2Test1.Bandwidth);
            ClusterAlgorithms.MeanShift<Vector2, Vector2Shape, GaussianKernel>(Vector2Tests.Vector2Test1.Input, kernel);
        }
    }
}
