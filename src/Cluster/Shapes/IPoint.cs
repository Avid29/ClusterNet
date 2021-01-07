using System.Collections.Generic;

namespace ClusterLib.Shapes
{
    public interface IPoint<T>
    {
        T Average(IEnumerable<T> items);

        T WeightedAverage(IEnumerable<(T, double)> items);

        T Sum(T it1, T it2);

        double FindDistanceSquared(T it1, T it2);
    }
}
