using ClusterNet.Kernels;
using ClusterNet.Shapes;
using Microsoft.Collections.Extensions;
using System;

namespace ClusterNet.MeanShift
{
    internal static class PrePostProcess
    {
        /// <summary>
        /// Sets up the cluster array to be shifted.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <param name="points">The full list of points to apply MeanShift with.</param>
        /// <param name="initialClusters">The amount of points to use as clusters. 0 for all.</param>
        /// <returns>An array of <typeparamref name="T"/>s to be clustered.</returns>
        public static T[] SetupClusters<T>(
            ReadOnlySpan<T> points,
            int initialClusters)
            where T : unmanaged, IEquatable<T>
        {
            int n;
            if (initialClusters == 0)
            {
                initialClusters = points.Length;
                n = 1;
            }
            else
            {
                n = points.Length / initialClusters;
            }

            // N will be 0 if initialClusters is greater than the point count.
            // N can't be 0
            if (n == 0)
            {
                n = 1;
                initialClusters = points.Length;
            }

            // Create a cluster for each point.
            T[] clusters = new T[initialClusters];
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i] = points[i * n];
            }
            return clusters;
        }

        /// <summary>
        /// Sets up the cluster array to be shifted.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <param name="points">The full list of points to apply MeanShift with.</param>
        /// <param name="initialClusters">The amount of points to use as clusters.</param>
        /// <returns>An array of <typeparamref name="T"/>s to be clustered.</returns>
        public static (T, int)[] SetupClusters<T>(
            ReadOnlySpan<(T, int)> points,
            int initialClusters)
            where T : unmanaged, IEquatable<T>
        {
            return SetupClusters<(T, int)>(points, initialClusters);
        }

        /// <summary>
        /// Merges really similar clusters, then sorts them by size.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="clusters">The clusters to merge and sort.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        public static (T, int)[] PostProcess<T, TShape, TKernel>(
            T[] clusters,
            TKernel kernel)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;

            // Remove explict duplicate values.
            DictionarySlim<T, int> mergedCentroidsMap = new DictionarySlim<T, int>();
            foreach (var cluster in clusters)
            {
                mergedCentroidsMap.GetOrAddValueRef(cluster)++;
            }

            // Sometimes clusters can be very similar due to the nature of discrete computation.
            // This is inaccurate, merge clusters with in too similar of a range.
            // TODO: Investigate better convergence to elimate this.
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

                    // Merge clusters if they're with in the kernel's WindowSize
                    if (shape.FindDistanceSquared(otherEntry.Key, entry.Key) < kernel.WindowSize)
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

            // TODO: Investigate issues allowing fullyUnqiueClusterCount to be negative
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

        /// <summary>
        /// Merges really similar clusters, then sorts them by size.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="clusters">The weighted clusters to merge and sort.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        public static (T, int)[] PostProcess<T, TShape, TKernel>(
            (T, int)[] clusters,
            TKernel kernel)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
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

                    if (shape.FindDistanceSquared(otherEntry.Key, entry.Key) < kernel.WindowSize)
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
