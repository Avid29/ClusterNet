namespace Tests
{
    public class Test<T>
    {
        public Test(string name, T[] input, double bandwidth, int clusters)
        {
            Name = name;
            Input = input;
            Bandwidth = bandwidth;
            K = clusters;
        }

        public string Name { get; }

        public virtual T[] Input { get; }

        public double Bandwidth { get; }

        public int K { get; }
    }
}
