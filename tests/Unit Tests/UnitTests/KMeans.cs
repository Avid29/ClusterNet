using ClusterNet.KMeans;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnitTests.Shapes;

namespace UnitTests
{
    [TestClass]
    public class KMeans
    {
        [TestMethod]
        public void DoubleTest1()
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

            KMeansMethod.KMeans<double, DoubleShape>(points, 3);
        }

        [TestMethod]
        public void Vector2Test1()
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

            KMeansMethod.KMeans<Vector2, Vector2Shape>(points, 2);
        }
    }
}
