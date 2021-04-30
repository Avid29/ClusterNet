using BenchmarkDotNet.Attributes;
using ClusterNet;
using ClusterNet.Kernels;
using ColorExtractor;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using System.Collections.Generic;

namespace Benchmarks.Shared.Color
{
    [MemoryDiagnoser]
    public class RGBBenchmarks
    {
        private RGBColor[] colors;
        private Dictionary<string, string> nameToImage;

        [Params(
            "Nirvana - Nevermind",
            "Vampie Weekend - Contra",
            "Pink Floyd - Time",
            "The Beatles - Revolver",
            "Band of Horses - Everything All The Time",
            "C418 - Minecraft - Volume Alpha",
            "Red Hot Chili Peppers - Califorinaction",
            "The Strokes - Is This It")]
        public string Url;

        [Params(1920)]
        public int PixelCount;

        [Params(480)]
        public int Quality;

        [GlobalSetup]
        public void GlobalSetup()
        {
            nameToImage = new Dictionary<string, string>()
            {
                { "Nirvana - Nevermind", "https://th.bing.com/th/id/OIP.eJ971LxYKRGqfkPXbzRkGAHaHa?pid=Api&rs=1" },
                { "Vampie Weekend - Contra", "https://th.bing.com/th/id/OIP.RgT_6lZp_G-peHs97hqMNAHaHa?pid=Api&rs=1" },
                { "Pink Floyd - Time", "https://s-media-cache-ak0.pinimg.com/736x/70/7c/98/707c98df5d2cffde6d4f755e3008771b.jpg" },
                { "The Beatles - Revolver", "https://neonmoderntimes.files.wordpress.com/2014/07/beatles-revolver-cover-art.jpg" },
                { "Band of Horses - Everything All The Time", "https://fanart.tv/fanart/music/07b6020a-c539-4d68-aeef-f159f3befc76/albumcover/everything-all-the-time-52c5e32f129ab.jpg" },
                { "C418 - Minecraft - Volume Alpha", "https://f4.bcbits.com/img/a3390257927_10.jpg" },
                { "Red Hot Chili Peppers - Califorinaction", "https://th.bing.com/th/id/OIP.F21MZzafrllhDLK-SZ4jhQHaHW?pid=Api&rs=1" },
                { "The Strokes - Is This It", "https://media.pitchfork.com/photos/5929a58b13d1975652138f9b/1:1/w_600/c1b895b7.jpg" },
            };
        }

        [IterationSetup]
        public void IterationSetup()
        {
            var image = ImageParser.GetImage(nameToImage[Url]).GetAwaiter().GetResult();

            if (image is null)
                return;

            colors = ImageParser.GetImageColors(image, PixelCount);
        }

        [Benchmark]
        public void KMeans()
        {
            ClusterAlgorithms.KMeans<RGBColor, RGBShape>(colors, 5);
        }

        [Benchmark]
        public void MeanShiftGaussianSingle()
        {
            GaussianKernel kernel = new GaussianKernel(.15);
            ClusterAlgorithms.MeanShift<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void MeanShiftGaussianMulti()
        {
            GaussianKernel kernel = new GaussianKernel(.15);
            ClusterAlgorithms.MeanShiftMultiThreaded<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void WeightedMeanShiftGaussianSingle()
        {
            GaussianKernel kernel = new GaussianKernel(.15);
            ClusterAlgorithms.WeightedMeanShift<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }

        [Benchmark]
        public void WeightedMeanShiftGaussianMulti()
        {
            GaussianKernel kernel = new GaussianKernel(.15);
            ClusterAlgorithms.WeightedMeanShiftMultiThreaded<RGBColor, RGBShape, GaussianKernel>(colors, kernel, Quality);
        }
    }
}
