using System.Collections.Generic;

namespace ClusterLib.Shapes
{
    public interface IPoint<T>
    {
        T Average(IEnumerable<T> items);

        T WeightedAverage(IEnumerable<(T, double)> items);

        double FindDistanceSquared(T it1, T it2);
    }
}
