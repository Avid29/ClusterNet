using BenchmarkDotNet.Running;
using Benchmarks.KMeans.Colors;

namespace Benchmarks.KMeans
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<KMeansColorBenchmarks>();
        }
    }
}
