using System.Numerics;
using Tests.Tests.Gradient.Easing;
using Tests.Tests.Gradient.Generator.Shape;
using Tests.Tests.Gradient.Shape;

namespace Tests.Tests.Gradient
{
    public static class GradientTests
    {
        public static GradientTest<double, DoubleGradientShape> Linear1D_101 = new GradientTest<double, DoubleGradientShape>(
            "1D Linear Gradient 101",
            new DimensionSpecs[]
            {
                new DimensionSpecs(0, 1, 101, new LinearEase()),
            },
            .1,
            1);

        public static GradientTest<double, DoubleGradientShape> QuadraticEaseIn1D_401 = new GradientTest<double, DoubleGradientShape>(
            "1D Quadratic EaseIn Gradient 401",
            new DimensionSpecs[]
            {
                new DimensionSpecs(0, 1, 401, new QuadraticEase(EasingMode.EaseIn)),
            },
            .05,
            3);

        public static GradientTest<Vector2, Vector2GradientShape> Linear2D_11x11 =
            new GradientTest<Vector2, Vector2GradientShape>(
            "2D Linear Gradient 11x11",
            new DimensionSpecs[]
            {
                new DimensionSpecs(0, 1, 11, new LinearEase()),
                new DimensionSpecs(0, 1, 11, new LinearEase()),    
            },
            .1,
            1);

        public static GradientTest<Vector2, Vector2GradientShape> QuadraticEaseInOut2D_21x21 =
            new GradientTest<Vector2, Vector2GradientShape>(
            "2D Quadratic EaseInOut Gradient 21x21",
            new DimensionSpecs[]
            {
                new DimensionSpecs(0, 1, 21, new QuadraticEase(EasingMode.EaseInOut)),
                new DimensionSpecs(0, 1, 21, new QuadraticEase(EasingMode.EaseInOut)),    
            },
            .05,
            5);

        public static GradientTest<double, DoubleGradientShape>[] All1D = new GradientTest<double, DoubleGradientShape>[]
        {
            Linear1D_101,
            QuadraticEaseIn1D_401,
        };

        public static GradientTest<Vector2, Vector2GradientShape>[] All2D = new GradientTest<Vector2, Vector2GradientShape>[]
        {
            Linear2D_11x11,
            QuadraticEaseInOut2D_21x21,
        };
    }
}
