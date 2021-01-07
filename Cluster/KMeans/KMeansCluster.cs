using ClusterLib.Abstract;
using ClusterLib.Shapes;
using System.Collections.Generic;

namespace ClusterLib.KMeans
{
    public class KMeansCluster<T, TShape> : Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private HashSet<T> _subPointSet;

        public KMeansCluster()
        {
            _subPointSet = new HashSet<T>();
        }

        protected override T CalculateCentroid()
        {
            T sum = default;
            TShape shape = default;

            foreach (T item in _subPointSet)
                sum = shape.Sum(sum, item);

            sum = shape.Divide(sum, _subPointSet.Count);
            return sum;
        }
    }
}
