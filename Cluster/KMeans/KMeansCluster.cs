using ClusterLib.Abstract;
using ClusterLib.Shapes;
using System.Collections.Generic;

namespace ClusterLib.KMeans
{
    public class KMeansCluster<T, TShape> : Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        private List<T> _subPointSet;

        public KMeansCluster()
        {
            _subPointSet = new List<T>();
        }

        public int Count => _subPointSet.Count;

        public T this[int i]
        {
            get => _subPointSet[i];
            set => _subPointSet[i] = value;
        }

        public void Add(T item)
        {
            _subPointSet.Add(item);
            _centroid = null;
        }

        public T RemoveAt(int index)
        {
            T removed = _subPointSet[index];
            _subPointSet.RemoveAt(index);
            return removed;
        }

        protected override T CalculateCentroid()
        {
            TShape shape = default;
            return shape.Average(_subPointSet);
        }
    }
}
