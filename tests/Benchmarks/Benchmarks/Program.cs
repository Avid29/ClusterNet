using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.KMeans.Colors;
using Benchmarks.MeanShift.Colors;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<KMeansRGBBenchmarks>();
            BenchmarkRunner.Run<MeanShiftRGBBenchmarks>();
        }
    }
}
