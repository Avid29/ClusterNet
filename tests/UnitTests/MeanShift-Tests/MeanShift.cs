using ClusterNet;
using ClusterNet.Kernels;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Tests.Shapes;
using Tests.Tests.Image;

namespace Tests.MeanShift
{
    [TestClass]
    public class MeanShift
    {
        [TestMethod]
        public void Double_Test1_Flat()
        {
            var test = DoubleTests.DoubleTest1;
            FlatKernel kernel = new FlatKernel(test.Bandwidth);
            ClusterAlgorithms.MeanShift<double, DoubleShape, FlatKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Vector2_Test1_Flat()
        {
            var test = Vector2Tests.Vector2Test1;
            FlatKernel kernel = new FlatKernel(test.Bandwidth);
            ClusterAlgorithms.MeanShift<Vector2, Vector2Shape, FlatKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Double_Test1_Gaussian()
        {
            var test = DoubleTests.DoubleTest1;
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            ClusterAlgorithms.MeanShift<double, DoubleShape, GaussianKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Vector2_Test1_Gaussian()
        {
            Assert.Fail();
            var test = Vector2Tests.Vector2Test1;
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            ClusterAlgorithms.MeanShift<Vector2, Vector2Shape, GaussianKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Image_Minecraft_Gaussian()
        {
            var test = ImageTests.ImageTest_Minecraft;
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            ClusterAlgorithms.MeanShift<RGBColor, RGBShape, GaussianKernel>(test.Input, kernel);
        }

        [TestMethod]
        public void Image_IsThisIt_Gaussian()
        {
            var test = ImageTests.ImageTest_IsThisIt;
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            ClusterAlgorithms.MeanShift<RGBColor, RGBShape, GaussianKernel>(test.Input, kernel);
        }
    }
}
