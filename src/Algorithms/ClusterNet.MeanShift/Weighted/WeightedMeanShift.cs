// Adam Dernis © 2021

using ClusterNet.Kernels;
using ClusterNet.Shapes;
using Microsoft.Collections.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

// Warning disabled for tuple pointers.
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace ClusterNet.MeanShift
{
    // Weighted MeanShift merges duplicates and weighs each point to begin.
    // This cuts time by reducing distance calculations for duplicate points.
    // Instead any given point is guarenteed to be calculated once, then the kernel weighted
    // distance can be multiplied by the point's weight to get the actual weight of a point.

    /// <summary>
    /// A class containing root implementations of Weighted MeanShift algorithms.
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
            {
                mergedPointsMap.GetOrAddValueRef(point)++;
            }

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
            {
                mergedPointsMap.GetOrAddValueRef(point)++;
            }

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
        /// Runs MeanShift a weighted cluster on a list of <typeparamref name="T"/> points. Runs in parallel on n threads.
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
        /// <param name="threadCount">Number of threads to open. <see cref="Environment.ProcessorCount"/> if 0.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] WeightedMeanShiftFixedThreaded<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0,
            int threadCount = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            DictionarySlim<T, int> mergedPointsMap = new DictionarySlim<T, int>();
            foreach (var point in points)
            {
                mergedPointsMap.GetOrAddValueRef(point)++;
            }

            (T, int)[] weightedPoints = new (T, int)[mergedPointsMap.Count];
            int pos = 0;
            foreach (var entry in mergedPointsMap)
            {
                weightedPoints[pos] = (entry.Key, entry.Value);
                pos++;
            }

            return WeightedMeanShiftFixedThreaded<T, TShape, TKernel>(weightedPoints, kernel, initialClusters, threadCount);
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
            (T, int)[] clusters = PrePostProcess.SetupClusters(weightedPoints, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[weightedPoints.Length];

            fixed ((T, int)* p = weightedPoints)
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    (T, int) cluster = clusters[i];
                    clusters[i] = PointShifting.MeanShiftPoint<T, TShape, TKernel>(cluster, p, weightedPoints.Length, kernel, weightedSubPointList);
                }
            }

            return PrePostProcess.PostProcess<T, TShape, TKernel>(clusters, kernel);
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
            (T, int)[] clusters = PrePostProcess.SetupClusters(weightedPoints, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[weightedPoints.Length];

            fixed ((T, int)* p0 = weightedPoints)
            {
                (T, int)* p = p0;
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

            return PrePostProcess.PostProcess<T, TShape, TKernel>(clusters, kernel);
        }

        /// <summary>
        /// Runs MeanShift a weighted cluster on a list of <typeparamref name="T"/> and <see cref="int"/> tuples. Runs in parallel on n threads.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="weightedPoints">The weighed list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <param name="threadCount">Number of threads to open. <see cref="Environment.ProcessorCount"/> if 0.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] WeightedMeanShiftFixedThreaded<T, TShape, TKernel>(
            ReadOnlySpan<(T, int)> weightedPoints,
            TKernel kernel,
            int initialClusters = 0,
            int threadCount = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            if (threadCount == 0)
            {
                threadCount = Environment.ProcessorCount;
            }

            (T, int)[] clusters = PrePostProcess.SetupClusters(weightedPoints, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[weightedPoints.Length];

            fixed ((T, int)* p0 = weightedPoints)
            {
                (T, int)* p = p0;
                int pointCount = weightedPoints.Length;
                int clusterCount = clusters.Length;

                // Shift each cluster until it's at its convergence point.

                // Create n threads and have each cluster until finished
                Thread[] threads = new Thread[threadCount];
                object mutex = new object();
                int convergedClusters = 0;
                int i;

                for (i = 0; i < threadCount; i++)
                {
                    threads[i] = new Thread(() =>
                    {
                        // Define this here, and reuse it on every iteration of Shift on this thread.
                        (T, double)[] weightedSubPointList = new (T, double)[pointCount];

                        while (true)
                        {
                            int activeCluster = 0;

                            // Return if no work is remaining
                            if (convergedClusters >= clusterCount)
                            {
                                return;
                            }

                            // Lock while getting current cluster point
                            lock (mutex)
                            {
                                activeCluster = convergedClusters;
                                convergedClusters++;
                            }

                            (T, int) cluster = clusters[activeCluster];
                            clusters[activeCluster] = PointShifting.MeanShiftPoint<T, TShape, TKernel>(cluster, p, pointCount, kernel, weightedSubPointList);
                        }
                    });
                    threads[i].Start();
                }

                // Join all the threads
                for (i = 0; i < threadCount; i++)
                {
                    threads[i].Join();
                }
            }

            return PrePostProcess.PostProcess<T, TShape, TKernel>(clusters, kernel);
        }
    }
}
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
