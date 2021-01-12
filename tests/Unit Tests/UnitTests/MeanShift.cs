using ClusterNet;
using ClusterNet.Kernels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnitTests.Shapes;

namespace UnitTests
{
    [TestClass]
    public class MeanShift
    {
        [TestMethod]
        public void DoubleTest1Flat()
        {
            double[] points = new double[]
            {
                0,
                1,
                8,
                10,
                12,
                22,
                24,
            };

            FlatKernel kernel = new FlatKernel(5);
            MeanShiftMethod.MeanShift<double, DoubleShape, FlatKernel>(points, kernel);
        }

        [TestMethod]
        public void Vector2Test1Flat()
        {
            Vector2[] points = new Vector2[]
            {
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 0),
                new Vector2(7, 5),
                new Vector2(5, 7),
                new Vector2(6, 6),
            };

            FlatKernel kernel = new FlatKernel(5);
            MeanShiftMethod.MeanShift<Vector2, Vector2Shape, FlatKernel>(points, kernel);
        }

        [TestMethod]
        public void Vector2Test1Gaussian()
        {
            Vector2[] points = new Vector2[]
            {
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 0),
                new Vector2(7, 5),
                new Vector2(5, 7),
                new Vector2(6, 6),
            };

            GaussianKernel kernel = new GaussianKernel(5);
            MeanShiftMethod.MeanShift<Vector2, Vector2Shape, GaussianKernel>(points, kernel);
        }
    }
}
