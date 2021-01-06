using ClusterLib;
using ClusterLib.Shapes;
using System.Numerics;

namespace UnitTests.Shapes
{
    public struct Flat2DShape : IPoint<Vector2>
    {
        public Vector2 Divide(Vector2 it, double count)
        {
            it.X /= (float)count;
            it.Y /= (float)count;
            return it;
        }

        public double FindDistance(Vector2 it1, Vector2 it2)
        {
            float x = it1.X - it2.X;
            float y = it1.Y - it2.Y;
            return x * x + y * y;
        }

        public Vector2 Sum(Vector2 it1, Vector2 it2, double weight = 1)
        {
            float x = it1.X + (it2.X * (float)weight);
            float y = it1.Y + (it2.Y * (float)weight);
            return new Vector2(x, y);
        }

        public double WeightDistance(double distance, double kernelBandwidth) =>
            Kernels.FlatKernel(distance, kernelBandwidth);
    }
}
