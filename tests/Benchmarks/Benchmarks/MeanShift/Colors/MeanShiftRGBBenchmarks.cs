using BenchmarkDotNet.Attributes;
using ClusterLib;
using ClusterLib.Kernels;
using ColorExtractor;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using System.Collections.Generic;

namespace Benchmarks.MeanShift.Colors
{
    [MemoryDiagnoser]
    public class MeanShiftRGBBenchmarks
    {
        private List<RGBColor> colors;

        [Params(256, 512)]
        public int Quality;

        [Params(.05, .1, .2, .5)]
        public double Bandwidth;

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
        public void Flat()
        {
            FlatKernel kernel = new FlatKernel(Bandwidth);
            MeanShiftMethod.MeanShift<RGBColor, RGBShape>(colors, kernel);
        }

        [Benchmark]
        public void Gaussian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            MeanShiftMethod.MeanShift<RGBColor, RGBShape>(colors, kernel);
        }
    }
}
