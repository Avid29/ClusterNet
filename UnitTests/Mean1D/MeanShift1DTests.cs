using ClusterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Mean1D
{
    [TestClass]
    public class MeanShift1DTests
    {
        [TestMethod]
        public void Flat1DTest1()
        {
            List<double> points = new List<double>()
            {
                0,
                1,
                8,
                10,
                12,
                22,
                24,
            };

            List<double> expectedClusters = new List<double>()
            {
                .5d,
                10d,
                23d,
            };

            var clusters = ClusterMethods.MeanShift<double, FlatDoubleShape>(points, 5)
                .Select(x => x.Centroid).ToList();

            CollectionAssert.AreEquivalent(expectedClusters, clusters);
        }

        [TestMethod]
        public void Gaussian1DTest1()
        {
            List<double> points = new List<double>()
            {
                0,
                1,
                8,
                10,
                12,
                22,
                24,
            };

            List<double> expectedClusters = new List<double>()
            {
                .5d,
                10d,
                23d,
            };

            double window = 5;
            double bandwidth = (Math.PI * 2);
            bandwidth = Math.Pow(bandwidth, window / 2);

            var clusters = ClusterMethods.MeanShift<double, GaussianDoubleShape>(points, bandwidth)
                .Select(x => x.Centroid).ToList();

            Assert.IsTrue(expectedClusters.Count == clusters.Count);
        }
    }
}
