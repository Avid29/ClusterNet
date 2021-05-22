// Adam Dernis © 2021

using ClusterNet.Shapes;
using System;
using System.Runtime.CompilerServices;

namespace ClusterNet.KMeans
{
    /// <summary>
    /// A class containing the root operations for KMeans.
    /// </summary>
    internal class Run
    {
        /// <summary>
        /// Runs KMeans cluster on a list of <typeparamref name="T"/> points.
        /// </summary>
        /// <remarks>
        /// This is not a wonderful implementation of KMeans.
        /// Many componenets shared between MeanShift and KMeans have been modified
        /// to only benefit MeanShift. Optimization will be required if this method were to actually be used.
        /// </remarks>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="points">A list of points to cluster.</param>
        /// <param name="clusterCount">The amount of clusters to form.</param>
        /// <returns>A list of weighted clusters based on their prevelence in the points.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T, int)[] KMeans<T, TShape, TAvgProgress>(
            ReadOnlySpan<T> points,
            int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T, TAvgProgress>
        {
            // Split to arbitrary clusters
            KMeansCluster<T, TShape, TAvgProgress>[] clusters = PrePost.Split<T, TShape, TAvgProgress>(points, clusterCount);

            // Run no items change cluster on iteration.
            bool changed = true;
            while (changed)
            {
                changed = false;

                // For each point in each cluster
                for (int i = 0; i < clusters.Length; i++)
                {
                    KMeansCluster<T, TShape, TAvgProgress> cluster = clusters[i];
                    for (int pointIndex = 0; pointIndex < cluster.Count; pointIndex++)
                    {
                        T point = cluster[pointIndex];

                        // Find the nearest cluster and move the item to it.
                        int nearestClusterIndex = FindNearestClusterIndex(point, clusters);
                        if (nearestClusterIndex != i)
                        {
                            // A cluster can't be made empty. Leave the item in place if the cluster would be empty
                            if (cluster.Count > 1)
                            {
                                T removedPoint = cluster.RemoveAt(pointIndex);
                                clusters[nearestClusterIndex].Add(removedPoint);
                                changed = true;
                            }
                        }
                    }
                }
            }

            (T, int)[] weightedColors = new (T, int)[clusters.Length];
            for (int i = 0; i < clusters.Length; i++)
            {
                var cluster = clusters[i];
                weightedColors[i] = (cluster.Centroid, cluster.Count);
            }

            Array.Sort(
                weightedColors,
                delegate (
                    (T, int) clus1,
                    (T, int) clus2)
                {
                    return clus2.Item2.CompareTo(clus1.Item2);
                });

            return weightedColors;
        }

        /// <summary>
        /// Find the nearest cluster to a point.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="point">The point to find a nearest cluster for.</param>
        /// <param name="clusters">The list of clusters.</param>
        /// <returns>The index in <paramref name="clusters"/> of the nearest cluster to <paramref name="point"/>.</returns>
        private static int FindNearestClusterIndex<T, TShape, TAvgProgress>(
            T point,
            KMeansCluster<T, TShape, TAvgProgress>[] clusters)
            where T : unmanaged
            where TShape : struct, IPoint<T, TAvgProgress>
        {
            TShape shape = default;

            // Track nearest seen value and its index.
            double minimumDistance = double.PositiveInfinity;
            int nearestClusterIndex = -1;

            for (int k = 0; k < clusters.Length; k++)
            {
                double distance = shape.FindDistanceSquared(point, clusters[k].Centroid);

                // Update tracking variables
                if (minimumDistance > distance)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = k;
                }
            }

            // Return index of nearest cluster
            return nearestClusterIndex;
        }
    }
}
