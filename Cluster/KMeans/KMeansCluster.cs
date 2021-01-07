using ClusterLib.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib.KMeans
{
    public class KMeansCluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private T? _centroid;
        private HashSet<T> _subPointSet;

        public T Centroid
        {
            get => _centroid ?? (T)(_centroid = CalculateCentroid());
            set => _centroid = value;
        }

        public KMeansCluster()
        {
            _subPointSet = new HashSet<T>();
        }

        private T CalculateCentroid()
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
