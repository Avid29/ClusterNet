using ColorExtractor;
using ColorExtractor.ColorSpaces;
using System;

namespace Tests.Tests.Image
{
    public class ImageTest : Test<RGBColor>
    {
        private string _url;
        private int _rows;
        private int _cols;

        public ImageTest(string name, string url, int quality, double bandwidth, int clusters) :
            base(name, null, bandwidth, clusters)
        {
            _url = url;
            _rows = (int)Math.Sqrt(quality);
            _cols = _rows;
        }

        public ImageTest(Image image, int quality, double bandwidth, int clusters) :
            this(image.Name, image.Url, quality, bandwidth, clusters)
        { }

        public override RGBColor[] Input
        {
            get
            {
                var image = ImageParser.GetImage(_url).Result;
                return ImageParser.SampleImage(ImageParser.GetImageColors(image), _rows, _cols);
            }
        }
    }
}
