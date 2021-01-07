using ClusterLib.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib.KMeans
{
    public static class KMeansMethod
    {
        public static List<KMeansCluster<T, TShape>> KMeans<T, TShape>(IEnumerable<T> points, int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            List<KMeansCluster<T, TShape>> clusters = Split<T, TShape>(points, clusterCount);

            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (var cluster in clusters)
                {
                    for (int pointIndex = 0; pointIndex < cluster.Count; pointIndex++)
                    {
                        T point = cluster[pointIndex];

                        int nearestCluster = FindNearestCluster(clusters, point);
                        if (nearestCluster != clusters.IndexOf(cluster))
                        {
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

            return clusters;
        }

        private static int FindNearestCluster<T, TShape>(List<KMeansCluster<T, TShape>> clusters, T point)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;
            double minimumDistance = 0.0;
            int nearestClusterIndex = -1;

            for (int k = 0; k < clusters.Count; k++)
            {
                double distance = shape.FindDistanceSquared(point, clusters[k].Centroid);
                if (k == 0)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = 0;
                }
                else if (minimumDistance > distance)
                {
                    minimumDistance = distance;
                    nearestClusterIndex = k;
                }
            }

            return (nearestClusterIndex);
        }

        private static List<KMeansCluster<T, TShape>> Split<T, TShape>(IEnumerable<T> points, int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            List<KMeansCluster<T, TShape>> clusters = new List<KMeansCluster<T, TShape>>();
            int pointCount = points.Count();
            int subSize = pointCount / clusterCount;

            int totalCount = 0;
            IEnumerator<T> enumerator = points.GetEnumerator();
            for (int i = 0; i < clusterCount; i++)
            {
                KMeansCluster<T, TShape> currentList = new KMeansCluster<T, TShape>();
                for (int j = 0; j < subSize && totalCount < pointCount; j++)
                {
                    currentList.Add(enumerator.Current);
                    totalCount++;
                    enumerator.MoveNext();
                }
                clusters.Add(currentList);
            }
            return clusters;
        }
    }
}
