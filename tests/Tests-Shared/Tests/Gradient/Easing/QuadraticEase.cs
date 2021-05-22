using System;

namespace Tests.Tests.Gradient.Easing
{
    public class QuadraticEase : EasingBase
    {
        public QuadraticEase(EasingMode mode)
        {
            EasingMode = mode;
            switch (mode)
            {
                case EasingMode.EaseIn:
                    EaseFunc = QuadEaseIn;
                    break;
                case EasingMode.EaseOut:
                    EaseFunc = QuadEaseOut;
                    break;
                case EasingMode.EaseInOut:
                    EaseFunc = QuadEaseInOut;
                    break;
            }
        }

        public EasingMode EasingMode { get; }

        private Func<double, double> EaseFunc { get; }

        public override double Ease(double pos)
        {
            return EaseFunc(pos);
        }

        private double QuadEaseIn(double x)
        {
            return x * x;
        }

        private double QuadEaseOut(double x)
        {
            return x * (2 - x);
        }

        private double QuadEaseInOut(double x)
        {
            if (x < .5)
                return 2 * x * x;
            return 2 * (2 - x) * x - 1;
        }
    }
}
