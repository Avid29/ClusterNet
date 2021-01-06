using ClusterLib.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib
{
    public class ClusterMethods
    {
        public static List<Cluster<T, TShape>> MeanShift<T, TShape>(
            IEnumerable<T> points,
            double window,
            bool isRadiusNorm = false)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            List<Cluster<T, TShape>> clusters = points.Select(x => new Cluster<T, TShape>(x)).ToList();
            for (int i = 0; i < clusters.Count; i++)
            {
                Cluster<T, TShape> cluster = clusters[i];
                Cluster<T, TShape> newCluster = default;
                bool changed = true;
                while (changed)
                {
                    newCluster = Shift(cluster, points, window);
                    changed = shape.FindDistance(newCluster.Centroid, cluster.Centroid) != 0;
                    cluster = newCluster;
                }
                clusters[i] = newCluster;
            }

            HashSet<T> clusterSet = new HashSet<T>();
            for (int i = 0; i < clusters.Count; i++)
            {
                var cluster = clusters[i];
                if (!clusterSet.Contains(cluster.Centroid))
                {
                    clusterSet.Add(cluster.Centroid);
                } else
                {
                    clusters.RemoveAt(i);
                    i--;
                }
            }

            return clusters;
        }

        private static Cluster<T, TShape> Shift<T, TShape>(Cluster<T, TShape> p, IEnumerable<T> points, double window)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            Cluster<T, TShape> newCluster = new Cluster<T, TShape>();

            foreach (T point in points)
            {
                double distance = shape.FindDistance(p.Centroid, point);
                double weight = shape.WeightDistance(distance, window);
                newCluster.Add(point, weight);
            }

            return newCluster;
        }
    }
}
