using BenchmarkDotNet.Attributes;
using ClusterNet.KMeans;
using ColorExtractor;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using System.Collections.Generic;

namespace Benchmarks.KMeans.Colors
{
    [MemoryDiagnoser]
    public class KMeansRGBBenchmarks
    {
        private RGBColor[] colors;

        [Params(64, 128, 256, 512)]
        public int Quality;

        [Params(1, 2, 3, 4, 5)]
        public int Clusters;

        [Params("https://i.pinimg.com/736x/b6/63/a5/b663a51354e2435219997157d33f2a77--the-eagles-vinyl-records.jpg")]
        public string Url;

        [GlobalSetup]
        public void Setup()
        {
            var image = ImageParser.GetImage(Url).GetAwaiter().GetResult(); ;

            if (image is null)
                return;

            colors = ImageParser.GetImageColors(image, Quality);
        }

        [Benchmark]
        public void Run()
        {
            KMeansMethod.KMeans<RGBColor, RGBShape>(colors, Clusters);
        }
    }
}
