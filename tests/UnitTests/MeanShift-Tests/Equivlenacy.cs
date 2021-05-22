﻿using ClusterNet;
using ClusterNet.Kernels;
using ClusterNet.Shapes;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using Tests.Shapes;
using Tests.Tests.Gradient;
using Tests.Tests.Gradient.Shape;
using Tests.Tests.Image;

namespace Tests.MeanShift
{
    [TestClass]
    public class Equivlenacy
    {
        public const double ACCEPTED_ERROR = .000001;

        private void CompareResults<T, TShape>(Test<T> test, (T, int)[] expected, (T, int)[] actual, double error = ACCEPTED_ERROR)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;
            Assert.AreEqual(
                expected.Length,
                actual.Length,
                $"Failed on test \"{test.Name}\" where {expected.Length} clusters were expected but {actual.Length} clusters were found.");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(
                    expected[i].Item2,
                    actual[i].Item2,
                    $"Failed on test \"{test.Name}\" because cluster {i} expected {expected[i].Item2} items and had {actual[i].Item2} items.");

                double distance = shape.FindDistanceSquared(expected[i].Item1, actual[i].Item1);
                Assert.IsTrue(
                     distance < ACCEPTED_ERROR,
                    $"Failed on test \"{test.Name}\" because cluster {i} expected was {distance} different from the expected value, which is greater than {ACCEPTED_ERROR}.");
            }
        }

        private void RunMultiThreadedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShift<T, TShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be exactly equal to MultiThreaded
            CompareResults<T, TShape>(test, expected, actual, 0);
        }

        private void RunWeightedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShift<T, TShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be approx equal to Weighted
            CompareResults<T, TShape>(test, expected, actual, ACCEPTED_ERROR);
        }

        private void RunWeightedMultiThreadedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShiftMultiThreaded<T, TShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be approx equal to Weighted
            CompareResults<T, TShape>(test, expected, actual, ACCEPTED_ERROR);
        }

        [TestMethod]
        public void MultiThreadedEquivilency_GradientTests()
        {
            foreach (var test in GradientTests.All_GradientTests)
            {
                RunMultiThreadedTest<Vector2, Vector2Shape>(test);
            }
        }

        [TestMethod]
        public void MultiThreadedEquivilency_ImageTests()
        {
            foreach (var test in ImageTests.All_ImageTests)
            {
                RunMultiThreadedTest<RGBColor, RGBShape>(test);
            }
        }

        [TestMethod]
        public void WeightedEquivilency_ImageTests()
        {
            foreach (var test in ImageTests.All_ImageTests)
            {
                RunWeightedTest<RGBColor, RGBShape>(test);
            }
        }

        [TestMethod]
        public void WeightedMultiThreadedEquivilency_ImageTests()
        {
            foreach (var test in ImageTests.All_ImageTests)
            {
                RunWeightedMultiThreadedTest<RGBColor, RGBShape>(test);
            }
        }
    }
}
