using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.KMeans.Colors;
using Benchmarks.MeanShift.Colors;
using Benchmarks.Shared.Color;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<RGBBenchmarks>();
        }
    }
}
