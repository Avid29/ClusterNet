using ClusterLib.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib.KMeans
{
    public static class KMeansMethod
    {
        public static MeanShiftCluster<T, TShape> KMeans<T, TShape>(IEnumerable<T> points, int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            List<MeanShiftCluster<T, TShape>> clusters = Split<T, TShape>(points, clusterCount);

            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (var cluster in clusters)
                {

                }
            }

            return null;
        }

        private static List<MeanShiftCluster<T, TShape>> Split<T, TShape>(IEnumerable<T> points, int clusterCount)
            where T : unmanaged
            where TShape : struct, IPoint<T>
        {
            List<MeanShiftCluster<T, TShape>> clusters = new List<MeanShiftCluster<T, TShape>>();
            int pointCount = points.Count();
            int subSize = pointCount / clusterCount;

            int totalCount = 0;
            IEnumerator<T> enumerator = points.GetEnumerator();
            for (int i = 0; i < clusterCount; i++)
            {
                MeanShiftCluster<T, TShape> currentList = new MeanShiftCluster<T, TShape>();
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
