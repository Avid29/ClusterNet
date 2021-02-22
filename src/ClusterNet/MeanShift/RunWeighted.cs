﻿using ClusterNet.Kernels;
using ClusterNet.MeanShift;
using ClusterNet.Shapes;
using Microsoft.Collections.Extensions;
using System;
using System.Threading.Tasks;

namespace ClusterNet.MeanShift
{
    /// <summary>
    /// A class containing root implementations of clustering algorithms.
    /// </summary>
    internal class RunWeighted
    {
        /// <summary>
        /// Runs MeanShift a weighted cluster on a list of <typeparamref name="T"/> points.
        /// </summary>
        /// <remarks>
        /// Runs weighted clusters by combining equivilent positions at the start.
        /// </remarks>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] WeightedMeanShift<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            DictionarySlim<T, int> mergedPointsMap = new DictionarySlim<T, int>();
            foreach (var point in points)
                mergedPointsMap.GetOrAddValueRef(point)++;

            (T, int)[] weightedPoints = new (T, int)[mergedPointsMap.Count];
            int pos = 0;
            foreach (var entry in mergedPointsMap)
            {
                weightedPoints[pos] = (entry.Key, entry.Value);
                pos++;
            }

            return WeightedMeanShift<T, TShape, TKernel>(weightedPoints, kernel, initialClusters);
        }

        /// <summary>
        /// Runs MeanShift a weighted cluster on a list of <typeparamref name="T"/> points. Runs in parallel.
        /// </summary>
        /// <remarks>
        /// Runs weighted clusters by combining equivilent positions at the start.
        /// </remarks>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            DictionarySlim<T, int> mergedPointsMap = new DictionarySlim<T, int>();
            foreach (var point in points)
                mergedPointsMap.GetOrAddValueRef(point)++;

            (T, int)[] weightedPoints = new (T, int)[mergedPointsMap.Count];
            int pos = 0;
            foreach (var entry in mergedPointsMap)
            {
                weightedPoints[pos] = (entry.Key, entry.Value);
                pos++;
            }

            return WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(weightedPoints, kernel, initialClusters);
        }

        /// <summary>
        /// Runs MeanShift a weighted cluster on a list of <typeparamref name="T"/> and <see cref="int"/> tuples.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="weightedPoints">The weighed list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] WeightedMeanShift<T, TShape, TKernel>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            (T, int)[] clusters = PrePost.SetupClusters(weightedPoints, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[weightedPoints.Length];

            fixed ((T, int) * p = weightedPoints)
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    (T, int) cluster = clusters[i];
                    clusters[i] = PointShifting.MeanShiftPoint<T, TShape, TKernel>(cluster, p, weightedPoints.Length, kernel, weightedSubPointList);
                }
            }

            return PrePost.PostProcess<T, TShape>(clusters);
        }

        /// <summary>
        /// Runs MeanShift a weighted cluster on a list of <typeparamref name="T"/> and <see cref="int"/> tuples. Runs in parallel.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="weightedPoints">The weighed list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] WeightedMeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            (T, int)[] clusters = PrePost.SetupClusters(weightedPoints, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[weightedPoints.Length];

            fixed ((T, int) * p0 = weightedPoints)
            {
                (T, int) * p = p0;
                int pointCount = weightedPoints.Length;

                // Shift each cluster until it's at its convergence point.
                Parallel.For(0, clusters.Length, (i, state) =>
                {
                    // Define this here, and reuse it on every iteration of Shift on this thread.
                    (T, double)[] weightedSubPointList = new (T, double)[pointCount];
                    (T, int) cluster = clusters[i];
                    clusters[i] = PointShifting.MeanShiftPoint<T, TShape, TKernel>(cluster, p, pointCount, kernel, weightedSubPointList);
                });
            }

            return PrePost.PostProcess<T, TShape>(clusters);
        }
    }
}
