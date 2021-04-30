using ClusterNet.Shapes;
using System;

namespace ClusterNet.KMeans
{
    internal static class PrePost
    {
        /// <summary>
        /// Splits a list of points in to arbitrary clusters.
        /// </summary>
        /// <typeparam name="T">The type of points to cluster.</typeparam>
        /// <typeparam name="TShape">The shape to use on the points to cluster.</typeparam>
        /// <param name="points">The list of points to place into clusters.</param>
        /// <param name="clusterCount">The amount of clusters to create.</param>
        /// <returns>A list of arbitrary clusters of size <paramref name="clusterCount"/> made out of the points in <paramref name="points"/>.</returns>
        public static KMeansCluster<T, TShape>[] Split<T, TShape>(
            ReadOnlySpan<T> points,
            int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            KMeansCluster<T, TShape>[] clusters = new KMeansCluster<T, TShape>[clusterCount];
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

                clusters[i] = currentCluster;
            }

            return clusters;
        }
    }
}
