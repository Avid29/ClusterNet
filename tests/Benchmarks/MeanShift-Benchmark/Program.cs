using BenchmarkDotNet.Running;
using Benchmarks.MeanShift.Colors;

namespace Benchmarks.MeanShift
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MeanShiftColorBenchmarks>();
        }
    }
}
