using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterLib.Shapes
{
    public interface IPoint<T>
    {
        T Average(IEnumerable<T> items);

        T WeightedAverage(IEnumerable<(T, double)> items);

        T Sum(T it1, T it2);

        double FindDistance(T it1, T it2);

        double WeightDistance(double distance, double kernelBandwidth);
    }
}
