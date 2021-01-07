using ClusterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnitTests.Shapes;

namespace UnitTests
{
    [TestClass]
    public class Tests
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

            var clusters = MeanShiftMethod.MeanShift<double, FlatDoubleShape>(points, 5)
                .Select(x => x.Centroid).ToList();

            CollectionAssert.AreEquivalent(expectedClusters, clusters);
        }

        [TestMethod]
        public void Flat2DTest1()
        {
            List<Vector2> points = new List<Vector2>()
            {
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 0),
                new Vector2(7, 5),
                new Vector2(5, 7),
                new Vector2(6, 6),
            };

            List<Vector2> expectedClusters = new List<Vector2>()
            {
                new Vector2(1, 1),
                new Vector2(6, 6),
            };

            var clusters = MeanShiftMethod.MeanShift<Vector2, Flat2DShape>(points, 5)
                .Select(x => x.Centroid).ToList();

            CollectionAssert.AreEquivalent(expectedClusters, clusters);
        }
    }
}
