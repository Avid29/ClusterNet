using Tests.Tests.Gradient.Shape;

namespace Tests.Tests.Gradient.Generator.Shape
{
    public struct DoubleGradientShape : IGradient<double>
    {
        public int N => 1;

        public double For(double[] coords)
        {
            return coords[0];
        }
    }
}
