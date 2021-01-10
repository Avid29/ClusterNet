using ClusterLib.Kernels;
using ClusterLib.Shapes;
using Microsoft.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static (T, int)[] MeanShift<T, TShape, TKernel>(
            ReadOnlySpan<T> points,
            TKernel kernel,
            int initialClusters = 0)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;

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
            MeanShiftCluster<T, TShape>[] clusters = new MeanShiftCluster<T, TShape>[points.Length / n];
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i] =(new MeanShiftCluster<T, TShape>(points[i * n]));
            }
            
            // Shift each cluster until it's at its convergence point.
            for (int i = 0; i < clusters.Length; i++)
            {
                MeanShiftCluster<T, TShape> cluster = clusters[i];
                MeanShiftCluster<T, TShape> newCluster = default;
                bool changed = true;

                // Shift the cluster until it does not shift.
                while (changed)
                {
                    newCluster = Shift(cluster, points, kernel);
                    changed = shape.FindDistanceSquared(newCluster.Centroid, cluster.Centroid) != 0;
                    cluster = newCluster;
                }

                // Replace original cluster with shifted cluster.
                clusters[i] = newCluster;
            }

            return PostProcess(clusters);
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
        private static MeanShiftCluster<T, TShape> Shift<T, TShape, TKernel>(
            MeanShiftCluster<T, TShape> p,
            ReadOnlySpan<T> points,
            TKernel kernel)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
            where TKernel : struct, IKernel
        {
            TShape shape = default;


            // Create cluster based on distance of points from the current cluster's centroid
            MeanShiftCluster<T, TShape> newCluster = new MeanShiftCluster<T, TShape>();
            foreach (T point in points)
            {
                double distance = shape.FindDistanceSquared(p.Centroid, point);
                double weight = kernel.WeightDistance(distance);
                newCluster.Add(point, weight);
            }

            return newCluster;
        }

        /// <summary>
        /// Merges really similar clusters, then sorts them by size.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="clusters">The clusters to merge and sort.</param>
        /// <returns>A merged sorted list of clusters.</returns>
        private static (T, int)[] PostProcess<T, TShape>(
            MeanShiftCluster<T, TShape>[] clusters)
            where T : unmanaged, IEquatable<T>
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            // Remove explict duplicate values.
            DictionarySlim<T, int> mergedCentroidsMap = new DictionarySlim<T, int>();
            foreach (var cluster in clusters)
            {
                mergedCentroidsMap.GetOrAddValueRef(cluster.Centroid)++;
            }

            int fullyUnqiueClusterCount = mergedCentroidsMap.Count;
            int i = 0;
            foreach (var entry in mergedCentroidsMap)
            {
                // This entry has been merged, skip it
                if (entry.Value == 0)
                {
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