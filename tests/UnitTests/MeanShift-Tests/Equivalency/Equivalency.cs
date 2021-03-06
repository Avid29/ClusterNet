﻿using ClusterNet;
using ClusterNet.Kernels;
using ClusterNet.Shapes;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tests.Tests.Image;

namespace Tests.MeanShift.Equivalency
{
    public class Equivalency
    {
        public const double ACCEPTED_ERROR = .000001;

        private static void CompareResults<T, TShape>(Test<T> test, (T, int)[] expected, (T, int)[] actual, double error = ACCEPTED_ERROR)
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
                     distance <= error,
                    $"Failed on test \"{test.Name}\" because cluster {i} expected was {distance} different from the expected value, which is greater than {error}.");
            }
        }

        public static void RunWeightedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShift<T, TShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be approx equal to Weighted
            CompareResults<T, TShape>(test, expected, actual, ACCEPTED_ERROR);
        }

        public static void RunMultiThreadedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShift<T, TShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be exactly equal to MultiThreaded
            CompareResults<T, TShape>(test, expected, actual, 0);
        }

        public static void RunWeightedMultiThreadedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShiftMultiThreaded<T, TShape, GaussianKernel>(test.Input, kernel);

            // MeanShift results should be approx equal to Weighted
            CompareResults<T, TShape>(test, expected, actual, ACCEPTED_ERROR);
        }

        public static void RunFixedThreadedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.MeanShiftFixedThreaded<T, TShape, GaussianKernel>(test.Input, kernel);

            //MeanShift results should be exactly equal to FixedThreaded
            CompareResults<T, TShape>(test, expected, actual, 0);
        }

        public static void RunWeightedFixedThreadedTest<T, TShape>(Test<T> test)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            GaussianKernel kernel = new GaussianKernel(test.Bandwidth);
            var expected = ClusterAlgorithms.MeanShift<T, TShape, GaussianKernel>(test.Input, kernel);
            var actual = ClusterAlgorithms.WeightedMeanShiftFixedThreaded<T, TShape, GaussianKernel>(test.Input, kernel);

            //MeanShift results should be approx equal to Weighted
            CompareResults<T, TShape>(test, expected, actual, ACCEPTED_ERROR);
        }
    }
}
