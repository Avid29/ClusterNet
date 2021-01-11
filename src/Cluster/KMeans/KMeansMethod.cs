using ClusterLib.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib.KMeans
{
    /// <summary>
    /// A class containing methods related to the KMeans algorithm.
    /// </summary>
    public static class KMeansMethod
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
        public static (T, int)[] KMeans<T, TShape>(
            ReadOnlySpan<T> points,
            int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            // Split to arbitrary clusters
            List<KMeansCluster<T, TShape>> clusters = Split<T, TShape>(points, clusterCount);

            // Run no items change cluster on iteration.
            bool changed = true;
            while (changed)
            {
                changed = false;

                // For each point in each cluster
                foreach (var cluster in clusters)
                {
                    for (int pointIndex = 0; pointIndex < cluster.Count; pointIndex++)
                    {
                        T point = cluster[pointIndex];

                        // Find the nearest cluster and move the item to it.
                        int nearestCluster = FindNearestCluster(point, clusters);
                        if (nearestCluster != clusters.IndexOf(cluster))
                        {
                            // A cluster can't be made empty. Leave the item in place if the cluster would be empty
                            if (cluster.Count > 1)
                            {
                                T removedPoint = cluster.RemoveAt(pointIndex);
                                clusters[nearestCluster].Add(removedPoint);
                                changed = true;
                            }
                        }
                    }
                }
            }

            return clusters
                .Select(x => (x.Centroid, x.Count))
                .OrderByDescending(x => x.Item2)
                .ToArray();
        }

        /// <summary>
        /// Find the nearest cluster to a point.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="clusters">The list of clusters.</param>
        /// <param name="point">The point to find a nearest cluster for.</param>
        /// <returns>The index in <paramref name="clusters"/> of the nearest cluster to <paramref name="point"/>.</returns>
        private static int FindNearestCluster<T, TShape>(
            T point,
            List<KMeansCluster<T, TShape>> clusters)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            // Track nearest seen value and its index.
            double minimumDistance = double.PositiveInfinity;
            int nearestClusterIndex = -1;

            for (int k = 0; k < clusters.Count; k++)
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

        /// <summary>
        /// Splits a list of points in to arbitrary clusters.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="points">The list of points to place into clusters.</param>
        /// <param name="clusterCount">The amount of clusters to create.</param>
        /// <returns>A list of arbitrary clusters of size <paramref name="clusterCount"/> made out of the points in <paramref name="points"/>.</returns>
        private static List<KMeansCluster<T, TShape>> Split<T, TShape>(
            ReadOnlySpan<T> points,
            int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            List<KMeansCluster<T, TShape>> clusters = new List<KMeansCluster<T, TShape>>();
            int subSize = points.Length / clusterCount;

            int iterationPos = 0;
            for (int i = 0; i < clusterCount; i++)
            {
                KMeansCluster<T, TShape> currentCluster = new KMeansCluster<T, TShape>();

                // Until the cluster is full or the enumerator is out of elements.
                for (int j = 0; j < subSize && iterationPos < points.Length; j++)
                {
                    // Add element to current cluster and advance iteration.
                    currentCluster.Add(points[iterationPos]);
                    iterationPos++;
                }
                clusters.Add(currentCluster);
            }
            return clusters;
        }
    }
}
