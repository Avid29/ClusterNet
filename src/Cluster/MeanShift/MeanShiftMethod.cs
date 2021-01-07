using ClusterLib.Kernels;
using ClusterLib.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib
{
    public class MeanShiftMethod
    {
        public static List<MeanShiftCluster<T, TShape>> MeanShift<T, TShape>(
            IEnumerable<T> points,
            IKernel kernel)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            List<MeanShiftCluster<T, TShape>> clusters = points.Select(x => new MeanShiftCluster<T, TShape>(x)).ToList();
            for (int i = 0; i < clusters.Count; i++)
            {
                MeanShiftCluster<T, TShape> cluster = clusters[i];
                MeanShiftCluster<T, TShape> newCluster = default;
                bool changed = true;
                while (changed)
                {
                    newCluster = Shift(cluster, points, kernel);
                    changed = shape.FindDistanceSquared(newCluster.Centroid, cluster.Centroid) != 0;
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

        private static MeanShiftCluster<T, TShape> Shift<T, TShape>(
            MeanShiftCluster<T, TShape> p,
            IEnumerable<T> points,
            IKernel kernel)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            TShape shape = default;

            MeanShiftCluster<T, TShape> newCluster = new MeanShiftCluster<T, TShape>();

            foreach (T point in points)
            {
                double distance = shape.FindDistanceSquared(p.Centroid, point);
                double weight = kernel.WeightDistance(distance);
                newCluster.Add(point, weight);
            }

            return newCluster;
        }
    }
}
