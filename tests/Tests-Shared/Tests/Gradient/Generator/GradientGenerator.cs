using System;
using Tests.Tests.Gradient.Easing;
using Tests.Tests.Gradient.Shape;

namespace Tests.Tests.Gradient
{
    public static class GradientGenerator
    {
        public static double[] Generate(DimensionSpecs specs)
        {
            return Generate(specs.Start, specs.End, specs.Steps, specs.Ease);
        }

        public static double[] Generate(double start, double end, int steps, EasingBase ease)
        {
            double[] output = new double[steps];
            double step = (end - start) / (steps - 1);

            for (int i = 0; i < steps; i++)
            {
                double linearX = i * step;
                output[i] = (ease.Ease(linearX) * end) + start;
            }

            return output;
        }

        public static T[] Generate<T, TShape>(DimensionSpecs[] dimensionsSpecs)
            where T : unmanaged
            where TShape : struct, IGradient<T>
        {
            TShape shape = default;

            if (shape.N != dimensionsSpecs.Length)
            {
                throw new Exception();
            }

            double[][] dimensions = new double[shape.N][];
            int total = 1;
            int i;
            for (i = 0; i < shape.N; i++)
            {
                total *= dimensionsSpecs[i].Steps;
                dimensions[i] = Generate(dimensionsSpecs[i]);
            }

            T[] output = new T[total];

            int[] indicies = new int[shape.N];
            for (i = 0; i < total; i++)
            {
                double[] points = new double[shape.N];
                for (int n = 0; n < shape.N; n++)
                {
                    points[n] = dimensions[n][indicies[n]];
                }

                output[i] = shape.For(points);

                for (int n = 0; n < shape.N; n++)
                {
                    indicies[n]++;
                    if (indicies[n] < dimensionsSpecs[n].Steps)
                    {
                        break;
                    }

                    indicies[n] = 0;
                }
            }

            return output;
        }
    }
}
