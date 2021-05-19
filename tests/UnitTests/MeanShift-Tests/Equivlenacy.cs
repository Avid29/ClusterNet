using ClusterNet;
using ClusterNet.Kernels;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tests.Tests.Image;

namespace Tests.MeanShift
{
    [TestClass]
    public class Equivlenacy
    {
        public const double ACCEPTED_ERROR = .000001;

        [TestMethod]
        public void WeightedEquivilency()
        {
            RGBShape shape = default;
            var test = ImageTests.ImageTest_IsThisIt;
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<RGBColor, RGBShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShift<RGBColor, RGBShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be equal to Weighted
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Item2, actual[i].Item2);
                Assert.IsTrue(shape.FindDistanceSquared(expected[i].Item1, actual[i].Item1) < ACCEPTED_ERROR);
            }
        }
    }
}
