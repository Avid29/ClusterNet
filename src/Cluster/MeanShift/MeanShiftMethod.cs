using ClusterLib.Kernels;
using ClusterLib.Shapes;
using Microsoft.Collections.Extensions;
using System;
using System.Threading.Tasks;

namespace ClusterLib
{
    /// <summary>
    /// A class containing methods related to the MeanShift algorithm.
    /// </summary>
    public static class MeanShiftMethod
    {
        /// <summary>
        /// Runs MeanShift cluster on a list of <typeparamref name="T"/> points.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] MeanShift<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            T[] clusters = SetupClusters(points, initialClusters);

            // Define this here, and reuse it on every iteration of Shift.
            (T, double)[] weightedSubPointList = new (T, double)[points.Length];

            fixed (T* p = points)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    T cluster = clusters[i];
                    clusters[i] = MeanShiftPoint<T, TShape, TKernel>(cluster, p, points.Length, kernel, weightedSubPointList);
                }
            }

            return PostProcess<T, TShape>(clusters);
        }

        /// <summary>
        /// Runs MeanShift cluster on a list of <typeparamref name="T"/> points. Runs in parallel.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
        /// <param name="points">The list of points to cluster.</param>
        /// <param name="kernel">The kernel used to weight the a points effect on the cluster.</param>
        /// <param name="initialClusters">How many of the points to shift into place. 0 means one for each point.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        public static unsafe (T, int)[] MeanShiftMultiThreaded<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            T[] clusters = SetupClusters(points, initialClusters);

            fixed (T* p0 = points)
            {
                T* p = p0;
                int pointCount = points.Length;

                // Shift each cluster until it's at its convergence point.
                Parallel.For(0, clusters.Length, (i, state) =>
                {
                    // Define this here, and reuse it on every iteration of Shift on this thread.
                    (T, double)[] weightedSubPointList = new (T, double)[pointCount];
                    T cluster = clusters[i];
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
        private static T[] SetupClusters<T>(
            ReadOnlySpan<T> points,
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
            T[] clusters = new T[points.Length / n];
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
        private static unsafe T MeanShiftPoint<T, TShape, TKernel>(
            T cluster,
            T* points,
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
            T newCluster = default;
            while (changed)
            {
                newCluster = Shift<T, TShape, TKernel>(cluster, points, pointCount, kernel, weightedSubPointList);
                changed = shape.FindDistanceSquared(newCluster, cluster) != 0;
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
        private static unsafe T Shift<T, TShape, TKernel>(
            T p,
            T* points,
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
                T point = points[i];
                double distance = shape.FindDistanceSquared(p, point);
                double weight = kernel.WeightDistance(distance);
                weightedSubPointList[i] = (point, weight);
            }

            return shape.WeightedAverage(weightedSubPointList);
        }

        /// <summary>
        /// Merges really similar clusters, then sorts them by size.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="clusters">The clusters to merge and sort.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        private static (T, int)[] PostProcess<T, TShape>(
            T[] clusters)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            // Remove explict duplicate values.
            DictionarySlim<T, int> mergedCentroidsMap = new DictionarySlim<T, int>();
            foreach (var cluster in clusters)
            {
                mergedCentroidsMap.GetOrAddValueRef(cluster)++;
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
                    if (j <= i )
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