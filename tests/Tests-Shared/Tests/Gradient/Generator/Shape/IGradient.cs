namespace Tests.Tests.Gradient.Shape
{
    public interface IGradient<T>
    {
        public abstract int N { get; }

        public abstract T For(double[] coords);
    }
}
