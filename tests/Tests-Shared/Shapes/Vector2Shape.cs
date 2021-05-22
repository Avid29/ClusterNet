using ClusterNet.Shapes;
using System.Numerics;

namespace Tests.Shapes
{
    public struct Vector2Shape : IPoint<Vector2>
    {
        public bool AreEqual(Vector2 it1, Vector2 it2)
        {
            return it1 == it2;
        }

        public bool AreEqual(Vector2 it1, Vector2 it2, double error = 0)
        {
            return FindDistanceSquared(it1, it2) <= error;
        }

        public Vector2 Average(Vector2[] items)
        {
            Vector2 sumVector = new Vector2(0);
            int count = 0;
            foreach(var item in items)
            {
                sumVector += item;
                count++;
            }
            sumVector.X /= count;
            sumVector.Y /= count;
            return sumVector;
        }

        public double FindDistanceSquared(Vector2 it1, Vector2 it2)
        {
            float x = it1.X - it2.X;
            float y = it1.Y - it2.Y;
            return x * x + y * y;
        }

        public Vector2 WeightedAverage((Vector2, double)[] items)
        {
            Vector2 sumVector = new Vector2(0);
            double totalWeight = 0;
            foreach (var item in items)
            {
                sumVector.X += item.Item1.X * (float)item.Item2;
                sumVector.Y += item.Item1.Y * (float)item.Item2;
                totalWeight += item.Item2;
            }
            sumVector.X /= (float)totalWeight;
            sumVector.Y /= (float)totalWeight;
            return sumVector;
        }
    }
}
