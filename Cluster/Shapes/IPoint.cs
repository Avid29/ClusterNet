using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterLib.Shapes
{
    public interface IPoint<T>
    {
        T Sum(T it1, T it2, double weight = 1);

        T Divide(T it, double count);

        double FindDistance(T it1, T it2);

        double WeightDistance(double distance, double kernelBandwidth);
    }
}
