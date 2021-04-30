using BenchmarkDotNet.Running;
using Benchmarks.KMeans.Colors;
using Benchmarks.MeanShift.Colors;

namespace AllBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<KMeansColorBenchmarks>();
            BenchmarkRunner.Run<MeanShiftColorBenchmarks>();
        }
    }
}
