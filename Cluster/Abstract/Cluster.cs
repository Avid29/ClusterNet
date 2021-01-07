using ClusterLib.Shapes;

namespace ClusterLib.Abstract
{
    public abstract class Cluster<T, TShape>
        where T : unmanaged
        where TShape : struct, IPoint<T>
    {
        protected T? _centroid;

        public T Centroid
        {
            get => _centroid ?? (T)(_centroid = CalculateCentroid());
        }

        protected abstract T CalculateCentroid();
    }
}
