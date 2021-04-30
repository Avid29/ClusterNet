namespace Tests
{
    public class Test<T>
    {
        public Test(T[] input, double bandwidth, int clusters)
        {
            Input = input;
            Bandwidth = bandwidth;
            K = clusters;
        }

        public T[] Input { get; }

        public double Bandwidth { get; }

        public int K { get; }
    }
}
