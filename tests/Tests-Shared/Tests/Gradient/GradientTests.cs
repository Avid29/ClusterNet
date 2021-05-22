using System.Numerics;
using Tests.Tests.Gradient.Easing;
using Tests.Tests.Gradient.Shape;

namespace Tests.Tests.Gradient
{
    public static class GradientTests
    {
        public static GradientTest<Vector2, Vector2GradientShape> Linear2D_11x11 =
            new GradientTest<Vector2, Vector2GradientShape>(
            "2D Linear Gradient",
            new DimensionSpecs[]
            {
                new DimensionSpecs(0, 1, 11, new LinearEase()),
                new DimensionSpecs(0, 1, 11, new LinearEase()),    
            },
            .1,
            1);
        public static GradientTest<Vector2, Vector2GradientShape> QuadraticEaseInOut2D_21x21 =
            new GradientTest<Vector2, Vector2GradientShape>(
            "2D Quadratic EaseInOut Gradient",
            new DimensionSpecs[]
            {
                new DimensionSpecs(0, 1, 21, new QuadraticEase(EasingMode.EaseInOut)),
                new DimensionSpecs(0, 1, 21, new QuadraticEase(EasingMode.EaseInOut)),    
            },
            .05,
            1);

        public static GradientTest<Vector2, Vector2GradientShape>[] All = new GradientTest<Vector2, Vector2GradientShape>[]
        {
            Linear2D_11x11,
            QuadraticEaseInOut2D_21x21,
        };
    }
}
