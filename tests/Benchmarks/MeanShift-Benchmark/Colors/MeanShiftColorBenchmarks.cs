using BenchmarkDotNet.Attributes;
using ClusterNet;
using ClusterNet.Kernels;
using ColorExtractor;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using System.Collections.Generic;
using Tests.Tests.Image;

namespace Benchmarks.MeanShift.Colors
{
    [MemoryDiagnoser]
    public class MeanShiftColorBenchmarks
    {
        private RGBColor[] colors;
        private readonly Dictionary<string, string> nameToImage = new Dictionary<string, string>();

        //[Params(64, 128, 256, 512)]
        [Params(480)]
        public int Quality;

        //[Params(.05, .1, .2, .5)]
        [Params(.15)]
        public double Bandwidth;

        [Params(
            Images.Califorinaction_Name,
            Images.Contra_Name,
            Images.EveryThingAllInTime_Name,
            Images.IsThisIt_Name,
            Images.Minecraft_Name,
            Images.Nevermind_Name,
            Images.Revolver_Name,
            Images.Time_Name)]
        public string Url;

        [GlobalSetup]
        public void GlobalSetup()
        {
            foreach (var image in Images.All)
            {
                nameToImage.Add(image.Name, image.Url);
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            var image = ImageParser.GetImage(nameToImage[Url]).GetAwaiter().GetResult();

            if (image is null)
                return;

            colors = ImageParser.GetImageColors(image, Quality);
        }

        [Benchmark]
        public void MeanShiftGuassian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            ClusterAlgorithms.MeanShift<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void WeightedMeanShiftGuassian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            ClusterAlgorithms.WeightedMeanShift<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void MeanShiftMultiThreadedGuassian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            ClusterAlgorithms.MeanShiftMultiThreaded<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void WeightedMeanShiftMultiThreadedGuassian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            ClusterAlgorithms.WeightedMeanShiftMultiThreaded<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void MeanShiftFixedThreadedGuassian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            ClusterAlgorithms.MeanShiftFixedThreaded<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void WeightedMeanShiftFixedThreadedGuassian()
        {
            GaussianKernel kernel = new GaussianKernel(Bandwidth);
            ClusterAlgorithms.WeightedMeanShiftFixedThreaded<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }
    }
}
