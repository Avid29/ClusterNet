namespace Tests.Tests.Gradient.Easing
{
    public class LinearEase : EasingBase
    {
        public override double Ease(double pos)
        {
            return pos;
        }
    }
}
