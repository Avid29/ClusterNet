using ClusterNet.Kernels;
using ClusterNet.Shapes;
using Microsoft.Collections.Extensions;
using System;
using System.Threading.Tasks;

namespace ClusterNet.MeanShift
{
    public static class WeightedMeanShiftMethod
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
            (T, int)[] clusters = SetupClusters(weightedPoints, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[weightedPoints.Length];

            fixed ((T, int)* p = weightedPoints)
            {
                for (int i = 0; i < clusters.Length; i++)
                {
                    (T, int) cluster = clusters[i];
                    clusters[i] = MeanShiftPoint<T, TShape, TKernel>(cluster, p, weightedPoints.Length, kernel, weightedSubPointList);
                }
            }

            return PostProcess<T, TShape>(clusters);
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
            (T, int)[] clusters = SetupClusters(weightedPoints, initialClusters);

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
                    clusters[i] = MeanShiftPoint<T, TShape, TKernel>(cluster, p, pointCount, kernel, weightedSubPointList);
                });
            }

            return PostProcess<T, TShape>(clusters);
        }

        /// <summary>
        /// Sets up the cluster array to be shifted.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <param name="points">The full list of points to apply MeanShift with.</param>
        /// <param name="initialClusters">The amount of points to use as clusters.</param>
        /// <returns>An array of <typeparamref name="T"/>s to be clustered.</returns>
        private static (T, int)[] SetupClusters<T>(
            ReadOnlySpan<(T, int)> points,
            int initialClusters)
            where T : unmanaged, IEquatable<T>
        {
            int n;
            if (initialClusters == 0)
                n = 1;
            else
                n = points.Length / initialClusters;

            // N will be 0 if initialClusters is greater than the point count.
            // N can't be 0
            if (n == 0)
                n = 1;

            // Create a cluster for each point.
            (T, int)[] clusters = new (T, int)[points.Length / n];
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i] = points[i * n];
            }
            return clusters;
        }

        /// <summary>
        /// Runs shift on a single cluster with the full readonly points.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="cluster">The cluster to MeanShift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="pointCount">The amount of points in the array.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="weightedSubPointList">The array to shift in (passed into to save allocation)</param>
        /// <returns>The <paramref name="cluster"/> point fully shifted via MeanShift.</returns>
        private static unsafe (T, int) MeanShiftPoint<T, TShape, TKernel>(
            (T, int) cluster,
            (T, int)* points,
            int pointCount,
            TKernel kernel,
            (T, double)[] weightedSubPointList)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;


            // Shift the cluster until it does not shift.
            bool changed = true;
            (T, int) newCluster = default;
            while (changed)
            {
                newCluster = Shift<T, TShape, TKernel>(cluster, points, pointCount, kernel, weightedSubPointList);
                changed = shape.FindDistanceSquared(newCluster.Item1, cluster.Item1) != 0;
                cluster = newCluster;
            }

            return cluster;
        }

        /// <summary>
        /// Shifts a cluster towards its convergence point.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="p">The cluster to shift.</param>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <returns>The new cluster from the cluster being shifted.</returns>
        private static unsafe (T, int) Shift<T, TShape, TKernel>(
            (T, int) p,
            (T, int)* points,
            int pointCount,
            TKernel kernel,
            (T, double)[] weightedSubPointList)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;

            // Create cluster based on distance of points from the current cluster's centroid
            for (int i = 0; i < pointCount; i++)
            {
                (T, int) point = points[i];
                double distance = shape.FindDistanceSquared(p.Item1, point.Item1);
                double weight = kernel.WeightDistance(distance);
                weightedSubPointList[i] = (point.Item1, weight * point.Item2);
            }

            return (shape.WeightedAverage(weightedSubPointList), p.Item2);
        }

        /// <summary>
        /// Merges really similar clusters, then sorts them by size.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="clusters">The clusters to merge and sort.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        private static (T, int)[] PostProcess<T, TShape>(
            (T, int)[] clusters)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            // Remove explict duplicate values.
            DictionarySlim<T, int> mergedCentroidsMap = new DictionarySlim<T, int>();
            foreach (var cluster in clusters)
            {
                mergedCentroidsMap.GetOrAddValueRef(cluster.Item1) += cluster.Item2;
            }

            int fullyUnqiueClusterCount = mergedCentroidsMap.Count;
            int i = 0;
            foreach (var entry in mergedCentroidsMap)
            {
                // This entry has been merged, skip it
                if (entry.Value == 0)
                {
                    i++;
                    continue;
                }

                int j = 0;
                foreach (var otherEntry in mergedCentroidsMap)
                {
                    // The comparison has already been made, or they are the same item.
                    if (j <= i)
                    {
                        j++;
                        continue;
                    }

                    if (shape.FindDistanceSquared(otherEntry.Key, entry.Key) < .1)
                    {
                        ref int otherValue = ref mergedCentroidsMap.GetOrAddValueRef(otherEntry.Key);
                        mergedCentroidsMap.GetOrAddValueRef(entry.Key) += otherValue;
                        otherValue = 0;
                        fullyUnqiueClusterCount--;
                    }
                    j++;
                }
                i++;
            }

            (T, int)[] mergedCentroids = new (T, int)[fullyUnqiueClusterCount];
            i = 0; // Reuse i as an iter again.
            foreach (var entry in mergedCentroidsMap)
            {
                if (entry.Value != 0)
                {
                    mergedCentroids[i] = (entry.Key, entry.Value);
                    i++;
                }
            }

            Array.Sort(mergedCentroids,
                delegate ((T, int) clus1,
                (T, int) clus2)
                {
                    return clus2.Item2.CompareTo(clus1.Item2);
                });

            return mergedCentroids;
        }
    }
}
