namespace Tests_Shared.Tests
{
    public class Test<T>
    {
        public Test(T[] input, double bandwidth)
        {
            Input = input;
            Bandwidth = bandwidth;
        }

        public T[] Input { get; }

        public double Bandwidth { get; }
    }
}
