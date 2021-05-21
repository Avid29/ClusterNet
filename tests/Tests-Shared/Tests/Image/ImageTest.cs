using ColorExtractor;
using ColorExtractor.ColorSpaces;

namespace Tests.Tests.Image
{
    public class ImageTest : Test<RGBColor>
    {
        private string _url;
        private int _quality;

        public ImageTest(string name, string url, int quality, double bandwidth, int clusters) :
            base(name, null, bandwidth, clusters)
        {
            _url = url;
            _quality = quality;
        }

        public override RGBColor[] Input
        {
            get
            {
                var image = ImageParser.GetImage(_url).Result;
                return ImageParser.GetImageColors(image, _quality);
            }
        }
    }
}
